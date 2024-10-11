using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

[System.Serializable]
public class Inventory
{
    public InventorySlot[] Slots = new InventorySlot[35];   //35개 수정
    
    public void Clear()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].card = new Card();
            Slots[i].amount = 0;
        }
    }

    public bool ContainsItem(CardObject cardObject)
    {
        return Array.Find(Slots, i => i.card.Id == cardObject.data.Id) != null;
    }

    public bool ContainsItem(int id)
    {
        return Slots.FirstOrDefault(i => i.card.Id == id) != null;
    }
}