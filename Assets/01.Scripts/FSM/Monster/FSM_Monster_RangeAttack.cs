using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Monster_RangeAttack : VMyState<FSM_MonsterState>
{
    public override FSM_MonsterState StateEnum => FSM_MonsterState.FSM_Monster_RangeAttack;

    private Monster monster;

    protected override void Awake()
    {
        base.Awake();
        monster = GetComponent<Monster>();
    }
    
    protected override void EnterState()
    {
        if (Random.value < 0.5f) //50%확률
        {
            monster.Attack();
        }
        else
        {
            monster.StopAttacking();
            monster.ShootInCircle();
            monster.Attack();
        }
    }

    protected override void ExcuteState()
    {
        if (monster.IsDeath())
        {
            OwnerStateMachine.ChangeState(FSM_MonsterState.FSM_Monster_Dead);
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
        if (monster.GetAttackDistance() > monster.attackRange)
        {
            OwnerStateMachine.ChangeState(FSM_MonsterState.FSM_Monster_Move);
            return;
        }
    }

    protected override void ExcuteState_LateUpdate()
    {
        
    }

    protected override void ExitState()
    {
        monster.StopAttacking();
    }
}
