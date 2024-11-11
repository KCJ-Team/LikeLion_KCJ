using UnityEngine;

public class ProjectileMovingState : ProjectileState
{
    protected Vector3 direction;
    protected Vector3 startPosition;
    protected float maxRange = 100f; // 기본 최대 사거리
    
    public ProjectileMovingState(Projectile projectile, Vector3 direction) : base(projectile)
    {
        this.direction = direction;
        this.startPosition = projectile.transform.position;
    }
    
    public override void EnterState()
    {
        // 발사 효과음이나 이펙트를 재생할 수 있음
    }
    
    public override void UpdateState()
    {
        // 발사체 이동
        projectile.transform.Translate(direction * (projectile.speed * Time.deltaTime), Space.World);
        
        // 최대 사거리 체크
        if (Vector3.Distance(startPosition, projectile.transform.position) > maxRange)
        {
            Object.Destroy(projectile.gameObject);
        }
    }
    
    public override void ExitState()
    {
        // 필요한 정리 작업 수행
    }
    
}