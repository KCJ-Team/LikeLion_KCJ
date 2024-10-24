using UnityEngine;

// 스킬 카드 오브젝트 클래스
[CreateAssetMenu(fileName = "New Skill", menuName = "Inventory System/Cards/Skill")]
public class SkillCardObject : CardObject
{
    public Skill skillData;
}