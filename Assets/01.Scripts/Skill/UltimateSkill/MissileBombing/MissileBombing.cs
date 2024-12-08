using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class MissileBombing : Skill
{
    private PlayerDecteting playerDecteting;

    [SerializeField] private int missileCount = 3; // 미사일 개수
    [SerializeField] private float spawnInterval = 0.5f; // 미사일 생성 간격
    [SerializeField] private GameObject missilePrefab; // 미사일 프리팹

    private void Awake()
    {
        playerDecteting = GameManager.Instance.Player.GetComponent<PlayerDecteting>();
    }

    public void FireMissile()
    {
        StartCoroutine(SpawnMissilesWithInterval());
    }

    private IEnumerator SpawnMissilesWithInterval()
    {
        for (int i = 0; i < missileCount; i++)
        {
            if (playerDecteting.nearestTarget != null)
            {
                GameObject spawnedMissile = Instantiate(missilePrefab, transform.position, Quaternion.identity);
            
                Vector3 targetPosition = playerDecteting.nearestTarget.transform.position;
                Vector2 direction = (targetPosition - transform.position).normalized;

                // 회전 계산
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);
            
                spawnedMissile.transform.rotation = rotation;
            
                if (spawnedMissile.TryGetComponent<ExplosionProjectile>(out ExplosionProjectile projectile))
                {
                    float finalDamage = damage;
                    projectile.Initialize(direction, finalDamage);
                }
            }
            
            yield return new WaitForSeconds(spawnInterval);
        }
        
        Destroy(gameObject);
    }

    public override SkillState GetInitialState()
    {
        return new MissileFiringState(this);
    }
}