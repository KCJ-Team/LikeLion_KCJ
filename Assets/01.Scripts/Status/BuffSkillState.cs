using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSkillState : SkillState
{
    private BuffSkill buffSkill;
    private Status targetStatus;
    
    public BuffSkillState(Skill skill) : base(skill)
    {
        buffSkill = skill as BuffSkill;
    }
    
    public override void EnterState()
    {
        targetStatus = GameManager.Instance.Player.GetComponent<Status>();
        
        if (targetStatus != null)
        {
            Buff newBuff = new Buff(
                buffSkill.GetBuffId(),
                buffSkill.GetBuffName(),
                buffSkill.skillDuration,
                buffSkill.buffEffects
            );
            
            targetStatus.AddBuff(newBuff);
        }
    }
    
    public override void UpdateState()
    {
        skill.ChangeState(null);
    }
    
    public override void ExitState() { }
}