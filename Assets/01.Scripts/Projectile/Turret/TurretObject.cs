using System;
using UnityEngine;

public class TurretObject : Projectile
{
    [SerializeField] private LayerMask targetLayer;        // 감지할 대상의 레이어
    [SerializeField] private float detectionRadius = 10f;  // 감지 범위
    [SerializeField] private float attackInterval = 1f;    // 공격 간격
    [SerializeField] private GameObject projectilePrefab;  // 발사체 프리팹
    [SerializeField] private Transform firePoint;          // 발사 위치
    [SerializeField] private Transform turretHead;         // 회전할 터렛 헤드 (자식 오브젝트)
    [SerializeField] private float duration = 10f;         // 터렛 지속시간 (초)

    private Transform target;                              // 현재 타겟
    private float destroyTime;                            // 터렛이 파괴될 시간

    private void Start()
    {
        //base.Start();
        destroyTime = Time.time + duration;
    }

    private void Update()
    {
        base.Update();
        
        // 지속시간이 끝나면 터렛 파괴
        if (Time.time >= destroyTime)
        {
            Destroy(gameObject);
        }
    }
    
    protected override ProjectileState GetInitialState()
    {
        return new TurretObjDetectingState(this);
    }

    // 발사체 발사 메서드
    public void FireProjectile(Vector3 direction)
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectileObj = Instantiate(projectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
            
            StandardProjectile pr = projectileObj.GetComponent<StandardProjectile>();
            
            if (pr != null)
            {
                pr.Initialize(direction, damage);
            }
        }
    }

    // 타겟을 향해 터렛 헤드를 회전시키는 메서드
    public void RotateTurretHead(Vector3 targetPosition)
    {
        if (turretHead != null)
        {
            // 타겟 방향 계산 (Y축 회전만 적용)
            Vector3 directionToTarget = targetPosition - turretHead.position;
            directionToTarget.y = 0; // Y축 높이 차이 무시

            if (directionToTarget != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
                turretHead.rotation = lookRotation;
            }
        }
    }

    // 타겟 탐색 메서드
    public Transform DetectTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, targetLayer);
        
        if (colliders.Length > 0)
        {
            return colliders[0].transform;
        }
        
        return null;
    }

    // 게터/세터 메서드들
    public float GetAttackInterval() => attackInterval;
    public Transform GetTarget() => target;
    public void SetTarget(Transform newTarget) => target = newTarget;
    public float GetDetectionRadius() => detectionRadius;
    public float GetRemainingTime() => Mathf.Max(0, destroyTime - Time.time);
}