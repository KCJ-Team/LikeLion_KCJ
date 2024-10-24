using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMovingState : ProjectileState
{
    private Vector3 direction;
    
    public MissileMovingState(Projectile projectile) : base(projectile) { }
    
    public override void EnterState()
    {
        direction = (projectile.transform.position - Vector3.up * 10f).normalized;
    }
    
    public override void UpdateState()
    {
        projectile.transform.position += direction * Time.deltaTime * 20f;
        
        RaycastHit hit;
        if (Physics.Raycast(projectile.transform.position, direction, out hit, 0.5f))
        {
            projectile.ChangeState(new MissileExplosionState(projectile));
        }
    }
    
    public override void ExitState() { }
}
