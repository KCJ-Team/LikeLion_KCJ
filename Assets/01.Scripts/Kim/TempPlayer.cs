using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayer : MonoBehaviour
{
    public InventoryObject inventory;
    public CardObject card;

    private void Start()
    {
        inventory.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            inventory.AddItem(new Card(card), 1);
        }
    }
}
