using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileChargingState : SkillState
{
    private float chargeTime = 1f;
    private float currentChargeTime = 0f;
    private MissileBombardment missileBombardment;
    
    public MissileChargingState(Skill skill) : base(skill)
    {
        missileBombardment = skill as MissileBombardment;
    }
    
    public override void EnterState()
    {
        currentChargeTime = 0f;
    }
    
    public override void UpdateState()
    {
        currentChargeTime += Time.deltaTime;
        if (currentChargeTime >= chargeTime)
        {
            skill.ChangeState(new MissileFiringState(skill));
        }
    }
    
    public override void ExitState() { }
}
