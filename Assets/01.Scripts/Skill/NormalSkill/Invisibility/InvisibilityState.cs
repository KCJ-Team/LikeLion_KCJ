using UnityEngine;

public class InvisibilityState : SkillState
{
    private Invisibility _invisibility;
    
    public InvisibilityState(Skill skill) : base(skill)
    {
        _invisibility = skill as Invisibility;
    }

    public override void EnterState()
    {
        if (_invisibility.CanUseSkill())
        {
            _invisibility.Invisible();
        }
    }

    public override void UpdateState()
    {
        
    }

    public override void ExitState()
    {
        // if (_invisibility.IsInvisible())
        // {
        //     Debug.Log("나가기");
        //     _invisibility.Visible();
        //     Object.Destroy(_invisibility.gameObject);
        // }
    }
}