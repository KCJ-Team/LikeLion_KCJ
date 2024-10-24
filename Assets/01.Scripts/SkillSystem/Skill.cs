using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    protected Character owner;
    protected SkillState currentState;
    
    [SerializeField] protected float cooldown;
    protected float currentCooldown;
    
    public virtual void Initialize(Character owner)
    {
        this.owner = owner;
        currentCooldown = 0;
    }
    
    public virtual void Execute()
    {
        if (currentCooldown <= 0)
        {
            ChangeState(GetInitialState());
            currentCooldown = cooldown;
        }
    }
    
    protected abstract SkillState GetInitialState();
    
    public void ChangeState(SkillState newState)
    {
        if (currentState != null)
            currentState.ExitState();
            
        currentState = newState;
        currentState.EnterState();
    }
    
    protected virtual void Update()
    {
        if (currentCooldown > 0)
            currentCooldown -= Time.deltaTime;
            
        if (currentState != null)
            currentState.UpdateState();
    }
}
