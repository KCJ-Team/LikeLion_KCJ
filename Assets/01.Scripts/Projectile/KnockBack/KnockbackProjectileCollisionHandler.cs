using UnityEngine;

[RequireComponent(typeof(KnockbackProjectile))]
public class KnockbackProjectileCollisionHandler : MonoBehaviour
{
    private KnockbackProjectile knockbackProjectile;
    
    private void Awake()
    {
        knockbackProjectile = GetComponent<KnockbackProjectile>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // 자신은 제외
        if (other.gameObject == gameObject) return;
        
        // 레이어 체크
        if (!knockbackProjectile.IsTargetLayer(other.gameObject)) return;
        
        // 충돌한 대상이나 그 부모에서 Rigidbody 검색
        Rigidbody targetRb = other.GetComponent<Rigidbody>();
        if (targetRb == null)
        {
            targetRb = other.GetComponentInParent<Rigidbody>();
        }
        
        if (targetRb != null)
        {
            // 넉백 효과 적용
            targetRb.AddForce(
                knockbackProjectile.GetDirection() * knockbackProjectile.GetKnockbackForce(), 
                ForceMode.Impulse
            );
            
            // 데미지 적용
            IHittable hittable = other.GetComponent<IHittable>();
            if (hittable != null)
            {
                hittable.TakeHit(knockbackProjectile.GetDamage());
            }
            
            // 히트 상태로 전환 (발사체 제거)
            knockbackProjectile.ChangeState(new KnockbackProjectileHitState(knockbackProjectile));
        }
    }
}