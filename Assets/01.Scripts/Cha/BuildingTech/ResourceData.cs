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
    Currency, // 재화
    
    // 24.11.15 관리하기 위해 추가하기
    HP,
    Stress,
    Attack,
    Defense
}

[CreateAssetMenu(fileName = "ResourceData", menuName = "Resources/Resource Data")]
public class ResourceData : ScriptableObject
{
    public int resourceId;
    public string resourceName;
    public ResourceType resourceType;
    public Sprite icon;
    public int initAmount; // 초기양
    public int currentAmount;
    public int maxAmount; 
}

