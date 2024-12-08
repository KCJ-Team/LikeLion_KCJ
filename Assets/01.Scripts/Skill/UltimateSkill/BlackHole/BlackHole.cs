using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class BlackHole : Skill
{
    private PlayerDecteting playerDecteting;
    public float spawnRadius;
    private GameObject currentBlackhole;
    
    [SerializeField] private GameObject blackHolePrefab;
    
    private void Awake()
    {
        spawnRadius = GameManager.Instance.playerData.currentWeapon.attackRange;
    }
    
    public void BlackHoleInstall()
    {
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(0f, spawnRadius);
        
        float angleRad = angle * Mathf.Deg2Rad;
        
        Vector2 spawnPosition = new Vector2(
            transform.position.x + Mathf.Cos(angleRad) * distance,
            transform.position.y + Mathf.Sin(angleRad) * distance
        );
        
        currentBlackhole = Instantiate(blackHolePrefab, spawnPosition, Quaternion.identity);

        if (currentBlackhole.TryGetComponent<BlackHoleProjectile>(out BlackHoleProjectile projectile))
        {
            projectile.Initialize(Vector3.zero, damage);
        }
        
        Destroy(gameObject);
    }
    
    public override SkillState GetInitialState()
    {
        return new BlackHoleInstallState(this);
    }
}
