/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 미사일 폭격 스킬을 구현한 클래스
// 여러 발의 미사일을 순차적으로 발사하는 스킬
public class MissileBombing : Skill
{
    [Header("Missile Settings")]
    [SerializeField] private GameObject missilePrefab;    // 미사일 프리팹
    [SerializeField] private int missileCount = 5;        // 발사할 미사일 수
    [SerializeField] private float explosionRadius = 5f;  // 폭발 반경
    [SerializeField] private GameObject EffectPrefab;

    [Header("Targeting Settings")]
    [SerializeField] private float spreadRadius = 5f;     // 미사일이 떨어질 범위
    [SerializeField] private float spawnHeight = 20f;     // 미사일 생성 높이
    [SerializeField] private LayerMask groundLayer;       // 지면 감지를 위한 레이어

    // 단일 미사일을 발사하는 메서드
    public void LaunchMissile(Vector3 targetPosition)
    {
        // 목표 지점 위쪽에서 미사일 생성
        Vector3 spawnPosition = targetPosition + Vector3.up * spawnHeight;

        // 실제 타겟 위치 계산 (spread radius 내의 랜덤한 위치)
        Vector3 randomOffset = Random.insideUnitCircle * spreadRadius;
        randomOffset.z = randomOffset.y;
        randomOffset.y = 0;
        Vector3 finalTarget = targetPosition + randomOffset;

        // 미사일 오브젝트 생성
        GameObject missileObj = Instantiate(missilePrefab, spawnPosition, Quaternion.identity);
        GameObject Effect = Instantiate(EffectPrefab, targetPosition, Quaternion.identity);

        Missile missile = missileObj.GetComponent<Missile>();

        if (missile != null)
        {
            // 미사일 초기화 및 상태 설정
            missile.Initialize(finalTarget, damage);
        }
    }

    // 스킬의 초기 상태를 MissileChargingState로 설정
    public override SkillState GetInitialState()
    {
        return new MissilePreparingState(this);
    }
}*/

using UnityEngine;

public class MissileBombing : Skill
{
    [Header("Effect Settings")]
    [SerializeField] private GameObject EffectPrefab;
    
    [Header("Damage Settings")]
    [SerializeField] private float damageRadius = 5f;     // 데미지를 입히는 범위
    [SerializeField] private float damageInterval = 0.5f; // 데미지를 주는 간격
    [SerializeField] private float damageDuration = 3f;   // 데미지 지속 시간
    [SerializeField] private LayerMask targetLayer;       // 데미지를 받을 대상의 레이어

    public void CreateEffectWithDamage(Vector3 targetPosition)
    {
        GameObject effect = Instantiate(EffectPrefab, targetPosition, Quaternion.identity);
        DamageArea damageArea = effect.AddComponent<DamageArea>();
        damageArea.Initialize(damage, damageRadius, damageInterval, damageDuration, targetLayer);
    }

    public override SkillState GetInitialState()
    {
        return new MissilePreparingState(this);
    }
}