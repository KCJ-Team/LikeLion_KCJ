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
        // Projectile 레이어의 오브젝트와 충돌했을 때
        if (other.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            // Projectile.cs에서 설정한 damage 값을 가져와 TakeDamage 메소드에 전달
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            projectileDamage = projectile.damage;
            TakeDamage(projectileDamage);
        }
    }

    public override void TakeDamage(float damage)
    {
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

    protected override void OnDeath()
    {
        base.OnDeath();
        // 플레이어 사망 시 추가 처리
    }
}