using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스킬의 기본 동작을 정의하는 추상 클래스
/// 모든 구체적인 스킬 클래스는 이 클래스를 상속받아야 함
/// </summary>
public abstract class Skill : MonoBehaviour
{
    // 스킬을 소유한 캐릭터의 참조
    protected CharacterController owner;
    // 현재 스킬의 상태를 저장하는 변수
    protected SkillState currentState;
    
    // 스킬의 재사용 대기시간 (인스펙터에서 설정 가능)
    [SerializeField] protected float cooldown;
    // 현재 남은 재사용 대기시간
    protected float currentCooldown;
    
    /// <summary>
    /// 스킬 초기화 메서드
    /// 스킬이 처음 생성될 때 호출됨
    /// </summary>
    /// <param name="owner">스킬을 사용하는 캐릭터</param>
    public virtual void Initialize(CharacterController owner)
    {
        this.owner = owner;
        currentCooldown = 0;
    }
    
    /// <summary>
    /// 스킬 실행 메서드
    /// 재사용 대기시간이 0 이하일 때만 실행 가능
    /// </summary>
    public virtual void Execute()
    {
        if (currentCooldown <= 0)
        {
            // 초기 상태로 변경하고 재사용 대기시간 설정
            ChangeState(GetInitialState());
            currentCooldown = cooldown;
        }
    }
    
    // 스킬의 초기 상태를 반환하는 추상 메서드
    protected abstract SkillState GetInitialState();
    
    /// <summary>
    /// 스킬의 상태를 변경하는 메서드
    /// 이전 상태를 종료하고 새로운 상태를 시작함
    /// </summary>
    public void ChangeState(SkillState newState)
    {
        // 현재 상태가 있다면 종료
        if (currentState != null)
            currentState.ExitState();
        
        // 새로운 상태로 변경하고 시작
        currentState = newState;
        if (currentState != null)
            currentState.EnterState();
    }
    
    /// <summary>
    /// 매 프레임마다 호출되는 업데이트 메서드
    /// 재사용 대기시간을 감소시키고 현재 상태를 업데이트
    /// </summary>
    protected virtual void Update()
    {
        // 재사용 대기시간 감소
        if (currentCooldown > 0)
            currentCooldown -= Time.deltaTime;
        
        // 현재 상태 업데이트
        if (currentState != null)
            currentState.UpdateState();
    }
}