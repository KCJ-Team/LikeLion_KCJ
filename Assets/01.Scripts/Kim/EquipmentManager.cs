using UnityEngine;
using System.Linq;

public class EquipmentManager : MonoBehaviour
{
    private PlayerData playerData;
    public StaticInterface equipmentInterface;

    private void Start()
    {
        playerData = LobbyMenuManager.Instance.playerData;
        
        if (equipmentInterface == null) return;
        
        equipmentInterface.inventory = playerData.equipment;
        
        foreach (var slot in playerData.equipment.GetSlots)
        {
            slot.onAfterUpdated += OnEquipmentUpdated;
        }
        
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
            }
            else
            {
                playerData.UnequipWeapon();
            }
        }

        for (int i = 1; i < playerData.equipment.GetSlots.Length; i++)
        {
            if (playerData.equipment.GetSlots[i] != null)
            {
                var itemObject = playerData.equipment.GetSlots[i].GetItemObject();

                if (itemObject is SkillCardObject skillCard)
                {
                    playerData.EquipSkill(skillCard);
                }
            }
        }
    }
}