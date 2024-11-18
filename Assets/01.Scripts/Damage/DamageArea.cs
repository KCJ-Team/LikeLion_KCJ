using UnityEngine;
using System.Collections;

public class DamageArea : MonoBehaviour
{
    private float damage;
    private float radius;
    private float interval;
    private float duration;
    private LayerMask targetLayer;
    private float startTime;

    public void Initialize(float damage, float radius, float interval, float duration, LayerMask targetLayer)
    {
        this.damage = damage;
        this.radius = radius;
        this.interval = interval;
        this.duration = duration;
        this.targetLayer = targetLayer;
        this.startTime = Time.time;

        StartCoroutine(DealDamageOverTime());
    }

    private IEnumerator DealDamageOverTime()
    {
        while (Time.time - startTime < duration)
        {
            // 범위 내의 모든 대상 검출
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, targetLayer);
            
            foreach (var hitCollider in hitColliders)
            {
                // IDamageable 인터페이스를 구현한 컴포넌트 검색
                IDamageable damageable = hitCollider.GetComponent<IDamageable>();
                if (damageable != null && damageable.IsAlive)
                {
                    // 데미지 적용
                    damageable.TakeDamage(damage);
                }
            }

            yield return new WaitForSeconds(interval);
        }

        // 지속시간이 끝나면 이펙트 오브젝트 제거
        Destroy(gameObject);
    }

    // 디버그용 기즈모 그리기 (데미지 범위 시각화)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}