using UnityEngine;

public class PullProjectileMovingState : ProjectileState
{
    private PullProjectile pullProjectile;
    private Vector3 velocity;
    
    public PullProjectileMovingState(Projectile projectile) : base(projectile)
    {
        pullProjectile = projectile as PullProjectile;
        velocity = pullProjectile.GetDirection() * pullProjectile.speed;
    }
    
    public override void EnterState()
    {
    }
    
    public override void UpdateState()
    {
        projectile.transform.position += velocity * Time.deltaTime;
    }
    
    public override void ExitState()
    {
        velocity = Vector3.zero;
    }
}