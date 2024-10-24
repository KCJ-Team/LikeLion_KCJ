using UnityEngine;
using System.Linq;

public class EquipmentManager : MonoBehaviour
{
    public PlayerData playerData;
    public WeaponManager weaponManager;
    public StaticInterface equipmentInterface;

    private void Start()
    {
        if (equipmentInterface == null) return;
        
        // 인터페이스 초기화
        equipmentInterface.inventory = playerData.equipment;

        // 모든 장비 슬롯에 대해 업데이트 이벤트 등록
        foreach (var slot in playerData.equipment.GetSlots)
        {
            slot.onAfterUpdated += OnEquipmentUpdated;
        }

        // 초기 장비 상태 설정
        UpdateEquipment();
    }

    private void OnEquipmentUpdated(InventorySlot slot)
    {
        UpdateEquipment();
    }

    private void UpdateEquipment()
    {
        var weaponSlot = playerData.equipment.GetSlots.FirstOrDefault();
        
        if (weaponSlot != null)
        {
            var itemObject = weaponSlot.GetItemObject();
            
            if (weaponSlot.card.Id >= 0 && itemObject != null && itemObject is WeaponCardObject weaponCard)
            {
                playerData.EquipWeapon(weaponCard);
                weaponManager.UpdateWeapon();
            }
            else
            {
                playerData.UnequipWeapon();
                weaponManager.UpdateWeapon();
            }
        }
    }
}