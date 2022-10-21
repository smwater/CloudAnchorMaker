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
            Debug.Log("���������� ȣ���� �ߴ�!");
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
            Debug.Log("ȣ���� ���Դϴ�. ��ø� ��ٷ��ּ���.");
            return;
        }

        if (Marker.ARCloudAnchor != null)
        {
            Debug.Log("������ ȣ������ Ŭ���� ��Ŀ�Դϴ�.");
            ButtonCanvas.SetActive(false);
            _playerInput.ModeSetting(Mode.MarkerPlacement);
            return;
        }

        Marker.ARCloudAnchor = ARAnchorManagerExtensions.HostCloudAnchor(_arAnchorManager, Marker.ARAnchor);
        CloudAnchorHosting = true;
    }
}
