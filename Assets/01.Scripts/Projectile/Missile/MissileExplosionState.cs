using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplosionState : ProjectileState
{
    private Missile missile;
    private float explosionDuration = 2f;   // 폭발 지속 시간
    private float currentTime = 0f;           // 현재 경과 시간
    private float explosionRadius = 5f;       // 폭발 반경
    private bool hasDamageApplied = false;    // 데미지 적용 여부 플래그
    private LayerMask targetLayer;
    private float damageDelay = 2f;          // 데미지 지연 시간
    private float damageTimer = 0f;          // 데미지 타이머
    private bool isDamageScheduled = false;   // 데미지 예약 여부
    
    public MissileExplosionState(Projectile projectile, LayerMask targetLayer) : base(projectile) 
    {
        missile = projectile as Missile;
        this.targetLayer = targetLayer;
    }
    
    public override void EnterState()
    {
        currentTime = 0f;
        damageTimer = 0f;
        hasDamageApplied = false;
        isDamageScheduled = false;
        ScheduleExplosionDamage(); // 데미지 적용을 예약
        
    }
    
    public override void UpdateState()
    {
        currentTime += Time.deltaTime;
        
        // 데미지 타이머 업데이트
        if (!hasDamageApplied && isDamageScheduled)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageDelay)
            {
                ApplyExplosionDamage();
            }
        }
        
        if (currentTime >= explosionDuration)
        {
            Object.Destroy(projectile.gameObject);
        }
    }
    
    private void ScheduleExplosionDamage()
    {
        if (isDamageScheduled) return;
        isDamageScheduled = true;
        damageTimer = 0f;
    }
    
    private void ApplyExplosionDamage()
    {
        if (hasDamageApplied) return;
        
        // 폭발 반경 내의 모든 콜라이더 검출
        Collider[] hitColliders = Physics.OverlapSphere(projectile.transform.position, explosionRadius, targetLayer);
        
        foreach (Collider hit in hitColliders)
        {
            // 거리에 따른 데미지 감소 계산
            float distance = Vector3.Distance(projectile.transform.position, hit.transform.position);
            float damageRatio = 1f - (distance / explosionRadius); // 거리가 멀수록 데미지 감소
            float finalDamage = projectile.damage * damageRatio;
            
            // 데미지를 받을 수 있는 컴포넌트가 있는지 확인
            IDamageable damageable = hit.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(finalDamage);
            }
        }
        
        hasDamageApplied = true;
    }
    
    public override void ExitState() { }
}