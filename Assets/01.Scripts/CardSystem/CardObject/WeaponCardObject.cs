using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory System/Cards/Weapon")]
public class WeaponCardObject : CardObject
{
    // 무기 속성
    public WeaponGrade grade;
    public WeaponType weaponType;
    
    public float attackSpeed;
    public float damage;
    
    public GameObject projectilePrefab; // 발사체 프리팹 (근접 무기는 null)
    public GameObject effectPrefab; // 공격 이펙트 프리팹
    
    
    // Card 클래스의 CreateCard 메서드를 오버라이드
    public override Card CreateCard()
    {
        return new WeaponCard(this);
    }
}