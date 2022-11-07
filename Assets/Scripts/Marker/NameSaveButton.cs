using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NameSaveButton : MonoBehaviour
{
    [SerializeField] private Marker _marker;
    [SerializeField] private TextMeshProUGUI _changeNameText;
    [SerializeField] private MarkerName _markerName;
    [SerializeField] private GameObject _buttonCanvas;
    [SerializeField] private GameObject _nameCanvas;

    /// <summary>
    /// 변경한 Marker Name을 저장하는 메서드
    /// </summary>
    public void Click()
    {
        _marker.Name = _changeNameText.text;
        _markerName.ChangeName();

        _buttonCanvas.SetActive(true);
        _nameCanvas.SetActive(false);
    }
}
