using System;
using UnityEngine;

public class RainBullet : Skill
{
    [SerializeField] public float duration = 3f;          
    [SerializeField] private float radius = 5f;            
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private ParticleSystem skillEffect;   // 스킬 이펙트
    [SerializeField] private Animator animator;            // 애니메이터 컴포넌트
    [SerializeField] private string animationTrigger = "RainBullet"; // 애니메이션 트리거 이름

    public float Radius => radius;
    public LayerMask TargetLayer => targetLayer;
    public ParticleSystem SkillEffect => skillEffect;
    public Animator SkillAnimator => animator;
    public string AnimationTrigger => animationTrigger;

    private void Awake()
    {
        animator = GameManager.Instance.Player.GetComponent<Animator>();
    }

    public override SkillState GetInitialState()
    {
        return new RainBulletAttackState(this, damage);
    }
}