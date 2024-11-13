using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using PlayerInfo;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MainMenu에서 사용하는 매니저
/// </summary>
public class MainMenuMaanger : SceneSingleton<MainMenuMaanger>
{
    [Header("MainMenu 씬에서 사용할 스크립터블 오브젝트 데이터들")]
    public SerializedDictionary<ResourceType, ResourceData> resourceDatas;
    public EncounterData encounterData;
    public GameTimeSetting gameTimeData;
    
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
        // 플레이어 타입 설정: Toggle 선택 여부에 따라 설정
        PlayerModelType selectedType = toggleMaleChar.isOn ? PlayerModelType.Male : PlayerModelType.Female;
        
        // PlayerService를 통해 DB에 접근해 플레이어 타입 저장
        PlayerService playerService = new PlayerService();
        
        if (playerService.CreatePlayer(selectedType))
        {
            // TODO : 시작 자원 정보들,
            // 랜덤 인카운터 정보, 인벤토리 정보를 맨 처음 게임시작시에
            // DB에 저장해서 초기화시킨다. 이때 id만 넘겨줘서 DB에는 id만 저장
            // 유니티에서 로딩할떄 id를 다시 가져와서 SO에서 찾아서 데이터 적재

            EncounterService encouterService = new EncounterService();

            if (encouterService.CreateEncounters())
            {
                // TODO : 인벤토리 데이터에 권총 기본 무기 저장하기..
                
                // Lobby 씬으로 넘거가기
                GameSceneDataManager.Instance.LoadScene("Lobby");
            }
        }
    }

    // TODO : DB에서 정보 가져와서 Continue 하기, 로딩씬으로 넘어가기
    private void ContinueGame()
    {
        // ContinueGame에서는 DB에 저장되어있던 정보들을 불러와야함. 
        
        
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
