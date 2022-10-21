using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameSaveButton : MonoBehaviour
{
    public Marker Marker;
    public TextMeshProUGUI ChangeNameText;
    public GameObject ButtonCanvas;
    public GameObject NameCanvas;

    public void Click()
    {
        Marker.Name = ChangeNameText.text;

        ButtonCanvas.SetActive(true);
        NameCanvas.SetActive(false);
    }
}
