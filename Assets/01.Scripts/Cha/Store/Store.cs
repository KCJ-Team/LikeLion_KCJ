using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Store
{
    public int storeItemId;
    public CardObject storeCard;
    public int cost = 200;
}
