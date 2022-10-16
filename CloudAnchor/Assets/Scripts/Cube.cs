using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Cube : MonoBehaviour
{
    public ARAnchor ARAnchor { get; private set; }

    private void Awake()
    {
        ARAnchor = gameObject.AddComponent<ARAnchor>();
    }
}
