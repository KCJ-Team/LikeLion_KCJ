using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Monster_Idle : VMyState<FSM_MonsterState>
{
    public override FSM_MonsterState StateEnum => FSM_MonsterState.FSM_Monster_Idle;

    private Monster monster;

    protected override void Awake()
    {
        base.Awake();
        monster = GetComponent<Monster>();
    }

    protected override void EnterState()
    {
        
    }

    protected override void ExcuteState()
    {
        if (monster.IsDeath())
        {
            OwnerStateMachine.ChangeState(FSM_MonsterState.FSM_Monster_Dead);
            return;
        }

        if (!GameManager.Instance.playerData.IsInvisable)
        {
            OwnerStateMachine.ChangeState(FSM_MonsterState.FSM_Monster_Move);
            return;
        }
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