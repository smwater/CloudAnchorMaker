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
    /// Marker�� Button UI�� ���� �޼���
    /// </summary>
    public void Click()
    {
        _buttonCanvas.SetActive(true);
    }

    /// <summary>
    /// Ư�� ������ ���� ��Ŀ�� �����ϴ� �޼���
    /// </summary>
    /// <param name="arHit">ArraycastHit�� Ư���� ����</param>
    public void CreateAnchor(ARRaycastHit arHit)
    {
        if (arHit.trackable is ARPlane arPlane)
        {
            ARAnchor = _arAnchorManager.AttachAnchor(arPlane, arHit.pose);
        }
    }
}
