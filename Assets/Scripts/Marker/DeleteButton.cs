using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    public Marker Marker;

    public void Click()
    {
        Marker.ARCloudAnchor?.OnDestroy();

        MarkerCount.Count--;

        Destroy(Marker.gameObject);
    }
}
