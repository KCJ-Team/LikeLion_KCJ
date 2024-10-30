using UnityEngine;

public class ShadowCloneState : SkillState
{
    private ShadowClone _shadowClone;
    
    public ShadowCloneState(Skill skill) : base(skill)
    {
        _shadowClone = skill as ShadowClone;
    }

    public override void EnterState()
    {
        if (_shadowClone.CanUseSkill())
        {
            _shadowClone.CreateClone();
        }
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}