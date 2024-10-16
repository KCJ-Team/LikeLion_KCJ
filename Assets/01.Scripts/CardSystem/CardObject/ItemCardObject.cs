using UnityEngine;

// 아이템 카드 오브젝트 클래스
[CreateAssetMenu(fileName = "New ItemCard", menuName = "Inventory System/Cards/ItemCard")]
public class ItemCardObject : CardObject
{
    public ItemData itemData; // 아이템 관련 데이터

    // 아이템 카드 인스턴스 생성 메서드
    public override Card CreateCard()
    {
        return new ItemCard(this);
    }
}

