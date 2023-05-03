using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPos : MonoBehaviour
{
    [Header("Camera Position")]
    public Transform target;    // ī�޶� ���� ���
    public Transform camPos;
    public Transform third_camPos;

    [Header("Third Person")]
    public float damping = 5.0f;    // �ε巯�� �̵��� ���� ���� ���
    float distance;   // ī�޶�� ��� ������ �Ÿ�
    float height;     // ī�޶� ����

    [Header("Input Key")]
    public KeyCode changeViewKey;

    public bool isThird = false;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        isThird = false;
        distance = -third_camPos.localPosition.z;
        height = third_camPos.transform.localPosition.y;
    }

    void Update()
    {
        InputKey();
        FollowCamPos();
    }
    private void LateUpdate()
    {
        if(isThird)
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
            transform.position = camPos.position;

    }
}
