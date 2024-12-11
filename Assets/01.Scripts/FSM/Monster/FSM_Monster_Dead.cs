using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Monster_Dead : VMyState<FSM_MonsterState>
{
    public override FSM_MonsterState StateEnum => FSM_MonsterState.FSM_Monster_Dead;
    
    private Monster monster;

    protected override void Awake()
    {
        base.Awake();
        monster = GetComponent<Monster>();
    }
    
    protected override void EnterState()
    {
        Destroy(gameObject);
    }

    protected override void ExcuteState()
    {
        
    }

    protected override void ExcuteState_FixedUpdate()
    {
        
    }

    protected override void ExcuteState_LateUpdate()
    {
        
    }

    protected override void ExitState()
    {
        
    }
}
