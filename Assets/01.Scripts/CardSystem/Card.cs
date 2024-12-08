using UnityEngine;

// 기본 카드 클래스
[System.Serializable]
public class Card
{
    public string Name; // 카드 이름
    public int Id = -1; // 카드 고유 ID

    // 기본 생성자
    public Card()
    {
        Name = "";
        Id = -1;
    }

    // CardObject로부터 카드 생성
    public Card(CardObject card)
    {
        Name = card.name;
        Id = card.cardData.Id;
    }
}

// 무기 카드 클래스
public class WeaponCard : Card
{
    public WeaponCardObject WeaponData { get; private set; }

    public WeaponCard(WeaponCardObject weaponCard) : base(weaponCard)
    {
        WeaponData = weaponCard;
    }
}