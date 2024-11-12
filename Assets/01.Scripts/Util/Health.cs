using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] protected float maxHealth;     
    [SerializeField] private float currentHealth;

    public bool IsDead;

    // 읽기 전용 프로퍼티
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    // 이벤트 선언
    public UnityEvent<float> OnHealthChanged;        // 체력 변경 시 발생하는 이벤트
    public UnityEvent<float> OnMaxHealthChanged;     // 최대 체력 변경 시 발생하는 이벤트
    public UnityEvent OnHealthDepleted;              // 체력이 0이 되었을 때 발생하는 이벤트

    protected virtual void Start()
    {
        // 초기 체력을 최대 체력으로 설정
        currentHealth = maxHealth;
    }

    // 최대 체력을 설정하는 메서드
    public void SetMaxHealth(float newMaxHealth)
    {
        maxHealth = Mathf.Max(1, newMaxHealth);  // 최대 체력은 최소 1
        OnMaxHealthChanged?.Invoke(maxHealth);
        
        // 현재 체력이 최대 체력을 넘지 않도록 조정
        currentHealth = Mathf.Min(currentHealth, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }

    // 체력을 감소시키는 메서드
    public void DecreaseHealth(float damage)
    {
        if (damage < 0) return;  // 음수 데미지 방지

        currentHealth = Mathf.Max(0, currentHealth - damage);
        OnHealthChanged?.Invoke(currentHealth);

        // 체력이 0 이하가 되면 사망 이벤트 발생
        if (currentHealth <= 0)
        {
            IsDead = true;
            OnHealthDepleted?.Invoke();
        }
        else
        {
            IsDead = false;
        }
    }

    // 체력을 증가시키는 메서드
    public void IncreaseHealth(float healing)
    {
        if (healing < 0) return;  // 음수 회복량 방지

        currentHealth = Mathf.Min(maxHealth, currentHealth + healing);
        OnHealthChanged?.Invoke(currentHealth);
    }

    // 체력을 완전히 회복하는 메서드
    public void RestoreFullHealth()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth);
    }
}