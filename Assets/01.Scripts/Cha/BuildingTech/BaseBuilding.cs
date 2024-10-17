using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuilding : MonoBehaviour, IBuilding
{
    [SerializeField]
    private BuildingData buildingData; // 빌딩의 ScriptableObject 데이터
    protected int currentLevel = 0; // 0, 1, 2(최종 업그레이드)

    public virtual void Build()
    {
        Debug.Log(buildingData.name + " has been built.");
    }

    public virtual void  Upgrade()
    {
        if (CanUpgrade())
        {
            currentLevel++;
            Debug.Log(buildingData.name + " upgraded to level " + currentLevel);
        }
        else
        {
            Debug.Log("Cannot upgrade " + buildingData.name);
        }
    }
    
    public bool CanUpgrade()
    {
        return currentLevel < buildingData.maxLevel;
    }

    // 현재 빌딩의 생산량 반환
    public virtual int GetProductOutput()
    {
        return buildingData.productionOutput;
    }

    public BuildingData GetBuildingData()
    {
        return buildingData;
    }
    
    public BuildingData SetBuildingData(BuildingData aBuildingData)
    {
        return buildingData = aBuildingData;
    }
}