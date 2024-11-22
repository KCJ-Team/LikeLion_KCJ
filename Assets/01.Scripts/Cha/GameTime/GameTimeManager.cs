using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameTimeManager : SceneSingleton<GameTimeManager>
{
    [Header("Game Time Settings")] 
    public GameTimeSetting gameTimeSetting;
    public bool isPaused = false; // 시간 정지 여부
    public bool enableXSpeed = false; // 배속 가능성

    public int currentDay; // 현재 일수
    private float dayTimer = 0f;

    private int hour;
    private int minute;

    [Header("UIs")] public Text textTimer;
    public Button btnStopNStart;
    public Button btnSpeed;
    public Image imageStopNStart;
    public Text textDday;
    public GameObject panelWarning;

    // icons..
    public Sprite iconPause;
    public Sprite iconPlay;
    
    private bool hasProducedAt6AM = false;
    private bool hasProducedAt18PM = false;
    
    // 플래그: 21시 자원 소비가 하루에 한 번만 실행되도록 제어
    private bool hasConsumedAt21 = false;

    // 12시의 자원 생산을 담당하는 플래그
    public bool hasProducedAt12PM = false; // 12시 자원 생산 플래그

    // 자원 상태가 경고인지 확인하는 변수
    private bool resourceWarningTriggered = false;
    
    private void Start()
    {
        if (gameTimeSetting == null)
        {
            Debug.LogError("GameTimeSettings ScriptableObject is not assigned!");
            return;
        }

        // 버튼 이벤트 등록
        btnStopNStart.onClick.AddListener(TogglePauseTime);
        btnSpeed.onClick.AddListener(ToggleDoubleTimeSpeed);

        StartCoroutine(DayCycle());
    }

    private void Update()
    {
        // 1번 키 입력: Pause/Resume
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (isPaused)
            {
                SetPauseTime(false);
            }
            else
            {
                SetPauseTime(true);
            }
        }

        // 2번 키 입력: 배속 토글
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ToggleDoubleTimeSpeed();
        }
    }

    // 3분동안 하루가 지나감
    private IEnumerator DayCycle()
    {
        while (currentDay >= 0)
        {
            // 하루 초기화
            dayTimer = 0f;
            hour = 0;
            minute = 0;

            UpdateTimeUI();
            UpdateDayUI();

            float encounterInterval = gameTimeSetting.dayDuration / 2f;
            float nextEncounterTime = encounterInterval;

            // 빌딩 애니메이션 3초 간격을 추적할 타이머
            float animationCheckTimer = 0f;

            while (dayTimer < gameTimeSetting.dayDuration)
            {
                if (!isPaused)
                {
                    dayTimer += Time.deltaTime * (enableXSpeed ? gameTimeSetting.xSpeed : 1f); // Time.deltaTime // deltaTime으로 하면 씹힘...

                    float totalMinutes = (dayTimer / gameTimeSetting.dayDuration) * 1440;
                    hour = (int)(totalMinutes / 60) % 24;
                    minute = (int)(totalMinutes % 60);

                    UpdateTimeUI();

                    // 3초 간격으로 BuildingManager의 애니메이션 검사 호출
                    animationCheckTimer += Time.deltaTime;
                    if (animationCheckTimer >= 3f)
                    {
                        BuildingManager.Instance.CheckBuildingAnimationLoop();
                        animationCheckTimer = 0f; // 타이머 초기화
                    }

                    // 6시, 18시에는 자원 생산, 
                    if (hour == 6 && !hasProducedAt6AM)
                    {
                        Debug.LogWarning("6시, 자원 생산 시작");
                        BuildingManager.Instance.ProduceResourcesAtScheduledTimes(hour);
                        hasProducedAt6AM = true;
                    }
                    
                    if (hour == 18 && !hasProducedAt18PM)
                    {
                        Debug.LogWarning("18시, 자원 생산 시작");
                        BuildingManager.Instance.ProduceResourcesAtScheduledTimes(hour);
                        hasProducedAt18PM = true;
                    }

                    if (hour == 12 && minute == 0)
                    {
                        // 12시에 만약 플래그가 true라면 자원을 생산하자
                        if (hasProducedAt12PM)
                        {
                            BuildingManager.Instance.ProduceResourcesAtScheduledTimes(hour);
                        }
                    }

                    // 21시에는 자원 소비 (한 번만 실행)
                    else if (hour == 21 && minute == 0 && !hasConsumedAt21)
                    {
                        ConsumeResources();
                        hasConsumedAt21 = true; // 플래그를 설정하여 중복 실행 방지
                    }

                    // 자정에 플래그 초기화 (다음 날을 위해)
                    if (hour == 0 && minute == 0)
                    {
                        hasConsumedAt21 = false; // 자정에 플래그 초기화
                        
                        if (currentDay != 0)
                            CheckResourceWarning();
                        //hasCheckedAtMidnight = true; // 자정 검사를 완료 표시
                    }

                    // 자정이 지난 후 플래그 초기화
                    if (hour == 1 && minute == 0)
                    {
                        //hasCheckedAtMidnight = false;
                    }

                    // 업그레이드 가능 여부 검사
                    BuildingManager.Instance.CheckUpgradeAvailability();

                    if (dayTimer >= nextEncounterTime)
                    {
                        EncounterManager.Instance.ActivateNextEncounter();
                        nextEncounterTime += encounterInterval;
                    }
                }

                yield return null;
            }

            currentDay--;

            UpdateDayUI();

            if (currentDay == 0)
            {
                // Day 0에서 리소스를 검사하고 엔딩 결정
                CheckResourceWarning();

                if (!resourceWarningTriggered) // 자원이 충분하다면 팩션 엔딩으로
                {
                    GameEndFaction();
                }

                yield break;
            }
        }

        Debug.Log("D-Day reached! Game cycle complete!");
    }

    // 시:분 형태로 시간 UI 업데이트
    private void UpdateTimeUI()
    {
        textTimer.text = $"{hour:D2}:{minute:D2}";
    }

    // D-day 텍스트 변경
    public void UpdateDayUI()
    {
        // textDday.text = $"D-{currentDay}";

        // 텍스트 업데이트 로직
        string newText = $"D-{currentDay}";

        // DOTween Sequence로 텍스트 효과 설정
        Sequence textTween = DOTween.Sequence();

        // 텍스트 크기 확대/축소 효과
        textTween.Append(textDday.transform.DOScale(1.5f, 0.2f).SetEase(Ease.OutQuad))
            .Append(textDday.transform.DOScale(1f, 0.2f).SetEase(Ease.OutQuad));

        // 텍스트 색상 변화 효과 (선택 사항: 흰색에서 빨간색으로 잠시 변경)
        textTween.Join(textDday.DOColor(Color.cyan, 0.2f))
            .Append(textDday.DOColor(Color.white, 0.2f));

        // 텍스트 변경
        textDday.text = newText;

        // 텍스트 트위닝 완료
        textTween.Play();
    }

    // 시간 정지 토글 버튼
    public void TogglePauseTime()
    {
        isPaused = !isPaused;

        // isPaused 상태에 따라 아이콘 업데이트
        imageStopNStart.sprite = isPaused ? iconPlay : iconPause;

        SoundManager.Instance.PlayUISound(UISoundType.Click);
    }

    // 외부에서 직접 게임 시간을 멈추거나 시작하도록 설정하는 메서드
    public void SetPauseTime(bool pause)
    {
        isPaused = pause;

        // isPaused 상태에 따라 아이콘 업데이트
        imageStopNStart.sprite = isPaused ? iconPlay : iconPause;

        // 빌딩 생산 중단 또는 재개
        // BuildingManager.Instance.UpdateAllProductions(isPaused);
    }

    // 2배속 토글 버튼
    public void ToggleDoubleTimeSpeed()
    {
        enableXSpeed = !enableXSpeed;

        SoundManager.Instance.PlayUISound(UISoundType.Click);
    }

    public bool IsScheduledProductionTime()
    {
        return (hour == 6 && minute == 0) || (hour == 18 && minute == 0);
    }

    // 하루 9시에 자원을 고정된 감소량으로 감소
    private void ConsumeResources()
    {
        // 기본 자원 감소량 설정 -10씩
        int baseConsumptionAmount = 10; // Day 14 ~ Day 10까지
        int amount = baseConsumptionAmount;

        // 현재 일수에 따른 감소량 설정
        if (currentDay <= 10 && currentDay > 5) // Day 10 ~ Day 5까지 -20
        {
            amount = baseConsumptionAmount * 2;
        }
        else if (currentDay <= 5 && currentDay > 1) // Day 5 ~ Day 2 -40
        {
            amount = baseConsumptionAmount * 4;
        }
        else if (currentDay == 1) // Day 1 에는 -60
        {
            amount = baseConsumptionAmount * 6; // Day 1에는 -60
        }

        // 자원 소비 적용
        // GameResourceManager.Instance.ConsumeResources(amount, amount, amount);
        GameResourceManager.Instance.ConsumeResourceWithCheck(ResourceType.Energy, amount);
        GameResourceManager.Instance.ConsumeResourceWithCheck(ResourceType.Food, amount);
        GameResourceManager.Instance.ConsumeResourceWithCheck(ResourceType.Fuel, amount);

        // 24.11.20 빼기 아이콘
        GameResourceManager.Instance.gameResourceUIPresneter.ShowIconConsumeAt9PM();

        // 디버깅 정보 출력
        Debug.Log("Resources consumed at 9AM.");
        Debug.Log($"Resources consumed: {amount} for each resource type on Day {currentDay}.");
    }

    // 0시마다 실행하는 자원 검사 함수. 폭동 엔딩 가능
    private void CheckResourceWarning()
    {
        bool hasZeroResource = GameResourceManager.Instance.FindZeroResource();
        bool hpWarning = LobbyMenuManager.Instance.hp < 10.0f; // HP 경고 조건, 10이하
        bool stressWarning = LobbyMenuManager.Instance.stress > 90.0f; // Stress 경고 조건, 90이상

        Debug.Log(
            $"체크 리소스 워닝 : [현재 리소스] hasZeroResource: {hasZeroResource}, hpWarning: {hpWarning}, stressWarning: {stressWarning}, Combined Condition: {hasZeroResource || hpWarning || stressWarning}");

        if (hasZeroResource || hpWarning || stressWarning)
        {
            // 처음 자원이 0이 되었을 때 경고 활성화 (panelWarning을 표시하고 다음날에만 폭동 엔딩 발동 조건 설정)
            if (!resourceWarningTriggered && !panelWarning.activeSelf)
            {
                Debug.Log("Warning: 첫번째로 0. 워닝 패널 활성화");

                resourceWarningTriggered = true; // 처음 경고 상태로 설정
                panelWarning.SetActive(true); // 경고 패널 표시

                SoundManager.Instance.PlayUISound(UISoundType.Alert);
            }

            // 경고가 활성화된 상태에서 여전히 조건 충족 시 폭동 엔딩 발동
            else if (resourceWarningTriggered && panelWarning.activeSelf)
            {
                Debug.Log("2번째로 0. 워닝 패널 활성화 된 상태에서 폭동 엔딩 발생");

                // 생존일 수 계산
                int daySurvived = gameTimeSetting.startDay - currentDay;

                // 엔딩 분기
                if (hpWarning)
                {
                    Debug.Log("HP Warning active. Triggering HP Ending.");
                    EndingManager.Instance.ShowEnding(EndingType.ILLNESS, daySurvived); // HP 엔딩
                }
                else if (stressWarning)
                {
                    Debug.Log("Stress Warning active. Triggering Stress Ending.");
                    EndingManager.Instance.ShowEnding(EndingType.STRESS, daySurvived); // Stress 엔딩
                }
                else if (hasZeroResource)
                {
                    Debug.Log("Resource Warning active. Triggering Riot Ending.");
                    EndingManager.Instance.ShowEnding(EndingType.RIOT, daySurvived); // Riot 엔딩
                }

                EndingManager.Instance.ShowEnding(EndingType.RIOT, daySurvived); // Riot 엔딩


                // 엔딩 이후 경고 초기화
                resourceWarningTriggered = false;
                panelWarning.SetActive(false);
            }
        }
        else
        {
            // 디버깅 메시지로 초기화 상태 확인
            Debug.Log($"리소스가 해제 되어야함!!!!");

            // 자원이 충분해지면 경고 초기화
            resourceWarningTriggered = false;
            panelWarning.SetActive(false);
        }
    }

    // Day 0이 될때 팩션 엔딩
    private void GameEndFaction()
    {
        Debug.Log("D-Day reached! Game cycle complete!");

        // 가장 지지도가 높은 팩션 가져오기
        Faction leadingFaction = FactionManager.Instance.GetLeadingFaction();
        EndingType endingType;

        if (leadingFaction != null)
        {
            switch (leadingFaction.type)
            {
                case FactionType.Red:
                    endingType = EndingType.RED;
                    break;
                case FactionType.Black:
                    endingType = EndingType.BLACK;
                    break;
                case FactionType.Green:
                    endingType = EndingType.GREEN;
                    break;
                case FactionType.Yellow:
                    endingType = EndingType.YELLOW;
                    break;
                default:
                    endingType = EndingType.NONE;
                    break;
            }

            // 생존일 수 계산
            int daySurvived = gameTimeSetting.startDay - currentDay;

            // 엔딩 타입에 따라 EndingManager를 통해 엔딩을 출력
            EndingManager.Instance.ShowEnding(endingType, daySurvived, leadingFaction.icon);
        }
        else
        {
            Debug.LogWarning("No leading faction found. Default ending may apply.");
        }
    }
} // end class