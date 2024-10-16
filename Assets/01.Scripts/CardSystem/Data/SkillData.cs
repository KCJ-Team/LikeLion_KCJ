using UnityEngine;

// 기본 스킬 데이터 클래스
public abstract class SkillData : ScriptableObject
{
    public string skillName;
    public float cooldown;
    public float manaCost;

    // 스킬 사용 메서드 (각 스킬 타입별로 구현)
    public abstract void Use();
}

