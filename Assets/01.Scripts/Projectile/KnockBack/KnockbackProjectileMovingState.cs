using UnityEngine;

public class KnockbackProjectileMovingState : ProjectileState
{
    private KnockbackProjectile knockbackProjectile;
    private Vector3 direction;
    
    public KnockbackProjectileMovingState(Projectile projectile, Vector3 direction) : base(projectile)
    {
        knockbackProjectile = projectile as KnockbackProjectile;

        this.direction = direction;
    }
    
    public override void EnterState()
    {
        
    }
    
    public override void UpdateState()
    {
        projectile.transform.Translate(direction * (projectile.speed * Time.deltaTime), Space.World);
    }
    
    public override void ExitState()
    {
        
    }
}