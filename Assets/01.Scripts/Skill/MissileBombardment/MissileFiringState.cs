using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileFiringState : SkillState
{
    private MissileBombardment missileBombardment;
    private int firedMissiles = 0;
    private float fireInterval = 0.2f;
    private float nextFireTime = 0f;
    
    public MissileFiringState(Skill skill) : base(skill)
    {
        missileBombardment = skill as MissileBombardment;
    }
    
    public override void EnterState()
    {
        nextFireTime = Time.time;
    }
    
    public override void UpdateState()
    {
        if (Time.time >= nextFireTime && firedMissiles < 5)
        {
            Vector3 randomOffset = Random.insideUnitSphere * 5f;
            randomOffset.y = 0;
            missileBombardment.LaunchMissile(skill.transform.position + randomOffset);
            
            firedMissiles++;
            nextFireTime = Time.time + fireInterval;
        }
    }
    
    public override void ExitState() { }
}
