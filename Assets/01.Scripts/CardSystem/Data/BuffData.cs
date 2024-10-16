using UnityEngine;

// 기본 버프 데이터 클래스
public abstract class BuffData : ScriptableObject
{
    public string buffName;
    public float duration;

    // 버프 적용 메서드 (각 버프 타입별로 구현)
    public abstract void Apply();
}