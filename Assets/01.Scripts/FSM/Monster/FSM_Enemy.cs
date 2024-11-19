using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum FSM_EnemyState
{
    None,
    FSM_EnemyState_Idle,
    FSM_EnemyState_Move,
    FSM_EnemyState_Attack,
    FSM_EnemyState_Dead,
    FSM_EnemyState_Skill,
    FSM_EnemyState_Spawn
}

public class AnimationHash
{
    public static readonly int IdleHash = Animator.StringToHash("Idle");
    public static readonly int MoveHash = Animator.StringToHash("Move");
    public static readonly int AttackHash = Animator.StringToHash("Attack");
    public static readonly int DeadHash = Animator.StringToHash("Dead");
    public static readonly int SkillHash = Animator.StringToHash("Skill");
    public static readonly int SpawnHash = Animator.StringToHash("Spawn");
}

public enum AttackType
{
    Melee,
    Boss
}

public class FSM_Enemy : StateMachine<FSM_EnemyState>
{
    [Header("Attack Type Settings")]
    public AttackType attackType = AttackType.Melee;
    
    [Header("Detection Settings")]
    public float detectionRadius = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public LayerMask playerLayer;
    
    [Header("Melee Attack Settings")]
    public GameObject meleeAttackPrefab;
    public Vector3 meleeAttackOffset = new Vector3(0, 0, 0);
    
    [Header("Boss Skill Settings")]
    public float skillCooldown = 10f;
    public GameObject monsterPrefab;
    public int spawnCount = 3;
    public float spawnRadius = 5f;
    
    [Header("General Attack Settings")]
    public float Damage;
    public float attackDuration = 0.5f;
    
    [Header("References")]
    public Transform playerTransform;
    public NavMeshAgent agent;
    
    [Header("Animation Settings")]
    public float crossFadeDuration = 0.25f;  // 애니메이션 전환 시간
    public bool useRootMotion = true;        // 루트 모션 사용 여부

    private MonsterHealth _monsterHealth;
    
    private void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        _monsterHealth = GetComponent<MonsterHealth>();
    }

    public bool IsDeath()
    {
        if (_monsterHealth.IsDead)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool DetectPlayer()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
        
        if (colliders.Length > 0)
        {
            float closestDistance = float.MaxValue;
            Transform closestPlayer = null;
            
            foreach (var collider in colliders)
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = collider.transform;
                }
            }

            playerTransform = closestPlayer;
            return true;
        }

        playerTransform = null;
        return false;
    }

    public void BossDeath()
    {
        //보스 죽었을 때 이벤트
    }
    
    public bool IsPlayerInRange(float range)
    {
        if (playerTransform == null) return false;
        
        // 플레이어와의 직선 거리만 체크
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        return distanceToPlayer <= range;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}