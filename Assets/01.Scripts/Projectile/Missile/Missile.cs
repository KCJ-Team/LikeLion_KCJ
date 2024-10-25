using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미사일 발사체 구현 클래스
/// Projectile을 상속받아 미사일의 특성 구현
/// </summary>
public class Missile : Projectile
{
    /// <summary>
    /// 미사일의 초기 상태를 MissileMovingState로 설정
    /// </summary>
    protected override ProjectileState GetInitialState()
    {
        return new MissileMovingState(this);
    }
}
