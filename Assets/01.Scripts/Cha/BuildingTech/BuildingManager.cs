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

    [Header("빌딩의 자원 생산 코루틴 관리")]
    private Dictionary<BaseBuilding, Coroutine> productionCoroutines = new();

    private void Start()
    {
        buildingUIPresenter = new BuildingUIPresenter(buildingUIView);
    }

    public void Build(BaseBuilding buildingPrefab)
    {
        // 개별 빌딩의 상태 머신을 통해 빌드 실행
        buildingPrefab.GetStateMachine().Build(buildingPrefab);

        // 초기 생성이라면인데, 병원, 오락실의 경우 X
        if (buildingPrefab.GetBuildingData().type != BuildingType.RecoveryRoom ||
            buildingPrefab.GetBuildingData().type != BuildingType.RecreationRoom ||
            !buildingPrefab.IsCreated)
        {
            // 자원 자동 생산 시작
            StartProducing(buildingPrefab);
        }
    }

    private void StartProducing(BaseBuilding buildingPrefab)
    {
        // 딕셔너리에서 이미 실행중인 코루틴을 검사하고 있다면 새로 시작하지 않음
        if (productionCoroutines.ContainsKey(buildingPrefab))
        {
            Debug.LogWarning($"{buildingPrefab.GetBuildingData().name}의 생산 코루틴이 이미 실행 중입니다.");
            return;
        }
        
        // 빌딩의 자원 생산 코루틴을 시작
        Coroutine coroutine = StartCoroutine(ProduceResources(buildingPrefab));
        productionCoroutines[buildingPrefab] = coroutine;
    }
    
    public void StopProducing(BaseBuilding building)
    {
        // 실행 중인 코루틴이 있다면 중지 및 딕셔너리에서 제거
        if (productionCoroutines.TryGetValue(building, out Coroutine coroutine))
        {
            StopCoroutine(coroutine);
            productionCoroutines.Remove(building);
            Debug.Log($"{building.GetBuildingData().name}의 생산 코루틴이 중지되었습니다.");
        }
    }

    // 개별 빌딩의 자원 생산을 관리하는 코루틴
    private IEnumerator ProduceResources(BaseBuilding building)
    {
        float productionInterval = 2f; // building.ProductionInterval; // 현재 빌딩의 생산 주기

        while (true)
        {
            yield return new WaitForSeconds(productionInterval); // 각 빌딩의 개별 생산 주기마다 생산

            // 자원 생산 및 UI 업데이트
            int productionAmount = building.GetCurrentProductOutput(); // 현재 빌딩의 생산량
            GameResourceManager.Instance.AddResource(building.GetBuildingData().resourceType, productionAmount);
        }
    }

    // 자원 생산량 텍스트 표시, 빌딩 이미지 변경
    public void UpdateEnergyProductUIAndImage(BuildingType type, string imagePath, int productionOutput)
    {
        buildingUIPresenter.UpdateEnergyProductUIAndImage(type, imagePath, productionOutput);
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
} // end class