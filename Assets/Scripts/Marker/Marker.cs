using Google.XR.ARCoreExtensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Marker : MonoBehaviour
{
    public ARAnchor ARAnchor { get; private set; }
    [HideInInspector] public ARCloudAnchor ARCloudAnchor;

    [HideInInspector] public int Index;
    [HideInInspector] public string Name = "Default";
    [HideInInspector] public string CloudAnchorID;

    [SerializeField] private GameObject _buttonCanvas;
    private ARAnchorManager _arAnchorManager;

    private void Awake()
    {
        ARAnchor = gameObject.AddComponent<ARAnchor>();
        _arAnchorManager = GameObject.Find("@GameManager").GetComponent<ARAnchorManager>();
    }

    /// <summary>
    /// Marker의 Button UI를 띄우는 메서드
    /// </summary>
    public void Click()
    {
        _buttonCanvas.SetActive(true);
    }

    /// <summary>
    /// 특정 지점에 로컬 앵커를 부착하는 메서드
    /// </summary>
    /// <param name="arHit">ArraycastHit로 특정된 지점</param>
    public void CreateAnchor(ARRaycastHit arHit)
    {
        if (arHit.trackable is ARPlane arPlane)
        {
            ARAnchor = _arAnchorManager.AttachAnchor(arPlane, arHit.pose);
        }
    }
}
