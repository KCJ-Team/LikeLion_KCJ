using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FSM_EnemyState_Skill : VMyState<FSM_EnemyState>
{
    private FSM_Enemy enemy;
    private Animator _animator;
    
    public override FSM_EnemyState StateEnum => FSM_EnemyState.FSM_EnemyState_Skill;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<FSM_Enemy>();
        _animator = GetComponent<Animator>();
    }

    protected override void EnterState()
    {
        _animator.CrossFade(AnimationHash.SkillHash, 0.0f);
        
        SpawnMonsters();
    }

    protected override void ExcuteState()
    {
        if (enemy.IsDeath())
        {
            OwnerStateMachine.ChangeState(FSM_EnemyState.FSM_EnemyState_Dead);
            return;
        }

        // 현재 애니메이션의 진행도 확인
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        
        // 스킬 애니메이션이 끝났는지 확인
        if (stateInfo.shortNameHash == AnimationHash.SkillHash && 
            stateInfo.normalizedTime >= 1.0f)
        {
            OwnerStateMachine.ChangeState(FSM_EnemyState.FSM_EnemyState_Attack);
        }
    }

    private void SpawnMonsters()
    {
        for (int i = 0; i < enemy.spawnCount; i++)
        {
            // 랜덤한 위치 계산
            Vector2 randomCircle = Random.insideUnitCircle * enemy.spawnRadius;
            Vector3 spawnPosition = enemy.transform.position + new Vector3(randomCircle.x, 0, randomCircle.y);

            // NavMesh 위의 가장 가까운 위치 찾기
            if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, enemy.spawnRadius, NavMesh.AllAreas))
            {
                GameObject monster = Instantiate(enemy.monsterPrefab, hit.position, Quaternion.identity);
            }
        }
    }
}