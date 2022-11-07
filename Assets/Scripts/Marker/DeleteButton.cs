using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    [SerializeField] private Marker _marker;
    [SerializeField] private HostButton _hostButton;
    private PlaceMarker _playerInput;
    private DataManager _dataManager;

    private void Awake()
    {
        _dataManager = GameObject.Find("@GameManager").GetComponent<DataManager>();
        _playerInput = GameObject.Find("AR Camera").GetComponent<PlaceMarker>();
    }

    /// <summary>
    /// ������ Ŭ���� ��Ŀ�� Marker�� �����ϴ� �޼���
    /// </summary>
    public void Click()
    {
        // ȣ���� ���̶�� return
        if (_hostButton.CloudAnchorHosting)
        {
            Debug.Log("ȣ���� ���Դϴ�. ��ø� ��ٷ��ּ���.");
            return;
        }

        // ������ Marker�� Ŭ���� ��Ŀ�� �����Ѵٸ� ����
        if (_marker.ARCloudAnchor != null)
        {
            _dataManager.DeleteAnchorData(_marker.Index);
            _marker.ARCloudAnchor.OnDestroy();
            _marker.CloudAnchorID = null;
        }

        _playerInput.FreeIndex(_marker.Index);
        _playerInput.ModeSetting(Mode.MarkerPlacement);

        Destroy(_marker.gameObject);
    }
}
