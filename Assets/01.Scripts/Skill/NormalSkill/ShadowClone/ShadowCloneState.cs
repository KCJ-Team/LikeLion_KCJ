using UnityEngine;

public class ShadowCloneState : SkillState
{
    private ShadowClone shadowClone;
    
    public ShadowCloneState(Skill skill) : base(skill)
    {
        shadowClone = skill as ShadowClone;
    }

    public override void EnterState()
    {
        shadowClone.SpawnClone();
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}