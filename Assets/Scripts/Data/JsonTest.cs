using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using TMPro;

[Serializable]
public class Serialization<T>
{
    [SerializeField]
    List<T> data;
    public List<T> ToList() { return data; }

    public Serialization(List<T> target)
    {
        this.data = target;
    }
}


[Serializable]
public class Data
{
    public int Index;
    public string Log;

    public Data(int index, string log)
    {
        Index = index;
        Log = log;
    }
}

public class JsonTest : MonoBehaviour
{
    public TextMeshProUGUI TestText;

    private int _index = 0;
    private string _toJson;

    private List<Data> _saveDatas = new List<Data>();

    public void Add()
    {
        Data data = new Data(_index, "Test" + _index.ToString());

        _saveDatas.Add(data);
        ChangeText();

        _index++;
    }

    private void ChangeText()
    {
        TestText.text = _saveDatas[_index].Log;
    }

    public void Save()
    {
        _toJson = JsonUtility.ToJson(new Serialization<Data>(_saveDatas));

        if (!Directory.Exists(Application.persistentDataPath + "/DataFile"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/DataFile");
        }

        string filePath = Application.persistentDataPath + "/DataFile/SaveData.Json";

        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        byte[] byteData = Encoding.UTF8.GetBytes(_toJson);
        fileStream.Write(byteData, 0, byteData.Length);
        fileStream.Close();
    }

    public void Load()
    {

    }
}
