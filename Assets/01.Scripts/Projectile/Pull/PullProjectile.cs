using UnityEngine;

public class PullProjectile : Projectile
{
    [SerializeField] private LayerMask pullableLayerMask;
    [SerializeField] private float pullForce = 20f;
    [SerializeField] private float maxPullDistance = 15f;
    [SerializeField] private float destroyDistance = 1f;
    
    private Transform playerTransform;
    private Rigidbody targetRigidbody;
    private bool isPulling = false;
    
    protected Vector3 direction;
    
    private void Awake()
    {
        playerTransform = GameManager.Instance.Player.transform;
    }

    public override void Initialize(Vector3 direction, float damage)
    {
        this.direction = direction;
        this.damage = damage;
        base.Initialize(direction, damage);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 이미 끌어당기는 중이면 무시
        if (isPulling) return;
        
        HandleCollision(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 이미 끌어당기는 중이면 무시
        if (isPulling) return;
        
        HandleCollision(other.gameObject);
    }

    private void HandleCollision(GameObject hitObject)
    {
        // 레이어 마스크 체크
        if (((1 << hitObject.layer) & pullableLayerMask.value) != 0)
        {
            // Rigidbody 가져오기
            targetRigidbody = hitObject.GetComponent<Rigidbody>();
            if (targetRigidbody == null)
            {
                targetRigidbody = hitObject.GetComponentInParent<Rigidbody>();
            }

            if (targetRigidbody != null)
            {
                isPulling = true;
                
                // 발사체의 물리 동작 조정
                Rigidbody projectileRb = GetComponent<Rigidbody>();
                if (projectileRb != null)
                {
                    projectileRb.velocity = Vector3.zero;
                    projectileRb.isKinematic = true;
                }

                // PullingState로 전환
                ChangeState(new PullProjectilePullingState(this));
            }
        }
    }
    
    public bool IsPulling() => isPulling;
    public Transform GetPlayerTransform() => playerTransform;
    public Rigidbody GetTargetRigidbody() => targetRigidbody;
    public float GetPullForce() => pullForce;
    public float GetMaxPullDistance() => maxPullDistance;
    public float GetDestroyDistance() => destroyDistance;
    
    protected override ProjectileState GetInitialState()
    {
        return new PullProjectileMovingState(this, direction);
    }
}