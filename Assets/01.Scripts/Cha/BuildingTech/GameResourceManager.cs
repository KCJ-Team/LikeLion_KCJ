using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResourceManager : DD_Singleton<GameResourceManager>
{
    [SerializeField]
    private List<ResourceData> resources;
    
    // 특정 리소스 타입의 현재 양을 가져오는 메서드
    public int GetResourceAmount(ResourceType type) {
        ResourceData resource = FindResourceByType(type);
        if (resource != null) {
            return resource.currentAmount;
        }
        Debug.LogWarning("Resource type not found: " + type);
        return 0;
    }

    // 특정 리소스 타입에 양을 추가하는 메서드
    public void AddResource(ResourceType type, int amount) {
        ResourceData resource = FindResourceByType(type);
        if (resource != null) {
            resource.currentAmount += amount;
            if (resource.currentAmount > resource.maxAmount) {
                resource.currentAmount = resource.maxAmount; // 최대값 초과 방지
            }
            Debug.Log($"Added {amount} {resource.resourceName}. Current amount: {resource.currentAmount}");
        }
    }

    // 특정 리소스 타입에 양을 소비하는 메서드
    public bool ConsumeResource(ResourceType type, int amount) {
        ResourceData resource = FindResourceByType(type);
        if (resource != null && resource.currentAmount >= amount) {
            resource.currentAmount -= amount;
            Debug.Log($"Consumed {amount} {resource.resourceName}. Remaining amount: {resource.currentAmount}");
            return true;
        }
        Debug.LogWarning("Not enough resources or resource type not found: " + type);
        return false;
    }

    // 특정 리소스 타입을 찾아주는 메서드
    private ResourceData FindResourceByType(ResourceType type) {
        foreach (ResourceData resource in resources) {
            if (resource.resourceType == type) {
                return resource;
            }
        }
        return null;
    }

    // 모든 자원의 상태를 출력
    public void PrintAllResources() {
        foreach (ResourceData resource in resources) {
            Debug.Log($"{resource.resourceName}: {resource.currentAmount}/{resource.maxAmount}");
        }
    }
}
