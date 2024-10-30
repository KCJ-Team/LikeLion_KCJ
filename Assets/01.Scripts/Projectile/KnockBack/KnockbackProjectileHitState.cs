using UnityEngine;

public class KnockbackProjectileHitState : ProjectileState
{
    private KnockbackProjectile knockbackProjectile;
    
    public KnockbackProjectileHitState(Projectile projectile) : base(projectile)
    {
        knockbackProjectile = projectile as KnockbackProjectile;
    }
    
    public override void EnterState()
    {
        // 충돌 처리는 이미 완료되었으므로 발사체 제거
        Object.Destroy(projectile.gameObject);
    }
    
    public override void UpdateState()
    {
        // 제거 상태에서는 업데이트 불필요
    }
    
    public override void ExitState()
    {
        // 제거 상태에서는 종료 로직 불필요
    }
}