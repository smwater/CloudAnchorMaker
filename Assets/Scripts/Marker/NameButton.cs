using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameButton : MonoBehaviour
{
    public HostButton HostButton;
    public GameObject NameCanvas;
    public GameObject ButtonCanvas;

    /// <summary>
    /// 이름 설정창을 띄워주는 메서드
    /// </summary>
    public void Click()
    {
        // 호스팅 중이라면 return
        if (HostButton.CloudAnchorHosting)
        {
            Debug.Log("호스팅 중입니다. 잠시만 기다려주세요.");
            return;
        }

        NameCanvas.SetActive(true);
        ButtonCanvas.SetActive(false);
    }
}
