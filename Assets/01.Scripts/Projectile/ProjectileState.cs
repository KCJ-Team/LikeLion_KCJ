using UnityEngine;

public abstract class ProjectileState
{
    protected Projectile projectile;
    
    public ProjectileState(Projectile projectile)
    {
        this.projectile = projectile;
    }
    
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}