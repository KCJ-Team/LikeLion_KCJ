using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "SO/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Avartar")]
    public GameObject Character;
    
    // 기본 스탯들
    [Header("Status")]
    public float BaseHP;
    public float BaseMP;
    public float MoveSpeed;
    public float RotationSpeed;
    public float AttackPower;  
    public float Defense;      
    
    // 현재 장착한 장비 및 인벤토리, equipment 데이터
    [Header("Skill and Inventory")]
    public WeaponCardObject currentWeapon;
    public SkillCardObject currentQSkill;
    public SkillCardObject currentESkill;
    public BuffCardObject currentBuff;
    public InventoryObject inventory;
    public InventoryObject equipment;
    

    // 무기 장착 메서드
    public void EquipWeapon(WeaponCardObject weapon)
    {
        currentWeapon = weapon;
    }

    // 무기 해제 메서드
    public void UnequipWeapon()
    {
        currentWeapon = null;
    }

    public void EquipQSkill(SkillCardObject skill)
    {
        currentQSkill = skill;
    }

    public void UnequipQSkill()
    {
        currentQSkill = null;
    }
    
    public void EquipESkill(SkillCardObject skill)
    {
        currentESkill = skill;
    }
    
    public void EquipBuffSkill(BuffCardObject buff)
    {
        currentBuff = buff;
    }
}