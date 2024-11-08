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
        
        var data = buildingBase.GetBuildingData();
        
        Title = data.name;
        Description = data.description;
        CurrentLevelText = $"Lv.{buildingBase.GetBuilding().CurrentLevel}";

        // ProductOutput 텍스트 설정 로직
        if (data.type == BuildingType.Quarters)
        {
            // '숙소' 건물 유형일 때, output은 인원수용량
            ProductOutputText = $"Max.{data.productionOutput}";
        }
        else if (data.productionOutput != 0)
        {
            ProductOutputText = $"+{data.productionOutput}";
        }
        else
        {
            ProductOutputText = "";
        }

        // 비용 데이터 설정
        CostEnergy = data.baseCostEnergy;
        CostFood = data.baseCostFood;
        CostWorkforce = data.baseCostWorkforce;
        CostFuel = data.baseCostFuel;

        // 아이콘 설정
        ProductOutputIcon = GameResourceManager.Instance.GetResourceData(data.resourceType)?.icon;
    }
}