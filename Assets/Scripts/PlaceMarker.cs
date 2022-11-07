using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// �÷��̾� �Է� ���
public enum Mode
{
    MarkerPlacement,
    MarkerSetting,
    None
}

public class PlaceMarker : MonoBehaviour
{
    [SerializeField] private GameObject _markerPrefab;
    private GameObject[] _markers;

    private Camera _camera;
    private ARRaycastManager _arRaycastManager;

    private int _markerMaxCount = 40;
    private int _markerIndex = 0;
    private int _markerUsedCount = 0;

    private Mode _currentMode = Mode.MarkerPlacement;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _arRaycastManager = GetComponent<ARRaycastManager>();
        _markers = new GameObject[_markerMaxCount];
    }

    // Update is called once per frame
    private void Update()
    {
        // �Է��� ������ return
        if (Input.touchCount == 0)
        {
            return;
        }

        // ���ÿ� ���� �հ������� ��ġ���� ���, ù��° ��ġ�� �ν�
        Touch touch = Input.GetTouch(0);

        // ù��° ��ġ�� ����, UI ���� �ν� ����
        if (EventSystem.current.IsPointerOverGameObject(0))
        {
            return;
        }

        // GetKeyDown�� ���
        if (touch.phase == TouchPhase.Began)
        {
            // Marker Setting Mode��� Marker�� AR Plane�� ���� �Է��� ���� ����
            if (_currentMode == Mode.MarkerSetting)
            {
                return;
            }

            Ray ray;
            ray = _camera.ScreenPointToRay(touch.position);

            // Raycast�� Marker�� ����
            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("Marker");
            if (Physics.Raycast(ray, out hit, 10f, layerMask))
            {
                // Marker�� �����ϸ� click()
                hit.transform.GetComponent<Marker>().Click();

                _currentMode = Mode.MarkerSetting;

                // return�� ������ �Ʒ� AR raycast�� marker�� �߰��� ������ �� ����
                return;
            }

            List<ARRaycastHit> arHits = new List<ARRaycastHit>();
            ARRaycastHit arHit;

            // AR raycast�� AR Plane�� ����
            if (_arRaycastManager.Raycast(ray, arHits, TrackableType.PlaneWithinPolygon | TrackableType.FeaturePoint))
            {
                // Marker�� ��� �����ߴٸ�, log ���
                if (_markerUsedCount >= _markerMaxCount)
                {
                    Debug.Log("���� ������ ��Ŀ�� �� �Ҹ��߽��ϴ�.");
                    return;
                }

                // ���� ��ü�� raycast�� �ν� �ƴٸ� ���� ������ ������ ���� ����
                arHit = arHits[0];

                // ������ ��ġ�� Marker�� �����ϰ�
                _markers[_markerIndex] = Instantiate(_markerPrefab, arHit.pose.position + new Vector3(0f, 0.2f), arHit.pose.rotation);
                // local Anchor�� ����
                _markers[_markerIndex].GetComponent<Marker>().CreateAnchor(arHit);
                // �� marker�� index�� �Ѱ���
                _markers[_markerIndex].GetComponent<Marker>().Index = _markerIndex;
                _markerIndex = (_markerIndex + 1) % _markerMaxCount;
                _markerUsedCount++;

                _currentMode = Mode.MarkerSetting;
            }
        }
    }

    /// <summary>
    /// ������ ��Ŀ���� index�� ���� ��Ŀ���� �������ִ� �޼���
    /// </summary>
    /// <param name="index">��� ������ Marker�� index</param>
    public void FreeIndex(int index)
    {
        _markerUsedCount--;
        for (int i = index; i < _markerUsedCount; i++)
        {
            _markers[i + 1].GetComponent<Marker>().Index = i;
        }
        _markerIndex = _markerUsedCount;
    }

    /// <summary>
    /// �ܺο��� Mode�� �������ִ� �޼���
    /// </summary>
    /// <param name="mode">������ Mode</param>
    public void ModeSetting(Mode mode)
    {
        _currentMode = mode;
    }
}
