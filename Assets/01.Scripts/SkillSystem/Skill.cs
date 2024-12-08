using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [SerializeField] protected float damage;
    protected PlayerController owner;
    protected SkillState currentState;
    
    [SerializeField] protected float cooldown;
    protected float currentCooldown = 0f;
    
    public float Cooldown => cooldown;
    
    public virtual void Initialize(PlayerController owner)
    {
        this.owner = owner;
    }
    
    public virtual void Execute()
    {
        ChangeState(GetInitialState());
    }
    
    public abstract SkillState GetInitialState();
    
    public void ChangeState(SkillState newState)
    {
        if (currentState != null)
            currentState.ExitState();
        
        currentState = newState;
        if (currentState != null)
            currentState.EnterState();
    }
}