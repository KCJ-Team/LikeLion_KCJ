using UnityEngine;

public class RayEffect : MonoBehaviour
{
    public float laserLength = 10f;
    public LayerMask targetLayer;
    public GameObject HitEffect;
    public float HitOffset = 0;
    public bool useLaserRotation = false;

    private LineRenderer Laser;
    private ParticleSystem[] Effects;
    private ParticleSystem[] Hit;

    void Start()
    {
        Laser = GetComponent<LineRenderer>();
        Effects = GetComponentsInChildren<ParticleSystem>();
        Hit = HitEffect.GetComponentsInChildren<ParticleSystem>();
        
        // 시작할 때 HitEffect 비활성화
        HitEffect.SetActive(false);
    }

    void Update()
    {
        if (Laser == null) return;

        Laser.SetPosition(0, transform.position);
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, laserLength, targetLayer))
        {
            // 지정된 레이어와 충돌했을 때
            Laser.SetPosition(1, hit.point);
            
            // HitEffect 활성화 및 위치 설정
            HitEffect.SetActive(true);
            HitEffect.transform.position = hit.point + hit.normal * HitOffset;
            
            if (useLaserRotation)
                HitEffect.transform.rotation = transform.rotation;
            else
                HitEffect.transform.LookAt(hit.point + hit.normal);

            // Hit Effect 파티클 재생
            foreach (var effect in Effects)
            {
                if (!effect.isPlaying) effect.Play();
            }
        }
        else
        {
            // 레이저가 지정된 레이어와 충돌하지 않았을 때
            var endPos = transform.position + transform.forward * laserLength;
            Laser.SetPosition(1, endPos);
            
            // HitEffect 비활성화
            HitEffect.SetActive(false);
            
            // Hit Effect 파티클 정지
            foreach (var effect in Hit)
            {
                if (effect.isPlaying) effect.Stop();
            }
        }
    }

    public void DisableLaser()
    {
        if (Laser != null)
        {
            Laser.enabled = false;
        }

        // HitEffect 비활성화
        if (HitEffect != null)
        {
            HitEffect.SetActive(false);
        }

        if (Effects != null)
        {
            foreach (var effect in Effects)
            {
                if (effect.isPlaying) effect.Stop();
            }
        }
    }
}