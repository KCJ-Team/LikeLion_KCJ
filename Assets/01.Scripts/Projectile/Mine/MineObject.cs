using UnityEngine;

public class MineObject : Projectile
{
    [SerializeField] private LayerMask targetLayer;        // 폭발을 트리거할 레이어
    [SerializeField] private float explosionRadius = 5f;   // 폭발 범위
    [SerializeField] private float explosionDuration = 1f; // 폭발 지속 시간
    [SerializeField] private ParticleSystem explosionEffect; // 폭발 이펙트
    
    private bool hasExploded = false;

    private void Start()
    {
        Initialize(Vector3.zero, 0f); // 데미지는 사용하지 않으므로 0으로 설정
    }

    protected override ProjectileState GetInitialState()
    {
        return new MineExplosionState(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 특정 레이어의 물체가 닿았는지 확인
        if (!hasExploded && (targetLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            Explode();
        }
    }

    public void Explode()
    {
        if (!hasExploded)
        {
            hasExploded = true;
            
            // 폭발 이펙트 재생
            if (explosionEffect != null)
            {
                explosionEffect.Play();
            }

            // 일정 시간 후 파괴
            Destroy(gameObject, explosionDuration);
        }
    }

    // 폭발 범위를 시각적으로 표시
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    // 게터 메서드들
    public bool HasExploded() => hasExploded;
    public float GetExplosionDuration() => explosionDuration;
}