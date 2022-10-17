using Google.XR.ARCoreExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CreateButton : MonoBehaviour
{
    public Marker Marker;
    public GameObject Panel;

    private ARAnchorManager _arAnchorManager;

    private void Awake()
    {
        _arAnchorManager = GameObject.Find("Manager").GetComponent<ARAnchorManager>();
    }

    public void Click()
    {
        if (Marker.ARAnchor == null)
        {
            return;
        }

        if (Marker.ARCloudAnchor != null)
        {
            Debug.Log("Ŭ���� ��Ŀ �̹� �ִµ�?");
            return;
        }

        Marker.ARCloudAnchor = ARAnchorManagerExtensions.HostCloudAnchor(_arAnchorManager, Marker.ARAnchor);

        if (Marker.ARCloudAnchor)
        {
            if (Marker.ARCloudAnchor.cloudAnchorState == CloudAnchorState.Success)
            {
                Debug.Log("Ŭ���� ���������� ȣ���� �ߴ�!");
                Panel.SetActive(false);
                MarkerCount.Count--;
            }
            else
            {
                Debug.Log("Ŭ���� ��Ŀ ���� �𸣰ڴµ� ȣ���� �ȵ�");
            }
        }
    }
}
