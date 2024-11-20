using System.Collections;
using UnityEngine;

public class MissileBombing : Skill
{
    [Header("Effect Settings")]
    [SerializeField] private GameObject EffectPrefab;
    
    [Header("Damage Settings")]
    [SerializeField] private float damageRadius = 5f;     // 데미지를 입히는 범위
    [SerializeField] private float damageInterval = 0.5f; // 데미지를 주는 간격
    public float damageDuration = 3f;   // 데미지 지속 시간
    [SerializeField] private LayerMask targetLayer;       // 데미지를 받을 대상의 레이어

    public void CreateEffectWithDamage(Vector3 targetPosition)
    {
        GameObject effect = Instantiate(EffectPrefab, targetPosition, Quaternion.identity);
        DamageArea damageArea = effect.GetComponent<DamageArea>();
        damageArea.Initialize(damage, damageRadius, damageInterval, damageDuration, targetLayer);
    }

    public override SkillState GetInitialState()
    {
        return new MissilePreparingState(this);
    }
}