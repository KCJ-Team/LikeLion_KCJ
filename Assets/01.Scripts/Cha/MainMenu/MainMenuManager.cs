using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MainMenu에서 사용하는 매니저
/// </summary>
public class MainMenuMaanger : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject mainTitlePanel;       // 메인 패널
    [SerializeField] private GameObject settingsPanel;       // 설정 패널
    [SerializeField] private GameObject aboutUsPanel;        // 정보 패널
    [SerializeField] private GameObject characterCreationPanel; // 캐릭터 생성 패널
    
    private List<GameObject> panels;
    
    [Header("Buttons")]
    public Button btnNewGame;
    public Button btnContinue;
    public Button btnSettings;
    public Button btnAboutUs;
    public Button btnQuitGame;
    
    public Button btnNewGameStart;

    public Button btnGoBack;
    
    [Header("아바타 토글 그룹")]
    public Toggle toggleMaleChar;
    public Toggle toggleFemaleChar;
    public ToggleGroup toggleGroup; // 두 옵션을 포함하는 ToggleGroup
    
    private void Start()
    {
        // 리스트에 추가하여 관리
        panels = new List<GameObject> { mainTitlePanel, settingsPanel, aboutUsPanel, characterCreationPanel };
        
        // 버튼 클릭 이벤트를 각 메서드에 연결
        btnNewGame.onClick.AddListener(OpenCharacterCreation);
        btnContinue.onClick.AddListener(ContinueGame);
        btnSettings.onClick.AddListener(OpenSettings);
        btnAboutUs.onClick.AddListener(OpenAboutUs);
        btnQuitGame.onClick.AddListener(QuitGame);
        
        btnNewGameStart.onClick.AddListener(CheckSelectedChracter);
        
        btnGoBack.onClick.AddListener(() => SetActivePanel(mainTitlePanel)); // GoBack 버튼 클릭 시 메인 패널로 이동
        
        // 초기 상태로 메인 패널 활성화
        SetActivePanel(mainTitlePanel);
    }

    /// <summary>
    /// 지정된 패널이 활성회 되면 나머진 비활성화
    /// </summary>
    /// <param name="activePanel"></param>
    private void SetActivePanel(GameObject activePanel)
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(panel == activePanel);
        }
        
        // mainTitlePanel이 아닐 때 GoBack 버튼 활성화
        btnGoBack.gameObject.SetActive(activePanel != mainTitlePanel);
    }
    
    private void OpenCharacterCreation()
    {
        SetActivePanel(characterCreationPanel);
    }
    
    private void CheckSelectedChracter()
    {
        // 캐릭터가 선택되었는지 확인
        if (CheckSelect())
        {
            StartNewGame();
        }
        else
        {
            Debug.LogWarning("Please select a character before proceeding.");
        }
    }
    
    private bool CheckSelect()
    {
        // ToggleGroup에서 현재 선택된 Toggle이 있는지 확인
        bool isSelected = toggleGroup.AnyTogglesOn();
        
        // 활성화된 Toggle이 있으면 true, 없으면 false 반환
        return isSelected;
    }
    
    private void StartNewGame()
    {
        // TODO : 게임 생성, 로딩씬으로 넘어가기
        
        
    }

    // TODO : DB에서 정보 가져와서 Continue 하기, 로딩씬으로 넘어가기
    private void ContinueGame()
    {
        // // 게임 상태를 불러와서, 예를 들어 최근 저장된 씬으로 이동
        // if (GameDataManager.Instance.HasSavedGame())
        // {
        //     string lastScene = GameDataManager.Instance.GetLastSavedScene();
        //     SceneManager.LoadScene("LoadingScene"); // 로딩 씬을 통해 불러오기
        // }
        // else
        // {
        //     Debug.LogWarning("No saved game found.");
        // }
    }

    private void OpenSettings()
    {
        SetActivePanel(settingsPanel);
    }


    private void OpenAboutUs()
    {
        SetActivePanel(aboutUsPanel);
    }

    private void QuitGame()
    {
        // 게임 종료
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 종료 테스트 시
#endif
    }
    
} // end class
