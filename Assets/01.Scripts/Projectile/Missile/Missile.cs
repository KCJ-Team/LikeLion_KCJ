using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 미사일 발사체 구현 클래스
// Projectile을 상속받아 미사일의 특성 구현
public class Missile : Projectile
{
    public float Damage { get; private set; }
    
    // 미사일의 초기 상태를 설정하고 데미지 값을 저장
    // <param name="targetPosition">목표 위치</param>
    // <param name="damage">미사일 데미지</param>
    public void Setup(Vector3 targetPosition, float damage)
    {
        Damage = damage;
        ChangeState(new MissileMovingState(this, targetPosition, damage));
    }
    
    // 미사일의 기본 상태를 MissileMovingState로 설정
    protected override ProjectileState GetInitialState()
    {
        return null;
    }
}