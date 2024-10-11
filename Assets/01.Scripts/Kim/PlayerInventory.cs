using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerInventory", menuName = "Inventory/Player Inventory")]
public class PlayerInventory : ScriptableObject
{
    public List<Card> cards = new List<Card>();
}