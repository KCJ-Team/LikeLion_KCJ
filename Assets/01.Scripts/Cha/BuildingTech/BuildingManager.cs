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

    private void Start()
    {
        buildingUIPresenter = new BuildingUIPresenter(buildingUIView);
    }

    public void Build(BaseBuilding buildingPrefab, bool isCreated)
    {
        // 개별 빌딩의 상태 머신을 통해 빌드 실행
        buildingPrefab.GetStateMachine().Build(buildingPrefab);

        // 초기 생성이라면인데, 병원, 오락실의 경우 X
        if (buildingPrefab.GetBuildingData().type != BuildingType.RecoveryRoom &&
            buildingPrefab.GetBuildingData().type != BuildingType.RecreationRoom &&
            !isCreated)
        {
            // 자원 자동 생산 시작
            StartProducing(buildingPrefab);
        }
    }

    private void StartProducing(BaseBuilding buildingPrefab)
    {
        // 빌딩의 자원 생산 코루틴을 시작
        StartCoroutine(ProduceResources(buildingPrefab));
    }

    // 개별 빌딩의 자원 생산을 관리하는 코루틴
    private IEnumerator ProduceResources(BaseBuilding building)
    {
        int productionAmount = building.GetCurrentProductOutput(); // 현재 빌딩의 생산량
        float productionInterval = 2f; // building.ProductionInterval; // 현재 빌딩의 생산 주기

        while (true)
        {
            yield return new WaitForSeconds(productionInterval); // 각 빌딩의 개별 생산 주기마다 생산

            // 자원 생산 및 UI 업데이트
            GameResourceManager.Instance.AddResource(building.GetBuildingData().resourceType, productionAmount);
        }
    }

    // 자원 생산량 텍스트 표시, 빌딩 이미지 변경
    public void UpdateEnergyProductUIAndImage(BuildingType type, string imagePath, int productionOutput)
    {
        buildingUIPresenter.UpdateEnergyProductUIAndImage(type, imagePath, productionOutput);
    }
} // end class