using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class KnockbackProjectile : Projectile
{
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private LayerMask targetLayers; // 충돌 대상 레이어 마스크
    
    private Vector3 direction;
    private Rigidbody rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        rb.useGravity = false;
        rb.isKinematic = true;
        
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }
    
    public override void Initialize(Vector3 direction, float damage)
    {
        this.direction = direction;
        base.Initialize(direction, damage);
        
        Destroy(gameObject, lifeTime);
    }
    
    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
    
    public void SetKnockbackForce(float force)
    {
        this.knockbackForce = force;
    }
    
    public void SetTargetLayers(LayerMask layers)
    {
        this.targetLayers = layers;
    }
    
    public Vector3 GetDirection()
    {
        return direction;
    }
    
    public float GetKnockbackForce()
    {
        return knockbackForce;
    }
    
    public float GetDamage()
    {
        return damage;
    }
    
    public LayerMask GetTargetLayers()
    {
        return targetLayers;
    }
    
    public bool IsTargetLayer(GameObject obj)
    {
        return ((1 << obj.layer) & targetLayers) != 0;
    }
    
    protected override ProjectileState GetInitialState()
    {
        return new KnockbackProjectileMovingState(this);
    }
}