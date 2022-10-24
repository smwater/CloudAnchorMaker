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
        Debug.Log($"���� ���� �̸� : {DataManager.AnchorDataFileName}");

        ChangePanel.SetActive(false);
    }
}
