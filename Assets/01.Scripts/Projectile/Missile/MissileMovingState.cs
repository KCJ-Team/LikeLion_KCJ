using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미사일이 이동하는 상태를 구현한 클래스
/// </summary>
public class MissileMovingState : ProjectileState
{
    private Vector3 direction;    // 미사일의 이동 방향
    
    public MissileMovingState(Projectile projectile) : base(projectile) { }
    
    /// <summary>
    /// 이동 상태 진입 시 초기화
    /// 미사일의 초기 방향을 아래쪽으로 설정
    /// </summary>
    public override void EnterState()
    {
        // 미사일이 위에서 아래로 떨어지도록 방향 설정
        direction = (projectile.transform.position - Vector3.up * 10f).normalized;
    }
    
    /// <summary>
    /// 이동 상태 업데이트
    /// 미사일을 이동시키고 충돌 검사
    /// </summary>
    public override void UpdateState()
    {
        // 설정된 방향으로 미사일 이동
        projectile.transform.position += direction * (Time.deltaTime * 20f);
        
        // 전방 충돌 검사
        RaycastHit hit;
        if (Physics.Raycast(projectile.transform.position, direction, out hit, 0.5f))
        {
            // 충돌이 감지되면 폭발 상태로 전환
            projectile.ChangeState(new MissileExplosionState(projectile));
        }
    }
    
    public override void ExitState() { }
}
