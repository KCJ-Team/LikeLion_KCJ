using UnityEngine;

public class MissileMovingState : ProjectileState
{
    private Missile missile;
    private Vector3 direction;        // 미사일의 이동 방향
    
    public MissileMovingState(Projectile projectile, Vector3 targetPosition, float damage) : base(projectile)
    {
        missile = projectile as Missile;
        
        this.direction = (targetPosition - projectile.transform.position).normalized;
    }
    
    public override void EnterState()
    {
        projectile.transform.rotation = Quaternion.LookRotation(direction);
    }
    
    public override void UpdateState()
    {
        projectile.transform.position += direction * (Time.deltaTime * missile.speed);
    }

    public override void ExitState()
    {
        
    }
}