using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameTimeManager : SceneSingleton<GameTimeManager>
{
    [FormerlySerializedAs("gameTimeSettings")] [Header("Game Time Settings")]
    public GameTimeSetting gameTimeSetting; 
    public bool isPaused = false; // 시간 정지 여부
    public bool enableXSpeed = false; // 배속 가능성

    private int currentDay; // 현재 일수
    private float dayTimer = 0f;

    private int hour;
    private int minute;

    [Header("UIs")] 
    public Text textTimer;
    public Button btnStopNStart;
    public Button btnSpeed;
    public Image imageStopNStart;
    public Text textDday;
    public GameObject panelWarning;
    
    // icons..
    public Sprite iconPause;
    public Sprite iconPlay;
    
    // 플래그: 21시 자원 소비가 하루에 한 번만 실행되도록 제어
    private bool hasConsumedAt21 = false;
    
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

        currentDay = gameTimeSetting.startDay; // 시작일 설정
        StartCoroutine(DayCycle());
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
            textDday.text = $"D-{currentDay}";

            float encounterInterval = gameTimeSetting.dayDuration / 2f;
            float nextEncounterTime = encounterInterval;

            while (dayTimer < gameTimeSetting.dayDuration)
            {
                if (!isPaused)
                {
                    dayTimer += Time.deltaTime * (enableXSpeed ? gameTimeSetting.xSpeed : 1f);

                    float totalMinutes = (dayTimer / gameTimeSetting.dayDuration) * 1440;
                    hour = (int)(totalMinutes / 60) % 24;
                    minute = (int)(totalMinutes % 60);

                    UpdateTimeUI();

                    // 6시, 18시에는 자원 생산, 
                    if (hour == 6 || hour == 18)
                    {
                        BuildingManager.Instance.ProduceResourcesAtScheduledTimes(hour);
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
                        CheckResourceWarning();
                    }

                    if (dayTimer >= nextEncounterTime)
                    {
                        EncounterManager.Instance.ActivateNextEncounter();
                        nextEncounterTime += encounterInterval;
                    }
                }

                yield return null;
            }

            currentDay--;

            textDday.text = $"D-{currentDay}";

            if (currentDay < 0)
            {
                GameEndFaction();
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
    
    // 시간 정지 토글 버튼
    public void TogglePauseTime()
    {
        isPaused = !isPaused;
        
        // isPaused 상태에 따라 아이콘 업데이트
        imageStopNStart.sprite = isPaused ? iconPlay : iconPause;
        
        // 모든 빌딩의 생산을 멈추거나 다시 시작
        // BuildingManager.Instance.UpdateAllProductions(isPaused);
    }

    // 2배속 토글 버튼
    public void ToggleDoubleTimeSpeed()
    {
        enableXSpeed = !enableXSpeed;

        //BuildingManager.Instance.UpdateProductionSpeed(enableXSpeed);
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
        else if (currentDay <= 5 && currentDay > 1)  // Day 5 ~ Day 2 -40
        {
            amount = baseConsumptionAmount * 4; 
        }
        else if (currentDay == 1) // Day 1 에는 -60
        {
            amount = baseConsumptionAmount * 6;  // Day 1에는 -60
        }

        // 자원 소비 적용
        GameResourceManager.Instance.ConsumeResources(amount, amount, amount);

        // 디버깅 정보 출력
        Debug.Log("Resources consumed at 9AM.");
        Debug.Log($"Resources consumed: {amount} for each resource type on Day {currentDay}.");
    }
    
    // 0시마다 실행하는 자원 검사 함수
    private void CheckResourceWarning()
    {
        bool hasZeroResource = GameResourceManager.Instance.FindZeroResource();

        if (hasZeroResource)
        {
            if (resourceWarningTriggered)
            {
                Debug.Log("Resource is still zero. Triggering bad ending.");
                
                // TODO : 확인하기
                EndingManager.Instance.ShowEnding(EndingType.RIOT); // 예시로 폭동 엔딩을 트리거
            }
            else
            {
                Debug.Log("Warning: Resource at zero. Displaying warning message.");
                resourceWarningTriggered = true;
                
                // 경고 UI를 표시하거나 로직 추가
                panelWarning.SetActive(true);
                
            }
        }
        else
        {
            resourceWarningTriggered = false; // 자원이 충분하면 경고 상태 초기화
            panelWarning.SetActive(false);
        }
    }
    
    // Day 0이 될때 팩션 엔딩
    // TODO : 확인하기
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
            
            // 엔딩 타입에 따라 EndingManager를 통해 엔딩을 출력
            EndingManager.Instance.ShowEnding(endingType);
        }
        else
        {
            Debug.LogWarning("No leading faction found. Default ending may apply.");
        }
    }
} // end class
