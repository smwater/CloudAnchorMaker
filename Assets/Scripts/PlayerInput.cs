using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlayerInput : MonoBehaviour
{
    public GameObject MarkerPrefab;

    private Camera _camera;
    private ARRaycastManager _arRaycastManager;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        _arRaycastManager = GetComponent<ARRaycastManager>();
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

                GameObject marker = Instantiate(MarkerPrefab, arHit.pose.position + new Vector3(0f, 0.2f), arHit.pose.rotation);

                MarkerCount.Count++;

                marker.GetComponent<Marker>().CreateAnchor(arHit);

                return;
            }
        }
    }
}
