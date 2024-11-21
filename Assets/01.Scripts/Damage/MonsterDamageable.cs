using System;
using UnityEngine;


// 몬스터의 데미지 처리 클래스
public class MonsterDamageable : DamageableObject
{
    private float projectileDamage;

    public GameObject BloodEffect;

    private void OnTriggerEnter(Collider other)
    {
        // Projectile 레이어의 오브젝트와 충돌했을 때
        if (other.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            Debug.Log("피격");
            GameObject Effect = Instantiate(BloodEffect, transform.position, Quaternion.identity);
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            projectileDamage = projectile.damage;
            TakeDamage(projectileDamage);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        
    }

    public override void TakeDamage(float damage)
    {
        // 몬스터는 damge 그대로 받음
        health.DecreaseHealth(damage);
    }
}