using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CamMovement : MonoBehaviourPunCallbacks
{
    [Header("CamPos Class")]
    public CamPos camPos;

    [Header("함께 회전할 Transform")]
    public Transform orientation;
    public Transform player;

    [Header("마우스 민감도")]
    public float sensX;
    public float sensY;

    float xRot, mouseX;
    float yRot, mouseY;

    void Update()
    {
        // 부모 오브젝트에 Photon View 컴포넌트가 있으므로 사용가능
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
