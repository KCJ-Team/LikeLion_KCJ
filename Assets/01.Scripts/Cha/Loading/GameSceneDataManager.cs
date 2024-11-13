using System.Collections;
using System.Collections.Generic;
using PlayerInfo;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 게임에서 파괴되지 않고 게임의 데이터(저장정보), 씬을 관리하는 매니저
/// </summary>
public class GameSceneDataManager : MonoBehaviour
{
    // 전역 싱글톤 인스턴스
    private static GameSceneDataManager _instance;
    public static GameSceneDataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 인스턴스가 없으면 GameObject 생성 후 컴포넌트 추가
                GameObject singletonObj = new GameObject(nameof(GameSceneDataManager));
                _instance = singletonObj.AddComponent<GameSceneDataManager>();
                DontDestroyOnLoad(singletonObj);
            }
            return _instance;
        }
    }
    
    private string loadSceneName;

    public string LoadSceneName
    {
        get => loadSceneName;
        set => loadSceneName = value;
    }
    
    private void Awake()
    {
        // 싱글톤 인스턴스가 중복으로 생성되지 않도록 보장
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    // LoadScene
    public void LoadScene(string targetSceneName)
    {
        loadSceneName = targetSceneName;
        SceneManager.LoadScene("Loading");
    }
    
    /// <summary>
    /// Scene이 로드가 되고 나서 여러 매니저들에 각자 정보 던져주기
    /// </summary>
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 목적지 씬이 로드되었을 때 필요한 작업
        Debug.Log("Target scene loaded: " + loadSceneName);
        
        // TODO : scene의 이름을 보고 Lobby, Map_Elimination이면 해줘야함
        switch (scene.name)
        {
            case "Lobby" :
                LoadDataInDB();

                break;
            
            case "Map_Elimination" :

                break;
        }
        
    }

    private void LoadDataInDB()
    {
        //  DB에서 Player랑 Encounter 테이블에서 정보들을 가져온다.
        PlayerService playerService = new PlayerService();
        PlayerModel playerModel = playerService.GetPlayer();
        
        if (playerModel == null)
        {
            Debug.LogError("Failed to load player data from the database.");
            return;
        }

        ObjectManager.Instance.ActivatePlayerModel((PlayerModelType)playerModel.PlayerType);
        
        /// 자원 데이터를 GameResourceManager에 설정
        GameResourceManager.Instance.SetResourceAmount(ResourceType.Energy, playerModel.PlayerEnergy);
        GameResourceManager.Instance.SetResourceAmount(ResourceType.Food, playerModel.PlayerFood);
        GameResourceManager.Instance.SetResourceAmount(ResourceType.Fuel, playerModel.PlayerFuel);
        GameResourceManager.Instance.SetResourceAmount(ResourceType.Workforce, playerModel.PlayerWorkforce);
        GameResourceManager.Instance.SetResourceAmount(ResourceType.Research, playerModel.PlayerResearch);
        GameResourceManager.Instance.SetResourceAmount(ResourceType.Currency, playerModel.PlayerCurrency);
        
        // 인카운터들 => 인카운터 매니저의 unresolved에 연결하고 뿌려주기
        EncounterService encounterService = new EncounterService();
        List<Encounter> encounters = encounterService.GetEncounters();

        EncounterManager.Instance.UnresolvedEncounters = encounters;
        
        // Player D-day
        GameTimeManager.Instance.currentDay = playerModel.PlayerDDay;

        // TODO : 빌딩 업그레이드 정보!
    }

    public void SaveDataInDB()
    {
        
    }

} // end class
