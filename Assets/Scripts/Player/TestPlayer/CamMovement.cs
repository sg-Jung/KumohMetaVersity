using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CamMovement : MonoBehaviourPunCallbacks
{
    [Header("CamPos Class")]
    public CamPos camPos;

    [Header("�Բ� ȸ���� Transform")]
    public Transform orientation;
    public Transform player;

    [Header("���콺 �ΰ���")]
    public float sensX;
    public float sensY;

    float xRot, mouseX;
    float yRot, mouseY;

    void Update()
    {
        // �θ� ������Ʈ�� Photon View ������Ʈ�� �����Ƿ� ��밡��
        if (!photonView.IsMine) return;


        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRot += mouseX;
        xRot -= mouseY;
        
        if(camPos.isThird)
            xRot = Mathf.Clamp(xRot, 0f, 45f);
        else
            xRot = Mathf.Clamp(xRot, -90f, 90f);

        ChangeRotation();
    }

    void ChangeRotation()
    {
        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
        orientation.rotation = Quaternion.Euler(0, yRot, 0);
        player.rotation = Quaternion.Euler(0, yRot, 0);
    }
}
