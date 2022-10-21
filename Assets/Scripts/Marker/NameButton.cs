using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameButton : MonoBehaviour
{
    public GameObject NameCanvas;
    public GameObject ButtonCanvas;

    public void Click()
    {
        NameCanvas.SetActive(true);
        ButtonCanvas.SetActive(false);
    }
}
