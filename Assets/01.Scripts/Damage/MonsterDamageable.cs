// MonsterDamageable.cs
using UnityEngine;

public class MonsterDamageable : DamageableObject
{
    public override void TakeDamage(float damage)
    {
        if (!IsAlive) return;

        // 방어력 무시하고 데미지 직접 적용
        base.TakeDamage(damage);
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