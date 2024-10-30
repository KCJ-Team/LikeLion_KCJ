using UnityEngine;

public class PullTarget : MonoBehaviour
{
    private Vector3 targetPosition;
    private bool isPulling = false;
    private float pullSpeed;
    private Rigidbody rb;
    private float originalY; // 원래 Y 위치 저장
    private System.Action onPullComplete;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        if (isPulling)
        {
            // XZ 평면에서의 거리만 계산
            Vector3 currentPosXZ = new Vector3(transform.position.x, targetPosition.y, transform.position.z);
            Vector3 targetPosXZ = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);
            float distanceToTarget = Vector3.Distance(currentPosXZ, targetPosXZ);
            
            if (distanceToTarget > 0.1f)
            {
                // XZ 평면에서만 이동 방향 계산
                Vector3 moveDirection = (targetPosXZ - currentPosXZ).normalized;
                
                // Y 속도는 0으로 유지하면서 XZ 평면에서만 이동
                Vector3 newVelocity = moveDirection * pullSpeed;
                newVelocity.y = 0f; // Y축 속도를 0으로 설정
                rb.velocity = newVelocity;
                
                // Y 위치 고정
                transform.position = new Vector3(transform.position.x, originalY, transform.position.z);
            }
            else
            {
                StopPull();
            }
        }
    }
    
    public void StartPull(Vector3 destination, float speed, System.Action onComplete = null)
    {
        originalY = transform.position.y; // 현재 Y 위치 저장
        targetPosition = new Vector3(destination.x, originalY, destination.z); // 목표 위치의 Y값을 현재 Y값으로 설정
        pullSpeed = speed;
        isPulling = true;
        onPullComplete = onComplete;
        
        if (rb != null)
        {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY; // Y축 이동 추가로 고정
        }
    }
    
    public void StopPull()
    {
        isPulling = false;
        
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = true;
            rb.constraints = RigidbodyConstraints.None;
        }
        
        onPullComplete?.Invoke();
        Destroy(this);
    }
}