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
    private Vector3 targetPosition;
    private float pullSpeed = 30f; // 끌어당기는 속도
    
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
        if (hasTarget && targetRigidbody != null)
        {
            targetRigidbody.useGravity = false;
            targetRigidbody.drag = 0;
            // 초기 타겟 위치를 플레이어의 현재 위치로 설정
            targetPosition = playerTransform.position;
        }
    }
    
    public override void UpdateState()
    {
        if (!hasTarget || targetRigidbody == null || playerTransform == null) 
        {
            Object.Destroy(projectile.gameObject);
            return;
        }
        
        // 타겟 위치를 플레이어의 현재 위치로 계속 업데이트
        targetPosition = playerTransform.position;
        
        // 현재 위치에서 목표 위치까지의 방향과 거리 계산
        Vector3 direction = targetPosition - targetRigidbody.position;
        float distance = direction.magnitude;
        
        if (distance > destroyDistance)
        {
            // MoveTowards를 사용하여 부드러운 이동 구현
            Vector3 newPosition = Vector3.MoveTowards(
                targetRigidbody.position,
                targetPosition,
                pullSpeed * Time.deltaTime
            );
            
            // Rigidbody의 위치 직접 설정
            targetRigidbody.MovePosition(newPosition);
            
            // 발사체를 타겟 위치로 이동 (시각적 효과)
            projectile.transform.position = targetRigidbody.position;
        }
        else
        {
            // 목표 지점에 도달했을 때 처리
            targetRigidbody.velocity = Vector3.zero;
            targetRigidbody.useGravity = true;
            targetRigidbody.drag = 0.5f;
            Object.Destroy(projectile.gameObject);
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