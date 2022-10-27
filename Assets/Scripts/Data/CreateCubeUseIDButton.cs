using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Google.XR.ARCoreExtensions;

public class CreateCubeUseIDButton : MonoBehaviour
{
    public GameObject CubePrefab;
    public ARAnchorManager ARAnchorManager;
    public DataManager DataManager;

    private GameObject Cube;
    private bool a = false;

    private void Update()
    {
        if (!a)
        {
            return;
        }

        if (Cube.transform.position != Vector3.zero)
        {
            Debug.Log(Cube.transform.position);
        }
    }

    /// <summary>
    /// 클라우드 앵커 ID를 이용해 앵커가 존재하는 지점에 큐브를 생성하는 메서드
    /// </summary>
    public void Click()
    {
        // 클라우드 앵커의 개수를 세고 없으면 log 출력
        int anchorCount = DataManager.CountAnchorData();
        if (anchorCount == 0)
        {
            Debug.Log("저장된 데이터가 없습니다.");
            return;
        }

        for (int i = 0; i < anchorCount; i++)
        {
            // 클라우드 앵커 ID에서 클라우드 앵커를 반환
            ARCloudAnchor arCloudAnchor = ARAnchorManager.ResolveCloudAnchorId(DataManager.GetAnchorID(i));

            // 클라우드 앵커가 존재하는 위치에 큐브를 생성
            Cube = Instantiate(CubePrefab, arCloudAnchor.transform);
            Debug.Log("CloudAnchorID를 기반으로 큐브를 생성했습니다.");
        }

        a = true;
    }
}
