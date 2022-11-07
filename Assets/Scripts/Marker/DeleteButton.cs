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
    /// 생성한 클라우드 앵커와 Marker를 삭제하는 메서드
    /// </summary>
    public void Click()
    {
        // 호스팅 중이라면 return
        if (_hostButton.CloudAnchorHosting)
        {
            Debug.Log("호스팅 중입니다. 잠시만 기다려주세요.");
            return;
        }

        // 선택한 Marker에 클라우드 앵커가 존재한다면 삭제
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
