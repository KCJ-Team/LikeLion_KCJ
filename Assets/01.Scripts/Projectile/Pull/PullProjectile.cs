using UnityEngine;

public class PullProjectile : Projectile
{
    [SerializeField] private LayerMask pullableLayerMask;  // 끌어당길 수 있는 레이어
    [SerializeField] private float pullForce = 20f;        // 끌어당기는 힘
    [SerializeField] private float maxPullDistance = 15f;  // 최대 끌어당기기 거리
    [SerializeField] private float destroyDistance = 1f;   // 발사체가 파괴될 거리
    
    private Transform playerTransform;        // 플레이어의 Transform
    private Rigidbody targetRigidbody;       // 끌어당길 대상의 Rigidbody
    private bool isPulling = false;          // 현재 끌어당기는 중인지 여부
    
    private void Awake()
    {
        playerTransform = GameManager.Instance.Player.transform;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // 아직 끌어당기는 중이 아니고 레이어가 일치할 때만
        if (!isPulling && ((1 << other.gameObject.layer) & pullableLayerMask.value) != 0)
        {
            targetRigidbody = other.GetComponent<Rigidbody>();
            if (targetRigidbody != null)
            {
                isPulling = true;
                // 발사체의 이동을 멈춤
                Rigidbody projectileRb = GetComponent<Rigidbody>();
                if (projectileRb != null)
                {
                    projectileRb.velocity = Vector3.zero;
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
        return new PullProjectilePullingState(this);
    }
}