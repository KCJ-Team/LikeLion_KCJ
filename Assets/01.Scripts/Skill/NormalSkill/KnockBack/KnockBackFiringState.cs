using UnityEngine;

public class KnockBackFiringState : SkillState
{
    private KnockBack _knockBack;
    
    private bool hasFired = false;
    
    [SerializeField] private float stateLifetime = 0.5f; // 상태가 지속되는 시간
    private float stateTimer = 0f;
    
    public KnockBackFiringState(Skill skill) : base(skill)
    {
        _knockBack = skill as KnockBack;
    }
    
    public override void EnterState()
    {
        hasFired = false;
        stateTimer = 0f;
    }
    
    public override void UpdateState()
    {
        if (!hasFired)
        {
            _knockBack.FireProjectile();
            hasFired = true;
        }
        
        // 타이머 업데이트
        stateTimer += Time.deltaTime;
        
        // 지정된 시간이 지나면 상태 종료
        if (stateTimer >= stateLifetime)
        {
            // 스킬의 상태를 null로 변경하여 스킬 종료
            Object.Destroy(_knockBack.gameObject);
            _knockBack.ChangeState(null);
        }
    }
    
    public override void ExitState()
    {
        hasFired = false;
        stateTimer = 0f;
    }
}