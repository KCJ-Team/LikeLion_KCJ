using UnityEngine;

// 기본 아이템 데이터 클래스
public abstract class ItemData : ScriptableObject
{
    public string itemName;
    public bool consumable;

    // 아이템 사용 메서드 (각 아이템 타입별로 구현)
    public abstract void Use();
}