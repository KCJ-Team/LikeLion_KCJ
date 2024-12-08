using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretInstallState : SkillState
{
    private Turret _turret;
    
    public TurretInstallState(Skill skill) : base(skill)
    {
        _turret = skill as Turret;
        
    }

    public override void EnterState()
    {
        _turret.TurretInstall();
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}