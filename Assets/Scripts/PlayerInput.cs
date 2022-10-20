using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public enum Mode
{
    AnchorPlacement,
    AnchorSetting,
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

    private Mode _currentMode = Mode.AnchorPlacement;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _arRaycastManager = GetComponent<ARRaycastManager>();
        Markers = new GameObject[_markerMaxCount];
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.touchCount == 0)
        {
            return;
        }

        // 동시에 여러 손가락으로 터치했을 경우, 첫번째 터치만 인식
        Touch touch = Input.GetTouch(0);

        // ColliderEnter와 비슷
        if (touch.phase == TouchPhase.Began)
        {
            if (_currentMode == Mode.AnchorSetting)
            {
                return;
            }

            Ray ray;
            ray = _camera.ScreenPointToRay(touch.position);

            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("Marker");
            if (Physics.Raycast(ray, out hit, 10f, layerMask))
            {
                hit.transform.GetComponent<Marker>().Click();

                _currentMode = Mode.AnchorSetting;
            }

            List<ARRaycastHit> arHits = new List<ARRaycastHit>();
            ARRaycastHit arHit;

            if (_arRaycastManager.Raycast(ray, arHits, TrackableType.PlaneWithinPolygon | TrackableType.FeaturePoint))
            {
                arHit = arHits[0];

                Markers[_markerNowCount] = Instantiate(MarkerPrefab, arHit.pose.position + new Vector3(0f, 0.2f), arHit.pose.rotation);
                Markers[_markerNowCount].GetComponent<Marker>().CreateAnchor(arHit);
                _markerNowCount++;

                _currentMode = Mode.AnchorSetting;
            }
        }
    }

    public void DecreaseMarkerNowCount()
    {
        _markerNowCount--;
    }

    public void ModeSetting(Mode mode)
    {
        _currentMode = mode;
    }
}
