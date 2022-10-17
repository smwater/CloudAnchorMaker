using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Marker : MonoBehaviour
{
    public GameObject Panel;
    public ARAnchor ARAnchor { get; private set; }

    private void Awake()
    {
        ARAnchor = gameObject.AddComponent<ARAnchor>();
    }

    public void Click()
    {
        Panel.SetActive(true);
    }
}
