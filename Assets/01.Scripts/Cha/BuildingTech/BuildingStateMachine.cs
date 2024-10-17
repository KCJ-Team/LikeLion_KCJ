using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using UnityEngine.UI;

public interface IBuildingState
{
    void Build(BuildingStateMachine stateMachine, BaseBuilding buildingPrefab);
}

public class Level0State : IBuildingState
{
    public void Build(BuildingStateMachine stateMachine, BaseBuilding buildingPrefab)
    {
        // Build시 자원 소비 로직
        BuildingData buildingData = buildingPrefab.GetBuildingData();

        if (GameResourceManager.Instance.ConsumeResources(
                buildingData.baseCostEnergy, buildingData.baseCostFood, buildingData.baseCostFuel,
                buildingData.baseCostWorkforce))
        {
            Debug.Log($"{buildingData.name} upgraded to Level 1.");

            // 이미지 활성화 (Level 0에서 Level 1로 전환) => 나중엔 불투명으로 ㄱ.. 
            buildingPrefab.gameObject.GetComponent<Image>().enabled = true;
            
            // 현재 레벨 증가
            buildingData.currentLevel++;

            stateMachine.ChangeState(new Level1State()); // 상태 전환
        }
        else
        {
            Debug.Log("Not enough resources to upgrade to Level 1.");
        }
    }
}

public class Level1State : IBuildingState
{
    public void Build(BuildingStateMachine stateMachine, BaseBuilding buildingPrefab)
    {
        BuildingData buildingData = buildingPrefab.GetBuildingData();

        // 4가지 자원을 한 번에 소비 (업그레이드 배율 적용)
        if (GameResourceManager.Instance.ConsumeResources(
                (int)(buildingData.baseCostEnergy * buildingData.upgradeMultiplier),
                (int)(buildingData.baseCostFood * buildingData.upgradeMultiplier),
                (int)(buildingData.baseCostFuel * buildingData.upgradeMultiplier),
                (int)(buildingData.baseCostWorkforce * buildingData.upgradeMultiplier)))
        {
            Debug.Log($"{buildingData.name} upgraded to Level 2.");

            // 이미지 전환
            string imagePath = buildingData.level1ImagePath;
            stateMachine.ChangeSpriteImage(buildingPrefab, imagePath);
            
            // 현재 레벨 증가
            buildingData.currentLevel++;

            stateMachine.ChangeState(new Level2State()); // 상태 전환
        }
        else
        {
            Debug.Log("Not enough resources to upgrade to Level 2.");
        }
    }
}

public class Level2State : IBuildingState
{
    public void Build(BuildingStateMachine stateMachine, BaseBuilding buildingPrefab)
    {
        BuildingData buildingData = buildingPrefab.GetBuildingData();
        
        // 현재 레벨이 maxLevel에 도달했는지 확인
        if (buildingData.currentLevel >= buildingData.maxLevel)
        {
            Debug.Log("Building is already at max level (Level 2).");
            return;  // 더 이상 업그레이드 불가
        }

        // 4가지 자원을 한 번에 소비 (업그레이드 배율 적용) => 의 2배.. 
        if (GameResourceManager.Instance.ConsumeResources(
                (int)(buildingData.baseCostEnergy * buildingData.upgradeMultiplier * 2),
                (int)(buildingData.baseCostFood * buildingData.upgradeMultiplier * 2),
                (int)(buildingData.baseCostFuel * buildingData.upgradeMultiplier * 2),
                (int)(buildingData.baseCostWorkforce * buildingData.upgradeMultiplier * 2)))
        {
            Debug.Log($"{buildingData.name} upgraded to Level 2.");

            // 이미지 전환
            string imagePath = buildingData.level2ImagePath;
            stateMachine.ChangeSpriteImage(buildingPrefab, imagePath);
        }
        else
        {
            Debug.Log("Not enough resources to upgrade to Level 2.");
        }
        
        Debug.Log("Building is already at max level (Level 2).");

    }
}

// 빌딩 UI 관리 FSM
public class BuildingStateMachine
{
    [SerializeField] private IBuildingState currentState;

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

    // 스프라이트 이미지 변경 메서드 (imagePath를 받아 처리)
    public void ChangeSpriteImage(BaseBuilding buildingPrefab, string imagePath)
    {
        Sprite newSprite = Resources.Load<Sprite>(imagePath);

        if (newSprite != null)
        {
            // buildingPrefab의 Image 컴포넌트의 스프라이트 변경
            Image prefabImage = buildingPrefab.gameObject.GetComponent<Image>();
            if (prefabImage != null)
            {
                prefabImage.sprite = newSprite; // 스프라이트 변경
                Debug.Log($"Image changed to: {imagePath}");
            }
            else
            {
                Debug.LogWarning("No Image component found on buildingPrefab.");
            }
        }
        else
        {
            Debug.LogWarning($"Image not found at: {imagePath}");
        }
    }
} // end class