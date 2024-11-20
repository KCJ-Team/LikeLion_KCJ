using UnityEngine;

public class FSM_EnemyState_Idle : VMyState<FSM_EnemyState>
{
    private FSM_Enemy enemy;
    private Animator _animator;
    public override FSM_EnemyState StateEnum => FSM_EnemyState.FSM_EnemyState_Idle;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<FSM_Enemy>();
        _animator = GetComponent<Animator>();
    }

    protected override void EnterState()
    {
        _animator.CrossFade(AnimationHash.IdleHash, enemy.crossFadeDuration);
    }

    protected override void ExcuteState()
    {
        if (enemy.IsDeath())
        {
            OwnerStateMachine.ChangeState(FSM_EnemyState.FSM_EnemyState_Dead);
        }
    }
}