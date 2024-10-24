using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffSkill : Skill
{
    public float skillDuration = 5f;
    public List<BuffEffect> buffEffects = new List<BuffEffect>();
    
    protected override SkillState GetInitialState()
    {
        return new BuffSkillState(this);
    }

    public abstract string GetBuffId();
    public abstract string GetBuffName();
}
