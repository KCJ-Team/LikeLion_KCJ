using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameResource
{
    [SerializeField]
    private ResourceData resourceData;
    
    [SerializeField]
    private int currentAmount;
    
    public ResourceData ResourceData
    {
        get => resourceData;
        set => resourceData = value;
    }

    public int CurrentAmount
    {
        get => currentAmount;
        set => currentAmount = value;
    }
    
    public GameResource(ResourceData data)
    {
        resourceData = data;
        currentAmount = 0; // 기본값을 설정하거나 초기값으로 설정
    }
}
