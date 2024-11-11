using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullProjectileMovingState : ProjectileState
{
    private PullProjectile pullProjectile;
    
    protected Vector3 direction;
    
    public PullProjectileMovingState(Projectile projectile, Vector3 direction) : base(projectile)
    {
        pullProjectile = projectile as PullProjectile;
        
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
