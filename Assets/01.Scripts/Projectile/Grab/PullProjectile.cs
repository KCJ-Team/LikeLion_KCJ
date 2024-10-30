using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PullProjectile : Projectile
{
    [SerializeField] private float pullForce = 40f;
    [SerializeField] private float pullToDistance = 3f;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private LayerMask targetLayers;
    
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
    
    public void SetPullForce(float force)
    {
        this.pullForce = force;
    }
    
    public void SetPullToDistance(float distance)
    {
        this.pullToDistance = distance;
    }
    
    public void SetTargetLayers(LayerMask layers)
    {
        this.targetLayers = layers;
    }
    
    public Vector3 GetDirection()
    {
        return direction;
    }
    
    public float GetPullForce()
    {
        return pullForce;
    }
    
    public float GetPullToDistance()
    {
        return pullToDistance;
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
        return new PullProjectileMovingState(this);
    }
}