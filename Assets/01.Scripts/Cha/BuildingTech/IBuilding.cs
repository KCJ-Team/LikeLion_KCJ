using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilding
{
    void Build();
    void Upgrade();
    bool CanUpgrade();
    int GetProductOutput(); // 생산량
    BuildingData GetBuildingData(); // 빌딩데이터 반환
}
