using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Monster_Move : VMyState<FSM_MonsterState>
{
    public override FSM_MonsterState StateEnum => FSM_MonsterState.FSM_Monster_Move;
    
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
        
        if (monster.GetAttackDistance() <= monster.attackRange && monster.monsterType == MonsterType.Ranged)
        {
            OwnerStateMachine.ChangeState(FSM_MonsterState.FSM_Monster_RangeAttack);
            return;
        }
        
        if (GameManager.Instance.playerData.IsInvisable)
        {
            OwnerStateMachine.ChangeState(FSM_MonsterState.FSM_Monster_Idle);
            return;
        }
    }

    protected override void ExcuteState_FixedUpdate()
    {
        monster.MoveToTarget();
    }

    protected override void ExcuteState_LateUpdate()
    {
        
    }

    protected override void ExitState()
    {
        monster.StopMoving();
    }
}