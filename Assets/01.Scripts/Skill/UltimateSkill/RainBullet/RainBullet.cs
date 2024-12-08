using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class RainBullet : Skill
{
    public float damageRadius = 5f;      // 데미지를 줄 거리
    public LayerMask targetLayer;       // 데미지를 받을 레이어
    public float damageInterval = 0.5f; // 데미지 주기 (초)
    public float totalDamageDuration = 3f; // 데미지 지속 시간 (초)
    
    private Collider2D[] targetsInRange;
    
    private void Start()
    {
        damageRadius = GameManager.Instance.playerData.currentWeapon.attackRange;
    }
    
    public void StartDamageProcess()
    {
        StartCoroutine(ApplyDamageOverTime());
    }
    
    private IEnumerator ApplyDamageOverTime()
    {
        float elapsedTime = 0f;

        while (elapsedTime < totalDamageDuration)
        {
            // 현재 범위 내의 모든 대상 가져오기
            targetsInRange = Physics2D.OverlapCircleAll(transform.position, damageRadius, targetLayer);

            foreach (var target in targetsInRange)
            {
                // 대상의 Health 컴포넌트를 찾아 데미지 적용
                MonsterDamageable damageable = target.GetComponent<MonsterDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damage);
                }
            }

            elapsedTime += damageInterval; 
            yield return new WaitForSeconds(damageInterval);
        }
        
        Destroy(gameObject);
    }
    
    public override SkillState GetInitialState()
    {
        return new RainBulletAttackState(this, damage);
    }
}