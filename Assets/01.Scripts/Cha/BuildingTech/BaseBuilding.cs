using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuilding : MonoBehaviour, IBuilding
{
    [SerializeField]
    private Building building;
    
    private BuildingStateMachine stateMachine;

    private void Awake()
    {
        stateMachine = new BuildingStateMachine(); // 각 빌딩별 고유의 상태 머신 생성
    }

    public virtual void Build()
    {
        Debug.Log(building.BuildingData.name + " has been built.");
        
        // 초기 생산량을 기본 생산량으로 설정
        building.CurretProductionOutput = building.BuildingData.productionOutput;
        Debug.Log($"{building.BuildingData.name} 생산량 초기화: {building.CurretProductionOutput}");
    }

    public virtual void Upgrade()
    {
        if (CanUpgrade())
        {
            building.CurrentLevel++;
            
            // TODO : 업그레이드시 생산량 증가..
            
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
    
    // 현재 빌딩의 현재 자원 생산량(업그레이드 된 경우가 있을 수 있으니)
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
    
    public BuildingStateMachine GetStateMachine()
    {
        return stateMachine;
    }
} // end class