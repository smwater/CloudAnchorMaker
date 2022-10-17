using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    public Marker Marker;

    public void Click()
    {
        if (Marker.ARCloudAnchor == null)
        {
            Debug.Log("클라우드 앵커 없는디?");
            return;
        }

        Marker.ARCloudAnchor.OnDestroy();

        MarkerCount.Count--;

        Destroy(Marker.gameObject);
    }
}
