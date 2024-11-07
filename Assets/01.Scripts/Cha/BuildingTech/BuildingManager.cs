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
    private BuildingStateMachine stateMachine; // 상태 전환을 관리할 StateMachine

    [Header("UI MVP 패턴")] 
    [SerializeField]
    private BuildingUIView buildingUIView;
    private BuildingUIPresenter buildingUIPresenter;

    private void Start()
    {
        stateMachine = new BuildingStateMachine();
        
        buildingUIPresenter = new BuildingUIPresenter(buildingUIView);
    }

    public void Build(BaseBuilding buildingPrefab, bool isCreated)
    {
        // 초기 생성이라면
        if (!isCreated)
        {
            // 자원 자동 생산 시작
            StartProducing(buildingPrefab);
        }

        // TODO : 무결성 검사하는 로직 필요, 클래스 만들어서 관리하기
        stateMachine.Build(buildingPrefab);
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
       // float productionInterval = building.ProductionInterval; // 현재 빌딩의 생산 주기

        while (true)
        {
           // yield return new WaitForSeconds(productionInterval); // 각 빌딩의 개별 생산 주기마다 생산

            // 자원 생산 및 UI 업데이트
            GameResourceManager.Instance.AddResource(ResourceType.Energy, productionAmount);
            //  ShowProductionText(building, productionAmount); // 자원 생산량 UI 표시
        }
    }
    
    // 에너지 생산량 UI
    public void UpdateEnergyProductUI(int productOutput)
    {
       // energyProductionOutput.text = $"Energy Product: {productOutput}";
    }
} // end class