using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Google.XR.ARCoreExtensions;

public class CreateCubeUseIDButton : MonoBehaviour
{
    public GameObject CubePrefab;
    public ARAnchorManager arAnchorManager;
    public DataManager DataManager;

    public void Click()
    {
        int anchorCount = DataManager.AnchorDatas.Count;

        if (anchorCount == 0)
        {
            Debug.Log("저장된 앵커 데이터 정보가 없습니다.");
            return;
        }

        for (int i = 0; i < anchorCount; i++)
        {
            ARCloudAnchor arCloudAnchor = ARAnchorManagerExtensions.ResolveCloudAnchorId(arAnchorManager, DataManager.AnchorDatas[i].AnchorID);

            Instantiate(CubePrefab, arCloudAnchor.gameObject.transform);
        }

        Debug.Log("CloudAnchorID를 기반으로 큐브를 생성했습니다.");
    }
}
