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
            Debug.Log("클라우드 앵커 이미 있는데?");
            return;
        }

        Marker.ARCloudAnchor = ARAnchorManagerExtensions.HostCloudAnchor(_arAnchorManager, Marker.ARAnchor);

        if (Marker.ARCloudAnchor)
        {
            if (Marker.ARCloudAnchor.cloudAnchorState == CloudAnchorState.Success)
            {
                Debug.Log("클라우드 성공적으로 호스팅 했다!");
                Panel.SetActive(false);
                MarkerCount.Count--;
            }
            else
            {
                Debug.Log("클라우드 앵커 왠진 모르겠는데 호스팅 안됨");
            }
        }
    }
}
