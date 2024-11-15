using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;

public interface IBuildingState
{
    void Build(BuildingStateMachine stateMachine, BaseBuilding buildingPrefab);
    void Init(BaseBuilding buildingPrefab);
}

// 레벨 0은 아무것도 X, 비활성화, Build를 누르면 그림만 활성화하고 생산 시작

public class Level0State : IBuildingState
{
    public void Build(BuildingStateMachine stateMachine, BaseBuilding buildingPrefab)
    {
        // Build시 자원 소비 로직
        Building building = buildingPrefab.GetBuilding();
        BuildingData buildingData = buildingPrefab.GetBuildingData();

        if (GameResourceManager.Instance.ConsumeResources(
                buildingData.baseCostEnergy, buildingData.baseCostFood, buildingData.baseCostFuel,
                buildingData.baseCostWorkforce))
        {
            Debug.Log($"{buildingData.name} upgraded to Level 1.");
            
            // 이미지 변경, 생산량 증가
            building.CurretProductionOutput = buildingData.productionOutput;
            Debug.Log($"Production output updated to {building.CurretProductionOutput}");
            
            BuildingManager.Instance.UpdateEnergyProductUIAndImage(buildingData.type , "", building.CurretProductionOutput);
            
            // 현재 레벨 증가
            building.CurrentLevel++;
            
            // 연구실이 아니라면
            if (buildingData.type != BuildingType.ResearchLab)
                stateMachine.ChangeState(new Level1State()); // 상태 전환
        }
        else
        {
            Debug.Log("Not enough resources to upgrade to Level 1.");
        }
    }

    // 자원 소비 로직만 빼고, Product Output, UI 설정
    public void Init(BaseBuilding buildingPrefab)
    {
        Building building = buildingPrefab.GetBuilding();
        building.CurretProductionOutput = building.BuildingData.productionOutput;
        
        BuildingData buildingData = buildingPrefab.GetBuildingData();
        BuildingManager.Instance.UpdateEnergyProductUIAndImage(buildingData.type , "", building.CurretProductionOutput);
        
        Debug.Log($"{building.BuildingData.name} initialized at Level 0 without consuming resources.");
    }
}

public class Level1State : IBuildingState
{
    public void Build(BuildingStateMachine stateMachine, BaseBuilding buildingPrefab)
    {
        Building building = buildingPrefab.GetBuilding();
        BuildingData buildingData = buildingPrefab.GetBuildingData();

        // 4가지 자원을 한 번에 소비 (업그레이드 배율 적용)
        if (GameResourceManager.Instance.ConsumeResources(
                (int)(buildingData.baseCostEnergy * buildingPrefab.CurrentMultiplier),
                (int)(buildingData.baseCostFood * buildingPrefab.CurrentMultiplier),
                (int)(buildingData.baseCostFuel * buildingPrefab.CurrentMultiplier),
                (int)(buildingData.baseCostWorkforce * buildingPrefab.CurrentMultiplier)))
        {
            Debug.Log($"{buildingData.name} upgraded to Level 2.");

            // 이미지 변경, 생산량 표시
            building.CurretProductionOutput = (int)(buildingData.productionOutput * buildingPrefab.CurrentMultiplier);
            Debug.Log($"Production output updated to {building.CurretProductionOutput}");

            string imagePath = buildingData.level1ImagePath;
            BuildingManager.Instance.UpdateEnergyProductUIAndImage(buildingData.type , imagePath, building.CurretProductionOutput);

            // 현재 레벨 증가
            building.CurrentLevel++;
            
            stateMachine.ChangeState(new Level2State()); // 상태 전환
        }
        else
        {
            Debug.Log("Not enough resources to upgrade to Level 2.");
        }
    }

    public void Init(BaseBuilding buildingPrefab)
    {
        Building building = buildingPrefab.GetBuilding();
        building.CurretProductionOutput = (int)(building.BuildingData.productionOutput * buildingPrefab.CurrentMultiplier);
        
        BuildingManager.Instance.UpdateEnergyProductUIAndImage(building.BuildingData.type, building.BuildingData.level1ImagePath, building.CurretProductionOutput);
        Debug.Log($"{building.BuildingData.name} initialized at Level 1 without consuming resources.");
    }
}

public class Level2State : IBuildingState
{
    public void Build(BuildingStateMachine stateMachine, BaseBuilding buildingPrefab)
    {
        Building building = buildingPrefab.GetBuilding();
        BuildingData buildingData = buildingPrefab.GetBuildingData();
        
        // 현재 레벨이 maxLevel에 도달했는지 확인
        if (building.CurrentLevel >= buildingData.maxLevel)
        {
            Debug.Log("Building is already at max level (Level 2).");
            return;  // 더 이상 업그레이드 불가
        }

        // 4가지 자원을 한 번에 소비 (업그레이드 배율 적용) => 의 2배.. 
        if (GameResourceManager.Instance.ConsumeResources(
                (int)(buildingData.baseCostEnergy * buildingPrefab.CurrentMultiplier),
                (int)(buildingData.baseCostFood * buildingPrefab.CurrentMultiplier),
                (int)(buildingData.baseCostFuel * buildingPrefab.CurrentMultiplier),
                (int)(buildingData.baseCostWorkforce * buildingPrefab.CurrentMultiplier)))
        {
            Debug.Log($"{buildingData.name} upgraded to Level 2.");

            // 이미지 변경, 생산량 표시
            building.CurretProductionOutput = (int)(buildingData.productionOutput * buildingPrefab.CurrentMultiplier);
            Debug.Log($"Production output updated to {building.CurretProductionOutput}");

            string imagePath = buildingData.level2ImagePath;
            BuildingManager.Instance.UpdateEnergyProductUIAndImage(buildingData.type , imagePath, building.CurretProductionOutput);

            // 현재 레벨 증가
            building.CurrentLevel++;
            
            // 최종 업그레이드 끝.
        }
        else
        {
            Debug.Log("Not enough resources to upgrade to Level 2.");
        }
        
        Debug.Log("Building is already at max level (Level 2).");

    }

    public void Init(BaseBuilding buildingPrefab)
    {
        Building building = buildingPrefab.GetBuilding();
        building.CurretProductionOutput = (int)(building.BuildingData.productionOutput * buildingPrefab.CurrentMultiplier);
        
        BuildingManager.Instance.UpdateEnergyProductUIAndImage(building.BuildingData.type, building.BuildingData.level2ImagePath, building.CurretProductionOutput);
        Debug.Log($"{building.BuildingData.name} initialized at Level 2 without consuming resources.");
    }
}

// 빌딩 UI 관리 FSM
public class BuildingStateMachine
{
    [SerializeField] 
    private IBuildingState currentState;

    public BuildingStateMachine()
    {
        // 기본 상태는 레벨 0
        currentState = new Level0State();
    }

    // 상태 전환
    public void ChangeState(IBuildingState newState)
    {
        currentState = newState;
    }

    // 빌드 실행 (상태에 따라 동작 처리)
    public void Build(BaseBuilding buildingPrefab)
    {
        currentState.Build(this, buildingPrefab);
    }
    
    // 저장된 데이터 로드시 빌딩 Init
    public void Init(BaseBuilding buildingPrefab)
    {
        currentState.Init(buildingPrefab);
    }
} // end class