using Google.XR.ARCoreExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CreateButton : MonoBehaviour
{
    public Marker Marker;
    public GameObject Panel;

    private bool _cloudAnchorHosting = false;

    private ARAnchorManager _arAnchorManager;
    private DataManager _dataManager;
    private PlayerInput _playerInput;

    private void Awake()
    {
        GameObject gameManager = GameObject.Find("@GameManager");

        _arAnchorManager = gameManager.GetComponent<ARAnchorManager>();
        _dataManager = gameManager.GetComponent<DataManager>();
        _playerInput = GameObject.Find("AR Camera").GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (!_cloudAnchorHosting)
        {
            return;
        }

        if (Marker.ARCloudAnchor.cloudAnchorState == CloudAnchorState.TaskInProgress)
        {
            return;
        }
        if (Marker.ARCloudAnchor.cloudAnchorState == CloudAnchorState.Success)
        {
            Debug.Log("Ŭ���� ���������� ȣ���� �ߴ�!");
            Marker.CloudAnchorID = Marker.ARCloudAnchor.cloudAnchorId;
            _cloudAnchorHosting = false;
            _dataManager.AddAnchorData(Marker.Index, Marker.Name, Marker.CloudAnchorID);

            Panel.SetActive(false);
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

        if (Marker.ARCloudAnchor != null)
        {
            Debug.Log("Ŭ���� ��Ŀ �̹� �ִµ�?");
            Panel.SetActive(false);
            _playerInput.ModeSetting(Mode.MarkerPlacement);
            return;
        }

        Marker.ARCloudAnchor = ARAnchorManagerExtensions.HostCloudAnchor(_arAnchorManager, Marker.ARAnchor);
        _cloudAnchorHosting = true;
    }
}
