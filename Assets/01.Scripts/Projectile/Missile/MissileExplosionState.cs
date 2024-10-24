using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplosionState : ProjectileState
{
    private float explosionDuration = 0.5f;
    private float currentTime = 0f;
    
    public MissileExplosionState(Projectile projectile) : base(projectile) { }
    
    public override void EnterState()
    {
        // 폭발 이펙트 생성
        currentTime = 0f;
    }
    
    public override void UpdateState()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= explosionDuration)
        {
            Object.Destroy(projectile.gameObject);
        }
    }
    
    public override void ExitState() { }
}
