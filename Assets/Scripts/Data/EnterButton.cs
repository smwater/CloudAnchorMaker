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
        if (FileNameChangeText.text != null)
        {
            DataManager.DataFileName = FileNameChangeText.text;
        }

        ChangePanel.SetActive(false);
    }
}
