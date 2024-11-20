using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_EnemyState_Spawn : VMyState<FSM_EnemyState>
{
    private FSM_Enemy enemy;
    private Animator _animator;
    public override FSM_EnemyState StateEnum => FSM_EnemyState.FSM_EnemyState_Spawn;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<FSM_Enemy>();
        _animator = GetComponent<Animator>();
    }

    protected override void EnterState()
    {
        _animator.CrossFade(AnimationHash.SpawnHash, 0.0f);
    }

    protected override void ExcuteState()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        
        if (stateInfo.shortNameHash == AnimationHash.SpawnHash && 
            stateInfo.normalizedTime >= 1.0f)
        {
            OwnerStateMachine.ChangeState(FSM_EnemyState.FSM_EnemyState_Move);
        }
    }
}