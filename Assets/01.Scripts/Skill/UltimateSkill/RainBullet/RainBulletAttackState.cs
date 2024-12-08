using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainBulletAttackState : SkillState
{
    private RainBullet rainBullet;
    private float damage;

    public RainBulletAttackState(Skill skill, float damage) : base(skill)
    {
        rainBullet = skill as RainBullet;
        this.damage = damage;
    }

    public override void EnterState()
    {
       //rainBullet.FireBullet();
       rainBullet.StartDamageProcess();
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}