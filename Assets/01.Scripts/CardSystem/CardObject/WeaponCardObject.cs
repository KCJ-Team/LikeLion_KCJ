using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory System/Cards/Weapon")]
public class WeaponCardObject : CardObject
{
    // 무기 속성
    public WeaponGrade grade;
    public WeaponType weaponType;
    
    [SerializeField]
    private float baseDamage; // 기본 데미지 값
    public float attackSpeed;
    public int Ammo;
    
    public GameObject projectilePrefab; // 발사체 프리팹 (근접 무기는 null)
    
    // 등급에 따른 데미지 계수 설정
    private const float NORMAL_DAMAGE_MULTIPLIER = 1.0f;
    private const float EPIC_DAMAGE_MULTIPLIER = 1.2f;
    private const float LEGEND_DAMAGE_MULTIPLIER = 1.5f;
    
    // 등급에 따라 계산된 최종 데미지를 반환하는 프로퍼티
    public float damage
    {
        get
        {
            switch (grade)
            {
                case WeaponGrade.Epic:
                    return baseDamage * EPIC_DAMAGE_MULTIPLIER;
                case WeaponGrade.Legend:
                    return baseDamage * LEGEND_DAMAGE_MULTIPLIER;
                default: // WeaponGrade.Normal
                    return baseDamage * NORMAL_DAMAGE_MULTIPLIER;
            }
        }
    }
    
    // Card 클래스의 CreateCard 메서드를 오버라이드
    public override Card CreateCard()
    {
        return new WeaponCard(this);
    }
}