using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    None,
    Melee,
    Ranged
}

public class Monster : MonoBehaviour
{
    public MonsterType monsterType;
    //private GameObject target;
    private Rigidbody2D rigidbody2D;
    
    private float moveSpeed = 1f;
    private float stopDistance = 0.1f;
    
    public GameObject bulletPrefab;
    public float attackRange = 5f;
    public float attackDelayTime;
    public float damage;
    private float lastAttackTime = 0f;
    
    public int bulletCount = 12;
    
    private MonsterHealth health;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        health = GetComponent<MonsterHealth>();
    }

    private void Start()
    {
        //target = GameManager.Instance.Player;
    }

    public void MoveToTarget()
    {
        Vector2 direction = GameManager.Instance.Player.transform.position - transform.position;
        
        float distance = direction.magnitude;
        
        if (distance > stopDistance)
        {
            direction.Normalize();
            rigidbody2D.velocity = direction * moveSpeed;
        }
        else
        {
            rigidbody2D.velocity = Vector2.zero;
        }
    }

    public void StopMoving()
    {
        rigidbody2D.velocity = Vector2.zero;
    }

    public float GetAttackDistance()
    {
        float distance = Vector2.Distance(transform.position, GameManager.Instance.Player.transform.position);
        return distance;
    }

    public void Attack()
    {
        StartCoroutine(AttackToTarget());
    }

    public void StopAttacking()
    {
        StopAllCoroutines();
    }
    
    private IEnumerator AttackToTarget()
    {
        while (true)
        {
            GameObject spawnedBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        
            Vector2 direction = (GameManager.Instance.Player.transform.position - transform.position).normalized;
        
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);
        
            spawnedBullet.transform.rotation = rotation;
        
            if (spawnedBullet.TryGetComponent<StandardProjectile>(out StandardProjectile projectile))
            {
                projectile.Initialize(direction, damage);
            }
            
            yield return new WaitForSeconds(attackDelayTime);
        }
    }

    public void ShootInCircle()
    {
        // 각 총알 간의 각도 계산
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            // 현재 각도 계산
            float angle = i * angleStep;

            // 각도를 라디안 값으로 변환
            float angleRad = angle * Mathf.Deg2Rad;

            // 총알 방향 벡터 계산
            Vector2 direction = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

            // 총알 생성
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            
            float rotationAngle = angle - 90; // 90도를 빼서 총알이 정확한 방향으로 날아가게 함
            bullet.transform.rotation = Quaternion.Euler(0, 0, rotationAngle);
            
            if (bullet.TryGetComponent<StandardProjectile>(out StandardProjectile projectile))
            {
                projectile.Initialize(direction, damage);
            }
        }
    }

    public bool IsDeath()
    {
        return health.IsDead;
    }
}
