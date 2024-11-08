using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI요소와 플레이어 정보 업데이트 스크립트
public class BuildingUIPresenter
{
    private BuildingUIView uiView;

    // 크래프팅 버튼 
    // private Dictionary<Button, bool> buttonUpgradeState = new Dictionary<Button, bool>();

    public BuildingUIPresenter(BuildingUIView uiView)
    {
        this.uiView = uiView;

        // uiView의 UI오브젝트들 초기화
        InitPopupButtonListeners();
    }

    // uiView의 UI오브젝트들 초기화
    private void InitPopupButtonListeners()
    {
        // 건설 버튼 및 업그레이드 버튼 이벤트 추가
        foreach (var entry in uiView.buildings)
        {
            var buildingType = entry.Key;
            Button[] buttons = entry.Value;

            if (buttons.Length >= 2)
            {
                // 첫 번째 버튼을 건설 버튼으로 사용
                buttons[0].onClick.AddListener(() => OnBuildingPopupButtonClicked(buttons[0]));

                // 두 번째 버튼을 빌딩 목록 버튼으로 사용
                buttons[1].onClick.AddListener(() => OnBuildingListButtonClicked(buildingType));
            }
        }
        
        // 팝업창의 업그레이드 버튼 등록
        uiView.buildUpgradeButton.onClick.AddListener(BuildOrUpgradeBuilding);
    }

    // 빌딩 크래프팅 버튼 클릭 시 호출
    private void OnBuildingPopupButtonClicked(Button buildingButton)
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

    // 빌딩 목록 버튼 클릭 시 호출
    private void OnBuildingListButtonClicked(BuildingType buildingType)
    {
        Debug.Log($"Building list button for {buildingType} clicked.");
        // TODO : 빌딩 목록 관련 추가 기능 수행
    }


    // 팝업창 열기 및 콘텐츠 설정
    private void OpenBuildingPopup(BaseBuilding buildingBase)
    {
        PopupUI popup = PopupUIManager.Instance.GetPopup(PopupType.BuildingUpgrade);

        // BuildingPopupData 생성 후 팝업에 전달
        BuildingPopupData popupData = new BuildingPopupData(buildingBase);
        popup.SetData(popupData); // 데이터를 PopupUI에 설정

        // 기본 UI 요소 설정
        popup.SetHeader(popupData.Title);
        popup.SetContent("Description", popupData.Description);
        popup.SetContent("CurrentLevel", popupData.CurrentLevelText);
        popup.SetContent("ProductOutput", popupData.ProductOutputText);
        popup.SetContent("CostEnergy", $"{popupData.CostEnergy}");
        popup.SetContent("CostFood", $"{popupData.CostFood}");
        popup.SetContent("CostWorkforce", $"{popupData.CostWorkforce}");
        popup.SetContent("CostFuel", $"{popupData.CostFuel}");
        popup.SetContent("ProductOutputIcon", "", popupData.ProductOutputIcon);

        // // 빌드/업그레이드 버튼
        string text = buildingBase.GetBuilding().CurrentLevel == 0 ? "Build" : "Upgrade";
        popup.SetContent("Build", text);

        // 팝업 열기
        PopupUIManager.Instance.OpenPopup(popup);
    }

    // 팝업의 업그레이드 버튼 클릭시 빌드..
    private void BuildOrUpgradeBuilding()
    {
        // 팝업 가져오기
        PopupUI popup = PopupUIManager.Instance.GetPopup(PopupType.BuildingUpgrade);

        // 팝업 데이터가 BuildingPopupData로 캐스팅 가능한지 확인
        BuildingPopupData buildingPopupData = popup.GetData() as BuildingPopupData;
        if (buildingPopupData == null)
        {
            Debug.LogError("Failed to retrieve BuildingPopupData from the popup.");
            return;
        }

        // BuildingManager를 통해 빌딩 생성 또는 업그레이드
        BaseBuilding building = buildingPopupData.BuildingBase;
        BuildingManager.Instance.Build(building, building.GetBuilding().CurrentLevel > 0);
        
        // 팝업 자동 닫기
        PopupUIManager.Instance.ClosePopup(popup);
    }

    // TODO 
    public void UpdateEnergyProductUIAndImage(BuildingType type, string imagePath, int productionOutput)
    {
        // 스프라이트를 받고, 딕셔너리의 빌딩에서 타입을 찾아 확인후 path 변경
        Sprite newSprite = Resources.Load<Sprite>(imagePath);
        
        if (uiView.buildings.ContainsKey(type))
        {
            Button buildingButton = uiView.buildings[type][1];
            Image buildingImage = buildingButton.image;

            if (newSprite != null)
                buildingImage.sprite = newSprite;
        
            // 알파값을 1로 설정하여 완전히 불투명하게 만들기
            Color color = buildingImage.color;
            color.a = 1f;
            buildingImage.color = color;
            
            buildingButton.enabled = true;
        }
        else
        {
            Debug.LogWarning($"Sprite not found at path: {imagePath} or BuildingType {type} not found in UI view.");
        }
        
        // 빌딩의 버튼 상태 설정
        Button craftingButton = uiView.buildings[type][0];
        
        if (craftingButton.gameObject.activeSelf)
        {
            craftingButton.gameObject.SetActive(false);
        }
        
        // 자원 생산 Text ui에 주기
        
    }
} // end class