using UnityEngine;

public class StandardProjectile : Projectile
{
    protected Vector3 direction;
    protected Vector3 startPosition;
    
    public override void Initialize(Vector3 direction, float damage)
    {
        this.direction = direction;
        this.startPosition = transform.position;
        base.Initialize(direction, damage);
    }
    
    protected override ProjectileState GetInitialState()
    {
        return new ProjectileMovingState(this, direction);
    }
}