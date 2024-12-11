using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    
    private Rigidbody2D rigid;
    private PlayerDecteting playerDecteting;
    private PlayerData playerData;
    private Animator animator;
    
    //private float moveSpeed;
    private WeaponType weaponType;
    private GameObject bullet;
    
    //공격 관련 변수들
    //private float attackSpeed; // 초당 공격 횟수
    private float lastAttackTime = 0f; // 마지막 공격 시간
    private int shotgunBulletCount = 5; // 산탄 총알 개수
    private float shotgunSpreadAngle = 30f; // 산탄 퍼짐 각도
    
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        playerDecteting = GetComponent<PlayerDecteting>();
        //animator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerData = GameManager.Instance.playerData;
        
        InitVariable();
    }

    private void Update()
    {
        // 자동 공격 로직
        Attack();
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }
    
    //변수 설정
    private void InitVariable()
    {
        weaponType = playerData.currentWeapon.weaponType;
        //moveSpeed = playerData.MoveSpeed;
        //attackSpeed = playerData.currentWeapon.attackSpeed;
        bullet = playerData.currentWeapon.projectilePrefab;
    }
    
    //이동 관련 메소드
    public void PlayerMove()
    {
        Vector2 input = InputManager.Instance.GetMovementInput();
        
        rigid.MovePosition(rigid.position + input * (playerData.MoveSpeed * Time.deltaTime));
    }
    
    //공격 관련 메소드
    public void Attack()
    {
        switch (weaponType)
        {
            case WeaponType.ShotGun:
                ShotGunAttack();
                break;
            default:
                AutoAttack();
                break;
        }
    }
    
    private void AutoAttack()
    {
        // 가장 가까운 타겟이 있고, 공격 쿨다운이 지났는지 확인
        if (playerDecteting.nearestTarget != null && Time.time >= lastAttackTime + (1f / playerData.currentWeapon.attackSpeed))
        {
            GameObject spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);

            // 방향 계산
            Vector3 targetPosition = playerDecteting.nearestTarget.transform.position;
            Vector2 direction = (targetPosition - transform.position).normalized;

            // 회전 계산
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);

            // 총알 회전 적용
            spawnedBullet.transform.rotation = rotation;

            if (spawnedBullet.TryGetComponent<StandardProjectile>(out StandardProjectile projectile))
            {
                float finalDamage = playerData.currentWeapon.damage + playerData.AttackPower;
                projectile.Initialize(direction, finalDamage);
            }

            // 마지막 공격 시간 업데이트
            lastAttackTime = Time.time;
        }
    }

    private void ShotGunAttack()
    {
        // 가장 가까운 타겟이 있고, 공격 쿨다운이 지났는지 확인
        if (playerDecteting.nearestTarget != null && Time.time >= lastAttackTime + (1f / playerData.currentWeapon.attackSpeed))
        {
            // 기본 방향 계산
            Vector3 targetPosition = playerDecteting.nearestTarget.transform.position;
            Vector2 baseDirection = (targetPosition - transform.position).normalized;

            // 기본 각도 계산
            float baseAngle = Mathf.Atan2(baseDirection.y, baseDirection.x) * Mathf.Rad2Deg;

            // 산탄 발사
            for (int i = 0; i < shotgunBulletCount; i++)
            {
                // 각도 분산 계산
                float randomSpread = Random.Range(-shotgunSpreadAngle / 2f, shotgunSpreadAngle / 2f);
                float currentAngle = baseAngle + randomSpread;

                // 분산된 방향 계산
                Vector2 spreadDirection = new Vector2(
                    Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                    Mathf.Sin(currentAngle * Mathf.Deg2Rad)
                );

                // 총알 생성
                GameObject spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);

                // 회전 계산
                Quaternion rotation = Quaternion.Euler(0, 0, currentAngle - 90);
                spawnedBullet.transform.rotation = rotation;

                if (spawnedBullet.TryGetComponent<StandardProjectile>(out StandardProjectile projectile))
                {
                    float finalDamage = playerData.currentWeapon.damage + playerData.AttackPower;
                    projectile.Initialize(spreadDirection, finalDamage);
                }
            }

            // 마지막 공격 시간 업데이트
            lastAttackTime = Time.time;
        }
    }
}