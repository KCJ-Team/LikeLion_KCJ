using UnityEngine;

public class PullFiringState : SkillState
{
    private Pull _pull;
    private bool hasFired = false;
    
    [SerializeField] private float stateLifetime = 0.5f; // 상태가 지속되는 시간
    private float stateTimer = 0f;
    
    public PullFiringState(Skill skill) : base(skill)
    {
        _pull = skill as Pull;
        
        if (_pull == null)
        {
            Debug.LogError("Failed to cast skill to Pull in PullFiringState");
        }
    }

    public override void EnterState()
    {
        // 상태 시작 시 초기화
        hasFired = false;
        stateTimer = 0f;
    }

    public override void UpdateState()
    {
        // 발사가 아직 이루어지지 않았다면 발사 실행
        if (!hasFired)
        {
            _pull.ShootProjectile();
            hasFired = true;
        }
        
        // 타이머 업데이트
        stateTimer += Time.deltaTime;
        
        // 지정된 시간이 지나면 상태 종료
        if (stateTimer >= stateLifetime)
        {
            // 스킬의 상태를 null로 변경하여 스킬 종료
            _pull.ChangeState(null);
        }
    }

    public override void ExitState()
    {
        // 상태 종료 시 필요한 정리 작업
        hasFired = false;
        stateTimer = 0f;
    }
}