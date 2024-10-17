using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuilding : MonoBehaviour, IBuilding
{
    [SerializeField]
    private Building building;

    public virtual void Build()
    {
        Debug.Log(building.BuildingData.name + " has been built.");
    }

    public virtual void Upgrade()
    {
        if (CanUpgrade())
        {
            building.CurrentLevel++;
            Debug.Log(building.BuildingData.name + " upgraded to level " + building.CurrentLevel);
        }
        else
        {
            Debug.Log("Cannot upgrade " + building.BuildingData.name);
        }
    }
    
    public bool CanUpgrade()
    {
        return building.CurrentLevel < building.BuildingData.maxLevel;
    }

    // 현재 빌딩의 생산량 반환
    public virtual int GetProductOutput()
    {
        return building.BuildingData.productionOutput;
    }
    
    public int GetCurrentProductOutput()
    {
        return building.CurretProductionOutput;
    }

    public Building GetBuilding()
    {
        return building;
    }
    
    public BuildingData GetBuildingData()
    {
        return building.BuildingData;
    }
}