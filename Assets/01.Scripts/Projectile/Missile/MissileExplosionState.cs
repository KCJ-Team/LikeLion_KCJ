using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 미사일이 폭발할 때의 상태를 구현한 클래스
public class MissileExplosionState : ProjectileState
{
    private float explosionDuration = 0.5f;   // 폭발 지속 시간
    private float currentTime = 0f;           // 현재 경과 시간
    
    public MissileExplosionState(Projectile projectile) : base(projectile) { }
    
    // 폭발 상태 진입 시 초기화
    public override void EnterState()
    {
        // TODO: 폭발 이펙트 생성
        currentTime = 0f;
    }
    
    // 폭발 상태 업데이트
    // 폭발 지속 시간이 끝나면 미사일 오브젝트 제거
    public override void UpdateState()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= explosionDuration)
        {
            Object.Destroy(projectile.gameObject);
        }
    }
    
    public override void ExitState() { }
}