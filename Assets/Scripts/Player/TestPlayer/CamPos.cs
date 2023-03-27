using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPos : MonoBehaviour
{
    [Header("Camera Position")]
    public Transform target;    // 카메라가 따라갈 대상
    public Transform camPos;
    public Transform third_camPos;

    [Header("Third Person")]
    public float damping = 5.0f;    // 부드러운 이동을 위한 감쇠 계수
    float distance;   // 카메라와 대상 사이의 거리
    float height;     // 카메라 높이

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

        // 부드러운 이동을 위해 Lerp 함수 사용
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);

        // 카메라가 항상 대상을 바라보도록 설정
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
