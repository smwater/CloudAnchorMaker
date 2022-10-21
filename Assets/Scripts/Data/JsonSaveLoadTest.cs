using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using TMPro;

// List를 직렬화하기 위한 class
[Serializable]
public class Serialization<T>
{
    [SerializeField] private List<T> _data;
    [SerializeField] private int _dataCount;

    /// <summary>
    /// 불러온 파일을 다시 List<T> 타입으로 변환해 반환하는 메서드
    /// </summary>
    /// <returns>List<T>의 data</returns>
    public List<T> ToList()
    { 
        return _data; 
    }

    /// <summary>
    /// 저장된 데이터의 개수를 반환하는 메서드
    /// </summary>
    /// <returns>데이터의 개수</returns>
    public int CheckCount()
    {
        return _dataCount;
    }

    /// <summary>
    /// List를 직렬화 해주는 생성자
    /// </summary>
    /// <param name="data">list에 저장된 데이터</param>
    /// <param name="count">list에 저장된 데이터의 개수</param>
    public Serialization(List<T> data, int count)
    {
        _data = data;
        _dataCount = count;
    }
}

// 저장할 데이터 class
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

    /// <summary>
    /// Data List에 데이터를 추가하는 메서드
    /// </summary>
    public void Add()
    {
        Data data = new Data(_index, "Test" + _index.ToString());

        _saveDatas.Add(data);
        ChangeText(_saveDatas[_index].Log);

        _index++;
    }

    /// <summary>
    /// Text UI의 내용을 변경해주는 메서드
    /// </summary>
    /// <param name="text">변경할 내용</param>
    private void ChangeText(string text)
    {
        TestText.text = text;
    }

    /// <summary>
    /// Data List를 파일로 저장하는 메서드
    /// </summary>
    public void Save()
    {
        // List에 저장할 내용이 없다면 return
        if (_saveDatas.Count == 0)
        {
            Debug.Log("저장할 내용이 없습니다.");
            
            return;
        }

        // List를 직렬화하는 생성자를 이용해 Json으로 변환
        string toJson = JsonUtility.ToJson(new Serialization<Data>(_saveDatas, _saveDatas.Count));

        // 해당 위치에 폴더가 없다면 생성
        if (!Directory.Exists(Application.persistentDataPath + "/DataFile"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/DataFile");
        }

        // 파일을 저장할 경로 생성
        string filePath = Path.Combine(Application.persistentDataPath, "DataFile/SaveData.Json");

        // 해당 경로에 파일을 저장
        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        byte[] byteData = Encoding.UTF8.GetBytes(toJson);
        fileStream.Write(byteData, 0, byteData.Length);
        fileStream.Close();

        Debug.Log("성공적으로 세이브했습니다.");
    }

    /// <summary>
    /// 저장된 파일을 불러와 Data List로 만드는 메서드
    /// </summary>
    public void Load()
    {
        List<Data> loadDatas;
        // 파일을 불러올 경로 생성
        string filePath = Path.Combine(Application.persistentDataPath, "DataFile/SaveData.Json");

        // 해당 경로에 파일이 존재하지 않는다면 log 출력
        if (!File.Exists(filePath))
        {
            Debug.Log("저장된 파일을 찾을 수 없습니다.");
        }
        else
        {
            // 해당 경로에서 파일을 불러옴
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            byte[] byteData = new byte[fileStream.Length];
            fileStream.Read(byteData, 0, byteData.Length);
            fileStream.Close();

            // 불러온 Json 파일을 String으로 변환
            string fromJson = Encoding.UTF8.GetString(byteData);

            // String data를 List 타입의 data로 변환
            Serialization<Data> serializationData = JsonUtility.FromJson<Serialization<Data>>(fromJson);
            loadDatas = serializationData.ToList();

            // data의 개수를 세고 데이터가 없다면 log 출력
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
