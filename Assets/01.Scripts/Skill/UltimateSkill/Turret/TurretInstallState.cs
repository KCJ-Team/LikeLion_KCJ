using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretInstallState : SkillState
{
    private Turret _turret;
    private Vector3 targetPosition;
    
    public TurretInstallState(Skill skill, Vector3 target) : base(skill)
    {
        _turret = skill as Turret;
        targetPosition = target;
    }

    public override void EnterState()
    {
        _turret.TurretInstall(targetPosition);
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}