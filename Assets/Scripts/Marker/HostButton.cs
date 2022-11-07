using Google.XR.ARCoreExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class HostButton : MonoBehaviour
{
    public bool CloudAnchorHosting { get; private set; }

    [SerializeField] private Marker _marker;
    [SerializeField] private GameObject _buttonCanvas;
    private ARAnchorManager _arAnchorManager;
    private DataManager _dataManager;
    private PlaceMarker _playerInput;

    private void Awake()
    {
        CloudAnchorHosting = false;

        GameObject gameManager = GameObject.Find("@GameManager");

        _arAnchorManager = gameManager.GetComponent<ARAnchorManager>();
        _dataManager = gameManager.GetComponent<DataManager>();
        _playerInput = GameObject.Find("AR Camera").GetComponent<PlaceMarker>();
    }

    private void Update()
    {
        // 호스팅 중이 아니라면 return
        if (!CloudAnchorHosting)
        {
            return;
        }

        // 호스팅 중이라면 return
        if (_marker.ARCloudAnchor.cloudAnchorState == CloudAnchorState.TaskInProgress)
        {
            return;
        }
        // 호스팅에 성공하면
        if (_marker.ARCloudAnchor.cloudAnchorState == CloudAnchorState.Success)
        {
            Debug.Log("성공적으로 호스팅 했다!");
            _marker.CloudAnchorID = _marker.ARCloudAnchor.cloudAnchorId;
            CloudAnchorHosting = false;
            _dataManager.AddAnchorData(_marker.Name, _marker.CloudAnchorID);

            _buttonCanvas.SetActive(false);
            _playerInput.ModeSetting(Mode.MarkerPlacement);
        }
        else
        {
            Debug.Log($"{_marker.ARCloudAnchor.cloudAnchorState}");
        }
    }

    /// <summary>
    /// Marker에 부착한 로컬 앵커를 클라우드 앵커로 호스팅하는 메서드
    /// </summary>
    public void Click()
    {
        // 부착된 앵커가 없다면 return
        if (_marker.ARAnchor == null)
        {
            return;
        }

        // 호스팅 중이라면 return
        if (CloudAnchorHosting)
        {
            Debug.Log("호스팅 중입니다. 잠시만 기다려주세요.");
            return;
        }

        // 이미 호스팅한 앵커라면
        if (_marker.ARCloudAnchor != null)
        {
            Debug.Log("기존에 호스팅한 클라우드 앵커입니다.");
            _buttonCanvas.SetActive(false);
            _playerInput.ModeSetting(Mode.MarkerPlacement);
            return;
        }

        // 마커에 부착된 앵커를 클라우드 앵커로 호스팅
        _marker.ARCloudAnchor = _arAnchorManager.HostCloudAnchor(_marker.ARAnchor, 365);
        CloudAnchorHosting = true;
    }
}
