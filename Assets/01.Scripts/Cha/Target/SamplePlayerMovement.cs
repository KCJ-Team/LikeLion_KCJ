using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SamplePlayerMovement : MonoBehaviour
{
    public float speed = 20f; // 플레이어의 이동 속도
    public float pickupRange = 2.0f; // 목표물을 픽업할 수 있는 범위
    
    [Header("이동 목적지")]
    private Vector3 targetPosition;
    private bool isMoving = false;

    // E키를 눌렀을때 목표 접근 타겟키를 일단 델리게이트로.. 
    public event Action OnPickupTarget;
    public event Action OnReachedDestination;
    
    void Update()
    {
        // E 키를 눌렀을 때 범위 내에 목표물이 있으면 PickUpTarget 호출
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckPickupTarget();
        }
        
        // 자동 이동: 목표물이 설정되어 있을 때만 이동
        if (isMoving)
        {
            MoveTowardsTarget();
        }
    }

    // 특정 위치로 이동하는 함수
    private void MoveTowardsTarget()
    {
        if (Vector3.Distance(transform.position, targetPosition) > 1.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            // 도착 시 이동을 멈춤
            isMoving = false;
            OnReachedDestination?.Invoke();
            Debug.Log("목표 지점에 도착했습니다.");
        }
    }
    
    // 목표물을 픽업
    private void CheckPickupTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget <= pickupRange)
        {
            // 목표물을 픽업하는 함수 호출
            OnPickupTarget?.Invoke();
            Debug.Log("목표물을 픽업했습니다.");
        }
        else
        {
            Debug.Log("목표물이 범위 밖에 있습니다.");
        }
    }

    // 목표 지점을 설정하는 함수
    public void SetDestination(Vector3 newDestination)
    {
        targetPosition = newDestination;
        isMoving = true;
    }
} // end class
