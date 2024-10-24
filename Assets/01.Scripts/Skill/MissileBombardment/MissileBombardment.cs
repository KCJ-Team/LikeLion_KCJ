using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileBombardment : Skill
{
    [Header("Missile Settings")]
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private int missileCount = 5;
    [SerializeField] private float damage = 50f;
    [SerializeField] private float missileSpeed = 20f;
    [SerializeField] private float explosionRadius = 5f;
    
    [Header("Targeting Settings")]
    [SerializeField] private float spreadRadius = 5f; // 미사일이 떨어질 범위
    [SerializeField] private LayerMask groundLayer; // 지면 레이어
    
    public void LaunchMissile(Vector3 targetPosition)
    {
        // 미사일 스폰 위치 (플레이어 위 10유닛)
        Vector3 spawnPosition = transform.position + Vector3.forward * 5f + Vector3.up * 5f;
        
        // 미사일 생성
        GameObject missileObj = Instantiate(missilePrefab, spawnPosition, Quaternion.identity);
        Missile missile = missileObj.GetComponent<Missile>();
        
        // 목표 지점에 약간의 랜덤성 추가
        Vector3 randomOffset = Random.insideUnitCircle * spreadRadius;
        randomOffset.z = randomOffset.y;
        randomOffset.y = 0;
        Vector3 finalTarget = targetPosition + randomOffset;
        
        // 미사일 초기화
        missile.Initialize(finalTarget - spawnPosition, damage);
    }

    protected override SkillState GetInitialState()
    {
        return new MissileChargingState(this);
    }
}
