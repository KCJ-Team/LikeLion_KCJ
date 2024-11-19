using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_EnemyState_Dead : VMyState<FSM_EnemyState>
{
    private FSM_Enemy enemy;
    private Animator _animator;
    private MonsterHealth _monsterHealth;
    
    public override FSM_EnemyState StateEnum => FSM_EnemyState.FSM_EnemyState_Dead;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<FSM_Enemy>();
        _animator = GetComponent<Animator>();
        _monsterHealth = GetComponent<MonsterHealth>();
    }
    
    protected override void EnterState()
    {
        _animator.CrossFade(AnimationHash.DeadHash, 0.0f);
        
        if (enemy.agent != null)
        {
            enemy.agent.isStopped = true;
        }
        
        Destroy(gameObject, 5f); 
    }
}
