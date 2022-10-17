using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    public Marker Marker;

    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GameObject.Find("AR Camera").GetComponent<PlayerInput>();
    }

    public void Click()
    {
        if (Marker.ARCloudAnchor != null)
        {
            Marker.ARCloudAnchor.OnDestroy();
            Marker.CloudAnchorID = null;
        }

        MarkerCount.Count--;
        _playerInput.DecreaseMarkerNowCount();

        Destroy(Marker.gameObject);
    }
}
