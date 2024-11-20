using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoSphere : MonoBehaviour
{
    public Color gizmoColor = Color.red; // 기즈모의 색상
    public float sphereRadius = 1.0f;    // 구의 반지름

    // 씬 뷰에 항상 그리기
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor; // 기즈모 색상 설정
        Gizmos.DrawSphere(transform.position, sphereRadius); // 구 그리기
    }
}
