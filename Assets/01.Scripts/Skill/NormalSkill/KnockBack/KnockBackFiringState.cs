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
        }
    }
    
    public override void UpdateState()
    {
    }
    
    public override void ExitState()
    {
    }
}