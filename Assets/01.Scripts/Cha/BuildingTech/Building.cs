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
    private int currentLevel;
    
    [SerializeField]
    private int currentProductionOutput;
    
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
