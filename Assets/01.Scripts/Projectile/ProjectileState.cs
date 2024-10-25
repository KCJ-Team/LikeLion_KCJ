using UnityEngine;

/// <summary>
/// 발사체의 상태를 나타내는 추상 클래스
/// State 패턴의 기본 구조 제공
/// </summary>
public abstract class ProjectileState
{
    protected Projectile projectile;  // 이 상태가 속한 발사체 참조
    
    public ProjectileState(Projectile projectile)
    {
        this.projectile = projectile;
    }
    
    // 상태 진입 시 호출되는 메서드
    public abstract void EnterState();
    // 상태 업데이트 시 호출되는 메서드
    public abstract void UpdateState();
    // 상태 종료 시 호출되는 메서드
    public abstract void ExitState();
}