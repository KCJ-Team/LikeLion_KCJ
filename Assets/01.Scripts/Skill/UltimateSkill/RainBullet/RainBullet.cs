using UnityEngine;

public class RainBullet : Skill
{
    [SerializeField] public float duration = 3f;          // 스킬 지속 시간
    [SerializeField] private float radius = 5f;            // 데미지를 주는 범위
    [SerializeField] private LayerMask targetLayer;        // 데미지를 받을 레이어

    public float Damage => damage;
    public float Radius => radius;
    public LayerMask TargetLayer => targetLayer;

    public override SkillState GetInitialState()
    {
        return new RainBulletAttackState(this);
    }
}