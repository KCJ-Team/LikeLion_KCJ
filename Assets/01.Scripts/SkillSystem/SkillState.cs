using UnityEngine;

/// <summary>
/// 스킬의 상태를 나타내는 추상 클래스
/// State 패턴을 구현하여 스킬의 다양한 단계를 관리
/// </summary>
public abstract class SkillState
{
    // 이 상태가 속한 스킬의 참조
    protected Skill skill;
    
    public SkillState(Skill skill)
    {
        this.skill = skill;
    }
    
    // 상태에 진입할 때 호출되는 메서드
    public abstract void EnterState();
    // 상태가 실행 중일 때 매 프레임마다 호출되는 메서드
    public abstract void UpdateState();
    // 상태가 종료될 때 호출되는 메서드
    public abstract void ExitState();
}
