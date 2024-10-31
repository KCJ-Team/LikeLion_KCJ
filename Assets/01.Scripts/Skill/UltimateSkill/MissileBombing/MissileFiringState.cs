using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미사일을 실제로 발사하는 상태를 구현한 클래스
/// 지정된 위치에 일정 간격으로 여러 발의 미사일을 발사
/// </summary>
public class MissileFiringState : SkillState
{
    private MissileBombing _missileBombing;
    private int firedMissiles = 0;           // 현재까지 발사된 미사일 수
    private float fireInterval = 0.2f;        // 미사일 발사 간격
    private float nextFireTime = 0f;          // 다음 발사 시간
    private Vector3 targetPosition;           // 미사일이 발사될 목표 위치
    
    public MissileFiringState(Skill skill, Vector3 target) : base(skill)
    {
        _missileBombing = skill as MissileBombing;
        targetPosition = target;
    }
    
    // 상태 시작 시 첫 발사 시간 설정
    public override void EnterState()
    {
        nextFireTime = Time.time;
    }
    
    // 일정 간격으로 미사일 발사
    public override void UpdateState()
    {
        // 발사 시간이 되었고 아직 발사할 미사일이 남았다면
        if (Time.time >= nextFireTime && firedMissiles < 5)
        {
            // 지정된 위치에 미사일 발사
            _missileBombing.LaunchMissile(targetPosition);
            
            // 발사 카운트 증가 및 다음 발사 시간 설정
            firedMissiles++;
            nextFireTime = Time.time + fireInterval;
        }
    }
    
    public override void ExitState() { }
}