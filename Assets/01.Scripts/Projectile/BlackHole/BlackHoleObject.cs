using UnityEngine;

public class BlackHoleObject : Projectile
{
    [SerializeField] private LayerMask attractableLayer;    // 끌어당길 레이어
    [SerializeField] private float attractRadius = 10f;     // 흡입 범위
    [SerializeField] private float attractForce = 20f;      // 흡입력
    [SerializeField] private float duration = 5f;           // 지속 시간
    [SerializeField] private ParticleSystem blackHoleEffect;// 블랙홀 이펙트

    private float creationTime;
    
    private void Start()
    {
        Initialize(Vector3.zero, 0f);
        creationTime = Time.time;
    }

    protected override ProjectileState GetInitialState()
    {
        return new BlackHoleActiveState(this);
    }

    // 주변 물체들을 끌어당기는 메서드
    public void AttractObjects()
    {
        // 범위 내의 모든 콜라이더 검출
        Collider[] colliders = Physics.OverlapSphere(transform.position, attractRadius, attractableLayer);

        foreach (Collider col in colliders)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // 물체와 블랙홀 사이의 방향과 거리 계산
                Vector3 direction = transform.position - col.transform.position;
                float distance = direction.magnitude;
                
                // 거리에 따른 흡입력 계산 (거리가 가까울수록 더 강한 힘)
                float forceMagnitude = attractForce * (1 - (distance / attractRadius));
                forceMagnitude = Mathf.Max(forceMagnitude, 0); // 음수 방지
                
                // 힘 적용
                rb.AddForce(direction.normalized * forceMagnitude, ForceMode.Force);
            }
        }
    }

    // 블랙홀의 수명이 다했는지 확인
    public bool IsExpired()
    {
        return Time.time - creationTime >= duration;
    }

    // 블랙홀 제거
    public void DestroyBlackHole()
    {
        if (blackHoleEffect != null)
        {
            // 파티클 시스템 정지 및 점진적 소멸
            blackHoleEffect.Stop();
            Destroy(gameObject, blackHoleEffect.main.duration);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 게터 메서드들
    public float GetAttractRadius() => attractRadius;
    public float GetDuration() => duration;

    // 시각적 디버깅을 위한 기즈모
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attractRadius);
    }
}