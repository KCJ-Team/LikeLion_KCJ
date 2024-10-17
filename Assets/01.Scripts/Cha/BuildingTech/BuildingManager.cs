using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class BuildingManager : SceneSingleton<BuildingManager>
{
    private List<BaseBuilding> builtBuildings = new List<BaseBuilding>();
    
    public void BuildBuilding(BaseBuilding buildingPrefab) {
        BuildingData data = buildingPrefab.GetBuildingData();  // ScriptableObject 데이터 가져오기

       
    }
    
    // 빌딩 업그레이드
    public void UpgradeBuilding(BaseBuilding building) {
        building.Upgrade();
    }

    // 현재 건설된 모든 빌딩 리스트 반환
    public List<BaseBuilding> GetBuiltBuildings() {
        return builtBuildings;
    }
} // end class
