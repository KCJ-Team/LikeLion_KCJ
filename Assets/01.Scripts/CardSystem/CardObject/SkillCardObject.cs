using UnityEngine;

// 스킬 카드 오브젝트 클래스
[CreateAssetMenu(fileName = "New SkillCard", menuName = "Inventory System/Cards/SkillCard")]
public class SkillCardObject : CardObject
{
    public SkillData skillData; // 스킬 관련 데이터

    // 스킬 카드 인스턴스 생성 메서드
    public override Card CreateCard()
    {
        return new SkillCard(this);
    }
}

