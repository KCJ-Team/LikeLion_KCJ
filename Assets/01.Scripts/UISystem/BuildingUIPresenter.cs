using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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

        // 연구실의 Learn 버튼들 클릭 이벤트 등록
        for (int i = 0; i < uiView.btnLearns.Length; i++)
        {
            int index = i; // 클로저 문제 방지
            Button button = uiView.btnLearns[index];
            button.onClick.AddListener(() => OnLearnButtonClicked(index));
        }
    }

    // Process Image 처리
    public void ShowProcessIcon(BuildingType buildingType)
    {
        if (uiView.buildingUIs.TryGetValue(buildingType, out var uiDictionary) &&
            uiDictionary.TryGetValue(BuildingUIType.ProcessImage, out var processImage))
        {
            processImage.SetActive(true);

            // 초기 투명도와 위치 설정
            var imageComponent = processImage.GetComponent<CanvasGroup>();
            if (imageComponent == null)
            {
                imageComponent = processImage.AddComponent<CanvasGroup>();
            }

            imageComponent.alpha = 1f;

            var originalPosition = processImage.transform.localPosition;
            var originalScale = processImage.transform.localScale;

            // hyuna sound
            SoundManager.Instance.PlaySFX(SFXSoundType.PositivePop);

            // 등장 애니메이션: 크기 팽창
            Sequence sequence = DOTween.Sequence();
            sequence.Append(processImage.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f, 10, 1f)) // 크기 팽창
                .AppendInterval(1.5f) // 1.5초 유지
                .Append(processImage.transform.DOLocalMoveY(originalPosition.y + 30f, 0.5f)) // 위로 이동
                .Join(imageComponent.DOFade(0f, 0.5f)) // 서서히 투명해짐
                .OnComplete(() =>
                {
                    processImage.SetActive(false); // 완전히 사라진 뒤 비활성화
                    processImage.transform.localPosition = originalPosition; // 위치 초기화
                    processImage.transform.localScale = originalScale; // 크기 초기화
                    imageComponent.alpha = 1f; // 투명도 초기화
                });

            Debug.Log($"ProcessImage for {buildingType} is now animated.");
        }
        else
        {
            Debug.LogWarning($"No ProcessImage found for BuildingType: {buildingType}");
        }
    }

    // 빌딩 업그레이드 가능시 업그레이드 아이콘 연출
    public void ShowEnableUpgradeIcon(BuildingType buildingType)
    {
        if (uiView.buildingUIs.TryGetValue(buildingType, out var uiDictionary) &&
            uiDictionary.TryGetValue(BuildingUIType.EnableUpgradeImage, out var upgradeIcon))
        {
            // 이미 활성화되어 있다면 다시 실행하지 않음
            if (upgradeIcon.activeSelf) return;

            upgradeIcon.SetActive(true);

            var canvasGroup = upgradeIcon.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = upgradeIcon.AddComponent<CanvasGroup>();
            }

            // 초기 투명도 설정
            canvasGroup.alpha = 1f;

            // 깜빡이는 애니메이션
            canvasGroup.DOFade(0.5f, 0.2f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }
        else
        {
            Debug.LogWarning($"No EnableUpgradeImage found for BuildingType: {buildingType}");
        }
    }

    // 빌딩 업그레이드 했을때 아이콘 사라지게 
    public void HideUpgradeIcon(BuildingType buildingType)
    {
        if (uiView.buildingUIs.TryGetValue(buildingType, out var uiDictionary) &&
            uiDictionary.TryGetValue(BuildingUIType.EnableUpgradeImage, out var upgradeIcon))
        {
            var canvasGroup = upgradeIcon.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = upgradeIcon.AddComponent<CanvasGroup>();
            }

            // 기존 애니메이션 정지 및 부드러운 페이드 아웃
            canvasGroup.DOKill();
            canvasGroup.DOFade(0f, 0.5f).OnComplete(() =>
            {
                upgradeIcon.SetActive(false);
                canvasGroup.alpha = 1f; // 다음 사용을 위해 초기화
            });

            // Debug.Log($"Upgrade icon for {buildingType} is now hidden.");
        }
    }

    /// 연구 버튼 클릭 시 호출
    private void OnLearnButtonClicked(int techIndex)
    {
        if (techIndex < 0 || techIndex >= TechManager.Instance.techs.Count)
        {
            Debug.LogError($"Invalid tech index: {techIndex}");
            return;
        }

        Tech tech = TechManager.Instance.techs[techIndex];

        // TechManager에 로직 전달
        TechManager.Instance.LearnTech(tech);

        // UI 업데이트 => 체크로바꾸기? 
        UpdateLearnButtonUI(techIndex);
    }

    private void UpdateLearnButtonUI(int techIndex)
    {
        if (techIndex < 0 || techIndex >= TechManager.Instance.techs.Count)
        {
            Debug.LogError($"Invalid tech index: {techIndex}");
            return;
        }

        // Tech 상태를 가져옴
        Tech tech = TechManager.Instance.techs[techIndex];

        // UI 요소 업데이트
        if (tech.isLearned)
        {
            // 학습된 경우 버튼 비활성화 및 체크 표시 활성화
            uiView.objLearn[techIndex].SetActive(false);
            uiView.imageLearned[techIndex].gameObject.SetActive(true);
        }
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
            // 빌딩이 연구실인지 확인
            if (buildingBase.GetBuildingData().type == BuildingType.ResearchLab)
            {
                OpenResearchLabPopup(buildingBase); // 연구실 전용 팝업 열기
            }
            else
            {
                OpenBuildingPopup(buildingBase); // 일반 빌딩 팝업 열기
            }
        }
        else
        {
            Debug.LogWarning("BaseBuilding component not found on the parent of the button.");
        }
    }

    // 연구실 전용 팝업창 열기
    private void OpenResearchLabPopup(BaseBuilding buildingBase)
    {
        PopupUI popup = PopupUIManager.Instance.GetPopup(PopupType.Lab);

        if (popup == null)
        {
            Debug.LogError("ResearchLab Popup not found in PopupUIManager.");
            return;
        }

        // 현재 연구포인트를 가져옴.
        int availableResearchPoints = GameResourceManager.Instance.GetResourceAmount(ResourceType.Research);

        // 기술 리스트 순회
        for (int i = 0; i < TechManager.Instance.techs.Count; i++)
        {
            Tech tech = TechManager.Instance.techs[i];

            if (!tech.isLearned && availableResearchPoints >= tech.techCost)
            {
                // 연구 가능: objLearn 활성화, imageLearned 비활성화
                uiView.objLearn[i].SetActive(true);
                uiView.imageLearned[i].gameObject.SetActive(false);
            }
            else if (!tech.isLearned && availableResearchPoints < tech.techCost)
            {
                // 연구 포인트 부족: objLearn 비활성화
                uiView.objLearn[i].SetActive(false);
            }
            else if (tech.isLearned)
            {
                // 이미 연구된 경우: imageLearned 활성화
                uiView.imageLearned[i].gameObject.SetActive(true);
                uiView.objLearn[i].SetActive(false);
            }
        }

        // 팝업 열기
        PopupUIManager.Instance.OpenPopup(popup);
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

        building.IsCreated = true;
        BuildingManager.Instance.BuildOrUpgrade(building);

        // isCreated가 false였다면, 이제 생성되었으므로 true로 설정
        //if (!building.IsCreated)
        //{
        //}

        // 업그레이드 완료 후 업그레이드 아이콘 숨기기
        HideUpgradeIcon(building.GetBuildingData().type);

        // 팝업 자동 닫기
        PopupUIManager.Instance.ClosePopup(popup);
    }

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

    // Punch 애니메이션 반복
    public void StartBuildingAnimation(BuildingType buildingType)
    {
        if (uiView.buildingUIs.TryGetValue(buildingType, out var uiDictionary) &&
            uiDictionary.TryGetValue(BuildingUIType.BuildingButton, out var buildingButton))
        {
            var rectTransform = buildingButton.GetComponent<RectTransform>();
            if (rectTransform == null)
            {
                Debug.LogWarning($"No RectTransform found for {buildingType}'s BuildingButton.");
                return;
            }

            // 기존 애니메이션 정지
            rectTransform.DOKill();

            // 랜덤 지연 시간 설정 (1초에서 3초 사이)
            float randomDelay = Random.Range(0.5f, 2f);

            // 랜덤 지연 후 애니메이션 시작
            DOVirtual.DelayedCall(randomDelay, () =>
            {
                rectTransform.DOShakePosition(1f, new Vector3(3f, 0f, 0f), 10, 90, false, true)
                    .SetEase(Ease.InOutSine);

                // hyuna 11.20 sound 추가
                SoundManager.Instance.PlayUISound(UISoundType.Thick);
            });

            Debug.Log($"Punch animation started for {buildingType}.");
        }
        else
        {
            Debug.LogWarning($"Building button not found for {buildingType}.");
        }
    }
} // end class