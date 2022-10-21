using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameSaveButton : MonoBehaviour
{
    public Marker Marker;
    public TextMeshProUGUI ChangeNameText;
    public MarkerName MarkerName;
    public GameObject ButtonCanvas;
    public GameObject NameCanvas;

    public void Click()
    {
        Marker.Name = ChangeNameText.text;
        MarkerName.ChangeName();

        ButtonCanvas.SetActive(true);
        NameCanvas.SetActive(false);
    }
}
