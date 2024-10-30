using UnityEngine;

public class PullProjectileHitState : ProjectileState
{
    private PullProjectile pullProjectile;
    
    public PullProjectileHitState(Projectile projectile) : base(projectile)
    {
        pullProjectile = projectile as PullProjectile;
    }
    
    public override void EnterState()
    {
        Object.Destroy(projectile.gameObject);
    }
    
    public override void UpdateState()
    {
    }
    
    public override void ExitState()
    {
    }
}