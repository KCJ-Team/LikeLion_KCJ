using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuManager : SceneSingleton<PauseMenuManager>
{
    [Header("게임 일시정지 UI")] 
    public GameObject canvasPauseMenu;
    public Button btnResume;
    public Button btnSave;
    public Button btnSaveMainTitle;

    private void Awake()
    {
        // ESC 메뉴 UI 초기화 및 비활성화
        if (canvasPauseMenu != null)
        {
            canvasPauseMenu.SetActive(false);
            
            // 버튼에 클릭 메서드 바인딩
            btnResume.onClick.AddListener(ResumeGame);
            btnSave.onClick.AddListener(SaveGameData);
            btnSaveMainTitle.onClick.AddListener(SaveAndBackToMainTitle);
        }
    }
    
    private void Update()
    {
        // ESC 입력으로 일시정지 메뉴 토글
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 팝업이 열려 있으면 일시정지 메뉴를 띄우지 않음
            if (!PopupUIManager.Instance.IsAnyPopupOpen())
            {
                TogglePauseMenu();
            }
        }
    }
    
    private void TogglePauseMenu()
    {
        if (canvasPauseMenu == null) return;

        bool isPaused = canvasPauseMenu.activeSelf;
        canvasPauseMenu.SetActive(!isPaused);
        
        Time.timeScale = isPaused ? 1 : 0; // 활성화 시 게임 일시정지
    }

    // Resume 버튼 클릭 메서드
    private void ResumeGame()
    {
        canvasPauseMenu.SetActive(false);
        
        Time.timeScale = 1; // 게임 재개
    }
    
    // Save 버튼 클릭 메서드
    private void SaveGameData()
    {
        GameSceneDataManager.Instance.SaveLobbyDataInDB();
        Debug.Log("Game data saved.");
    }

    // Save and Main Title 버튼 클릭 메서드
    private void SaveAndBackToMainTitle()
    {
        GameSceneDataManager.Instance.SaveLobbyDataInDB();
        Debug.Log("Game data saved. And Back To MainMenu");
        
        Time.timeScale = 1; // 시간 스케일 복구
        GameSceneDataManager.Instance.LoadScene("MainMenu"); // 메인 타이틀 씬으로 이동
    }
}
