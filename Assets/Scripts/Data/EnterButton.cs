using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnterButton : MonoBehaviour
{
    public TextMeshProUGUI FileNameChangeText;
    public DataManager DataManager;
    public GameObject ChangePanel;

    public void Click()
    {
        DataManager.AnchorDataFileName = FileNameChangeText.text + ".json";
        Debug.Log($"현재 파일 이름 : {DataManager.AnchorDataFileName}");

        ChangePanel.SetActive(false);
    }
}
