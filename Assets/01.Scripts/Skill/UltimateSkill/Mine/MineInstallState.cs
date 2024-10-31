using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineInstallState : SkillState
{
    private Mine _mine;
    private Vector3 targetPosition;
    
    public MineInstallState(Skill skill, Vector3 target) : base(skill)
    {
        _mine = skill as Mine;
        targetPosition = target;
    }

    public override void EnterState()
    {
        _mine.MineInstall(targetPosition);
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
