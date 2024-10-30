using UnityEngine;

public class GrabFiringState : SkillState
{
    private Grab _grab;
    
    public GrabFiringState(Skill skill) : base(skill)
    {
        _grab = skill as Grab;
    }
    
    public override void EnterState()
    {
        if (_grab.CanUseSkill())
        {
            _grab.FireProjectile();
        }
    }
    
    public override void UpdateState()
    {
    }
    
    public override void ExitState()
    {
    }
}