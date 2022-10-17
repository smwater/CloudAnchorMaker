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
        _arAnchorManager = GameObject.Find("Manager").GetComponent<ARAnchorManager>();
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
            Debug.Log("클라우드 성공적으로 호스팅 했다!");
            Panel.SetActive(false);
            MarkerCount.Count--;
            _cloudAnchorHosting = false;
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
            Debug.Log("클라우드 앵커 이미 있는데?");
            return;
        }

        Marker.ARCloudAnchor = ARAnchorManagerExtensions.HostCloudAnchor(_arAnchorManager, Marker.ARAnchor);

        _cloudAnchorHosting = true;
    }
}
