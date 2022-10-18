using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using Google.XR.ARCoreExtensions;

public class CreateCubeUseIDButton : MonoBehaviour
{
    public PlayerInput PlayerInput;
    public GameObject CubePrefab;
    public ARAnchorManager arAnchorManager;

    public void Click()
    {
        string cloudAnchorId = PlayerInput.Markers[0].GetComponent<Marker>().CloudAnchorID;

        ARCloudAnchor arCloudAnchor = ARAnchorManagerExtensions.ResolveCloudAnchorId(arAnchorManager, cloudAnchorId);

        DataManager.Instance.SaveAnchorData();
        Instantiate(CubePrefab, arCloudAnchor.gameObject.transform);
    }
}
