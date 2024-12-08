using PlayerInfo;
using UnityEngine;

public class TurretObjAttackingState : ProjectileState
{
    private TurretObject turret;
    
    public TurretObjAttackingState(Projectile projectile) : base(projectile)
    {
        turret = projectile as TurretObject;
    }

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        if (turret.nearestTarget == null)
        {
            projectile.ChangeState(new TurretObjDetectingState(projectile));
            return;
        }
        
        turret.AttackingTarget();
    }
    
    public override void ExitState()
    {
        
    }
}