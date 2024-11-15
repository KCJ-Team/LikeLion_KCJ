using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UI요소와 플레이어 정보 업데이트 스크립트
public class BuildingUIPresenter
{
    private BuildingUIView uiView;

    public BuildingUIPresenter(BuildingUIView uiView)
    {
        this.uiView = uiView;

        // uiView의 UI오브젝트들 초기화
        InitPopupButtonListeners();
    }

    private void InitPopupButtonListeners()
    {
        foreach (var entry in uiView.buildingUIs)
        {
            var buildingType = entry.Key;
            var uiElements = entry.Value;

            // CraftingButton 클릭 이벤트 등록
            var craftingGO = uiElements[BuildingUIType.CraftingButton];
            var craftingButton = craftingGO.GetComponent<Button>();
            craftingButton?.onClick.AddListener(() => OnBuildingPopupButtonClicked(craftingButton));
            
            // BuildingButton 클릭 이벤트 등록
            var buildingGO = uiElements[BuildingUIType.BuildingButton];
            var buildingButton = buildingGO.GetComponent<Button>();
            buildingButton?.onClick.AddListener(() => OnBuildingListButtonClicked(buildingButton));
            
            // 초기 비활성화
            uiElements[BuildingUIType.ProcessImage].SetActive(false);
            uiElements[BuildingUIType.EnableUpgradeImage].SetActive(false);
            
            // UseButton은 선택적으로 처리
            if (uiElements.TryGetValue(BuildingUIType.UseButton, out GameObject useGO) && useGO != null)
            {
                var useButton = useGO.GetComponent<Button>();
                useButton?.onClick.AddListener(() => OnBuildingStartingProcess(useButton));

                // 초기 비활성화
                useGO.SetActive(false);
            }
        }

        // 팝업창의 업그레이드 버튼 등록
        uiView.buildUpgradeButton.onClick.AddListener(BuildOrUpgradeBuilding);
    }

    // 빌딩 크래프팅 버튼 클릭 시 호출
    private void OnBuildingPopupButtonClicked(Button craftingButton)
    {
        // 버튼의 상위 오브젝트에서 BaseBuilding 컴포넌트 가져오기
        BaseBuilding buildingBase = craftingButton.GetComponentInParent<BaseBuilding>();

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
    private void OnBuildingListButtonClicked(Button buildingButton)
    {
        BaseBuilding buildingBase = buildingButton.GetComponent<BaseBuilding>();

        if (buildingBase != null)
        {
            OpenBuildingPopup(buildingBase);
        }
        else
        {
            Debug.LogWarning("BaseBuilding component not found on the parent of the button.");
        }
    }

    // 병원/오락시설이 Use가 되면 처리.. 
    private void OnBuildingStartingProcess(Button useButton)
    {
        // 버튼의 상위 오브젝트에서 BaseBuilding 컴포넌트 가져오기
        BaseBuilding buildingBase = useButton.GetComponentInParent<BaseBuilding>();

        if (buildingBase != null)
        {
            // 병원이라면, 
            if (buildingBase.GetBuildingData().type == BuildingType.RecoveryRoom)
            {
                // 병실 입장해 Hp 회복
                if (!BuildingManager.Instance.isRecoveryRoomUsed)
                {
                    BuildingManager.Instance.isRecoveryRoomUsed = true;

                    useButton.transform.GetChild(0).GetComponent<Image>().sprite = uiView.iconUnused;
                    
                    Debug.Log("병원 플레이어 사용. HP 회복 시작");
                }
                else
                {
                    BuildingManager.Instance.isRecoveryRoomUsed = false;
                   
                    useButton.transform.GetChild(0).GetComponent<Image>().sprite = uiView.iconUse;

                    Debug.Log("병원 플레이어 미사용. HP 회복 스탑");
                }
            } 
            // 오락시설이라면, 플레이어의 Stress 자원을 감소시키는 로직
            else if (buildingBase.GetBuildingData().type == BuildingType.RecreationRoom)
            {
                // 오락시설 입장해 Hp 회복
                if (!BuildingManager.Instance.isRecreationRoomUsed)
                {
                    BuildingManager.Instance.isRecreationRoomUsed = true;
                  
                    useButton.transform.GetChild(0).GetComponent<Image>().sprite = uiView.iconUnused;
                    
                    Debug.Log("오락시설 플레이어 사용. 스트레스 감소 시작");
                }
                else
                {
                    BuildingManager.Instance.isRecreationRoomUsed = false;
                    
                    useButton.transform.GetChild(0).GetComponent<Image>().sprite = uiView.iconUse;
                    
                    Debug.Log("오락시설 플레이어 미사용. 스트레스 감소 스탑");
                }
            }
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

        // BuildingPopupData 생성 후 팝업에 전달
        BuildingPopupData popupData = new BuildingPopupData(buildingBase);
        popup.SetData(popupData); // 데이터를 PopupUI에 설정

        // 기본 UI 요소 설정
        popup.SetHeader(popupData.Title);
        popup.SetContent("Description", popupData.Description);
        popup.SetContent("CurrentLevel", popupData.CurrentLevelText);
        popup.SetContent("ProductOutput", popupData.ProductOutputText);
        popup.SetContent("CostEnergy", $"-{popupData.CostEnergy}");
        popup.SetContent("CostFood", $"-{popupData.CostFood}");
        popup.SetContent("CostWorkforce", $"-{popupData.CostWorkforce}");
        popup.SetContent("CostFuel", $"-{popupData.CostFuel}");
        popup.SetContent("ProductOutputIcon", "", popupData.ProductOutputIcon);

        // 빌드/업그레이드 버튼 텍스트 설정
        string text;
        if (!buildingBase.IsCreated)
        {
            text = "Build";
        }
        else if (buildingBase.GetBuilding().CurrentLevel < buildingBase.GetBuildingData().maxLevel)
        {
            // 이미 생성된 상태이며 최대 레벨이 아니라면 "Upgrade"
            text = "Upgrade";
        }
        else
        {
            // 최대 레벨이라면 버튼을 숨기기
            uiView.buildUpgradeButton.gameObject.SetActive(false);
            text = null; // 버튼 텍스트는 필요 없음
        }

        popup.SetContent("Build", text);

        // 현재 자원이 업그레이드가 가능한지를 검사후 가능하면 버튼을 보여주고, 아니면 X
        bool canUpgrade = BuildingManager.Instance.CanUpgradeBuilding(buildingBase);
        uiView.buildUpgradeButton.gameObject.SetActive(canUpgrade);

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

        BuildingManager.Instance.BuildOrUpgrade(building);

        // isCreated가 false였다면, 이제 생성되었으므로 true로 설정
        if (!building.IsCreated)
        {
            building.IsCreated = true;
        }

        // 팝업 자동 닫기
        PopupUIManager.Instance.ClosePopup(popup);
    }

    // TODO 
    public void UpdateProductUIAndImage(BuildingType type, string imagePath, int productionOutput)
    {
        Sprite newSprite = !string.IsNullOrEmpty(imagePath) ? Resources.Load<Sprite>(imagePath) : null;
        
        if (uiView.buildingUIs.ContainsKey(type))
        {
            Button buildingButton = uiView.buildingUIs[type][BuildingUIType.BuildingButton].GetComponent<Button>();
            Image buildingImage = buildingButton.image;
        
            // 이미지 경로가 유효하면 스프라이트 변경, 그렇지 않으면 알파값만 조정
            if (newSprite != null)
            {
                buildingImage.sprite = newSprite;
            }
        
            // 알파값을 1로 설정하여 완전히 불투명하게 만들기
            Color color = buildingImage.color;
            color.a = 1f;
            buildingImage.color = color;
        
            // 버튼 활성화
            buildingButton.enabled = true;
            
            // Clinic 또는 RecreationRoom 타입이라면 UseButton 활성화
            if ((type == BuildingType.RecoveryRoom || type == BuildingType.RecreationRoom) && buildingButton.enabled)
            {
                if (uiView.buildingUIs[type].TryGetValue(BuildingUIType.UseButton, out GameObject useButtonGO))
                {
                    useButtonGO.SetActive(true);
                }
            }
        }
        else
        {
            Debug.LogWarning($"Sprite not found at path: {imagePath} or BuildingType {type} not found in UI view.");
        }
        
        // 빌딩의 크래프팅 버튼 상태 설정
        Button craftingButton = uiView.buildingUIs[type][BuildingUIType.CraftingButton].GetComponent<Button>();
        
        if (craftingButton.gameObject.activeSelf)
        {
            craftingButton.gameObject.SetActive(false);
        }
    }
} // end class