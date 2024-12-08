using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinePreparingState : SkillState
{
    private Mine mine;
    
    public MinePreparingState(Skill skill) : base(skill)
    {
        mine = skill as Mine;
    }

    public override void EnterState()
    {
        
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
