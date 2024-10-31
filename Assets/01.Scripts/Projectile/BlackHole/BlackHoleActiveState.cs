using UnityEngine;

public class BlackHoleActiveState : ProjectileState
{
    private BlackHoleObject blackHole;
    private bool isActive = true;

    public BlackHoleActiveState(Projectile projectile) : base(projectile)
    {
        blackHole = (BlackHoleObject)projectile;
    }

    public override void EnterState()
    {
        isActive = true;
    }

    public override void UpdateState()
    {
        if (isActive)
        {
            if (blackHole.IsExpired())
            {
                // 수명이 다한 경우 블랙홀 제거
                blackHole.DestroyBlackHole();
                isActive = false;
            }
            else
            {
                // 주변 물체 끌어당기기
                blackHole.AttractObjects();
            }
        }
    }

    public override void ExitState()
    {
        isActive = false;
    }
}