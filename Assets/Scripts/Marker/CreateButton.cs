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
    private bool _cloudAnchorHosting = false;

    private void Awake()
    {
        _arAnchorManager = GameObject.Find("@GameManager").GetComponent<ARAnchorManager>();
    }

    private void Update()
    {
        if (!_cloudAnchorHosting)
        {
            return;
        }

        if (Marker.ARCloudAnchor.cloudAnchorState == CloudAnchorState.TaskInProgress)
        {
            return;
        }
        if (Marker.ARCloudAnchor.cloudAnchorState == CloudAnchorState.Success)
        {
            Debug.Log("Ŭ���� ���������� ȣ���� �ߴ�!");
            Marker.CloudAnchorID = Marker.ARCloudAnchor.cloudAnchorId;
            _cloudAnchorHosting = false;

            Panel.SetActive(false);
            MarkerCount.Count--;

            DataManager.Instance.InitAnchorData(0, "Test", Marker.CloudAnchorID);
        }
        else
        {
            Debug.Log($"{Marker.ARCloudAnchor.cloudAnchorState}");
        }
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

        _cloudAnchorHosting = true;
    }
}
