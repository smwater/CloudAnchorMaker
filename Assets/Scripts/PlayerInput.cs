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

public class PlayerInput : MonoBehaviour
{
    public GameObject MarkerPrefab;
    [HideInInspector] public GameObject[] Markers;

    private Camera _camera;
    private ARRaycastManager _arRaycastManager;

    private int _markerMaxCount = 3;
    private int _markerUsedCount = 0;
    private int _markerIndex = 0;
    private List<int> _deletedMarkerIndexs = new List<int>();

    private Mode _currentMode = Mode.MarkerPlacement;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _arRaycastManager = GetComponent<ARRaycastManager>();
        Markers = new GameObject[_markerMaxCount];
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

                // ����Ʈ�� ����� index�� �����Ѵٸ�
                if (_deletedMarkerIndexs.Contains(_markerIndex))
                {
                    _deletedMarkerIndexs.Remove(_markerIndex);
                }

                // ����� index�� ��Ŀ�� �̹� �����Ѵٸ�
                if (Markers[_markerIndex] != null)
                {
                    // ������ Marker�� index�� ����
                    _markerIndex = _deletedMarkerIndexs[0];
                    _deletedMarkerIndexs.Remove(_markerIndex);
                }

                // ���� ��ü�� raycast�� �ν� �ƴٸ� ���� ������ ������ ���� ����
                arHit = arHits[0];

                // ������ ��ġ�� Marker�� �����ϰ�
                Markers[_markerIndex] = Instantiate(MarkerPrefab, arHit.pose.position + new Vector3(0f, 0.2f), arHit.pose.rotation);
                // local Anchor�� ����
                Markers[_markerIndex].GetComponent<Marker>().CreateAnchor(arHit);
                // �� marker�� index�� �Ѱ���
                Markers[_markerIndex].GetComponent<Marker>().Index = _markerIndex;
                Debug.Log($"����� index : {_markerIndex}");
                _markerIndex = (_markerIndex + 1) % _markerMaxCount;
                _markerUsedCount++;

                _currentMode = Mode.MarkerSetting;
            }
        }
    }

    /// <summary>
    /// ������ ��Ŀ�� index�� ����Ʈ�� �ִ� �޼���
    /// </summary>
    /// <param name="index">��� ������ Marker�� index</param>
    public void FreeIndex(int index)
    {
        Debug.Log($"������ ��ȣ : {index}");
        _deletedMarkerIndexs.Add(index);
        _markerUsedCount--;
       Debug.Log($"���� ���� : {_deletedMarkerIndexs.Count} / �� �տ� �ִ� �� : {_deletedMarkerIndexs[0]}");
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
