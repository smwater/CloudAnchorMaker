using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateButton : MonoBehaviour
{
    private Marker Marker;
    public GameObject Panel;

    public void Click()
    {
        if (Marker.ARAnchor == null)
        {
            return;
        }

        //_arCloudAnchor = ARAnchorManagerExtensions.HostCloudAnchor(ARAnchorManager, _arAnchor);

        //if (_arCloudAnchor)
        //{
        //    if (_arCloudAnchor.cloudAnchorState == CloudAnchorState.Success)
        //    {
        //        Instantiate(Prefab, _arCloudAnchor.transform);
        //        _arCloudAnchor = null;
        //    }
        //}

        Panel.SetActive(false);
    }
}
