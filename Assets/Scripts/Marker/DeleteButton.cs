using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    public Marker Marker;
    public HostButton CreateButton;

    private PlayerInput _playerInput;
    private DataManager _dataManager;

    private void Awake()
    {
        _dataManager = GameObject.Find("@GameManager").GetComponent<DataManager>();
        _playerInput = GameObject.Find("AR Camera").GetComponent<PlayerInput>();
    }

    /// <summary>
    /// 생성한 클라우드 앵커와 Marker를 삭제하는 메서드
    /// </summary>
    public void Click()
    {
        // 호스팅 중이라면 return
        if (CreateButton.CloudAnchorHosting)
        {
            Debug.Log("호스팅 중입니다. 잠시만 기다려주세요.");
            return;
        }

        // 선택한 Marker에 클라우드 앵커가 존재한다면 삭제
        if (Marker.ARCloudAnchor != null)
        {
            _dataManager.DeleteAnchorData(Marker.Index);
            Marker.ARCloudAnchor.OnDestroy();
            Marker.CloudAnchorID = null;
        }

        _playerInput.DecreaseMarkerNowCount();
        _playerInput.ModeSetting(Mode.MarkerPlacement);

        Destroy(Marker.gameObject);
    }
}
