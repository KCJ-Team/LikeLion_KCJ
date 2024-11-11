using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    None, // 없음
    Energy, // 에너지
    Food, // 식량
    Workforce, // 일꾼
    Fuel, // 연료
    Research, // 연구
    Currency // 재화
}

[CreateAssetMenu(fileName = "ResourceData", menuName = "Resources/Resource Data")]
public class ResourceData : ScriptableObject
{
    public int resourceId;
    public string resourceName;
    public ResourceType resourceType;
    public Sprite icon;
    public int currentAmount;
    public int maxAmount; 
}

