using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameButton : MonoBehaviour
{
    public HostButton HostButton;
    public GameObject NameCanvas;
    public GameObject ButtonCanvas;

    /// <summary>
    /// �̸� ����â�� ����ִ� �޼���
    /// </summary>
    public void Click()
    {
        // ȣ���� ���̶�� return
        if (HostButton.CloudAnchorHosting)
        {
            Debug.Log("ȣ���� ���Դϴ�. ��ø� ��ٷ��ּ���.");
            return;
        }

        NameCanvas.SetActive(true);
        ButtonCanvas.SetActive(false);
    }
}
