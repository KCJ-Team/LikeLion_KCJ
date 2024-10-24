using System.Collections.Generic;

public enum BuffType
{
    MoveSpeed,
    MaxHealth,
    AttackPower,
    Defense
}

// 버프 효과를 정의하는 클래스
[System.Serializable]
public class BuffEffect
{
    public BuffType type;
    public float value; // 증가량 (퍼센트)
}

// 버프 정보를 담는 클래스
public class Buff
{
    public string id;
    public string name;
    public float duration;
    public List<BuffEffect> effects;
    
    public Buff(string id, string name, float duration, List<BuffEffect> effects)
    {
        this.id = id;
        this.name = name;
        this.duration = duration;
        this.effects = effects;
    }
}