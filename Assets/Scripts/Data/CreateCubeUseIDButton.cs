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

    /// <summary>
    /// Ŭ���� ��Ŀ ID�� �̿��� ��Ŀ�� �����ϴ� ������ ť�긦 �����ϴ� �޼���
    /// </summary>
    public void Click()
    {
        // Ŭ���� ��Ŀ�� ������ ���� ������ log ���
        int anchorCount = DataManager.CountAnchorData();
        if (anchorCount == 0)
        {
            Debug.Log("����� �����Ͱ� �����ϴ�.");
            return;
        }

        for (int i = 0; i < anchorCount; i++)
        {
            // Ŭ���� ��Ŀ ID���� Ŭ���� ��Ŀ�� ��ȯ
            ARCloudAnchor arCloudAnchor = ARAnchorManager.ResolveCloudAnchorId(DataManager.GetAnchorID(i));
            Instantiate(CubePrefab, arCloudAnchor.transform);
            // Ŭ���� ��Ŀ�� �����ϴ� ��ġ�� ť�긦 ����
            Debug.Log("CloudAnchorID�� ������� ť�긦 �����߽��ϴ�.");
        }
    }
}
