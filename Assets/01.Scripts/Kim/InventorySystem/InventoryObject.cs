using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public CardDatabase database;
    public InventoryType invenType;
    public CardType type;

    public InventorySlot[] GetSlots => Container.Slots;
    
    [SerializeField] private Inventory Container = new Inventory();
    
    public bool AddItem(Card card, int amount)
    {
        if (EmptySlotCount <= 0) return false;
        
        //여기에 들어가야됨
        
        InventorySlot slot = FindItemOnInventory(card);
        
        if (!database.CardObjects[card.Id].stackable || slot == null)
        {
            GetEmptySlot().UpdateSlot(card, amount);
            return true;
        }
        
        slot.AddAmount(amount);
        return true;
    }
    
    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].card.Id <= -1)
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    public InventorySlot FindItemOnInventory(Card item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].card.Id == item.Id)
            {
                return GetSlots[i];
            }
        }
        return null;
    }

    public bool IsItemInInventory(CardObject item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].card.Id == item.cardData.Id)
            {
                return true;
            }
        }
        return false;
    }

    public InventorySlot GetEmptySlot()
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].card.Id <= -1)
            {
                return GetSlots[i];
            }
        }
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item1 == item2) return;
        
        if (item2.CanPlaceInSlot(item1.GetItemObject()) && item1.CanPlaceInSlot(item2.GetItemObject()))
        {
            InventorySlot temp = new InventorySlot(item2.card, item2.amount);
            item2.UpdateSlot(item1.card, item1.amount);
            item1.UpdateSlot(temp.card, temp.amount);
        }
    }
    
    public void Clear()
    {
        Container.Clear();
    }
}