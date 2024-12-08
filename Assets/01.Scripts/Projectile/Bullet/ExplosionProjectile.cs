using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ExplosionProjectile : Projectile
{
    [SerializeField] private float explosionRadius;
    [SerializeField] private LayerMask targetLayers;

    protected Vector3 direction;
    
    public override void Initialize(Vector3 direction, float damage)
    {
        this.direction = direction;
        base.Initialize(direction, damage);
    }
    
    protected override ProjectileState GetInitialState()
    {
        return new ProjectileMovingState(this, direction);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & targetLayers) != 0)
        {
            Explosion();
            Destroy(gameObject);
        }
    }

    private void Explosion()
    {
        //터지는 이펙트 적용
        //Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, targetLayers);

        foreach (Collider2D hitCollider in hitColliders)
        {
            MonsterDamageable damageable = hitCollider.gameObject.GetComponent<MonsterDamageable>();
            
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
            
            Debug.Log(hitCollider.gameObject.name);
        }
    }
}
