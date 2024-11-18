using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainBulletAttackState : SkillState
{
    private RainBullet rainBullet;
    private float elapsedTime = 0f;
    private float damageTickInterval = 0.5f;
    private float lastDamageTime = 0f;
    private float damage;
    private ParticleSystem activeEffect;
    private bool isAnimationPlaying = false;

    public RainBulletAttackState(Skill skill, float damage) : base(skill)
    {
        rainBullet = skill as RainBullet;
        this.damage = damage;
    }

    public override void EnterState()
    {
        elapsedTime = 0f;
        lastDamageTime = 0f;
    }

    public override void UpdateState()
    {
        elapsedTime += Time.deltaTime;

        if (Time.time - lastDamageTime >= damageTickInterval)
        {
            ApplyDamage();
            lastDamageTime = Time.time;
        }
        
        if (elapsedTime >= rainBullet.duration)
        {
            Object.Destroy(rainBullet.gameObject);
            rainBullet.ChangeState(null);
        }
    }

    public override void ExitState()
    {
        
    }

    private void ApplyDamage()
    {
        Collider[] hitColliders = Physics.OverlapSphere(rainBullet.transform.position, rainBullet.Radius, rainBullet.TargetLayer);

        foreach (var hitCollider in hitColliders)
        {
            DamageableObject damageableObject = hitCollider.GetComponent<DamageableObject>();
            if (damageableObject != null)
            {
                damageableObject.TakeDamage(damage);
            }
        }
    }
}