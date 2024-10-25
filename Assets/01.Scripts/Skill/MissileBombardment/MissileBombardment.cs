using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미사일 폭격 스킬을 구현한 클래스
/// 여러 발의 미사일을 순차적으로 발사하는 스킬
/// </summary>
public class MissileBombardment : Skill
{
    [Header("Missile Settings")]
    [SerializeField] private GameObject missilePrefab;    // 미사일 프리팹
    [SerializeField] private int missileCount = 5;        // 발사할 미사일 수
    [SerializeField] private float damage = 50f;          // 미사일 당 데미지
    [SerializeField] private float missileSpeed = 20f;    // 미사일 이동 속도
    [SerializeField] private float explosionRadius = 5f;  // 폭발 반경
    
    [Header("Targeting Settings")]
    [SerializeField] private float spreadRadius = 5f;     // 미사일이 떨어질 범위
    [SerializeField] private LayerMask groundLayer;       // 지면 감지를 위한 레이어
    
    /// <summary>
    /// 단일 미사일을 발사하는 메서드
    /// </summary>
    /// <param name="targetPosition">목표 위치</param>
    public void LaunchMissile(Vector3 targetPosition)
    {
        // 미사일 스폰 위치 설정 (플레이어 앞 5유닛, 위로 5유닛)
        Vector3 spawnPosition = transform.position + Vector3.forward * 5f + Vector3.up * 5f;
        
        // 미사일 오브젝트 생성
        GameObject missileObj = Instantiate(missilePrefab, spawnPosition, Quaternion.identity);
        Missile missile = missileObj.GetComponent<Missile>();
        
        // 랜덤한 목표 지점 계산
        Vector3 randomOffset = Random.insideUnitCircle * spreadRadius;
        randomOffset.z = randomOffset.y;
        randomOffset.y = 0;
        Vector3 finalTarget = targetPosition + randomOffset;
        
        // 미사일 초기화
        missile.Initialize(finalTarget - spawnPosition, damage);
    }

    // 스킬의 초기 상태를 MissileChargingState로 설정
    protected override SkillState GetInitialState()
    {
        return new MissileChargingState(this);
    }
}
