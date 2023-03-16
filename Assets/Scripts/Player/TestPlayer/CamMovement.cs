using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
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
        mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRot += mouseX;
        xRot -= mouseY;

        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRot, yRot, 0);
        orientation.rotation = Quaternion.Euler(0, yRot, 0);
        player.rotation = Quaternion.Euler(0, yRot, 0);
    }
}
