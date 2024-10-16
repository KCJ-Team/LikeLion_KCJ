using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public CardType[] AllowedItems = new CardType[0];
    
    [System.NonSerialized]
    public UserInterface parent;
    
    [System.NonSerialized]
    public GameObject slotDisplay;
    
    [System.NonSerialized] 
    public Action<InventorySlot> onAfterUpdated;
    [System.NonSerialized] 
    public Action<InventorySlot> onBeforeUpdated;
    
    public Card card;
    public int amount;
    
    
    public CardObject GetItemObject()
    {
        return card.Id >= 0 ? parent.inventory.database.CardObjects[card.Id] : null;
    }
    
    public InventorySlot() => UpdateSlot(new Card(), 0);

    public InventorySlot(Card item, int amount) => UpdateSlot(item, amount);

    public void RemoveItem() => UpdateSlot(new Card(), 0);

    public void AddAmount(int value) => UpdateSlot(card, amount += value);
    
    public void UpdateSlot(Card itemValue, int amountValue)
    {
        onBeforeUpdated?.Invoke(this);
        card = itemValue;
        amount = amountValue;
        onAfterUpdated?.Invoke(this);
    }
    
    public bool CanPlaceInSlot(CardObject itemObject)
    {
        if (AllowedItems.Length <= 0 || itemObject == null || itemObject.cardData.Id < 0)
            return true;
        
        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (itemObject.type == AllowedItems[i])
                return true;
        }
        return false;
    }
}