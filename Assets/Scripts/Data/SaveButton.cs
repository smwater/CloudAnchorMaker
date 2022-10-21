using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public DataManager DataManager;

    public void Click()
    {
        DataManager.SaveAnchorData();
    }
}
