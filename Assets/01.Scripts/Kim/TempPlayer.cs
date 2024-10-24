using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TempPlayer : MonoBehaviour
{
    public PlayerData playerData;
    public InventoryObject inventory;
    public InventoryObject equipment;
    
    public CardObject card1;
    public CardObject card2;

    private void Start()
    {
        inventory.Clear();
        if (equipment != null)
        {
            for (int i = 0; i < equipment.GetSlots.Length; i++)
            {
                equipment.GetSlots[i].onBeforeUpdated += OnRemoveItem;
                equipment.GetSlots[i].onAfterUpdated += OnAddItem;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            //인벤토리에 추가하는 방법
            inventory.AddItem(new Card(card1), 1);
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            inventory.AddItem(new Card(card2), 1);
        }
    }

    private void OnRemoveItem(InventorySlot slot)
    {
        if (slot.GetItemObject() == null) return;

        if (slot.parent.inventory.invenType == InventoryType.Equipment)
        {
            if (slot.GetItemObject().characterDisplay != null)
            {
                switch (slot.AllowedItems[0])
                {
                    case CardType.Weapon :
                        playerData.currentWeapon = null;
                        break;
                }
            }
        }
        
    }

    private void OnAddItem(InventorySlot slot)
    {
        if (slot.GetItemObject() == null) return;

        if (slot.parent.inventory.invenType == InventoryType.Equipment)
        {
            if (slot.GetItemObject().characterDisplay != null)
            {
                switch (slot.AllowedItems[0])
                {
                    case CardType.Weapon :
                        //playerData.currentWeapon = slot.GetItemObject().characterDisplay;
                        break;
                }
            }
        }
    }
}
