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
    [SerializeField] private List<T> _data;
    [SerializeField] private int _dataCount;

    public List<T> ToList()
    { 
        return _data; 
    }

    public int CheckCount()
    {
        return _dataCount;
    }

    public Serialization(List<T> data, int count)
    {
        _data = data;
        _dataCount = count;
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

public class JsonSaveLoadTest : MonoBehaviour
{
    public TextMeshProUGUI TestText;

    private int _index = 0;

    private List<Data> _saveDatas = new List<Data>();

    public void Add()
    {
        Data data = new Data(_index, "Test" + _index.ToString());

        _saveDatas.Add(data);
        ChangeText(_saveDatas[_index].Log);

        _index++;
    }

    private void ChangeText(string text)
    {
        TestText.text = text;
    }

    public void Save()
    {
        if (_saveDatas.Count == 0)
        {
            Debug.Log("저장할 내용이 없습니다.");
            
            return;
        }

        string toJson = JsonUtility.ToJson(new Serialization<Data>(_saveDatas, _saveDatas.Count));

        if (!Directory.Exists(Application.persistentDataPath + "/DataFile"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/DataFile");
        }

        string filePath = Path.Combine(Application.persistentDataPath, "DataFile/SaveData.Json");

        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        byte[] byteData = Encoding.UTF8.GetBytes(toJson);
        fileStream.Write(byteData, 0, byteData.Length);
        fileStream.Close();
    }

    public void Load()
    {
        List<Data> loadDatas;
        string filePath = Path.Combine(Application.persistentDataPath, "DataFile/SaveData.Json");

        if (!File.Exists(filePath))
        {
            Debug.Log("저장된 파일을 찾을 수 없습니다.");
        }
        else
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            byte[] byteData = new byte[fileStream.Length];
            fileStream.Read(byteData, 0, byteData.Length);
            fileStream.Close();
            string fromJson = Encoding.UTF8.GetString(byteData);

            Serialization<Data> serializationData = JsonUtility.FromJson<Serialization<Data>>(fromJson);
            loadDatas = serializationData.ToList();
            int dataCount = serializationData.CheckCount();

            if (dataCount == 0)
            {
                Debug.Log("저장된 내용이 없습니다.");
                return;
            }

            ChangeText(loadDatas[dataCount - 1].Log);
        }
    }
}
