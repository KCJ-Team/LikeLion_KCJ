using UnityEngine;
using System.Linq;

public class EquipmentManager : MonoBehaviour
{
    private PlayerData playerData;
    //public WeaponManager weaponManager;
    public StaticInterface equipmentInterface;

    private void Start()
    {
        playerData = LobbyMenuManager.Instance.playerData;
        //weaponManager = GameManager.Instance.Player.GetComponent<WeaponManager>();
        
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
        var QskillSlot = playerData.equipment.GetSlots[1];
        var EskillSlot = playerData.equipment.GetSlots[2];
        var BuffskillSlot = playerData.equipment.GetSlots[3];
        
        if (weaponSlot != null)
        {
            var itemObject = weaponSlot.GetItemObject();
            
            if (weaponSlot.card.Id >= 0 && itemObject != null && itemObject is WeaponCardObject weaponCard)
            {
                playerData.EquipWeapon(weaponCard); 
                //weaponManager.UpdateWeapon();
            }
            else
            {
                playerData.UnequipWeapon();
                //weaponManager.UpdateWeapon();
            }
        }

        if (QskillSlot != null)
        {
            var itemObject = QskillSlot.GetItemObject();
            
            if (itemObject is SkillCardObject skillCard)
            {
                playerData.EquipQSkill(skillCard);
            }
        }
        
        if (EskillSlot != null)
        {
            var itemObject = EskillSlot.GetItemObject();
            
            if (itemObject is SkillCardObject skillCard)
            {
                playerData.EquipESkill(skillCard);
            }
        }
        
        if (BuffskillSlot != null)
        {
            var itemObject = BuffskillSlot.GetItemObject();
            
            if (itemObject is BuffCardObject buffCard)
            {
                playerData.EquipBuffSkill(buffCard);
            }
        }
    }
}