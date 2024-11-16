using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

public class BuildingManager : SceneSingleton<BuildingManager>
{
    [Header("Buildings")] 
    [SerializeField] 
    private SerializedDictionary<BuildingType, BaseBuilding> buildings = new();

    [Header("UI MVP 패턴")] 
    [SerializeField] 
    private BuildingUIView buildingUIView;
    private BuildingUIPresenter buildingUIPresenter;
    
    [Header("특정 빌딩의 UseButton 상태 플래그")]
    public bool isRecoveryRoomUsed = false; // 병원 UseButton 플래그
    public bool isRecreationRoomUsed = false; // 오락시설 UseButton 플래그

    // 자원 생산 플래그
    private bool hasProducedAt6AM = false;
    private bool hasProducedAt6PM = false;
    private bool hasProducedAt12PM = false; // 12시 자원 생산 플래그
    
    private void Start()
    {
        if (buildingUIPresenter == null)
        {
            buildingUIPresenter = new BuildingUIPresenter(buildingUIView);
        }
    }
    
    public BaseBuilding GetBuilding(BuildingType buildingType)
    {
        if (buildings.TryGetValue(buildingType, out BaseBuilding building))
        {
            return building; // 해당 BuildingType의 빌딩 반환
        }
        else
        {
            Debug.LogWarning($"Building of type {buildingType} not found.");
            return null; // 없을 경우 null 반환
        }
    }

    public void BuildOrUpgrade(BaseBuilding buildingPrefab)
    {
        // 개별 빌딩의 상태 머신을 통해 빌드 실행
        buildingPrefab.GetStateMachine().Build(buildingPrefab);
    }
    
    // 특정 시간에 자원을 생산하도록 관리
    public void ProduceResourcesAtScheduledTimes(int hour)
    {
        if (hour == 6 && !hasProducedAt6AM)
        {
            ProduceAllResources();
            hasProducedAt6AM = true;
            hasProducedAt6PM = false; // 오후 생산 플래그 초기화
        }
        else if (hour == 18 && !hasProducedAt6PM)
        {
            ProduceAllResources();
            hasProducedAt6PM = true;
            hasProducedAt6AM = false; // 다음 날 오전 생산 플래그 초기화
        }
        else if (hour == 12)
        {
            ProduceAllResources();
        }
    }

    // 모든 빌딩의 자원을 생산하는 메서드
    private void ProduceAllResources()
    {
        foreach (var building in buildings.Values)
        {
            if (building.IsCreated)
            {
                int productionAmount = building.GetCurrentProductOutput();
                
                // 만약 현재 빌딩 타입이 병원, 오락시설일때 눌려 있는 상태라면 
                if (building.GetBuildingData().type == BuildingType.RecoveryRoom)
                {
                    if (isRecoveryRoomUsed)
                    {
                        LobbyMenuManager.Instance.ChangeHp(productionAmount);
                    }
                }
                else if (building.GetBuildingData().type == BuildingType.RecreationRoom)
                {
                    if (isRecreationRoomUsed)
                    {
                        LobbyMenuManager.Instance.ChangeStress(productionAmount);
                    }
                }
                else
                {
                    GameResourceManager.Instance.AddResource(building.GetBuildingData().resourceType, productionAmount);
                }
                
                Debug.Log($"{building.GetBuildingData().name} produced {productionAmount} resources.");
            }
        }
    }

    // 자원 생산량 텍스트 표시, 빌딩 이미지 변경
    public void UpdateEnergyProductUIAndImage(BuildingType type, string imagePath, int productionOutput)
    {
        if (buildingUIPresenter == null)
        {
            buildingUIPresenter = new BuildingUIPresenter(buildingUIView);
        }
        
        buildingUIPresenter.UpdateProductUIAndImage(type, imagePath, productionOutput);
    }

    // 빌딩 업그레이드에 필요한 자원을 확인하는 메서드
    public bool CanUpgradeBuilding(BaseBuilding building)
    {
        BuildingData buildingData = building.GetBuildingData();
        float multiplier = building.CurrentMultiplier; // 레벨에 따른 배율 적용

        // 멀티플라이어가 적용된 업그레이드 비용 계산
        int energyCost = (int)(buildingData.baseCostEnergy * multiplier);
        int foodCost = (int)(buildingData.baseCostFood * multiplier);
        int fuelCost = (int)(buildingData.baseCostFuel * multiplier);
        int workforceCost = (int)(buildingData.baseCostWorkforce * multiplier);

        // 자원이 충분한지 검사
        return GameResourceManager.Instance.GetResourceAmount(ResourceType.Energy) >= energyCost &&
               GameResourceManager.Instance.GetResourceAmount(ResourceType.Food) >= foodCost &&
               GameResourceManager.Instance.GetResourceAmount(ResourceType.Fuel) >= fuelCost &&
               GameResourceManager.Instance.GetResourceAmount(ResourceType.Workforce) >= workforceCost;
    }
    
    // 빌딩의 레벨을 Get
    public int GetBuildingLevel(BuildingType buildingType)
    {
        if (buildings.TryGetValue(buildingType, out BaseBuilding building))
        {
            return building.GetBuilding().CurrentLevel; // BaseBuilding의 현재 레벨 반환
        }
        else
        {
            Debug.LogWarning($"{buildingType} 빌딩을 찾을 수 없습니다.");
            return -1; // 빌딩이 없을 경우 -1 반환 (에러 표시용)
        }
    }
    
    // 빌딩의 레벨을 설정
    public void SetBuildingLevel(BuildingType buildingType, int level)
    {
        if (buildings.TryGetValue(buildingType, out BaseBuilding building))
        {
            building.SetLevel(level); // BaseBuilding에서 레벨 설정
            building.InitializeState(); // 상태 초기화

            if (building.GetBuilding().CurrentLevel > -1)
            {
                building.IsCreated = true;
                building.GetStateMachine().Init(building);
            }
            
            Debug.Log($"{buildingType} 레벨이 {level}로 설정되었습니다.");
        }
        else
        {
            Debug.LogWarning($"{buildingType} 빌딩을 찾을 수 없습니다.");
        }
    }
} // end class