using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

public class GameResourceManager : SceneSingleton<GameResourceManager>
{
    [Header("Resources")]
    public SerializedDictionary<ResourceType, GameResource> resources = new();

    [Header("UI MVP 패턴")] 
    [SerializeField]
    private GameResourceUIView gameResourceUIView;
    private GameResourceUIPresenter gameResourceUIPresneter;
    
    private void Start()
    {
        gameResourceUIPresneter = new GameResourceUIPresenter(gameResourceUIView);
        
        // 게임이 시작되면 리소스 업데이트 시작
        gameResourceUIPresneter.UpdateResourceUI();
    }
    
    // 특정 리소스 타입의 현재 수량을 가져옴
    public int GetResourceAmount(ResourceType type)
    {
        if (resources.TryGetValue(type, out GameResource resource))
        {
            return resource.CurrentAmount;
        }

        Debug.LogWarning("Resource type not found: " + type);
        return 0;
    }

    // 특정 리소스 타입에 양을 추가하는 메서드
    public void AddResource(ResourceType type, int amount)
    {
        if (resources.TryGetValue(type, out GameResource resource))
        {
            resource.CurrentAmount += amount;
            
            if (resource.CurrentAmount > resource.ResourceData.maxAmount)
            {
                resource.CurrentAmount = resource.ResourceData.maxAmount; // 최대값 초과 방지
            }

            Debug.Log($"Added {amount} {resource.ResourceData.resourceName}. Current amount: {resource.CurrentAmount}");
            
            gameResourceUIPresneter.UpdateResourceUI();
        }
    }

    // 단일 자원 소비하는 메서드
    public bool ConsumeResource(ResourceType type, int amount)
    {
        if (resources.TryGetValue(type, out GameResource resource) && resource.CurrentAmount >= amount)
        {
            resource.CurrentAmount -= amount;
            Debug.Log($"Consumed {amount} {resource.ResourceData.resourceName}. Remaining amount: {resource.CurrentAmount}");
            gameResourceUIPresneter.UpdateResourceUI();
            return true;
        }

        Debug.LogWarning("Not enough resources or resource type not found: " + type);
        return false;
    }

    // Energy, Food, Fuel, Workforce를 소비하는 메소드
    public bool ConsumeResources(int energyCost, int foodCost, int fuelCost, int workforceCost)
    {
        bool canConsume = 
            GetResourceAmount(ResourceType.Energy) >= energyCost &&
            GetResourceAmount(ResourceType.Food) >= foodCost &&
            GetResourceAmount(ResourceType.Fuel) >= fuelCost &&
            GetResourceAmount(ResourceType.Workforce) >= workforceCost;

        if (canConsume) {
            // 각 자원 소비
            ConsumeResource(ResourceType.Energy, energyCost);
            ConsumeResource(ResourceType.Food, foodCost);
            ConsumeResource(ResourceType.Fuel, fuelCost);
            ConsumeResource(ResourceType.Workforce, workforceCost);
            return true;
        }
        else
        {
            Debug.LogWarning("Not enough resources to upgrade.");
            return false;
        }
    }
    
    // 특정 자원의 현재 수량을 설정하는 메소드
    public void SetResourceAmount(ResourceType type, int amount)
    {
        if (resources.TryGetValue(type, out GameResource resource))
        {
            resource.CurrentAmount = Mathf.Clamp(amount, 0, resource.ResourceData.maxAmount); // 최대값 제한
            Debug.Log($"{type} resource amount set to {resource.CurrentAmount}");
        }
        else
        {
            Debug.LogWarning("Resource type not found: " + type);
        }
    }
    
    // 특정 자원 타입과 비용을 받아 소비하는 메소드
    public void ConsumeResourceWithCheck(ResourceType type, int cost)
    {
        int currentAmount = GetResourceAmount(type);

        if (currentAmount >= cost)
        {
            ConsumeResource(type, cost);
        }
        else if (currentAmount > 0)
        {
            // 자원이 cost보다 적지만 0이 아닐 때 남은 자원을 모두 소비
            ConsumeResource(type, currentAmount);
            Debug.LogWarning($"Not enough {type}. Consuming all available {type}.");
        }
        else
        {
            Debug.LogWarning($"No {type} left to consume.");
        }
    }
    
    // ResourceData 반환
    public ResourceData GetResourceData(ResourceType type)
    {
        if (resources.TryGetValue(type, out GameResource resource))
        {
            return resource.ResourceData;
        }

        Debug.LogWarning("Resource type not found: " + type);
        return null;
    }
    
    // Currency와 Research 자원을 제외하고 0인 자원이 있는지 검사하는 메서드
    public bool FindZeroResource()
    {
        foreach (var resource in resources)
        {
            if (resource.Key != ResourceType.Currency && 
                resource.Key != ResourceType.Research && 
                resource.Value.CurrentAmount == 0)
            {
                Debug.Log($"Resource {resource.Value.ResourceData.resourceName} has zero amount.");
                return true;
            }
        }
        return false;
    }
} // end class