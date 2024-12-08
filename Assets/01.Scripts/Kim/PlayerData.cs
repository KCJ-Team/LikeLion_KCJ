using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "SO/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Avartar")]
    public GameObject Character;
    
    // 기본 스탯들
    [Header("Status")]
    public float BaseHP;
    public float MoveSpeed;
    public float AttackPower;
    public float Defense;
    
    // 현재 장착한 장비 및 인벤토리, equipment 데이터
    [Header("Skill and Inventory")]
    public WeaponCardObject currentWeapon;
    public InventoryObject inventory;
    public InventoryObject equipment;
    public List<CardObject> skillCards;
    
    public void Init()
    {
        currentWeapon = null;
    }
    
    public void EquipWeapon(WeaponCardObject weapon)
    {
        currentWeapon = weapon;
    }
    
    public void UnequipWeapon()
    {
        currentWeapon = null;
    }

    public void EquipSkill(CardObject skillCard)
    {
        skillCards.Add(skillCard);
    }
}