using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;

public class GameResourceManager : DD_Singleton<GameResourceManager>
{
    public SerializedDictionary<ResourceType, GameResource> resources = new();
    
    // 각 자원의 UI 텍스트
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI fuelText;
    public TextMeshProUGUI workforceText;
    public TextMeshProUGUI currencyText;
    
    private void Start()
    {
        UpdateResourceUI();
    }

    // UI 업데이트
    private void UpdateResourceUI()
    {
        energyText.text = $"Energy: {GetResourceAmount(ResourceType.Energy)}";
        foodText.text = $"Food: {GetResourceAmount(ResourceType.Food)}";
        fuelText.text = $"Fuel: {GetResourceAmount(ResourceType.Fuel)}";
        workforceText.text = $"Workforce: {GetResourceAmount(ResourceType.Workforce)}";
        currencyText.text = $"Currency: {GetResourceAmount(ResourceType.Currency)}";
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
            UpdateResourceUI();
        }
    }

    // 단일 자원 소비하는 메서드
    public bool ConsumeResource(ResourceType type, int amount)
    {
        if (resources.TryGetValue(type, out GameResource resource) && resource.CurrentAmount >= amount)
        {
            resource.CurrentAmount -= amount;
            Debug.Log($"Consumed {amount} {resource.ResourceData.resourceName}. Remaining amount: {resource.CurrentAmount}");
            UpdateResourceUI();
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
    
} // end class