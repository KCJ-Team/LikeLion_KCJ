using UnityEngine;

public class KnockBackFiringState : SkillState
{
    private KnockBack _knockBack;
    
    public KnockBackFiringState(Skill skill) : base(skill)
    {
        _knockBack = skill as KnockBack;
    }
    
    public override void EnterState()
    {
        if (_knockBack.CanUseSkill())
        {
            _knockBack.FireProjectile();
            Object.Destroy(_knockBack.gameObject);
        }
    }
    
    public override void UpdateState()
    {
    }
    
    public override void ExitState()
    {
    }
}