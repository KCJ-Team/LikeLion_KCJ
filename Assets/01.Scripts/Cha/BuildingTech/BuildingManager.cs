using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

public class BuildingManager : SceneSingleton<BuildingManager>
{
    [SerializeField] 
    private SerializedDictionary<BuildingType, BaseBuilding> buildings = new();
    private BuildingStateMachine stateMachine; // 상태 전환을 관리할 StateMachine

    // Test : 생산량 UI 테스트
    public TextMeshProUGUI energyProductionOutput;

    private void Start()
    {
        stateMachine = new BuildingStateMachine();
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
    
    // 자원 생산을 관리하는 코루틴
    private IEnumerator ProduceResources(BaseBuilding building)
    {
        while (true) // 무한 반복, 1초마다 자원 생산
        {
            int output = building.GetCurrentProductOutput(); // 현재 생산량
            
            GameResourceManager.Instance.AddResource(ResourceType.Energy, output); // 에너지를 생산
            
            Debug.Log($"{building.GetBuildingData().name} produced {output} Energy.");

            // 1초 대기
            yield return new WaitForSeconds(1f);
        }
    }
    
    // 에너지 생산량 UI
    public void UpdateEnergyProductUI(int productOutput)
    {
        energyProductionOutput.text = $"Energy Product: {productOutput}";
    }
} // end class