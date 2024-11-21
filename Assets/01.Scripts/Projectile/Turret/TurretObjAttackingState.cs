using PlayerInfo;
using UnityEngine;

public class TurretObjAttackingState : ProjectileState
{
    private TurretObject turret;
    private float nextAttackTime;

    public TurretObjAttackingState(Projectile projectile) : base(projectile)
    {
        turret = projectile as TurretObject;
    }

    public override void EnterState()
    {
        nextAttackTime = Time.time;
    }

    public override void UpdateState()
    {
        Transform target = turret.GetTarget();

        // 타겟이 없거나 범위를 벗어났다면 감지 상태로 전환
        if (target == null || !IsTargetInRange())
        {
            turret.ChangeState(new TurretObjDetectingState(turret));
            return;
        }

        // 터렛 헤드만 타겟을 향해 회전
        turret.RotateTurretHead(target.position);

        // 공격 간격마다 발사
        if (Time.time >= nextAttackTime)
        {
            Vector3 direction = (target.position - turret.transform.position).normalized;
            Attack(direction);
            nextAttackTime = Time.time + turret.GetAttackInterval();
        }
    }

    private bool IsTargetInRange()
    {
        Transform target = turret.GetTarget();
        return target != null && Vector3.Distance(turret.transform.position, target.position) <= turret.GetDetectionRadius();
    }

    private void Attack(Vector3 direction)
    {
        SoundManager.Instance.WeaponSound(PlayerWeaponType.Pistol);
        turret.FireProjectile(direction);
    }

    public override void ExitState()
    {
        
    }
}