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

// 스킬 카드 클래스
public class SkillCard : Card
{
    /*public SkillData SkillData { get; private set; }

    public SkillCard(SkillCardObject skillCard) : base(skillCard)
    {
        SkillData = skillCard.skillData;
    }

    public void Use()
    {
        SkillData.Use();
    }*/
}

// 버프 카드 클래스
public class BuffCard : Card
{
    /*public BuffData BuffData { get; private set; }

    public BuffCard(BuffCardObject buffCard) : base(buffCard)
    {
        BuffData = buffCard.buffData;
    }

    public void Apply()
    {
        BuffData.Apply();
    }*/
}

// 아이템 카드 클래스
public class ItemCard : Card
{
    /*public ItemData ItemData { get; private set; }

    public ItemCard(ItemCardObject itemCard) : base(itemCard)
    {
        ItemData = itemCard.itemData;
    }

    public void Use()
    {
        ItemData.Use();
    }*/
}