using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    public GameObject Marker;

    public void Click()
    {
        Destroy(Marker);
    }
}
