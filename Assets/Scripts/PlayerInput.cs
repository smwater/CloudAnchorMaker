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

                // 리스트에 사용할 index가 존재한다면
                if (_deletedMarkerIndexs.Contains(_markerIndex))
                {
                    _deletedMarkerIndexs.Remove(_markerIndex);
                }

                // 사용할 index의 마커가 이미 존재한다면
                if (Markers[_markerIndex] != null)
                {
                    // 해제된 Marker의 index를 재사용
                    _markerIndex = _deletedMarkerIndexs[0];
                    _deletedMarkerIndexs.Remove(_markerIndex);
                }

                // 여러 물체가 raycast로 인식 됐다면 가장 가깝게 감지된 곳을 저장
                arHit = arHits[0];

                // 감지된 위치에 Marker를 생성하고
                Markers[_markerIndex] = Instantiate(MarkerPrefab, arHit.pose.position + new Vector3(0f, 0.2f), arHit.pose.rotation);
                // local Anchor를 생성
                Markers[_markerIndex].GetComponent<Marker>().CreateAnchor(arHit);
                // 이 marker의 index를 넘겨줌
                Markers[_markerIndex].GetComponent<Marker>().Index = _markerIndex;
                Debug.Log($"사용한 index : {_markerIndex}");
                _markerIndex = (_markerIndex + 1) % _markerMaxCount;
                _markerUsedCount++;

                _currentMode = Mode.MarkerSetting;
            }
        }
    }

    /// <summary>
    /// 해제된 마커의 index를 리스트에 넣는 메서드
    /// </summary>
    /// <param name="index">사용 해제된 Marker의 index</param>
    public void FreeIndex(int index)
    {
        Debug.Log($"삭제한 번호 : {index}");
        _deletedMarkerIndexs.Add(index);
        _markerUsedCount--;
       Debug.Log($"남은 개수 : {_deletedMarkerIndexs.Count} / 맨 앞에 있는 거 : {_deletedMarkerIndexs[0]}");
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
