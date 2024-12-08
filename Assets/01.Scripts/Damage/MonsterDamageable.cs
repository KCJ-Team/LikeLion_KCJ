using System;
using UnityEngine;

public class MonsterDamageable : DamageableObject
{
    private float projectileDamage;

    public GameObject BloodEffect;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Projectile"))
        {
            //GameObject Effect = Instantiate(BloodEffect, transform.position, Quaternion.identity);
            Projectile projectile = other.gameObject.GetComponent<Projectile>();
            projectileDamage = projectile.damage;
            TakeDamage(projectileDamage);
        }
    }

    public override void TakeDamage(float damage)
    {
        health.DecreaseHealth(damage);
    }
}