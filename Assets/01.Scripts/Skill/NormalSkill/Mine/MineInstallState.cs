using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineInstallState : SkillState
{
    private Mine mine;
    
    public MineInstallState(Skill skill) : base(skill)
    {
        mine = skill as Mine;
    }

    public override void EnterState()
    {
        mine.MineInstall();
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
