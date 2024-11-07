using UnityEngine;

/// <summary>
/// 몬스터의 데미지 처리 클래스
/// </summary>
public class MonsterDamageable : DamageableObject
{
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

        // 몬스터는 damge 그대로 받음
        health.DecreaseHealth(damage);

        if (!IsAlive)
        {
            OnDeath();
        }
    }

    protected override void OnDeath()
    {
        base.OnDeath();

        GrantRewards();
        StartCoroutine(DeathSequence());
    }

    private void GrantRewards()
    {
        // 경험치와 골드 지급
        if (GameManager.Instance.playerData != null)
        {
            // 여기에 플레이어에게 보상 지급 로직 추가
        }
    }

    private System.Collections.IEnumerator DeathSequence()
    {
        // 사망 애니메이션 또는 이펙트 재생
        // animator.SetTrigger("Die");

        // 사망 이펙트를 위한 대기 시간
        yield return new WaitForSeconds(1f);

        // 오브젝트 제거
        Destroy(gameObject);
    }
}