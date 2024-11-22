using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

public class BuildingManager : SceneSingleton<BuildingManager>
{
    [Header("Buildings")] [SerializeField] private SerializedDictionary<BuildingType, BaseBuilding> buildings = new();

    [Header("UI MVP 패턴")] [SerializeField] private BuildingUIView buildingUIView;
    private BuildingUIPresenter buildingUIPresenter;

    [Header("특정 빌딩의 UseButton 상태 플래그")] public bool isRecoveryRoomUsed = false; // 병원 UseButton 플래그
    public bool isRecreationRoomUsed = false; // 오락시설 UseButton 플래그
    
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

    public SerializedDictionary<BuildingType, BaseBuilding> GetBuildings()
    {
        return buildings;
    }

    public void BuildOrUpgrade(BaseBuilding buildingPrefab)
    {
        // 개별 빌딩의 상태 머신을 통해 빌드 실행
        buildingPrefab.GetStateMachine().Build(buildingPrefab);
    }

    private bool hasProducedAt6AM = false;
    private bool hasProducedAt12PM = false;
    private bool hasProducedAt18PM = false;
    
    // 특정 시간에 자원을 생산하도록 관리
    public void ProduceResourcesAtScheduledTimes(int hour)
    {
        if (hour == 6 && !hasProducedAt6AM)
        {
            ProduceAllResources();
            hasProducedAt6AM = true; // 6시 생산 완료 플래그 설정
        }
        else if (hour == 18 && !hasProducedAt12PM)
        {
            ProduceAllResources();
            hasProducedAt18PM = true; // 18시 생산 완료 플래그 설정
        }
        else if (hour == 12 && !hasProducedAt18PM)
        {
            ProduceAllResources();
            hasProducedAt12PM = true; // 12시 생산 완료 플래그 설정
        }
    }

    // 모든 빌딩의 자원을 생산하는 메서드
    private void ProduceAllResources()
    {
        Debug.Log("6시랑 18시, ProduceAllResources()");

        foreach (var building in buildings.Values)
        {
            if (building.IsCreated)
            {
                Debug.Log($"6시랑 18시, ProduceAllResources()_{building.name} is 만들어진 빌딩");

                int productionAmount = building.GetCurrentProductOutput();

                // 만약 현재 빌딩 타입이 병원, 오락시설일때 눌려 있는 상태라면 
                if (building.GetBuildingData().type == BuildingType.RecoveryRoom)
                {
                    if (isRecoveryRoomUsed)
                    {
                        LobbyMenuManager.Instance.ChangeHp(productionAmount);

                        buildingUIPresenter.ShowProcessIcon(building.GetBuildingData().type);
                    }
                }
                else if (building.GetBuildingData().type == BuildingType.RecreationRoom)
                {
                    if (isRecreationRoomUsed)
                    {
                        LobbyMenuManager.Instance.ChangeStress(productionAmount);

                        buildingUIPresenter.ShowProcessIcon(building.GetBuildingData().type);
                    }
                }
                else
                {
                    GameResourceManager.Instance.AddResource(building.GetBuildingData().resourceType, productionAmount);

                    buildingUIPresenter.ShowProcessIcon(building.GetBuildingData().type);
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
        // 자원이 업그레이드가 가능할떄, 충분할떄 현재 레벨을 확인해야한다.. 
        if (building.GetBuilding().CurrentLevel < building.GetBuildingData().maxLevel)
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

        return false;
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
                
                // Punch 애니메이션 반복
               // buildingUIPresenter.StartPunchAnimation(buildingType);
            }

            Debug.Log($"{buildingType} 레벨이 {level}로 설정되었습니다.");
        }
        else
        {
            Debug.LogWarning($"{buildingType} 빌딩을 찾을 수 없습니다.");
        }
    }

    public void CheckBuildingAnimationLoop()
    {
        foreach (var kvp in GetBuildings())
        {
            BaseBuilding building = kvp.Value;

            if (building.IsCreated)
            {
                buildingUIPresenter.StartBuildingAnimation(building.GetBuildingData().type);
            }
        }
    }
    
    // 업그레이드가 가능하면 이미지 아이콘 활성화하기
    public void CheckUpgradeAvailability()
    {
        foreach (var kvp in GetBuildings())
        {
            BuildingType buildingType = kvp.Key;
            BaseBuilding building = kvp.Value;

            if (CanUpgradeBuilding(building))
            {
                // 업그레이드 가능 UI 활성화
                buildingUIPresenter.ShowEnableUpgradeIcon(buildingType);
            }
            else
            {
                // 업그레이드 가능 UI 비활성화
                buildingUIPresenter.HideUpgradeIcon(buildingType);
            }
        }
    }
} // end class