using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected ProjectileState currentState;
    protected float damage;
    protected float speed;
    
    public virtual void Initialize(Vector3 direction, float damage)
    {
        this.damage = damage;
        ChangeState(GetInitialState());
    }
    
    protected abstract ProjectileState GetInitialState();
    
    public void ChangeState(ProjectileState newState)
    {
        if (currentState != null)
            currentState.ExitState();
            
        currentState = newState;
        currentState.EnterState();
    }
    
    protected virtual void Update()
    {
        if (currentState != null)
            currentState.UpdateState();
    }
}