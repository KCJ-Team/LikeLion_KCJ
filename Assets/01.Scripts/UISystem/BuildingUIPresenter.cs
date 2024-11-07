using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI요소와 플레이어 정보 업데이트 스크립트
public class BuildingUIPresenter
{
    private PlayerInfo.PlayerInfo playerInfo;
    private BuildingUIView uiView;

    public BuildingUIPresenter(BuildingUIView uiView)
    {
        this.uiView = uiView;
        SetupBuildingButtonListeners();
    }
    
    // 각 빌딩 버튼에 이벤트 리스너 추가
    private void SetupBuildingButtonListeners()
    {
        foreach (var buildingButton in uiView.buildingCraftButtons)
        {
            buildingButton.onClick.AddListener(() => OnBuildingButtonClicked(buildingButton));
        }
    }
    
    // 빌딩 크래프팅 버튼 클릭 시 호출
    private void OnBuildingButtonClicked(Button buildingButton)
    {
        // 버튼의 상위 오브젝트에서 BaseBuilding 컴포넌트 가져오기
        BaseBuilding buildingBase = buildingButton.GetComponentInParent<BaseBuilding>();

        if (buildingBase != null)
        {
            OpenBuildingPopup(buildingBase);
        }
        else
        {
            Debug.LogWarning("BaseBuilding component not found on the parent of the button.");
        }
    }

    // 팝업창 열기 및 콘텐츠 설정
    private void OpenBuildingPopup(BaseBuilding buildingBase)
    {
        PopupUI popup = PopupUIManager.Instance.GetPopup(PopupType.BuildingUpgrade);
        
        // 헤더의 텍스트 접근해 바꾸기
        popup.SetHeader($"{buildingBase.GetBuildingData().name}");
        
        // Contents 안의 요소에 접근해서 바꾸기
        popup.SetContent("Description", buildingBase.GetBuildingData().description);
        popup.SetContent("CurrentLevel", $"Lv.{buildingBase.GetBuilding().CurrentLevel}");
       
        // 숙소 ProductOutput의 경우 자원 증가가 아니고, 자원 제한이다.
        if (buildingBase.GetBuildingData().type == BuildingType.Quarters) // '숙소' 건물 유형일 때
        {
            popup.SetContent("ProductOutput", $"Max.{buildingBase.GetBuildingData().productionOutput}");
        }
        else // 그 외 건물
        {
            if (buildingBase.GetBuildingData().productionOutput != 0)
                popup.SetContent("ProductOutput", $"+{buildingBase.GetBuildingData().productionOutput}");
            else
                popup.SetContent("ProductOutput", "");
        }
        
        popup.SetContent("CostEnergy", $"{buildingBase.GetBuildingData().baseCostEnergy}");
        popup.SetContent("CostFood", $"{buildingBase.GetBuildingData().baseCostFood}");
        popup.SetContent("CostWorkforce", $"{buildingBase.GetBuildingData().baseCostWorkforce}");
        popup.SetContent("CostFuel", $"{buildingBase.GetBuildingData().baseCostFuel}");
        
        // Icon 가져오기
        var resourceData = GameResourceManager.Instance.GetResourceData(buildingBase.GetBuildingData().resourceType);
        if (resourceData != null)
        {
            Sprite icon = resourceData.icon;

            // 디버깅 로그
            Debug.Log($"Setting ProductOutputIcon for {buildingBase.GetBuildingData().name} with resource type {buildingBase.GetBuildingData().resourceType}");
            Debug.Log($"Icon Name: {icon?.name ?? "No Icon Found"}");

            // Product Output 생산하는 자원 스프라이트 아이콘
            popup.SetContent("ProductOutputIcon", "", icon);
        }
        else // ResourceData가 없는 건물이면 null
        {
            popup.SetContent("ProductOutputIcon", "", null);
        }
        
        // 팝업 열기
        PopupUIManager.Instance.OpenPopup(popup);
    }
} // end class