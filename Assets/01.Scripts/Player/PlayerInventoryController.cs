using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryController
{
    private readonly PlayerData _playerData;

    public PlayerInventoryController(PlayerData playerData)
    {
        _playerData = playerData;
    }

    public void Initialize()
    {
        _playerData.inventory.Clear();
        _playerData.equipment.Clear();
    }

    public void HandleInventory()
    {
        // Handle inventory related logic here
        // You can move the InvenToCard logic here if needed
    }
}
