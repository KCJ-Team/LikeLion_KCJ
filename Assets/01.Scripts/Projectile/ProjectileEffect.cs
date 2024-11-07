using UnityEngine;

public class ProjectileEffect : MonoBehaviour 
{
    public GameObject muzzlePrefab;
    public GameObject hitPrefab;
    public LayerMask targetLayer;

    void Start()
    {
        PlayMuzzleEffect();
    }

    void PlayMuzzleEffect()
    {
        if (muzzlePrefab != null)
        {
            var muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.forward = gameObject.transform.forward;
            
            var psMuzzle = muzzleVFX.GetComponent<ParticleSystem>();
            if (psMuzzle != null)
            {
                Destroy(muzzleVFX, psMuzzle.main.duration);
            }
            else
            {
                var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(muzzleVFX, psChild.main.duration);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 레이어 마스크와 충돌 오브젝트의 레이어 비교
        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            // 충돌 지점과 방향 계산
            Vector3 pos = transform.position;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, -transform.forward);

            if (hitPrefab != null)
            {
                var hitVFX = Instantiate(hitPrefab, pos, rot);
                var psHit = hitVFX.GetComponent<ParticleSystem>();
                if (psHit != null)
                {
                    Destroy(hitVFX, psHit.main.duration);
                }
                else
                {
                    var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(hitVFX, psChild.main.duration);
                }
            }
            Destroy(gameObject);
        }
    }
}