using UnityEngine;

public class TurretObject : Projectile
{
    [SerializeField] private LayerMask targetLayer;        // 감지할 대상의 레이어
    [SerializeField] private float detectionRadius = 10f;  // 감지 범위
    [SerializeField] private float attackInterval = 1f;    // 공격 간격
    [SerializeField] private GameObject projectilePrefab;  // 발사체 프리팹
    [SerializeField] private Transform firePoint;          // 발사 위치

    private Transform target;                              // 현재 타겟

    private void Start()
    {
        Initialize(Vector3.zero, 10f); // 기본 데미지 설정
        firePoint = this.transform;
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
            
            Rigidbody rb = projectileObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = direction * speed;
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

    // 시각적 디버깅을 위한 기즈모
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        if (firePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(firePoint.position, 0.5f);
        }
    }
}