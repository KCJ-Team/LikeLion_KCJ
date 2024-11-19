using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayerInfo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
                LoadLobbyDataInDB();

                break;
            
            case "Map_Elimination" :
                // TODO : DB에서 플레이어 정보..!
                
                break;
        }
        
    }

    // 데이터 로드
    private void LoadLobbyDataInDB()
    {
        //  DB에서 Player랑 Encounter 테이블에서 정보들을 가져온다.
        PlayerService playerService = new PlayerService();
        PlayerModel playerModel = playerService.GetPlayer();
        
        if (playerModel == null)
        {
            Debug.LogError("Failed to load player data from the database.");
            return;
        }

        // 플레이어 모델 타입 설정
        ObjectManager.Instance.ActivatePlayerModel((PlayerModelType)playerModel.PlayerType);
        
        // 자원 데이터를 GameResourceManager에 설정
        GameResourceManager.Instance.SetResourceAmount(ResourceType.Energy, playerModel.PlayerEnergy);
        GameResourceManager.Instance.SetResourceAmount(ResourceType.Food, playerModel.PlayerFood);
        GameResourceManager.Instance.SetResourceAmount(ResourceType.Fuel, playerModel.PlayerFuel);
        GameResourceManager.Instance.SetResourceAmount(ResourceType.Workforce, playerModel.PlayerWorkforce);
        GameResourceManager.Instance.SetResourceAmount(ResourceType.Research, playerModel.PlayerResearch);
        GameResourceManager.Instance.SetResourceAmount(ResourceType.Currency, playerModel.PlayerCurrency);
        
        // 인카운터들 => 인카운터 매니저의 unresolved에 연결하고 뿌려주기
        EncounterService encounterService = new EncounterService();
        EncounterManager.Instance.UnresolvedEncounters = encounterService.GetEncounters();
        
        Debug.Log("Encounters: " + string.Join(", ", EncounterManager.Instance.UnresolvedEncounters.Select(e => $"ID: {e.encounterId}")));
        
        // Player D-day
        GameTimeManager.Instance.currentDay = playerModel.PlayerDDay;

        // 빌딩 업그레이드 정보
        BuildingManager.Instance.SetBuildingLevel(BuildingType.PowerPlant, playerModel.PlayerPowerplantLevel);
        BuildingManager.Instance.SetBuildingLevel(BuildingType.BioFarm, playerModel.PlayerBiofarmLevel);
        BuildingManager.Instance.SetBuildingLevel(BuildingType.Quarters, playerModel.PlayerQuartersLevel);
        BuildingManager.Instance.SetBuildingLevel(BuildingType.FuelPlant, playerModel.PlayerFuelplantLevel);
        BuildingManager.Instance.SetBuildingLevel(BuildingType.ResearchLab, playerModel.PlayerResearchLabLevel);
        BuildingManager.Instance.SetBuildingLevel(BuildingType.RecoveryRoom, playerModel.PlayerRecoveryRoomLevel);
        BuildingManager.Instance.SetBuildingLevel(BuildingType.RecreationRoom, playerModel.PlayerRecreationRoomLevel);
        
        // 플레이어 정보
        LobbyMenuManager.Instance.SetHpAndStress(playerModel.PlayerHp, playerModel.PlayerStress);
        LobbyMenuManager.Instance.SetAttackAndDefense(playerModel.PlayerAttack, playerModel.PlayerDefense);
        
        // 기술 연구 상태 로드
        TechManager.Instance.techs[0].isLearned = playerModel.PlayerTech01IsLearned == 1;
        TechManager.Instance.techs[1].isLearned = playerModel.PlayerTech02IsLearned == 1;
        TechManager.Instance.techs[2].isLearned = playerModel.PlayerTech03IsLearned == 1;
        
        // TODO : 확인필요
        // 인벤토리
         // InventoryService inventoryService = new InventoryService();
         // inventoryService.GetAllInventory(); // 인벤토리를 가져와 playerData의 inventory에 적재
        //
        // // // 이큅먼트
        // EquipmentService equipmentService = new EquipmentService();
        // equipmentService.LoadEquipment();
    }

    // TODO : 데이터 저장
    public void SaveLobbyDataInDB()
    {
        PlayerService playerService = new PlayerService();
        
        // 현재 플레이어 아이디를 가지고 와서
        string playerId = playerService.GetPlayer().PlayerId;
        
        // 자원
        int currentEnergy = GameResourceManager.Instance.GetResourceAmount(ResourceType.Energy);
        int currentFood = GameResourceManager.Instance.GetResourceAmount(ResourceType.Food);
        int currentWorkforce = GameResourceManager.Instance.GetResourceAmount(ResourceType.Workforce);
        int currentFuel = GameResourceManager.Instance.GetResourceAmount(ResourceType.Fuel);
        int currentResearch = GameResourceManager.Instance.GetResourceAmount(ResourceType.Research);
        int currentCurrency = GameResourceManager.Instance.GetResourceAmount(ResourceType.Currency);

        playerService.UpdatePlayerResources(playerId, currentEnergy, currentFood, currentWorkforce, currentFuel, currentResearch, currentCurrency);
        
        // 빌딩 상황
        int powerPlantLevel = BuildingManager.Instance.GetBuildingLevel(BuildingType.PowerPlant);
        int bioFarmLevel = BuildingManager.Instance.GetBuildingLevel(BuildingType.BioFarm);
        int quartersLevel = BuildingManager.Instance.GetBuildingLevel(BuildingType.Quarters);
        int fuelPlantLevel = BuildingManager.Instance.GetBuildingLevel(BuildingType.FuelPlant);
        int researchLabLevel = BuildingManager.Instance.GetBuildingLevel(BuildingType.ResearchLab);
        int recoveryRoomLevel = BuildingManager.Instance.GetBuildingLevel(BuildingType.RecoveryRoom);
        int recreationRoomLevel = BuildingManager.Instance.GetBuildingLevel(BuildingType.RecreationRoom);

        
        playerService.UpdateBuildingLevels(playerId, powerPlantLevel, bioFarmLevel, quartersLevel, fuelPlantLevel,
            researchLabLevel, recoveryRoomLevel, recreationRoomLevel);

        // 날짜
        int currentDay = GameTimeManager.Instance.currentDay;

        playerService.UpdatePlayerDay(playerId, currentDay);

        // 랜덤인카운터
        EncounterService encounterService = new EncounterService();
        encounterService.UpdateEncounters(EncounterManager.Instance.UnresolvedEncounters);
        
        // 플레이어 Hp, 방어력 등 기본 정보
        float hp = LobbyMenuManager.Instance.hp;
        float stress = LobbyMenuManager.Instance.stress;
        float attack = LobbyMenuManager.Instance.attack;
        float defense = LobbyMenuManager.Instance.defense;
        
        playerService.UpdatePlayerStats(playerId, hp, stress, attack, defense);
        
        // 플레이어 연구실 Learned flag
        int isLearnedTech01 = TechManager.Instance.techs[0].isLearned ? 1 : 0;
        int isLearnedTech02 = TechManager.Instance.techs[1].isLearned ? 1 : 0;
        int isLearnedTech03 = TechManager.Instance.techs[2].isLearned ? 1 : 0;
        
        playerService.UpdateTechLearnedStatus(playerId, isLearnedTech01, isLearnedTech02, isLearnedTech03);
        
        // TODO : 확인필요
        // 인벤토리
        // InventoryService inventoryService = new InventoryService();
        // inventoryService.UpdateInventory();
        //
        // // 이큅먼트
        // EquipmentService equipmentService = new EquipmentService();
        // equipmentService.SaveEquipment();
        
        Debug.Log("DB에 데이터 저장 완료");
    }
    
    // TODO : 던전 진입시 씬 넘어갈때 로드할 데이터 
    public void DungeonLoadData()
    {
        
    }

    // TODO : 던전 게임 종료후 로비씬으로 넘어가기전 저장할 데이터 
    public void DungeonSaveData()
    {
        
    }
    
    // DB의 데이터들을 초기화. 게임 엔딩시 사용.
    // TODO : 
    public void ClearDataInDB()
    {
        PlayerService playerService = new PlayerService();
        EncounterService encounterService = new EncounterService();

        playerService.DeletePlayer();
        encounterService.DeleteEncounters();
        
        Debug.Log("All game data cleared successfully.");
    }
    
    /// <summary>
    /// 게임이 종료될때 데이터를 한번 더 저장
    /// </summary>
    private void OnApplicationQuit()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        // 현재 씬이 로비일 경우에 로비 저장, 던전일 경우 던전 저장 
        if (currentSceneName == "Lobby")
        {
            SaveLobbyDataInDB();
        }
        else if (currentSceneName.Contains("Dungeon")) // 던전 이름이 "Dungeon"을 포함한다고 가정
        {
            // SaveDungeonData();
        }
        else
        {
            Debug.LogWarning($"No specific save logic implemented for scene: {currentSceneName}");
        }
    }
} // end class
