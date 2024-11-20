using System;
using UnityEngine;

/// <summary>
/// 플레이어의 데미지 처리 클래스
/// </summary>
public class PlayerDamageable : DamageableObject
{
    // 방어력당 데미지 감소율 (0.005f = 0.5%)
    private const float DEFENSE_REDUCTION_RATE = 0.005f;

    private float projectileDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
        {
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            projectileDamage = projectile.damage;
            TakeDamage(projectileDamage);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyDamage"))
        {
            MonsterDamageValue monsterDamageValue = other.gameObject.GetComponent<MonsterDamageValue>();
            
            TakeDamage(monsterDamageValue.Damage);
        }
    }
    
    public override void TakeDamage(float damage)
    {
        Debug.Log("데미지 입음");
        
        if (!IsAlive) return;

        // 방어력에 따른 데미지 감소 계산
        float damageReduction = _playerData.Defense * DEFENSE_REDUCTION_RATE;
        // 데미지 감소율이 100%를 넘지 않도록 제한
        damageReduction = Mathf.Min(damageReduction, 1f);

        // 최종 데미지 계산
        float finalDamage = damage * (1f - damageReduction);
        // 최소 데미지 보장 (0이하로 내려가지 않도록)
        finalDamage = Mathf.Max(finalDamage, 1f);

        health.DecreaseHealth(finalDamage);

        if (!IsAlive)
        {
            OnDeath();
        }
    }
    
}