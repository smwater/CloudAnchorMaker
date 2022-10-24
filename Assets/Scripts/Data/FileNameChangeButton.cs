using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileNameChangeButton : MonoBehaviour
{
    public GameObject ChangePanel;

    public void Click()
    {
        ChangePanel.SetActive(true);
    }
}
