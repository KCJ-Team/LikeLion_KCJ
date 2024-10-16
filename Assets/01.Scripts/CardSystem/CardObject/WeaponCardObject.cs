using UnityEngine;

// 무기 카드 오브젝트 클래스
[CreateAssetMenu(fileName = "New WeaponCard", menuName = "Inventory System/Cards/WeaponCard")]
public class WeaponCardObject : CardObject
{
    public WeaponData weaponData; // 무기 관련 데이터

    // 무기 카드 인스턴스 생성 메서드
    public override Card CreateCard()
    {
        return new WeaponCard(this);
    }
}
