using Google.XR.ARCoreExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public static class MarkerCount
{
    public static int Count = 0;
}

public class Marker : MonoBehaviour
{
    public GameObject Panel;
    public ARAnchor ARAnchor { get; private set; }
    [HideInInspector] public ARCloudAnchor ARCloudAnchor;

    private ARAnchorManager _arAnchorManager;

    private void Awake()
    {
        ARAnchor = gameObject.AddComponent<ARAnchor>();
        _arAnchorManager = GameObject.Find("Manager").GetComponent<ARAnchorManager>();
    }

    public void Click()
    {
        Panel.SetActive(true);
        MarkerCount.Count++;
    }

    public void CreateAnchor(ARRaycastHit arHit)
    {
        if (arHit.trackable is ARPlane arPlane)
        {
            ARAnchor = _arAnchorManager.AttachAnchor(arPlane, arHit.pose);
        }
    }
}
