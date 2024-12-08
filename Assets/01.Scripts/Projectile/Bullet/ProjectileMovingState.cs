using UnityEngine;

public class ProjectileMovingState : ProjectileState
{
    protected Vector3 direction;
    
    public ProjectileMovingState(Projectile projectile, Vector3 direction) : base(projectile)
    {
        this.direction = direction;
    }
    
    public override void EnterState()
    {
        
    }
    
    public override void UpdateState()
    {
        projectile.transform.Translate(direction * (projectile.speed * Time.deltaTime), Space.World);
    }
    
    public override void ExitState()
    {
        
    }
}