using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    public Marker Marker;
    public HostButton HostButton;

    private PlayerInput _playerInput;
    private DataManager _dataManager;

    private void Awake()
    {
        _dataManager = GameObject.Find("@GameManager").GetComponent<DataManager>();
        _playerInput = GameObject.Find("AR Camera").GetComponent<PlayerInput>();
    }

    /// <summary>
    /// ������ Ŭ���� ��Ŀ�� Marker�� �����ϴ� �޼���
    /// </summary>
    public void Click()
    {
        // ȣ���� ���̶�� return
        if (HostButton.CloudAnchorHosting)
        {
            Debug.Log("ȣ���� ���Դϴ�. ��ø� ��ٷ��ּ���.");
            return;
        }

        // ������ Marker�� Ŭ���� ��Ŀ�� �����Ѵٸ� ����
        if (Marker.ARCloudAnchor != null)
        {
            _dataManager.DeleteAnchorData(Marker.Index);
            Marker.ARCloudAnchor.OnDestroy();
            Marker.CloudAnchorID = null;
        }

        _playerInput.FreeIndex(Marker.Index);
        _playerInput.ModeSetting(Mode.MarkerPlacement);

        Destroy(Marker.gameObject);
    }
}
