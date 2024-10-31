using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미사일 발사체 구현 클래스
/// Projectile을 상속받아 미사일의 특성 구현
/// </summary>
public class Missile : Projectile
{
    public float Damage { get; private set; }

    /// <summary>
    /// 미사일의 초기 상태를 설정하고 데미지 값을 저장
    /// </summary>
    /// <param name="targetPosition">목표 위치</param>
    /// <param name="damage">미사일 데미지</param>
    public void Setup(Vector3 targetPosition, float damage)
    {
        Damage = damage;
        ChangeState(new MissileMovingState(this, targetPosition, damage));
    }

    /// <summary>
    /// 미사일의 기본 상태를 MissileMovingState로 설정
    /// </summary>
    protected override ProjectileState GetInitialState()
    {
        // Setup 메서드를 통해 상태가 설정되므로 null 반환
        return null;
    }

    /// <summary>
    /// 미사일이 파괴될 때 호출되는 메서드
    /// </summary>
    private void OnDestroy()
    {
        // TODO: 파티클 효과나 사운드 등을 처리
    }
}