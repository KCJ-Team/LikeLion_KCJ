using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

//UI요소와 플레이어 정보 업데이트 스크립트
public class EncounterUIPresenter
{
    private EncounterUIView uiView;
    private int activeCount = 0; // 활성화된 인카운터 개수 추적
    
    private Button selectedEncounterButton; // 현재 선택된 Encounter 버튼

    public EncounterUIPresenter(EncounterUIView uiView)
    {
        this.uiView = uiView;
        
        // uiView의 UI오브젝트들 초기화
        InitEncounterObjects();
    }
    
    public void InitEncounterObjects()
    {
        // 초기화 시 모든 인카운터 오브젝트를 비활성화
        foreach (Button btnEncounter in uiView.encounterObjects)
        {
            btnEncounter.onClick.AddListener(() => OnEncounterButtonClicked(btnEncounter));
            btnEncounter.gameObject.SetActive(false);
        }
        
        // 버튼 클릭 이벤트 등록
        uiView.btnChoice1.onClick.AddListener(SelectChoice1);
        uiView.btnChoice2.onClick.AddListener(SelectChoice2);
    }
    
    // Encounter 버튼 클릭 시 호출되는 메서드
    private void OnEncounterButtonClicked(Button btnEncounter)
    {
        selectedEncounterButton = btnEncounter; // 현재 클릭된 버튼 저장
        EncounterManager.Instance.PrintRandomEncounter();
    }

    // 다음 인카운터 오브젝트를 활성화
    public void ActivateNextEncounter()
    {
        // 모든 오브젝트가 활성화되어 있으면 더 이상 활성화하지 않음
        if (activeCount >= uiView.encounterObjects.Count)
        {
            // SoundManager.Instance.PlayUISound(UISoundType.Noti);
            
            Debug.Log("All encounter objects are currently active.");
            return;
        }
        
        // 다음 오브젝트 가져오기
        var encounterObject = uiView.encounterObjects[activeCount].gameObject;

        // 초기 위치 설정 및 애니메이션 적용
        var rectTransform = encounterObject.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // 현재 위치에서 X축으로 +100만큼 이동
            float targetX = rectTransform.anchoredPosition.x + 200f;

            // 화면 안으로 슬라이드 애니메이션
            rectTransform.DOAnchorPos(new Vector2(targetX, rectTransform.anchoredPosition.y), 1f)
                .SetEase(Ease.OutQuad);
        }
        
        // 활성화
        encounterObject.SetActive(true);
        activeCount++;
    }
    
    // UI를 업데이트
    public void OpenEncounterPopup(Encounter encounter)
    {
        // 랜덤인카운터 팝업 열기
        PopupUI popup = PopupUIManager.Instance.GetPopup(PopupType.Encounter);

        EncounterPopupData popupData = new EncounterPopupData(encounter);
        popup.SetData(popupData);
        
        // 기본 UI요소 설정
        popup.SetHeader(popupData.Title);
        popup.SetContent("Description", popupData.Description);
        
        popup.SetContent("Choice1Text", popupData.Choice1Text);
       
        popup.SetContent("Choice2Text", popupData.Choice2Text);
        // popup.SetContent("Choice2Result", popupData.Choice2Result);
        // popup.SetContent("Choice2RewardEnergy", $"{popupData.Choice2RewardEnergy}");
        // popup.SetContent("Choice2RewardFood", $"{popupData.Choice2RewardFood}");
        // popup.SetContent("Choice2RewardFuel", $"{popupData.Choice2RewardFuel}");
        // popup.SetContent("Choice2RewardWorkforce", $"{popupData.Choice2RewardWorkforce}");

        // icon 설정.
        popup.SetContent("IconEncounter", "", popupData.EncounterChoiceIcon);
        
        // 팝업의 버튼과 자원 아이콘들 
        ToggleButtonsAndIcons(popup, true);
        popup.ToggleActiveState("ImageFactionBg", false);
        popup.ToggleActiveState("IconFaction", false);
        
        // 팝업 열기
        PopupUIManager.Instance.OpenPopup(popup);
    }
    
    // 1번 선택지
    private void SelectChoice1()
    {
        // 팝업 가져오기
        PopupUI popup = PopupUIManager.Instance.GetPopup(PopupType.Encounter);

        // 팝업 데이터가 EncounterPopupData 캐스팅 가능한지 확인
        EncounterPopupData popupData = popup.GetData() as EncounterPopupData;
        if (popupData == null)
        {
            Debug.LogError("Failed to retrieve BuildingPopupData from the popup.");
            return;
        }
        
        // 선택지 1에 따른 자원 로직 처리(빼기도 봐야함..)
        GameResourceManager.Instance.AddResource(ResourceType.Energy, popupData.Choice1RewardEnergy);
        GameResourceManager.Instance.AddResource(ResourceType.Food, popupData.Choice1RewardFood);
        GameResourceManager.Instance.AddResource(ResourceType.Fuel, popupData.Choice1RewardFuel);
        
        // 11.20 hyuna 노동력을 추가할땐, 현재 빌딩의 레벨을 보고 판단해야함. 만약 초과된다면 UI에 표시를 해주어야 한다. (빨갛게 잠깐)
        int currentWorkforce = GameResourceManager.Instance.GetResourceAmount(ResourceType.Workforce); // 현재 노동력량
        int totalWorkforce = currentWorkforce + popupData.Choice1RewardWorkforce; // 현재 노동력량 + 리워드 노동력량
        int currentProductionOutput = BuildingManager.Instance.GetBuilding(BuildingType.Quarters).GetCurrentProductOutput(); // 숙소의 현재 가능한 수용량

        if (totalWorkforce > currentProductionOutput) // 현재 일꾼수 + 보상 일꾼수가 숙소의 현재 수용량보다 높다면
        {
            // 초과된 Workforce 계산
            int excessWorkforce = totalWorkforce - currentProductionOutput;

            // 초과된 Workforce를 제한하여 추가
            GameResourceManager.Instance.AddResource(ResourceType.Workforce, popupData.Choice1RewardWorkforce - excessWorkforce);

            // UI에 초과된 상태 표시 (빨간색으로 잠시 강조)
            GameResourceManager.Instance.WarningWorkforceResourceUI();

            Debug.Log($"Workforce 추가 초과: {excessWorkforce}. 제한된 Workforce가 추가되었습니다.");
        }
        else
        {
            // Workforce 추가
            GameResourceManager.Instance.AddResource(ResourceType.Workforce, popupData.Choice1RewardWorkforce);
        }

        // 팩션 수치 변경
        if (popupData.Choice1Faction != FactionType.None)
        {
            FactionManager.Instance.ChangeFactionSupport(popupData.Choice1Faction, popupData.encounter.choice1FactionSupport);
        }
        
        // 결과를 팝업에 업데이트
        popup.SetContent("Description", $"{popupData.Choice1Result}");
        popup.SetContent("ChoiceRewardEnergy", $"{popupData.Choice1RewardEnergy}");
        popup.SetContent("ChoiceRewardFood", $"{popupData.Choice1RewardFood}");
        popup.SetContent("ChoiceRewardFuel", $"{popupData.Choice1RewardFuel}");
        popup.SetContent("ChoiceRewardWorkforce", $"{popupData.Choice1RewardWorkforce}");
        
        // 해당 팩션 아이콘 활성화, 인카운터 아이콘 변경
        popup.SetContent("IconEncounter", "", popupData.EncounterResultIcon);

        Sprite icon = FactionManager.Instance.GetFactionIcon(popupData.encounter.choice1Faction);
        if (icon != null)
        {
            popup.SetContent("IconFaction", "", icon);
            popup.ToggleActiveState("ImageFactionBg", true);
        }
       
        // 팝업의 버튼과 자원 아이콘들 
        ToggleButtonsAndIcons(popup, false);
        
        // 클릭된 Encounter 버튼을 비활성화 TODO : DOTWeen 적용
        if (selectedEncounterButton != null)
        {
            DeactivateSelectedEncounterButton();
        }
        
        // 팩션 지지도를 확인하고 UI에 변경해주기
        FactionManager.Instance.ChangeFactionSupport(popupData.Choice1Faction, popupData.Choice1FactionSupport);
        
        // 선택된 인카운터를 리스트에서 제거
        EncounterManager.Instance.UnresolvedEncounters.Remove(popupData.encounter);
    }
    
    // 2번 선택지
    private void SelectChoice2()
    {
        // 팝업 가져오기
        PopupUI popup = PopupUIManager.Instance.GetPopup(PopupType.Encounter);

        // 팝업 데이터가 EncounterPopupData로 캐스팅 가능한지 확인
        EncounterPopupData popupData = popup.GetData() as EncounterPopupData;
        if (popupData == null)
        {
            Debug.LogError("Failed to retrieve EncounterPopupData from the popup.");
            return;
        }
    
        // 선택지 2에 따른 자원 로직 처리
        GameResourceManager.Instance.AddResource(ResourceType.Energy, popupData.Choice2RewardEnergy);
        GameResourceManager.Instance.AddResource(ResourceType.Food, popupData.Choice2RewardFood);
        GameResourceManager.Instance.AddResource(ResourceType.Fuel, popupData.Choice2RewardFuel);
        GameResourceManager.Instance.AddResource(ResourceType.Workforce, popupData.Choice2RewardWorkforce);

        // 팩션 수치 변경
        if (popupData.Choice2Faction != FactionType.None)
        {
            FactionManager.Instance.ChangeFactionSupport(popupData.Choice2Faction, popupData.encounter.choice2FactionSupport);
        }
    
        // 결과를 팝업에 업데이트
        popup.SetContent("Description", $"{popupData.Choice2Result}");
        popup.SetContent("ChoiceRewardEnergy", $"{popupData.Choice2RewardEnergy}");
        popup.SetContent("ChoiceRewardFood", $"{popupData.Choice2RewardFood}");
        popup.SetContent("ChoiceRewardFuel", $"{popupData.Choice2RewardFuel}");
        
        // 11.20 hyuna 노동력을 추가할땐, 현재 빌딩의 레벨을 보고 판단해야함. 만약 초과된다면 UI에 표시를 해주어야 한다. (빨갛게 잠깐)
        int currentWorkforce = GameResourceManager.Instance.GetResourceAmount(ResourceType.Workforce); // 현재 노동력량
        int totalWorkforce = currentWorkforce + popupData.Choice2RewardWorkforce; // 현재 노동력량 + 리워드 노동력량
        int currentProductionOutput = BuildingManager.Instance.GetBuilding(BuildingType.Quarters).GetCurrentProductOutput(); // 숙소의 현재 가능한 수용량

        if (totalWorkforce > currentProductionOutput) // 현재 일꾼수 + 보상 일꾼수가 숙소의 현재 수용량보다 높다면
        {
            // 초과된 Workforce 계산
            int excessWorkforce = totalWorkforce - currentProductionOutput;

            // 초과된 Workforce를 제한하여 추가
            GameResourceManager.Instance.AddResource(ResourceType.Workforce, popupData.Choice2RewardWorkforce - excessWorkforce);

            // UI에 초과된 상태 표시 (빨간색으로 잠시 강조)
            GameResourceManager.Instance.WarningWorkforceResourceUI();

            Debug.Log($"Workforce 추가 초과: {excessWorkforce}. 제한된 Workforce가 추가되었습니다.");
        }
        else
        {
            // Workforce 추가
            GameResourceManager.Instance.AddResource(ResourceType.Workforce, popupData.Choice2RewardWorkforce);
        }
        
        // 해당 팩션 아이콘 활성화, 인카운터 아이콘 변경
        popup.SetContent("IconEncounter", "", popupData.EncounterResultIcon);

        Sprite icon = FactionManager.Instance.GetFactionIcon(popupData.encounter.choice2Faction);
        if (icon != null)
        {
            popup.SetContent("IconFaction", "", icon);
            popup.ToggleActiveState("ImageFactionBg", true);
        }
   
        // 팝업의 버튼과 자원 아이콘들 비활성화
        ToggleButtonsAndIcons(popup, false);
        
        // 클릭된 Encounter 버튼을 비활성화 TODO : DOTWeen 적용
        if (selectedEncounterButton != null)
        {
            DeactivateSelectedEncounterButton();
        }
        
        // 팩션 지지도를 확인하고 UI에 변경해주기
        FactionManager.Instance.ChangeFactionSupport(popupData.Choice2Faction, popupData.Choice2FactionSupport);
        
        // 선택된 인카운터를 리스트에서 제거
        EncounterManager.Instance.UnresolvedEncounters.Remove(popupData.encounter);
    }
    
    // 버튼과 아이콘 활성화/비활성화 메서드
    private void ToggleButtonsAndIcons(PopupUI popup, bool isActive)
    {
        // 버튼 활성화 설정
        uiView.btnChoice1.gameObject.SetActive(isActive);
        uiView.btnChoice2.gameObject.SetActive(isActive);

        // 아이콘 활성화 설정
        popup.ToggleActiveState("IconEnergy", !isActive);
        popup.ToggleActiveState("IconFood", !isActive);
        popup.ToggleActiveState("IconWorkforce", !isActive);
        popup.ToggleActiveState("IconFuel", !isActive);
        popup.ToggleActiveState("ChoiceRewardEnergy", !isActive);
        popup.ToggleActiveState("ChoiceRewardFood", !isActive);
        popup.ToggleActiveState("ChoiceRewardWorkforce", !isActive);
        popup.ToggleActiveState("ChoiceRewardFuel", !isActive);
    }

    // 특정 Encounter 오브젝트를 비활성화
    public void DeactivateSelectedEncounterButton()
    {
        // 버튼이 null이거나 이미 비활성화된 경우 처리하지 않음
        if (selectedEncounterButton == null || !selectedEncounterButton.gameObject.activeSelf)
        {
            Debug.LogWarning("The selected button is already inactive or null.");
            return;
        }
      
        // 비활성화 애니메이션
        var rectTransform = selectedEncounterButton.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // 현재 위치에서 X축으로 -200만큼 이동
            float targetX = rectTransform.anchoredPosition.x - 200f;

            // 화면 밖으로 슬라이드 애니메이션
            rectTransform.DOAnchorPos(new Vector2(targetX, rectTransform.anchoredPosition.y), 1f)
                .SetEase(Ease.InQuad) // 부드럽게 사라지는 애니메이션
                .OnComplete(() =>
                {
                    // 애니메이션 완료 후 비활성화
                    selectedEncounterButton.gameObject.SetActive(false);
                    selectedEncounterButton = null;
                });
        }
        else
        {
            // 애니메이션 적용이 불가능한 경우 즉시 비활성화
            selectedEncounterButton.gameObject.SetActive(false);
            selectedEncounterButton = null;
        }

        // 해당 버튼이 리스트에서 몇 번째인지 확인하고 activeCount 조정
        int index = uiView.encounterObjects.IndexOf(selectedEncounterButton);
        if (index >= 0 && index < activeCount)
        {
            activeCount--; // activeCount 감소
            
            // 비활성화된 버튼 이후의 버튼들을 한 칸씩 앞으로 당김
            for (int i = index; i < activeCount; i++)
            {
                uiView.encounterObjects[i] = uiView.encounterObjects[i + 1];
            }
            uiView.encounterObjects[activeCount] = selectedEncounterButton; // 비활성화된 버튼을 리스트 끝으로 이동
        }

        selectedEncounterButton = null;
    }
} // end class