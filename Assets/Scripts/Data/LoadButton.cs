using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadButton : MonoBehaviour
{
    public DataManager DataManager;

    public void Click()
    {
        DataManager.LoadAnchorData();
    }
}
