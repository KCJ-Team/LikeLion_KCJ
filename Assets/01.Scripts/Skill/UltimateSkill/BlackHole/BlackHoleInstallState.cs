using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleInstallState : SkillState
{
    private BlackHole blackHole;
    
    public BlackHoleInstallState(Skill skill) : base(skill)
    {
        blackHole = skill as BlackHole;
    }

    public override void EnterState()
    {
        blackHole.BlackHoleInstall();
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}
