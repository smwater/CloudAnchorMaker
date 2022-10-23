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
            ARCloudAnchor arCloudAnchor = ARAnchorManagerExtensions.ResolveCloudAnchorId(arAnchorManager, DataManager.GetAnchorID(i));

            if (arCloudAnchor == null)
            {
                // Ŭ���� ��Ŀ�� ���ٸ� log ���
                Debug.Log("�ش� ID�� ��Ŀ�� ������ �� �����ϴ�.");
            }
            else
            {
                // Ŭ���� ��Ŀ�� �����ϴ� ��ġ�� ť�긦 ����
                Instantiate(CubePrefab, arCloudAnchor.transform);
                Debug.Log("CloudAnchorID�� ������� ť�긦 �����߽��ϴ�.");
            }
        }
    }
}
