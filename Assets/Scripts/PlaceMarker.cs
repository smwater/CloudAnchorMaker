using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// 플레이어 입력 모드
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
        // 입력이 없으면 return
        if (Input.touchCount == 0)
        {
            return;
        }

        // 동시에 여러 손가락으로 터치했을 경우, 첫번째 터치만 인식
        Touch touch = Input.GetTouch(0);

        // 첫번째 터치에 한해, UI 뒤쪽 인식 방지
        if (EventSystem.current.IsPointerOverGameObject(0))
        {
            return;
        }

        // GetKeyDown과 비슷
        if (touch.phase == TouchPhase.Began)
        {
            // Marker Setting Mode라면 Marker나 AR Plane에 대한 입력을 받지 않음
            if (_currentMode == Mode.MarkerSetting)
            {
                return;
            }

            Ray ray;
            ray = _camera.ScreenPointToRay(touch.position);

            // Raycast로 Marker를 감지
            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("Marker");
            if (Physics.Raycast(ray, out hit, 10f, layerMask))
            {
                // Marker가 존재하면 click()
                hit.transform.GetComponent<Marker>().Click();

                _currentMode = Mode.MarkerSetting;

                // return이 없으면 아래 AR raycast로 marker가 추가로 생성될 수 있음
                return;
            }

            List<ARRaycastHit> arHits = new List<ARRaycastHit>();
            ARRaycastHit arHit;

            // AR raycast로 AR Plane을 감지
            if (_arRaycastManager.Raycast(ray, arHits, TrackableType.PlaneWithinPolygon | TrackableType.FeaturePoint))
            {
                // Marker를 모두 생성했다면, log 출력
                if (_markerUsedCount >= _markerMaxCount)
                {
                    Debug.Log("생성 가능한 마커를 다 소모했습니다.");
                    return;
                }

                // 여러 물체가 raycast로 인식 됐다면 가장 가깝게 감지된 곳을 저장
                arHit = arHits[0];

                // 감지된 위치에 Marker를 생성하고
                _markers[_markerIndex] = Instantiate(_markerPrefab, arHit.pose.position + new Vector3(0f, 0.2f), arHit.pose.rotation);
                // local Anchor를 생성
                _markers[_markerIndex].GetComponent<Marker>().CreateAnchor(arHit);
                // 이 marker의 index를 넘겨줌
                _markers[_markerIndex].GetComponent<Marker>().Index = _markerIndex;
                _markerIndex = (_markerIndex + 1) % _markerMaxCount;
                _markerUsedCount++;

                _currentMode = Mode.MarkerSetting;
            }
        }
    }

    /// <summary>
    /// 해제된 마커보다 index가 높은 마커들을 정리해주는 메서드
    /// </summary>
    /// <param name="index">사용 해제된 Marker의 index</param>
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
    /// 외부에서 Mode를 변경해주는 메서드
    /// </summary>
    /// <param name="mode">변경할 Mode</param>
    public void ModeSetting(Mode mode)
    {
        _currentMode = mode;
    }
}
