using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Building
{
    [SerializeField]
    private BuildingData buildingData;
    
    [SerializeField]
    private int currentLevel = -1; // 현재 빌딩의 레벨. 처음에는 -1로 하자
    
    [SerializeField]
    private int currentProductionOutput; // 현재 자원 생산량
    
    public BuildingData BuildingData
    {
        get => buildingData;
        set => buildingData = value;
    }

    public int CurrentLevel
    {
        get => currentLevel;
        set => currentLevel = value;
    }

    public int CurretProductionOutput
    {
        get => currentProductionOutput;
        set => currentProductionOutput = value;
    }
    
    public Building()
    {
        
    }
}
