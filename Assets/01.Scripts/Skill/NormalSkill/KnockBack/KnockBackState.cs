using UnityEngine;

public class KnockBackState : SkillState
{
    private KnockBack knockBack;
    
    public KnockBackState(Skill skill) : base(skill)
    {
        knockBack = skill as KnockBack;
    }
    
    public override void EnterState()
    {
        knockBack.KnockBackObject();
    }
    
    public override void UpdateState()
    {
        
    }
    
    public override void ExitState()
    {
        
    }
}