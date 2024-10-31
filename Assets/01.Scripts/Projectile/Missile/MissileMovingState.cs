using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미사일이 이동하는 상태를 구현한 클래스
/// </summary>
public class MissileMovingState : ProjectileState
{
    private Vector3 direction;        // 미사일의 이동 방향
    private float moveSpeed = 20f;    // 미사일 이동 속도
    private float damage;             // 미사일 데미지
    
    public MissileMovingState(Projectile projectile, Vector3 targetPosition, float damage) : base(projectile)
    {
        // 목표 지점으로 향하는 방향 계산
        this.direction = (targetPosition - projectile.transform.position).normalized;
        this.damage = damage;
    }
    
    /// <summary>
    /// 이동 상태 진입 시 초기화
    /// 미사일의 방향을 목표점을 향하도록 설정
    /// </summary>
    public override void EnterState()
    {
        // 미사일이 이동 방향을 바라보도록 회전
        projectile.transform.rotation = Quaternion.LookRotation(direction);
    }
    
    /// <summary>
    /// 이동 상태 업데이트
    /// 미사일을 이동시키고 충돌 검사
    /// </summary>
    public override void UpdateState()
    {
        // 설정된 방향으로 미사일 이동
        projectile.transform.position += direction * (Time.deltaTime * moveSpeed);
        
        // 전방 충돌 검사
        RaycastHit hit;
        if (Physics.Raycast(projectile.transform.position, direction, out hit, 0.5f))
        {
            // TODO: 데미지 처리
            
            // 충돌이 감지되면 폭발 상태로 전환
            projectile.ChangeState(new MissileExplosionState(projectile));
        }
    }
    
    public override void ExitState() { }
}