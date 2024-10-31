using UnityEngine;

public class PullProjectilePullingState : ProjectileState
{
    private PullProjectile pullProjectile;
    private Transform playerTransform;
    private Rigidbody targetRigidbody;
    private float pullForce;
    private float maxPullDistance;
    private float destroyDistance;
    private bool hasTarget = false;
    
    public PullProjectilePullingState(Projectile projectile) : base(projectile)
    {
        pullProjectile = projectile as PullProjectile;
        if (pullProjectile != null)
        {
            playerTransform = pullProjectile.GetPlayerTransform();
            targetRigidbody = pullProjectile.GetTargetRigidbody();
            pullForce = pullProjectile.GetPullForce();
            maxPullDistance = pullProjectile.GetMaxPullDistance();
            destroyDistance = pullProjectile.GetDestroyDistance();
            hasTarget = targetRigidbody != null;
        }
    }
    
    public override void EnterState()
    {
        if (hasTarget)
        {
            // 물리 설정 변경
            targetRigidbody.useGravity = false;
            targetRigidbody.drag = 0;
        }
    }
    
    public override void UpdateState()
    {
        if (!hasTarget) return;
        
        if (targetRigidbody != null && playerTransform != null)
        {
            // 플레이어와 대상 사이의 거리 계산
            Vector3 direction = playerTransform.position - targetRigidbody.position;
            float distance = direction.magnitude;
            
            if (distance > destroyDistance) // destroyDistance보다 멀때만 끌어당김
            {
                // 거리에 따른 힘 계산
                float forceMagnitude = Mathf.Clamp(distance, 0, maxPullDistance) * pullForce;
                Vector3 pullDirection = direction.normalized;
                
                // 힘 적용
                targetRigidbody.velocity = pullDirection * forceMagnitude;
            }
            else
            {
                // 목표 지점에 도달하면 물리 설정 복구하고 발사체 파괴
                targetRigidbody.velocity = Vector3.zero;
                targetRigidbody.useGravity = true;
                targetRigidbody.drag = 0.5f;
                Object.Destroy(projectile.gameObject);
            }
            
            
        }
    }
    
    public override void ExitState()
    {
        if (hasTarget && targetRigidbody != null)
        {
            // 물리 설정 복구
            targetRigidbody.useGravity = true;
            targetRigidbody.drag = 0.5f;
        }
    }
}