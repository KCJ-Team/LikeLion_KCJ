using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Monster_Skill : VMyState<FSM_MonsterState>
{
    public override FSM_MonsterState StateEnum => FSM_MonsterState.FSM_Monster_Skill;
    
    private Monster monster;

    protected override void Awake()
    {
        base.Awake();
        monster = GetComponent<Monster>();
    }
    
    protected override void EnterState()
    {
        //monster.ShootInCircle();
        
        OwnerStateMachine.ChangeState(FSM_MonsterState.FSM_Monster_Move);
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
