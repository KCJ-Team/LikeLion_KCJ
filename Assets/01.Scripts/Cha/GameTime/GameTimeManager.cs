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
    
    // icons..
    public Sprite iconPause;
    public Sprite iconPlay;
    
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
        while (currentDay >= 0) // D-Day 도달할 때까지 반복
        {
            // 하루 시작 시 0시로 초기화
            dayTimer = 0f;
            hour = 0; 
            minute = 0;
            
            UpdateTimeUI(); // 초기 시간 UI 업데이트
            
            textDday.text = $"D-{currentDay}";
            
            // 3분 동안 두 번의 인카운터를 호출할 간격 계산
            float encounterInterval = gameTimeSetting.dayDuration / 2f;
            float nextEncounterTime = encounterInterval;

            // 타이머가 3분 지나가는중(100f)
            while (dayTimer < gameTimeSetting.dayDuration) 
            {
                if (!isPaused)
                {
                    dayTimer += Time.deltaTime * (enableXSpeed ? gameTimeSetting.xSpeed : 1f); // 2배속 적용
                    
                    // 게임 내 시간 계산
                    float totalMinutes = (dayTimer / gameTimeSetting.dayDuration) * 1440; // 하루 24시간 = 1440분
                    hour = (int)(totalMinutes / 60) % 24; // 시 계산
                    minute = (int)(totalMinutes % 60); // 분 계산

                    UpdateTimeUI(); // UI 업데이트 
                    
                    // 설정된 간격에 따라 랜덤 인카운터 호출
                    if (dayTimer >= nextEncounterTime)
                    {
                        EncounterManager.Instance.ActivateNextEncounter();
                        nextEncounterTime += encounterInterval; // 다음 호출 시간 설정
                    }
                }
                
                yield return null;
            }

            currentDay--; // 남은 일수 감소
            
            textDday.text = $"D-{currentDay}";
            
            if (currentDay < 0) // D-Day가 0일 때 종료 로직 호출
            {
                EndGame();
                yield break; // End game; stop the coroutine
            }
        }

        Debug.Log("D-Day reached! Game cycle complete!"); // 0일에 도달하면 게임 종료 또는 리셋 로직 실행
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
    
    // TODO : D-day가 0이되어 게임종료시
    private void EndGame()
    {
        Debug.Log("D-Day reached! Game cycle complete!");

    }
} // end class
