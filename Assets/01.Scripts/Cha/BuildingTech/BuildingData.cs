using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{
    PowerPlant, // 발전실
    BioFarm, // 바이오팜(농장)
    Quarters, // 숙소
    FuelPlant, // 연료실
    ResearchLab, // 연구실
    RecoveryRoom, // 회복실
    RecreationRoom, // 오락실
    SpecialFacility
}

[CreateAssetMenu(fileName = "BuildingData", menuName = "Building/Building Data")]
public class BuildingData : ScriptableObject
{
    public int buildingId; 
    public string name;
    public BuildingType type;
    public int baseCostEnergy; // 에너지 기본 비용
    public int baseCostFood;
    public int baseCostFuel;
    public int baseCostWorkforce;
    public float upgradeMultiplier; // 업그레이드 시 적용할 배율
    public int maxLevel; // 업그레이드 가능 최대 레벨
    public int productionOutput; // 생산 자원량 (기본 레벨 기준, 업그레이드 시 배율 적용)
    public string description; // 빌딩 설명
    public string level1ImagePath; 
    public string level2ImagePath;
    public ResourceType resourceType; // 생산자원타입
}
