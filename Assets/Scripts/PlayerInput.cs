using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInput : MonoBehaviour
{
    private Camera _camera;
    public GameObject MarkerPrefab;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
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
            RaycastHit hit;

            ray = _camera.ScreenPointToRay(touch.position);

            if (Physics.Raycast(ray, out hit))
            {
                if (EventSystem.current.IsPointerOverGameObject() == true)
                {
                    return;
                }

                Pose pose = new Pose(hit.transform.position, hit.transform.rotation);

                if (hit.transform.CompareTag("Marker"))
                {
                    hit.transform.GetComponent<Marker>().Click();
                }

                if (hit.transform.CompareTag("Plane"))
                {
                    Instantiate(MarkerPrefab, pose.position + new Vector3(0f, 0.2f), pose.rotation);
                }
            }
        }
    }
}
