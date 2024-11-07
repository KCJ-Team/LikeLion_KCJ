using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleInstallState : SkillState
{
    private BlackHole _blackHole;
    private Vector3 targetPosition;
    
    public BlackHoleInstallState(Skill skill, Vector3 target) : base(skill)
    {
        _blackHole = skill as BlackHole;
        targetPosition = target;
    }

    public override void EnterState()
    {
        _blackHole.BlackHoleInstall(targetPosition);
    }

    public override void UpdateState()
    {
        Object.Destroy(_blackHole.gameObject);
    }

    public override void ExitState()
    {
        
    }
}
