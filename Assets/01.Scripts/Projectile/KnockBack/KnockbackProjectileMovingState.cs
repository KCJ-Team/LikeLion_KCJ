using UnityEngine;

public class KnockbackProjectileMovingState : ProjectileState
{
    private KnockbackProjectile knockbackProjectile;
    private Vector3 velocity;
    
    public KnockbackProjectileMovingState(Projectile projectile) : base(projectile)
    {
        knockbackProjectile = projectile as KnockbackProjectile;
        velocity = knockbackProjectile.GetDirection() * knockbackProjectile.speed;
    }
    
    public override void EnterState()
    {
        // Trigger 사용 시에는 Transform으로 이동
    }
    
    public override void UpdateState()
    {
        // Transform을 사용하여 이동
        projectile.transform.position += velocity * Time.deltaTime;
    }
    
    public override void ExitState()
    {
        // 이동 중지
        velocity = Vector3.zero;
    }
}