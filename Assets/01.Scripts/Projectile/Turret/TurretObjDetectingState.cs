using UnityEngine;

public class TurretObjDetectingState : ProjectileState
{
    private TurretObject turret;
    private float detectionCheckInterval = 0.5f;  // 감지 체크 간격
    private float nextCheckTime;
    
    public TurretObjDetectingState(Projectile projectile) : base(projectile)
    {
        turret = projectile as TurretObject;
    }
    
    public override void EnterState()
    {
        nextCheckTime = Time.time;
        turret.SetTarget(null);
    }
    
    public override void UpdateState()
    {
        if (Time.time >= nextCheckTime)
        {
            Transform target = turret.DetectTarget();
            if (target != null)
            {
                turret.SetTarget(target);
                turret.ChangeState(new TurretObjAttackingState(turret));
            }
            nextCheckTime = Time.time + detectionCheckInterval;
        }
    }
    
    public override void ExitState()
    {
        
    }
}