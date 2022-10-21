using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteButton : MonoBehaviour
{
    public Marker Marker;
    public CreateButton CreateButton;

    private PlayerInput _playerInput;
    private DataManager _dataManager;

    private void Awake()
    {
        _dataManager = GameObject.Find("@GameManager").GetComponent<DataManager>();
        _playerInput = GameObject.Find("AR Camera").GetComponent<PlayerInput>();
    }

    public void Click()
    {
        if (CreateButton.CloudAnchorHosting)
        {
            Debug.Log("호스팅 중입니다. 잠시만 기다려주세요.");
            return;
        }

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
