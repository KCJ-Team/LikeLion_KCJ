using UnityEngine;

public class InvisibilityState : SkillState
{
    private Invisibility invisibility;
    
    public InvisibilityState(Skill skill) : base(skill)
    {
        invisibility = skill as Invisibility;
    }

    public override void EnterState()
    {
        invisibility.Invisible();
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        
    }
}