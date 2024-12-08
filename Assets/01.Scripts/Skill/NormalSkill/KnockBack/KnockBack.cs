using System;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : Skill
{
    public float pushRadius;
    public float pushForce = 10f;
    public LayerMask targetLayer;
    
    private HashSet<Collider2D> pushedObjects = new HashSet<Collider2D>();

    private void Start()
    {
        pushRadius = GameManager.Instance.playerData.currentWeapon.attackRange;
    }

    public void KnockBackObject()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, pushRadius, targetLayer);

        foreach (var hitCollider in hitColliders)
        {
            if (!pushedObjects.Contains(hitCollider))
            {
                pushedObjects.Add(hitCollider);

                Vector2 pushDirection = hitCollider.transform.position - transform.position;
                pushDirection.Normalize();

                Rigidbody2D rb = hitCollider.GetComponent<Rigidbody2D>();
                
                if (rb != null)
                {
                    float distance = Vector2.Distance(transform.position, hitCollider.transform.position);
                    float forceMultiplier = Mathf.Clamp01(1 - (distance / pushRadius));

                    rb.drag = 5f; // 저항 추가
                    rb.AddForce(pushDirection * (pushForce * forceMultiplier), ForceMode2D.Impulse);

                    // 속도 제한
                    float maxSpeed = 10f;
                    if (rb.velocity.magnitude > maxSpeed)
                    {
                        rb.velocity = rb.velocity.normalized * maxSpeed;
                    }
                }
            }
        }
        
        Destroy(gameObject);
    }
    
    public override SkillState GetInitialState()
    {
        return new KnockBackState(this);
    }
}