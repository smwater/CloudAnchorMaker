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
    /// Marker ���� �����ϴ� Text ������ �������ִ� �޼���
    /// </summary>
    public void ChangeName()
    {
        _name.text = Marker.Name;
    }
}
