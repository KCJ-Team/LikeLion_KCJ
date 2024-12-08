using UnityEngine;

public class TurretObjDetectingState : ProjectileState
{
    private TurretObject turret;
    
    
    public TurretObjDetectingState(Projectile projectile) : base(projectile)
    {
        turret = projectile as TurretObject;
    }
    
    public override void EnterState()
    {
        
    }
    
    public override void UpdateState()
    {
        if (turret.nearestTarget != null)
        {
            projectile.ChangeState(new TurretObjAttackingState(projectile));
            return;
        }
        
        turret.DetectingEnemy();
    }
    
    public override void ExitState()
    {
        
    }
}