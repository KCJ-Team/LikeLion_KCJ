using System;
using UnityEngine;

public class TurretObject : Projectile
{
    public RaycastHit2D[] targets;
    public float detectingRange;
    public LayerMask targetLayer;
    public Transform nearestTarget;
    public GameObject bulletPrefab;
    public float duration;
    
    [SerializeField] private float attackSpeed; // 초당 공격 횟수
    private float lastAttackTime = 0f; // 마지막 공격 시간
    private float spawnTime;
    
    private void Start()
    {
        spawnTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - spawnTime >= duration)
        {
            Destroy(gameObject);
            return;
        }
        
        base.Update();
    }
    
    public void AttackingTarget()
    {
        if (nearestTarget != null && Time.time >= lastAttackTime + (1f / attackSpeed))
        {
            GameObject spawnedBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // 방향 계산
            Vector3 targetPosition = nearestTarget.transform.position;
            Vector2 direction = (targetPosition - transform.position).normalized;

            // 회전 계산
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);

            // 총알 회전 적용
            spawnedBullet.transform.rotation = rotation;

            if (spawnedBullet.TryGetComponent<StandardProjectile>(out StandardProjectile projectile))
            {
                projectile.Initialize(direction, damage);
            }

            // 마지막 공격 시간 업데이트
            lastAttackTime = Time.time;
        }
    }
    
    public void DetectingEnemy()
    {
        targets = Physics2D.CircleCastAll(transform.position, detectingRange, Vector2.zero, 0, targetLayer);

        nearestTarget = GetNearest();
    }
    
    private Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float curDiff = Vector3.Distance(myPos, targetPos);

            if (curDiff < diff)
            {
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }
    
    protected override ProjectileState GetInitialState()
    {
        return new TurretObjDetectingState(this);
    }
}