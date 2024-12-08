using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;  // 플레이어의 Transform을 참조
    public float moveSpeed = 2f;  // 이동 속도
    public float stopDistance = 2f;  // 플레이어와의 최소 거리

    private void Start()
    {
        player = GameManager.Instance.Player.transform;
    }

    void Update()
    {
        // 플레이어와의 거리 계산
        float distance = Vector2.Distance(transform.position, player.position);

        // 플레이어와의 거리가 stopDistance보다 크면 따라감
        if (distance > stopDistance)
        {
            // 플레이어 방향 계산
            Vector3 direction = (player.position - transform.position).normalized;

            // 적이 플레이어 방향으로 이동
            transform.position += direction * (moveSpeed * Time.deltaTime);

            // 적이 플레이어를 향해 회전
            //transform.LookAt(player);
        }
    }
}