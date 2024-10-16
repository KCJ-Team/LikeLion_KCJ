using UnityEngine;

// 기본 무기 데이터 클래스
public abstract class WeaponData : ScriptableObject
{
    public string weaponName;
    public int damage;
    public float attackSpeed;

    // 무기 사용 메서드 (각 무기 타입별로 구현)
    public abstract void Use();
}

//무기 데이터
[CreateAssetMenu(fileName = "New Sword Data", menuName = "Inventory System/Weapons/Sword")]
public class SwordData : WeaponData
{
    public float criticalChance;

    public override void Use()
    {
        Debug.Log($"Swinging {weaponName} with {damage} damage");
    }
}

