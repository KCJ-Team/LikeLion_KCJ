using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleActiveState : ProjectileState
{
    private BlackHoleProjectile blackHoleProjectile;
    
    public BlackHoleActiveState(Projectile projectile) : base(projectile)
    {
        blackHoleProjectile = projectile as BlackHoleProjectile;
    }

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        blackHoleProjectile.attract();
    }

    public override void ExitState()
    {
        
    }
}
