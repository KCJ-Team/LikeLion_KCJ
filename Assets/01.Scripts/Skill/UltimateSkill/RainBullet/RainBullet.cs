using System;
using UnityEngine;

public class RainBullet : Skill
{
    [SerializeField] public float duration = 3f;          
    [SerializeField] private float radius = 5f;            
    [SerializeField] private LayerMask targetLayer;
    
    
    [SerializeField] private GameObject skillEffect;   // 스킬 이펙트
    [SerializeField] private Transform effectParent;   // 이펙트가 생성될 부모 Transform
    [SerializeField] private Animator animator;        // 애니메이터 컴포넌트
    
    [SerializeField] private string animationTrigger = "RainBullet"; // 애니메이션 트리거 이름

    public float Radius => radius;
    public LayerMask TargetLayer => targetLayer;
    public Animator SkillAnimator => animator;
    public GameObject SkillEffect => skillEffect;
    public Transform EffectParent => effectParent;
    public string AnimationTrigger => animationTrigger;

    private void Awake()
    {
        animator = GameManager.Instance.Player.GetComponent<Animator>();
        //effectParent = GameManager.Instance.Player.transform;
    }


    public override SkillState GetInitialState()
    {
        return new RainBulletAttackState(this, damage);
    }
}