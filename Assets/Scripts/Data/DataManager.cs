using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class AnchorData
{
    public int Index;
    public string AnchorName;
    public string AnchorID;

    public AnchorData(int index, string anchorName, string anchorID)
    {
        Index = index;
        AnchorName = anchorName;
        AnchorID = anchorID;
    }
}

public class DataManager : MonoBehaviour
{
    private static GameObject _container;
    private static DataManager _instance;
    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _container = new GameObject();
                _container.name = "DataManager";
                _instance = _container.AddComponent<DataManager>();

                DontDestroyOnLoad(_container);
            }

            return _instance;
        }
    }

    [HideInInspector] public string AnchorDataFileName = "AnchorData.json";

    private AnchorData _anchorDatas;
    public AnchorData AnchorData
    {
        get
        {
            if (_anchorDatas == null)
            {
                LoadAnchorData();
                SaveAnchorData();
            }

            return _anchorDatas;
        }
    }

    public void InitAnchorData(int index, string name, string ID)
    {
        _anchorDatas = new AnchorData(index, name, ID);
    }

    public void SaveAnchorData()
    {
        InitAnchorData(-1, "Name", "ID");
        string toJsonData = JsonUtility.ToJson(_anchorDatas, true);
        string filePath = Application.persistentDataPath + AnchorDataFileName;
        File.WriteAllText(filePath, toJsonData);
    }

    public void LoadAnchorData()
    {
        string filePath = Application.persistentDataPath + AnchorDataFileName;

        if (File.Exists(filePath))
        {
            string fromJsonData = File.ReadAllText(filePath);
            _anchorDatas = JsonUtility.FromJson<AnchorData>(fromJsonData);

            if (_anchorDatas == null)
            {
                InitAnchorData(-1, "Name", "ID");
            }
        }
        else
        {
            InitAnchorData(-1, "Name", "ID");
        }
    }
}
