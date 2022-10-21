using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MarkerName : MonoBehaviour
{
    public Marker Marker;

    private TextMeshProUGUI _name;

    private void Awake()
    {
        _name = GetComponent<TextMeshProUGUI>();

        ChangeName();
    }

    /// <summary>
    /// Marker 위에 존재하는 Text 내용을 변경해주는 메서드
    /// </summary>
    public void ChangeName()
    {
        _name.text = Marker.Name;
    }
}
