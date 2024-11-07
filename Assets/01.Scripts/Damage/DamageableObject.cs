using UnityEngine;

public class DamageableObject : MonoBehaviour, IDamageable
{
    protected Health health;
    protected PlayerData _playerData;
    
    // 생존 여부 확인 프로퍼티
    public bool IsAlive => health.CurrentHealth > 0;
    
    // 방어력당 데미지 감소율 (0.005f = 0.5%)
    private const float DEFENSE_REDUCTION_RATE = 0.005f;

    private void Awake()
    {
        _playerData = GameManager.Instance.playerData;
        health = GetComponent<Health>();
    }

    // 데미지를 받는 메서드
    public virtual void TakeDamage(float damage)
    {
        if (!IsAlive) return;
        
        health.DecreaseHealth(damage);
        
        if (!IsAlive)
        {
            OnDeath();
        }
    }

    // 치유를 받는 메서드
    public void TakeHealing(float healing)
    {
        if (!IsAlive) return;
        
        health.IncreaseHealth(healing);
    }

    // 사망 시 호출되는 가상 메서드
    protected virtual void OnDeath()
    {
        
    }
}