using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Energy, // 에너지
    Food, // 식량
    Fuel, // 연료
    Workforce, // 일꾼
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

