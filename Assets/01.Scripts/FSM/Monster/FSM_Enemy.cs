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
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public float moveSpeed = 2f;
    
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
    
    [Header("Animation Settings")]
    public float crossFadeDuration = 0.25f;  // 애니메이션 전환 시간
    public bool useRootMotion = true;        // 루트 모션 사용 여부

    private MonsterHealth _monsterHealth;
    
    private void Awake()
    {
        base.Awake();
        _monsterHealth = GetComponent<MonsterHealth>();
    }

    protected override void Start()
    {
        base.Start();
        playerTransform = GameManager.Instance.Player.transform;
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

    public void DetectPlayer()
    {
        float distance = Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position);

        // 플레이어와의 거리가 stopDistance보다 크면 따라감
        if (distance > attackRange)
        {
            // 플레이어 방향 계산
            Vector3 direction = (GameManager.Instance.Player.transform.position - transform.position).normalized;

            // 적이 플레이어 방향으로 이동
            transform.position += direction * (moveSpeed * Time.deltaTime);

            // 적이 플레이어를 향해 회전
            transform.LookAt(GameManager.Instance.Player.transform.position);
        }
    }
    
    public bool IsPlayerInRange(float range)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, GameManager.Instance.Player.transform.position);
        return distanceToPlayer <= range;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}