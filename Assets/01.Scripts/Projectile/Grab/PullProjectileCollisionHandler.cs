using UnityEngine;

[RequireComponent(typeof(PullProjectile))]
public class PullProjectileCollisionHandler : MonoBehaviour
{
    private PullProjectile pullProjectile;
    
    private void Awake()
    {
        pullProjectile = GetComponent<PullProjectile>();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == gameObject) return;
        
        if (!pullProjectile.IsTargetLayer(other.gameObject)) return;
        
        Rigidbody targetRb = other.GetComponent<Rigidbody>();
        if (targetRb == null)
        {
            targetRb = other.GetComponentInParent<Rigidbody>();
        }
        
        if (targetRb != null)
        {
            // 플레이어 위치 가져오기
            Transform playerTransform = GameManager.Instance.Player.transform;
            
            // 플레이어 앞쪽 위치 계산 (Y축은 무시)
            Vector3 pullToPosition = playerTransform.position + 
                (playerTransform.forward * pullProjectile.GetPullToDistance());
            
            // PullTarget 컴포넌트 추가 및 설정
            PullTarget pullTarget = other.gameObject.AddComponent<PullTarget>();
            if (pullTarget != null)
            {
                // XZ 평면에서의 거리만 계산
                Vector3 currentPosXZ = new Vector3(targetRb.position.x, 0f, targetRb.position.z);
                Vector3 targetPosXZ = new Vector3(pullToPosition.x, 0f, pullToPosition.z);
                float distance = Vector3.Distance(currentPosXZ, targetPosXZ);
                
                float pullSpeed = pullProjectile.GetPullForce() * (distance / 10f);
                pullSpeed = Mathf.Clamp(pullSpeed, 5f, pullProjectile.GetPullForce());
                
                pullTarget.StartPull(pullToPosition, pullSpeed);
            }
            
            // 데미지 적용
            IHittable hittable = other.GetComponent<IHittable>();
            if (hittable != null)
            {
                hittable.TakeHit(pullProjectile.GetDamage());
            }
            
            pullProjectile.ChangeState(new PullProjectileHitState(pullProjectile));
        }
    }
}