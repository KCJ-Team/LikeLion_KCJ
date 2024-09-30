using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 따라갈 대상
    public Vector3 offset;   // 카메라가 플레이어에서 떨어진 위치
    public float smoothSpeed = 0.125f; // 부드럽게 움직이는 속도

    void LateUpdate()
    {
        // 목표 위치 계산
        Vector3 desiredPosition = target.position + offset;

        // Lerp를 사용하여 부드럽게 카메라를 이동
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 카메라 위치 갱신
        transform.position = smoothedPosition;

        // 캐릭터를 바라보도록 카메라 회전 (필요에 따라)
        transform.LookAt(target);
    }
}
