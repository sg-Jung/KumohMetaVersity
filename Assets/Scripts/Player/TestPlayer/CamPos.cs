using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CamPos : MonoBehaviourPunCallbacks
{
    [Header("Camera Position")]
    public Transform target;    // ī�޶� ���� ���
    public Transform one_camPos;
    public Transform third_camPos;

    [Header("Third Person")]
    public float damping = 5.0f;    // �ε巯�� �̵��� ���� ���� ���
    float distance;   // ī�޶�� ��� ������ �Ÿ�
    float height;     // ī�޶� ����

    [Header("Input Key")]
    public KeyCode changeViewKey;

    public bool isThird = false;

    private void Start()
    {
        isThird = false;
        distance = -third_camPos.localPosition.z;
        height = third_camPos.transform.localPosition.y;
    }

    void Update()
    {
        // �θ� ������Ʈ�� Photon View ������Ʈ�� �����Ƿ� ��밡��
        if (!photonView.IsMine) return;

        InputKey();
        FollowCamPos();
    }
    private void LateUpdate()
    {
        if (!photonView.IsMine) return;


        if (isThird)
            CameraNoDrill();
    }

    void CameraNoDrill()
    {
        Vector3 desiredPosition = target.position + (-target.forward * distance) + (Vector3.up * height);

        RaycastHit hit;
        if (Physics.Raycast(target.position, desiredPosition - target.position, out hit, distance))
        {
            desiredPosition = hit.point;
        }

        // �ε巯�� �̵��� ���� Lerp �Լ� ���
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);

        // ī�޶� �׻� ����� �ٶ󺸵��� ����
        transform.LookAt(target);

    }

    void InputKey()
    {
        if (Input.GetKeyDown(changeViewKey)) isThird = !isThird;
    }

    void FollowCamPos()
    {
        if(isThird)
            transform.position = third_camPos.position;
        else
            transform.position = one_camPos.position;

    }
}
