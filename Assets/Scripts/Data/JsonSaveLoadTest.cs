using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using TMPro;

// List�� ����ȭ�ϱ� ���� class
[Serializable]
public class Serialization<T>
{
    [SerializeField] private List<T> _data;
    [SerializeField] private int _dataCount;

    /// <summary>
    /// �ҷ��� ������ �ٽ� List<T> Ÿ������ ��ȯ�� ��ȯ�ϴ� �޼���
    /// </summary>
    /// <returns>List<T>�� data</returns>
    public List<T> ToList()
    { 
        return _data; 
    }

    /// <summary>
    /// ����� �������� ������ ��ȯ�ϴ� �޼���
    /// </summary>
    /// <returns>�������� ����</returns>
    public int CheckCount()
    {
        return _dataCount;
    }

    /// <summary>
    /// List�� ����ȭ ���ִ� ������
    /// </summary>
    /// <param name="data">list�� ����� ������</param>
    /// <param name="count">list�� ����� �������� ����</param>
    public Serialization(List<T> data, int count)
    {
        _data = data;
        _dataCount = count;
    }
}

// ������ ������ class
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
    /// Data List�� �����͸� �߰��ϴ� �޼���
    /// </summary>
    public void Add()
    {
        Data data = new Data(_index, "Test" + _index.ToString());

        _saveDatas.Add(data);
        ChangeText(_saveDatas[_index].Log);

        _index++;
    }

    /// <summary>
    /// Text UI�� ������ �������ִ� �޼���
    /// </summary>
    /// <param name="text">������ ����</param>
    private void ChangeText(string text)
    {
        TestText.text = text;
    }

    /// <summary>
    /// Data List�� ���Ϸ� �����ϴ� �޼���
    /// </summary>
    public void Save()
    {
        // List�� ������ ������ ���ٸ� return
        if (_saveDatas.Count == 0)
        {
            Debug.Log("������ ������ �����ϴ�.");
            
            return;
        }

        // List�� ����ȭ�ϴ� �����ڸ� �̿��� Json���� ��ȯ
        string toJson = JsonUtility.ToJson(new Serialization<Data>(_saveDatas, _saveDatas.Count));

        // �ش� ��ġ�� ������ ���ٸ� ����
        if (!Directory.Exists(Application.persistentDataPath + "/DataFile"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/DataFile");
        }

        // ������ ������ ��� ����
        string filePath = Path.Combine(Application.persistentDataPath, "DataFile/SaveData.Json");

        // �ش� ��ο� ������ ����
        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        byte[] byteData = Encoding.UTF8.GetBytes(toJson);
        fileStream.Write(byteData, 0, byteData.Length);
        fileStream.Close();

        Debug.Log("���������� ���̺��߽��ϴ�.");
    }

    /// <summary>
    /// ����� ������ �ҷ��� Data List�� ����� �޼���
    /// </summary>
    public void Load()
    {
        List<Data> loadDatas;
        // ������ �ҷ��� ��� ����
        string filePath = Path.Combine(Application.persistentDataPath, "DataFile/SaveData.Json");

        // �ش� ��ο� ������ �������� �ʴ´ٸ� log ���
        if (!File.Exists(filePath))
        {
            Debug.Log("����� ������ ã�� �� �����ϴ�.");
        }
        else
        {
            // �ش� ��ο��� ������ �ҷ���
            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            byte[] byteData = new byte[fileStream.Length];
            fileStream.Read(byteData, 0, byteData.Length);
            fileStream.Close();

            // �ҷ��� Json ������ String���� ��ȯ
            string fromJson = Encoding.UTF8.GetString(byteData);

            // String data�� List Ÿ���� data�� ��ȯ
            Serialization<Data> serializationData = JsonUtility.FromJson<Serialization<Data>>(fromJson);
            loadDatas = serializationData.ToList();

            // data�� ������ ���� �����Ͱ� ���ٸ� log ���
            int dataCount = serializationData.CheckCount();
            if (dataCount == 0)
            {
                Debug.Log("����� ������ �����ϴ�.");
                return;
            }

            ChangeText(loadDatas[dataCount - 1].Log);
        }
    }
}
