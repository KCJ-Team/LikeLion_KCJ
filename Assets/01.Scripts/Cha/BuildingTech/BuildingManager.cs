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

    // 개별 코루틴 시작
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
    
    // 개별 코루틴 종료
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

    private IEnumerator ProduceResources(BaseBuilding building)
    {
        float baseProductionInterval = 2f; // 기본 생산 주기
        float elapsedTime = 0f;

        while (true)
        {
            // 배속을 적용한 생산 주기를 계산
            float productionInterval = baseProductionInterval / (GameTimeManager.Instance.enableXSpeed ? GameTimeManager.Instance.gameTimeSetting.xSpeed : 1f);
        
            // 일시정지 상태일 때는 생산을 중지하고 대기
            if (!GameTimeManager.Instance.isPaused)
            {
                elapsedTime += Time.deltaTime;
            
                // 설정된 생산 주기 경과 시 자원 생산
                if (elapsedTime >= productionInterval)
                {
                    elapsedTime = 0f;
                
                    // 자원 생산 및 UI 업데이트
                    int productionAmount = building.GetCurrentProductOutput();
                    GameResourceManager.Instance.AddResource(building.GetBuildingData().resourceType, productionAmount);
                }
            }
        
            yield return null; // 매 프레임마다 대기하면서 업데이트 체크
        }
    }
    // 모든 자원 생산 코루틴을 정지하거나 재개
    public void UpdateAllProductions(bool isPaused)
    {
        // 코루틴 정지라면
        if (isPaused)
        {
            foreach (var building in productionCoroutines.Keys)
            {
                StopProducing(building);
            }
            Debug.Log("All building productions paused.");
        }
        // 코루틴 재개라면
        else
        {
            foreach (var building in productionCoroutines.Keys)
            {
                StartProducing(building);
            }
            Debug.Log("All building productions resumed.");
        }
    }
    
    // 배속 변경을 적용하는 메서드
    public void UpdateProductionSpeed(bool enableXspeed)
    {
        // 모든 자원 생산 코루틴을 멈추고 재개하여 새로운 배속 반영
        foreach (var building in productionCoroutines.Keys)
        {
            StopProducing(building);
            StartProducing(building);
        }
        Debug.Log("All building production speeds updated to current game speed.");
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