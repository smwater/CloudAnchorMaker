using Google.XR.ARCoreExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class HostButton : MonoBehaviour
{
    public bool CloudAnchorHosting { get; private set; }

    public Marker Marker;
    public GameObject ButtonCanvas;

    private ARAnchorManager _arAnchorManager;
    private DataManager _dataManager;
    private PlayerInput _playerInput;

    private void Awake()
    {
        CloudAnchorHosting = false;

        GameObject gameManager = GameObject.Find("@GameManager");

        _arAnchorManager = gameManager.GetComponent<ARAnchorManager>();
        _dataManager = gameManager.GetComponent<DataManager>();
        _playerInput = GameObject.Find("AR Camera").GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (!CloudAnchorHosting)
        {
            return;
        }

        if (Marker.ARCloudAnchor.cloudAnchorState == CloudAnchorState.TaskInProgress)
        {
            return;
        }
        if (Marker.ARCloudAnchor.cloudAnchorState == CloudAnchorState.Success)
        {
            Debug.Log("성공적으로 호스팅 했다!");
            Marker.CloudAnchorID = Marker.ARCloudAnchor.cloudAnchorId;
            CloudAnchorHosting = false;
            _dataManager.AddAnchorData(Marker.Index, Marker.Name, Marker.CloudAnchorID);

            ButtonCanvas.SetActive(false);
            _playerInput.ModeSetting(Mode.MarkerPlacement);
        }
        else
        {
            Debug.Log($"{Marker.ARCloudAnchor.cloudAnchorState}");
        }
    }

    public void Click()
    {
        if (Marker.ARAnchor == null)
        {
            return;
        }

        if (CloudAnchorHosting)
        {
            Debug.Log("호스팅 중입니다. 잠시만 기다려주세요.");
            return;
        }

        if (Marker.ARCloudAnchor != null)
        {
            Debug.Log("기존에 호스팅한 클라우드 앵커입니다.");
            ButtonCanvas.SetActive(false);
            _playerInput.ModeSetting(Mode.MarkerPlacement);
            return;
        }

        Marker.ARCloudAnchor = ARAnchorManagerExtensions.HostCloudAnchor(_arAnchorManager, Marker.ARAnchor);
        CloudAnchorHosting = true;
    }
}
