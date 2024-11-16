using System;
using System.Collections;
using System.Collections.Generic;
using Messages;
using UnityEngine;

/// <summary>
/// WASD로 간단하게 포지션 이동하고 서버에 전송하는 테스트 스크립트
/// </summary>
public class TestPlayerMovePosition : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int health = 100;
    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        // WASD 키 입력 처리
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, moveZ, 0f) * (moveSpeed * Time.deltaTime);
        transform.Translate(movement);

        // 위치가 변경되었을 경우 서버에 전송
        // if (transform.position != lastPosition)
        // {
        //     PlayerManager.Instance.SendPlayerPosition(
        //         MessageType.PlayerPositionUpdate, transform.position, moveSpeed, health
        //     );
        //     
        //     lastPosition = transform.position;
        // }
    }
}
