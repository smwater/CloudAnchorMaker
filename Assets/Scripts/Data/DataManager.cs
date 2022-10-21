using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

// List�� ����ȭ�ϱ� ���� class
[Serializable]
public class SerializationData<T>
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
    public SerializationData(List<T> data, int count)
    {
        _data = data;
        _dataCount = count;
    }
}

// ������ ������ class
[Serializable]
public class AnchorData
{
    public string AnchorName;
    public string AnchorID;

    public AnchorData(string anchorName, string anchorID)
    {
        AnchorName = anchorName;
        AnchorID = anchorID;
    }
}

public class DataManager : MonoBehaviour
{
    private List<AnchorData> _anchorDatas = new List<AnchorData>();

    [HideInInspector] public string DataFileName = "DataFile";
    [HideInInspector] public string AnchorDataFileName = "AnchorData.json";

    /// <summary>
    /// Anchor List�� data�� �� �� �ִ��� ��ȯ�ϴ� �޼���
    /// </summary>
    /// <returns>Anchor data�� ����</returns>
    public int CountAnchorData()
    {
        return _anchorDatas.Count;
    }

    /// <summary>
    /// Ư�� ��Ŀ�� ID�� ��ȯ�ϴ� �޼���
    /// </summary>
    /// <param name="index">ID�� ��ȯ�ϰ� ���� Anchor�� index</param>
    /// <returns>Anchor�� ID</returns>
    public string GetAnchorID(int index)
    {
        return _anchorDatas[index].AnchorID;
    }

    /// <summary>
    /// Anchor List�� Anchor�� �߰��ϴ� �޼���
    /// </summary>
    /// <param name="index">�߰��� Anchor�� index</param>
    /// <param name="name">�߰��� Anchor�� �̸�</param>
    /// <param name="ID">�߰��� Anchor�� ID</param>
    /// <returns>�߰��� �����ߴٸ� true, �ƴϸ� false</returns>
    public bool AddAnchorData(int index, string name, string ID)
    {
        // �ش� index�� Anchor�� �����Ѵٸ� false ��ȯ
        //if (AnchorDatas[index] != null)
        //{
        //    Debug.Log("��Ŀ�� �̹� �����");
        //    return false;
        //}

        // list�� ���ο� Anchor�� �߰��Ѵ�.
        AnchorData createdAnchor = new AnchorData(name, ID);
        _anchorDatas.Add(createdAnchor);

        return true;
    }

    /// <summary>
    /// Anchor list���� Anchor�� �����ϴ� �޼���
    /// </summary>
    /// <param name="index">������ Anchor�� index</param>
    /// <returns>������ �����ߴٸ� true, �ƴϸ� false</returns>
    public bool DeleteAnchorData(int index)
    {
        // �ش� index�� Anchor�� �������� �ʴ´ٸ� false ��ȯ
        if (_anchorDatas[index] == null)
        {
            return false;
        }

        // list���� Anchor�� �����Ѵ�.
        AnchorData deletedAnchor = _anchorDatas[index];
        _anchorDatas.Remove(deletedAnchor);

        return true;
    }

    /// <summary>
    /// Anchor List�� ���Ϸ� �����ϴ� �޼���
    /// </summary>
    public void SaveAnchorData()
    {
        // Anchor List�� ����� ���� ���ٸ� return
        if (_anchorDatas.Count == 0)
        {
            Debug.Log("������ ��Ŀ�� �����ϴ�.");
            return;
        }

        // List�� ����ȭ�ϴ� �����ڸ� �̿��� Json���� ��ȯ
        string toJson = JsonUtility.ToJson(new SerializationData<AnchorData>(_anchorDatas, _anchorDatas.Count));

        // �ش� ��ġ�� ������ ���ٸ� ����
        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, DataFileName)))
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, DataFileName));
        }

        // ������ ������ ��� ����
        string filePath = Path.Combine(Application.persistentDataPath, DataFileName, AnchorDataFileName);

        // �ش� ��ο� ������ ����
        FileStream fileStream = new FileStream(filePath, FileMode.Create);
        byte[] byteData = Encoding.UTF8.GetBytes(toJson);
        fileStream.Write(byteData, 0, byteData.Length);
        fileStream.Close();

        Debug.Log("���������� �����߽��ϴ�.");
    }

    /// <summary>
    /// ����� ������ �ҷ��� Anchor List�� ����� �޼���
    /// </summary>
    public void LoadAnchorData()
    {
        // ������ �ҷ��� ��� ����
        string filePath = Path.Combine(Application.persistentDataPath, DataFileName, AnchorDataFileName);

        // �ش� ��ο� ������ �������� �ʴ´ٸ� log ���
        if (!File.Exists(filePath))
        {
            Debug.Log("�ش� ������ ã�� �� �����ϴ�.");
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
            SerializationData<AnchorData> serializationData = JsonUtility.FromJson<SerializationData<AnchorData>>(fromJson);
            _anchorDatas = serializationData.ToList();

            Debug.Log("���������� �ε��߽��ϴ�.");

            // data�� ������ ���� �����Ͱ� ���ٸ� log ���
            int dataCount = serializationData.CheckCount();
            if (dataCount == 0)
            {
                Debug.Log("����� ������ �����ϴ�.");
                return;
            }
        }
    }
}
