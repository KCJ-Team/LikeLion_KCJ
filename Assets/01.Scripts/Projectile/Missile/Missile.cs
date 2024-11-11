using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    [SerializeField] protected LayerMask targetLayers; // 충돌 대상 레이어 마스크
    [SerializeField] protected LayerMask GroundLayers;
    private Vector3 targetPos;

    public override void Initialize(Vector3 direction, float damage)
    {
        targetPos = direction;
        this.damage = damage;
        base.Initialize(direction, damage);
    }


    // 미사일의 기본 상태를 MissileMovingState로 설정
    protected override ProjectileState GetInitialState()
    {
        return new MissileMovingState(this, targetPos, damage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & GroundLayers) != 0)
        {
            ChangeState(new MissileExplosionState(this, targetLayers));
            
        }
    }
}