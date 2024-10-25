using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 버프 스킬의 상태를 관리하는 클래스
/// </summary>
public class BuffSkillState : SkillState
{
    private BuffSkill buffSkill;         // 버프 스킬 참조
    private Status targetStatus;         // 버프를 적용할 대상의 Status 컴포넌트
    
    public BuffSkillState(Skill skill) : base(skill)
    {
        buffSkill = skill as BuffSkill;
    }
    
    /// <summary>
    /// 상태 진입 시 버프 적용
    /// </summary>
    public override void EnterState()
    {
        // 플레이어의 Status 컴포넌트 가져오기
        targetStatus = GameManager.Instance.Player.GetComponent<Status>();
        
        if (targetStatus != null)
        {
            // 새로운 버프 생성
            Buff newBuff = new Buff(
                buffSkill.GetBuffId(),
                buffSkill.GetBuffName(),
                buffSkill.skillDuration,
                buffSkill.buffEffects
            );
            
            // 대상에게 버프 적용
            targetStatus.AddBuff(newBuff);
        }
    }
    
    // 버프 적용 후 바로 상태 종료
    public override void UpdateState()
    {
        skill.ChangeState(null);
    }
    
    public override void ExitState() { }
}