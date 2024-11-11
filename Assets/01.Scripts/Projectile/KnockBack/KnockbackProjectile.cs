using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class KnockbackProjectile : Projectile
{
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private LayerMask targetLayers; // 충돌 대상 레이어 마스크
    
    private Vector3 direction;
    private Rigidbody rb;
    
    public override void Initialize(Vector3 direction, float damage)
    {
        this.direction = direction;
        base.Initialize(direction, damage);
        
        Destroy(gameObject, lifeTime);
    }
    
    protected override ProjectileState GetInitialState()
    {
        return new KnockbackProjectileMovingState(this, direction);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // 레이어 마스크와 충돌한 오브젝트의 레이어를 비교
        if (((1 << other.gameObject.layer) & targetLayers) != 0)
        {
            // 충돌한 오브젝트의 Rigidbody 컴포넌트 가져오기
            Rigidbody targetRb = other.GetComponent<Rigidbody>();
            
            if (targetRb != null)
            {
                // 발사체의 진행 방향으로 넉백 적용
                Vector3 knockbackDirection = direction.normalized;
                
                // 넉백 힘 적용
                targetRb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            }
            
            // 충돌 후 상태 변경
            ChangeState(new KnockbackProjectileHitState(this));
        }
    }
}