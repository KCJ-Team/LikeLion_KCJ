using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미사일 발사 전 차징 상태를 구현한 클래스
/// 일정 시간 동안 차징 후 발사 상태로 전환
/// </summary>
public class MissileChargingState : SkillState
{
    private float chargeTime = 1f;           // 총 차징 시간
    private float currentChargeTime = 0f;    // 현재 차징된 시간
    private MissileBombardment missileBombardment;
    
    public MissileChargingState(Skill skill) : base(skill)
    {
        missileBombardment = skill as MissileBombardment;
    }
    
    // 상태 시작 시 차징 시간 초기화
    public override void EnterState()
    {
        currentChargeTime = 0f;
    }
    
    // 차징 시간을 누적하고, 완료되면 발사 상태로 전환
    public override void UpdateState()
    {
        currentChargeTime += Time.deltaTime;
        if (currentChargeTime >= chargeTime)
        {
            skill.ChangeState(new MissileFiringState(skill));
        }
    }
    
    public override void ExitState() { }
}