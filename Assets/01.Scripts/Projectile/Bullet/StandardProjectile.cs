using System;
using UnityEngine;

public class StandardProjectile : Projectile
{
    [SerializeField]
    private LayerMask destructibleLayers;
    protected Vector3 direction;
    
    public override void Initialize(Vector3 direction, float damage)
    {
        this.direction = direction;
        base.Initialize(direction, damage);
    }
    
    protected override ProjectileState GetInitialState()
    {
        return new ProjectileMovingState(this, direction);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & destructibleLayers) != 0)
        {
            Destroy(gameObject);
        }
    }
}