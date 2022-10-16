using Google.XR.ARCoreExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CloudUpload : MonoBehaviour
{
    public ARAnchorManager ARAnchorManager;
    public Cube Cube;
    public GameObject Prefab;

    private ARCloudAnchor _arCloudAnchor;
    private ARAnchor _arAnchor;

    private void Awake()
    {
        _arAnchor = Cube.ARAnchor;
    }

    public void Click()
    {
        _arCloudAnchor = ARAnchorManagerExtensions.HostCloudAnchor(ARAnchorManager, _arAnchor);

        if (_arCloudAnchor)
        {
            if (_arCloudAnchor.cloudAnchorState == CloudAnchorState.Success)
            {
                Instantiate(Prefab, _arCloudAnchor.transform);
                _arCloudAnchor = null;
            }
        }
    }
}
