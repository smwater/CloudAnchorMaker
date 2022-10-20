using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[Serializable]
public class SerializationData<T>
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

    public SerializationData(List<T> data, int count)
    {
        _data = data;
        _dataCount = count;
    }
}

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
    private List<AnchorData> _anchorDatas = new List<AnchorData>();

    [HideInInspector] public string DataFileName = "DataFile";
    [HideInInspector] public string AnchorDataFileName = "AnchorData.json";

    public bool AddAnchorData(int index, string name, string ID)
    {
        if (_anchorDatas[index] != null)
        {
            return false;
        }

        AnchorData createdAnchor = new AnchorData(index, name, ID);
        _anchorDatas.Add(createdAnchor);

        return true;
    }

    public bool DeleteAnchorData(int index)
    {
        if (_anchorDatas[index] == null)
        {
            return false;
        }

        AnchorData deletedAnchor = _anchorDatas[index];
        _anchorDatas.Remove(deletedAnchor);

        return true;
    }

    public void SaveAnchorData()
    {
        if (_anchorDatas.Count == 0)
        {
            Debug.Log("저장할 앵커가 없습니다.");
            return;
        }

        string toJson = JsonUtility.ToJson(new SerializationData<AnchorData>(_anchorDatas, _anchorDatas.Count));

        if (!Directory.Exists(Application.persistentDataPath + DataFileName))
        {
            Directory.CreateDirectory(Application.persistentDataPath + DataFileName);
        }

        string filePath = Path.Combine(Application.persistentDataPath, DataFileName, AnchorDataFileName);

        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        byte[] byteData = Encoding.UTF8.GetBytes(toJson);
        fileStream.Write(byteData, 0, byteData.Length);
        fileStream.Close();
    }

    public void LoadAnchorData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, DataFileName, AnchorDataFileName);

        if (!File.Exists(filePath))
        {
            Debug.Log("해당 파일을 찾을 수 없습니다.");
        }
        else
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            byte[] byteData = new byte[fileStream.Length];
            fileStream.Read(byteData, 0, byteData.Length);
            fileStream.Close();
            string fromJson = Encoding.UTF8.GetString(byteData);

            SerializationData<AnchorData> serializationData = JsonUtility.FromJson<SerializationData<AnchorData>>(fromJson);
            _anchorDatas = serializationData.ToList();
            int dataCount = serializationData.CheckCount();

            if (dataCount == 0)
            {
                Debug.Log("저장된 내용이 없습니다.");
                return;
            }
        }
    }
}
