using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Cards/Database")]
public class CardDatabase : ScriptableObject
{
    public CardObject[] CardObjects;

    public void OnValidate()
    {
        for (int i = 0; i < CardObjects.Length; i++)
        {
            CardObjects[i].data.Id = i;
        }
    }
}