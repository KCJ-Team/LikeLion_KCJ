using System.Collections.Generic;

// 버프의 종류를 정의하는 열거형
public enum BuffType
{
    MoveSpeed,    // 이동 속도
    AttackPower,  // 공격력
    Defense,       // 방어력
    AttackSpeed     //공격 속도
}

// 개별 버프 효과를 정의하는 클래스
// Unity Inspector에서 설정 가능하도록 Serializable 속성 추가
[System.Serializable]
public class BuffEffect
{
    public BuffType type;    // 버프 타입
    public float value;      // 효과 수치 (퍼센트 단위)
}

// 버프의 기본 정보를 담는 클래스
public class Buff
{
    public string id;                    // 버프 고유 식별자
    public string name;                  // 버프 이름
    public float duration;               // 지속 시간
    public List<BuffEffect> effects;     // 버프 효과 목록
    
    // 버프 생성자
    public Buff(string id, string name, float duration, List<BuffEffect> effects)
    {
        this.id = id;
        this.name = name;
        this.duration = duration;
        this.effects = effects;
    }
}