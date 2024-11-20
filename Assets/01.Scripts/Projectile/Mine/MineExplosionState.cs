using UnityEngine;

public class MineExplosionState : ProjectileState
{
    private MineObject mine;
    private float explosionStartTime;
    private bool isExploding = false;

    public MineExplosionState(Projectile projectile) : base(projectile)
    {
        mine = (MineObject)projectile;
    }

    public override void EnterState()
    {
        if (mine.HasExploded())
        {
            explosionStartTime = Time.time;
            isExploding = true;
        }
        
        //TODO: 이펙트 집어넣기
    }

    public override void UpdateState()
    {
        if (isExploding)
        {
            // 폭발 진행 중 추가적인 효과나 로직을 구현할 수 있음
            float elapsedTime = Time.time - explosionStartTime;
            
            // 폭발 종료 시점 확인
            if (elapsedTime >= mine.GetExplosionDuration())
            {
                isExploding = false;
            }
        }
    }

    public override void ExitState()
    {
        isExploding = false;
    }
}