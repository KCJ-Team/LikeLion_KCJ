using UnityEngine;

public abstract class SkillState
{
    protected Skill skill;
    
    public SkillState(Skill skill)
    {
        this.skill = skill;
    }
    
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}