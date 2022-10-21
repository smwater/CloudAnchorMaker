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

    private int _markerMaxCount = 40;
    private int _markerNowCount = 0;

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
                // �ش��ϴ� index�� ��Ŀ�� �̹� �����Ѵٸ�
                if (Markers[_markerNowCount] != null)
                {
                    // count�� �ٲ��ְ� return
                    _markerNowCount++;
                    return;
                }

                // ���� ��ü�� raycast�� �ν� �ƴٸ� ���� ������ ������ ���� ����
                arHit = arHits[0];

                // ������ ��ġ�� Marker�� �����ϰ�
                Markers[_markerNowCount] = Instantiate(MarkerPrefab, arHit.pose.position + new Vector3(0f, 0.2f), arHit.pose.rotation);
                // local Anchor�� ����
                Markers[_markerNowCount].GetComponent<Marker>().CreateAnchor(arHit);
                // �� marker�� ���°�� ������ marker���� index�� �Ѱ���
                Markers[_markerNowCount].GetComponent<Marker>().Index = _markerNowCount;
                _markerNowCount++;

                _currentMode = Mode.MarkerSetting;
            }
        }
    }

    /// <summary>
    /// Marker�� �������� ��, Marker Count�� ���ҽ����ִ� �޼���
    /// </summary>
    public void DecreaseMarkerNowCount()
    {
        _markerNowCount--;
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
