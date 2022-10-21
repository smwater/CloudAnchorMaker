using Google.XR.ARCoreExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Marker : MonoBehaviour
{
    public GameObject ButtonCanvas;
    public ARAnchor ARAnchor { get; private set; }
    [HideInInspector] public ARCloudAnchor ARCloudAnchor;

    [HideInInspector] public int Index;
    [HideInInspector] public string Name = "Default";
    [HideInInspector] public string CloudAnchorID;

    private ARAnchorManager _arAnchorManager;

    private void Awake()
    {
        ARAnchor = gameObject.AddComponent<ARAnchor>();
        _arAnchorManager = GameObject.Find("@GameManager").GetComponent<ARAnchorManager>();
    }

    public void Click()
    {
        ButtonCanvas.SetActive(true);
    }

    public void CreateAnchor(ARRaycastHit arHit)
    {
        if (arHit.trackable is ARPlane arPlane)
        {
            ARAnchor = _arAnchorManager.AttachAnchor(arPlane, arHit.pose);
        }
    }
}
