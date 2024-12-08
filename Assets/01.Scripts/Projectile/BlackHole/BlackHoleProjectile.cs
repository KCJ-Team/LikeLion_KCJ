using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleProjectile : Projectile
{
    protected Vector3 direction;
    
    public float attractionForce = 10f;  // 끌어당기는 힘의 세기
    public float attractionRadius = 5f;  // 영향을 미치는 반경
    public float maxSpeed = 3f;  // 최대 이동 속도 제한
    public LayerMask targetLayer;
    
    private List<Rigidbody2D> attractedEnemies = new List<Rigidbody2D>();
    
    public override void Initialize(Vector3 direction, float damage)
    {
        this.direction = direction;
        base.Initialize(direction, damage);
    }

    public void attract()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attractionRadius);
        
        attractedEnemies.Clear();
        
        foreach (Collider2D hitCollider in hitColliders)
        {
            
            if (((1 << hitCollider.gameObject.layer) & targetLayer) != 0)
            {
                Rigidbody2D enemyRigidbody = hitCollider.GetComponent<Rigidbody2D>();
                
                if (enemyRigidbody != null)
                {
                    attractedEnemies.Add(enemyRigidbody);
                }
            }
        }
        
        ApplyBlackHoleAttraction();
    }
    
    private void ApplyBlackHoleAttraction()
    {
        foreach (Rigidbody2D enemyRb in attractedEnemies)
        {
            if (enemyRb != null)
            {
                // 블랙홀 중심을 향한 방향 벡터 계산
                Vector2 direction = (Vector2)transform.position - enemyRb.position;
            
                // 거리에 따라 힘 감소
                float distance = direction.magnitude;
            
                // 중심에 가까워질수록 힘을 점점 줄임
                float distanceRatio = distance / attractionRadius;
                float forceMagnitude = attractionForce * (1f - distanceRatio);
            
                // 중심 근처에서는 속도를 거의 0으로 만듦
                if (distanceRatio < 0.1f)
                {
                    enemyRb.velocity = Vector2.zero;
                }
                else
                {
                    // 힘 제한
                    forceMagnitude = Mathf.Clamp(forceMagnitude, 0, maxSpeed);

                    // 물리적 힘 적용
                    enemyRb.AddForce(direction.normalized * forceMagnitude, ForceMode2D.Force);
                }
            }
        }
    }
    
    protected override ProjectileState GetInitialState()
    {
        return new BlackHoleActiveState(this);
    }
}