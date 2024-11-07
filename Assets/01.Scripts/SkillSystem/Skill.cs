using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected PlayerCharacterController owner;
    protected SkillState currentState;
    
    [SerializeField] protected float cooldown;
    protected float currentCooldown = 0f;  // 추가된 currentCooldown
    
    public float Cooldown => cooldown;
    
    public virtual void Initialize(PlayerCharacterController owner)
    {
        this.owner = owner;
    }
    
    public virtual bool TryExecute()
    {
        Execute();
        return true;
    }
    
    protected virtual void Execute()
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
    
    protected virtual void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }

        if (currentState != null)
            currentState.UpdateState();
    }
    
    protected virtual void OnDestroy()
    {
        if (currentState != null)
            currentState.ExitState();
    }
}