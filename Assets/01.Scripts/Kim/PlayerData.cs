using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "SO/PlayerData")]
public class PlayerData : ScriptableObject
{
    public GameObject Character; // 선택한 캐릭터 모델
    // 스탯
    public float HP;
    public float MP;
    public float MoveSpeed;
    public float RotationSpeed;
    
    // 현재 장착한 장비 및 인벤토리, equipment 데이터
    public WeaponCardObject currentWeapon;
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
}