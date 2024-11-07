public interface IDamageable
{
    void TakeDamage(float damage);    // 데미지를 받는 메서드
    void TakeHealing(float healing);   // 치유를 받는 메서드
    bool IsAlive { get; }             // 생존 여부를 확인하는 프로퍼티
}