using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 버프를 부여하는 스킬의 기본 클래스
// Skill 클래스를 상속받아 버프 관련 기능 추가
public abstract class BuffSkill : Skill
{
    public float skillDuration;
    public GameObject EffectPrefab;
    public List<BuffEffect> buffEffects = new List<BuffEffect>();   // 적용할 버프 효과들
    
    // 초기 상태를 BuffSkillState로 설정
    public override SkillState GetInitialState()
    {
        return new BuffSkillState(this);
    }

    // 버프의 고유 ID를 반환하는 추상 메서드
    public abstract string GetBuffId();
    // 버프의 이름을 반환하는 추상 메서드
    public abstract string GetBuffName();
}
