using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBuilding : MonoBehaviour, IBuilding
{
    [SerializeField]
    private Building building;
    
    private BuildingStateMachine stateMachine;
    
    public bool IsCreated { get; set; } = false; // 생성 여부를 나타내는 속성

    private void Awake()
    {
        stateMachine = new BuildingStateMachine(); // 각 빌딩별 고유의 상태 머신 생성
    }
    
    public virtual void Build()
    {
        Debug.Log(building.BuildingData.name + " has been built.");
        
        if (!IsCreated)
        {
            IsCreated = true;
            Debug.Log($"{building.BuildingData.name} has been built.");
            
            building.CurretProductionOutput = building.BuildingData.productionOutput;
            Debug.Log($"{building.BuildingData.name} 생산량 초기화: {building.CurretProductionOutput}");
        }
    }

    public virtual void Upgrade()
    {
        if (CanUpgrade())
        {
            // building.CurrentLevel++;
            
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
    
    // 레벨에 따라 적용되는 업그레이드 배율 계산
    public float CurrentMultiplier
    {
        get
        {
            int level = building.CurrentLevel;
            float baseMultiplier = building.BuildingData.upgradeMultiplier;

            // 레벨이 0일 때 isCreated 여부에 따라 배율 결정
            if (level == 0)
            {
                return IsCreated ? baseMultiplier : 1;
            }

            // 레벨이 1 이상일 때는 baseMultiplier * 2^level
            return baseMultiplier * Mathf.Pow(2, level);
        }
    }
    
    public void SetLevel(int level)
    {
        building.CurrentLevel = level; 
    }

    public void InitializeState()
    {
        switch (building.CurrentLevel)
        {
            case 0:
                stateMachine.ChangeState(new Level0State());
                break;
            case 1:
                stateMachine.ChangeState(new Level1State());
                break;
            case 2:
                stateMachine.ChangeState(new Level2State());
                break;
            default:
                Debug.LogWarning("-1 level for building initialization.");
                break;
        }
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