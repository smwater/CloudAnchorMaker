using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlayerInput : MonoBehaviour
{
    public GameObject MarkerPrefab;
    [HideInInspector] public GameObject[] Markers;

    private Camera _camera;
    private ARRaycastManager _arRaycastManager;
    
    private int _markerMaxCount = 40;
    private int _markerNowCount = 0;

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

        // ���ÿ� ���� �հ������� ��ġ���� ���, ù��° ��ġ�� �ν�
        Touch touch = Input.GetTouch(0);

        // ColliderEnter�� ���
        if (touch.phase == TouchPhase.Began)
        {
            Ray ray;
            ray = _camera.ScreenPointToRay(touch.position);

            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("Marker");
            if (Physics.Raycast(ray, out hit, 10f, layerMask))
            {
                if (EventSystem.current.IsPointerOverGameObject() == true)
                {
                    return;
                }

                hit.transform.GetComponent<Marker>().Click();

                return;
            }

            if (MarkerCount.Count >= 1)
            {
                return;
            }

            List<ARRaycastHit> arHits = new List<ARRaycastHit>();
            ARRaycastHit arHit;

            if (_arRaycastManager.Raycast(ray, arHits, TrackableType.PlaneWithinPolygon | TrackableType.FeaturePoint))
            {
                arHit = arHits[0];

                Markers[_markerNowCount] = Instantiate(MarkerPrefab, arHit.pose.position + new Vector3(0f, 0.2f), arHit.pose.rotation);
                Markers[_markerNowCount].GetComponent<Marker>().CreateAnchor(arHit);

                MarkerCount.Count++;
                _markerNowCount++;

                return;
            }
        }
    }

    public void DecreaseMarkerNowCount()
    {
        _markerNowCount--;
    }
}
