using UnityEngine;

public class FSM_EnemyState_Move : VMyState<FSM_EnemyState>
{
    private FSM_Enemy enemy;
    private Animator _animator;
    public override FSM_EnemyState StateEnum => FSM_EnemyState.FSM_EnemyState_Move;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<FSM_Enemy>();
        _animator = GetComponent<Animator>();
    }

    protected override void EnterState()
    {
        _animator.CrossFade(AnimationHash.MoveHash, enemy.crossFadeDuration);
        
        if (enemy.agent != null)
        {
            enemy.agent.isStopped = false;
            // 이동 중에는 NavMeshAgent가 이동을 제어하도록 루트모션 비활성화
            _animator.applyRootMotion = false;
        }
    }

    protected override void ExcuteState()
    {
        if (enemy.IsDeath())
        {
            OwnerStateMachine.ChangeState(FSM_EnemyState.FSM_EnemyState_Dead);
            return;
        }
        
        if (!enemy.DetectPlayer() || !enemy.IsPlayerInRange(enemy.detectionRadius))
        {
            OwnerStateMachine.ChangeState(FSM_EnemyState.FSM_EnemyState_Idle);
            return;
        }

        if (enemy.IsPlayerInRange(enemy.attackRange))
        {
            OwnerStateMachine.ChangeState(FSM_EnemyState.FSM_EnemyState_Attack);
            return;
        }

        // 플레이어 위치로 이동
        enemy.agent.SetDestination(enemy.playerTransform.position);
    }

    protected override void ExitState()
    {
        if (enemy.agent != null)
        {
            enemy.agent.isStopped = true;
        }
    }
}