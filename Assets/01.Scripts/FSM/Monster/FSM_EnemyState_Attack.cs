using UnityEngine;

public class FSM_EnemyState_Attack : VMyState<FSM_EnemyState>
{
    private FSM_Enemy enemy;
    private Animator _animator;
    private float nextAttackTime;
    private float nextSkillTime;
    
    public override FSM_EnemyState StateEnum => FSM_EnemyState.FSM_EnemyState_Attack;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<FSM_Enemy>();
        _animator = GetComponent<Animator>();
    }

    protected override void EnterState()
    {
        _animator.CrossFade(AnimationHash.AttackHash, enemy.crossFadeDuration);
        
        if (enemy.agent != null)
        {
            enemy.agent.isStopped = true;
            // 공격 애니메이션의 루트모션 적용
            _animator.applyRootMotion = enemy.useRootMotion;
        }

        // 상태 진입 시 다음 공격 시간 초기화
        nextAttackTime = 0;
        
        // 보스 타입일 경우 다음 스킬 사용 시간 설정
        if (enemy.attackType == AttackType.Boss)
        {
            nextSkillTime = Time.time + enemy.skillCooldown;
        }
    }

    protected override void ExcuteState()
    {
        if (enemy.IsDeath())
        {
            OwnerStateMachine.ChangeState(FSM_EnemyState.FSM_EnemyState_Dead);
            return;
        }
        
        if (!enemy.DetectPlayer())
        {
            OwnerStateMachine.ChangeState(FSM_EnemyState.FSM_EnemyState_Idle);
            return;
        }

        if (!enemy.IsPlayerInRange(enemy.attackRange))
        {
            OwnerStateMachine.ChangeState(FSM_EnemyState.FSM_EnemyState_Move);
            return;
        }

        // 보스 타입이고 스킬 쿨다운이 끝났으면 스킬 상태로 전환
        if (enemy.attackType == AttackType.Boss && Time.time >= nextSkillTime)
        {
            OwnerStateMachine.ChangeState(FSM_EnemyState.FSM_EnemyState_Skill);
            return;
        }

        // 일반 공격 처리
        if (Time.time >= nextAttackTime)
        {
            if (enemy.attackType == AttackType.Melee || enemy.attackType == AttackType.Boss)
            {
                MeleeAttack();
            }
            
            nextAttackTime = Time.time + enemy.attackCooldown;
        }
    }

    private void MeleeAttack()
    {
        if (enemy.playerTransform == null || enemy.meleeAttackPrefab == null) return;

        // 플레이어를 바라보도록 회전
        Vector3 direction = enemy.playerTransform.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);

        // 공격 오브젝트 생성
        Vector3 attackPosition = transform.position + transform.TransformDirection(enemy.meleeAttackOffset);
        
        GameObject attackObject = Instantiate(enemy.meleeAttackPrefab, attackPosition, transform.rotation);
        
        attackObject.GetComponent<MonsterDamageValue>().Damage = enemy.Damage;
        
        Destroy(attackObject, enemy.attackDuration);
    }
    
    private void OnAnimatorMove()
    {
        // 루트모션이 활성화된 경우, 애니메이션의 이동을 실제 위치에 적용
        if (_animator.applyRootMotion)
        {
            Vector3 position = _animator.rootPosition;
            position.y = transform.position.y; // Y축 위치는 유지
            transform.position = position;
        }
    }
}