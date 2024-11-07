using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainBulletAttackState : SkillState
{
    private RainBullet rainBullet;
    private float elapsedTime = 0f;
    private float damageTickInterval = 0.5f;  // 데미지를 주는 간격
    private float lastDamageTime = 0f;

    public RainBulletAttackState(Skill skill) : base(skill)
    {
        rainBullet = skill as RainBullet;
    }

    public override void EnterState()
    {
        elapsedTime = 0f;
        lastDamageTime = 0f;
    }

    public override void UpdateState()
    {
        elapsedTime += Time.deltaTime;

        // 일정 간격으로 데미지 처리
        if (Time.time - lastDamageTime >= damageTickInterval)
        {
            ApplyDamage();
            lastDamageTime = Time.time;
        }

        // 지속시간이 끝나면 스킬 종료
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
        // 범위 내의 모든 대상을 찾음
        Collider[] hitColliders = Physics.OverlapSphere(
            rainBullet.transform.position, 
            rainBullet.Radius, 
            rainBullet.TargetLayer
        );

        // 찾은 모든 대상에게 데미지를 줌
        foreach (var hitCollider in hitColliders)
        {
            // // IHealth 인터페이스를 구현한 컴포넌트를 찾음
            DamageableObject damageableObject = hitCollider.GetComponent<DamageableObject>();
            if (damageableObject != null)
            {
                damageableObject.TakeDamage(rainBullet.Damage);
            }
        }
    }
}
