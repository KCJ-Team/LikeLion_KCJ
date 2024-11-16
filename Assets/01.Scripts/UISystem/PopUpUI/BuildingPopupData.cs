using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPopupData : IPopupData
{
    public BaseBuilding BuildingBase { get; set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string ProductOutputText { get; private set; }
    public string CurrentLevelText { get; private set; }
    public int CostEnergy { get; private set; }
    public int CostFood { get; private set; }
    public int CostWorkforce { get; private set; }
    public int CostFuel { get; private set; }
    public Sprite ProductOutputIcon { get; private set; }

    public BuildingPopupData(BaseBuilding buildingBase)
    {
        BuildingBase = buildingBase;
        
        Title = buildingBase.GetBuildingData().name;
        Description = buildingBase.GetBuildingData().description;
        CurrentLevelText = $"Lv.{buildingBase.GetBuilding().CurrentLevel}";
        
        float multiplier = buildingBase.CurrentMultiplier; // 현재 레벨에 따른 배율

        // ProductOutput 텍스트 설정 로직
        if (buildingBase.GetBuildingData().type == BuildingType.Quarters)
        {
            // '숙소' 건물 유형일 때, output은 인원수용량
            ProductOutputText = $"Max.{buildingBase.GetCurrentProductOutput()}";
        }
        else if (buildingBase.GetBuildingData().productionOutput != 0)
        {
            ProductOutputText = $"+{buildingBase.GetCurrentProductOutput()}";
        }
        else
        {
            ProductOutputText = "";
        }

        // 비용 데이터 설정
        CostEnergy = (int)(buildingBase.GetBuildingData().baseCostEnergy * multiplier);
        CostFood = (int)(buildingBase.GetBuildingData().baseCostFood * multiplier);
        CostWorkforce = (int)(buildingBase.GetBuildingData().baseCostWorkforce * multiplier);
        CostFuel = (int)(buildingBase.GetBuildingData().baseCostFuel * multiplier);

        // 아이콘 설정
        // 빌딩 프리팹의 ResourceData에 있는걸 가져오자
        ProductOutputIcon = GameResourceManager.Instance.GetResourceData(buildingBase.GetBuildingData().resourceType)?.icon;
    }
}