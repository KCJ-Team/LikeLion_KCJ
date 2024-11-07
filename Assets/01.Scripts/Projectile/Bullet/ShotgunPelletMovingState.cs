using UnityEngine;

public class ShotgunPelletMovingState : ProjectileMovingState
{
    public ShotgunPelletMovingState(Projectile projectile, Vector3 direction) : base(projectile, direction)
    {
        maxRange = 50f; // 샷건 펠릿의 최대 사거리는 더 짧게 설정
    }
    
    public override void UpdateState()
    {
        base.UpdateState();
        
        // 샷건 펠릿만의 특별한 동작을 추가할 수 있음
        // 예: 시간에 따른 데미지 감소, 특별한 궤적 등
    }
}