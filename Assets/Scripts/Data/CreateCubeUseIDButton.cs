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
            Debug.Log("����� ��Ŀ ������ ������ �����ϴ�.");
            return;
        }

        for (int i = 0; i < anchorCount; i++)
        {
            ARCloudAnchor arCloudAnchor = ARAnchorManagerExtensions.ResolveCloudAnchorId(arAnchorManager, DataManager.AnchorDatas[i].AnchorID);

            Instantiate(CubePrefab, arCloudAnchor.gameObject.transform);
        }

        Debug.Log("CloudAnchorID�� ������� ť�긦 �����߽��ϴ�.");
    }
}
