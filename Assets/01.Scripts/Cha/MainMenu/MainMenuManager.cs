using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using PlayerInfo;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// MainMenu에서 사용하는 매니저
/// </summary>
public class MainMenuManger : SceneSingleton<MainMenuManger>
{
    [Header("MainMenu 씬에서 사용할 스크립터블 오브젝트 데이터들")]
    public SerializedDictionary<ResourceType, ResourceData> resourceDatas;
    public EncounterData encounterData;
    public GameTimeSetting gameTimeData;
    public PlayerData playerData;
    public CardDatabase CardDatabase;
    
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
        
        // 플레이어 정보가 저장이 되어있는지를 검사하고 있으면 Continue 버튼 활성화 아님 비활성화
        VisibleContinueButton();
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
    
    private void VisibleContinueButton()
    {
        PlayerService playerService = new PlayerService();

        if (playerService.GetPlayer() == null)
        {
            btnContinue.gameObject.SetActive(false);
        }
        else
        {
            btnContinue.gameObject.SetActive(true);
        }
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
        // 캐릭터 선택에 따른 플레이어 타입 설정
        PlayerModelType selectedType = toggleMaleChar.isOn ? PlayerModelType.Male : PlayerModelType.Female;

        // DB에 데이터를 저장
        if (SaveInitGameData(selectedType))
        {
            // 씬을 로드
            GameSceneDataManager.Instance.LoadScene("Lobby");
        }
    }
    
    /// <summary>
    /// 새로운 게임 데이터 저장
    /// </summary>
    /// <param name="selectedType"></param>
    /// <returns></returns>
    public bool SaveInitGameData(PlayerModelType selectedType)
    {
        PlayerService playerService = new PlayerService();

        // 플레이어 테이블에 저장
        if (playerService.CreatePlayer(selectedType))
        {
            // 인카운터 테이블에 저장
            EncounterService encounterService = new EncounterService();

            if (encounterService.CreateEncounters())
            {
                // // 인벤토리에 기본 무기 저장
                // InventoryService inventoryService = new InventoryService();
                //
                // if (inventoryService.CreateInventory())
                // {
                     Debug.Log("New game data successfully saved.");
                    return true;
                // }
            }
        }

        Debug.LogError("Failed to save new game data.");
        return false;
    }
    
    private void ContinueGame()
    {
        // Continue를 누를때 DB에 플레이어 정보가 저장이 되어있는지를 검사해야함. 
        
        GameSceneDataManager.Instance.LoadScene("Lobby");
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
