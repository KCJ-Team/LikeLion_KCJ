using System.Collections;
using UnityEngine;

public class MissileFiringState : SkillState
{
    private MissileBombing missileBombing;
    
    public MissileFiringState(Skill skill) : base(skill)
    {
        missileBombing = skill as MissileBombing;
        
    }

    public override void EnterState()
    {
        missileBombing.FireMissile();
    }
    
    public override void UpdateState()
    {
        
    }
    
    public override void ExitState() 
    {
        
    }
}