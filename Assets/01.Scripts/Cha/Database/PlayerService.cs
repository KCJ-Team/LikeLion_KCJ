using System;
using System.Collections;
using System.Collections.Generic;
using PlayerInfo;
using SQLite;
using UnityEngine;

/// <summary>
/// CRUD 서비스단 코드. DB와 소통하는 단
/// </summary>
public class PlayerService
{
    private SQLiteConnection dbConnection;

    public PlayerService()
    {
        dbConnection = DatabaseManager.Instance.GetConnection();
    }

    /// <summary>
    /// 처음 게임 시작시 플레이어 캐릭터 생성
    /// </summary>
    public bool CreatePlayer(PlayerModelType type)
    {
        // 기존 플레이어 데이터가 있는지 확인
        var existingPlayer = dbConnection.Table<PlayerModel>().FirstOrDefault();

        // 기존 플레이어 데이터가 있으면 전체 데이터를 삭제하기.
        if (existingPlayer != null)
        {
            dbConnection.Delete(existingPlayer);
            Debug.Log("Existing player data deleted.");
            
            // TODO :
            // Encounter 테이블의 모든 데이터 삭제
            dbConnection.Execute("DELETE FROM encounter");
            Debug.Log("Encounter table data deleted.");
        }
        
        // guid 생성
        string id = Guid.NewGuid().ToString();
        
        // 플레이어 모델 생성
        PlayerModel playerModel = new PlayerModel
        {
            PlayerId = id,
            PlayerType = (int)type,
            PlayerEnergy = MainMenuManger.Instance.resourceDatas[ResourceType.Energy].initAmount,
            PlayerFood = MainMenuManger.Instance.resourceDatas[ResourceType.Food].initAmount,
            PlayerWorkforce = MainMenuManger.Instance.resourceDatas[ResourceType.Workforce].initAmount,
            PlayerFuel = MainMenuManger.Instance.resourceDatas[ResourceType.Fuel].initAmount,
            PlayerDDay = MainMenuManger.Instance.gameTimeData.startDay,
            PlayerPowerplantLevel = -1,
            PlayerBiofarmLevel = -1,
            PlayerQuartersLevel = -1,
            PlayerFuelplantLevel = -1,
            PlayerResearchLabLevel = -1,
            PlayerRecoveryRoomLevel = -1,
            PlayerRecreationRoomLevel = -1,
            PlayerHp = (int)MainMenuManger.Instance.playerData.BaseHP,
            PlayerAttack = (int)MainMenuManger.Instance.playerData.AttackPower,
            PlayerDefense = (int)MainMenuManger.Instance.playerData.Defense,
        };
        
        try
        {
            dbConnection.Insert(playerModel);
            
            // 데이터 저장 확인 로그
            Debug.Log($"Player created successfully: {JsonUtility.ToJson(playerModel, true)}");
        }
        catch (Exception ex)
        {
            Debug.LogError("Player Insert Fail : " + ex.Message);
            return false;
        }
        
       return true;
    }

    public PlayerModel GetPlayer()
    {
        try
        {
            // 첫 번째 플레이어 ID 가져오기
            PlayerModel playerModel = dbConnection.Table<PlayerModel>().FirstOrDefault();

            if (playerModel == null)
            {
                Debug.Log("No player found in the Table. Creating New Player will Start");
                return null;
            }
            
            return playerModel; // 플레이어가 없으면 null 반환
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to get player ID: " + ex.Message);
            return null;
        }
    }
    
    /// <summary>
    /// 플레이어의 자원
    /// </summary>
    public bool UpdatePlayerResources(string playerId, int energy, int food, int workforce, int fuel, int research, int currency)
    {
        try
        {
            var playerModel = dbConnection.Table<PlayerModel>().FirstOrDefault(p => p.PlayerId == playerId);
            
            if (playerModel != null)
            {
                playerModel.PlayerEnergy = energy;
                playerModel.PlayerFood = food;
                playerModel.PlayerWorkforce = workforce;
                playerModel.PlayerFuel = fuel;
                playerModel.PlayerResearch = research;
                playerModel.PlayerCurrency = currency;
                
                dbConnection.Update(playerModel);
                
                Debug.Log("Player resources updated successfully.");
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to update player resources: " + ex.Message);
            return false;
        }
    }
    
    /// <summary>
    /// 빌딩 레벨 저장
    /// </summary>
    public bool UpdateBuildingLevels(string playerId, int powerPlantLevel, int bioFarmLevel, int quartersLevel, int fuelPlantLevel, int researchLabLevel, int recoveryRoomLevel, int recreationRoomLevel)
    {
        try
        {
            var playerModel = dbConnection.Table<PlayerModel>().FirstOrDefault(p => p.PlayerId == playerId);
            if (playerModel != null)
            {
                playerModel.PlayerPowerplantLevel = powerPlantLevel;
                playerModel.PlayerBiofarmLevel = bioFarmLevel;
                playerModel.PlayerQuartersLevel = quartersLevel;
                playerModel.PlayerFuelplantLevel = fuelPlantLevel;
                playerModel.PlayerResearchLabLevel = researchLabLevel;
                playerModel.PlayerRecoveryRoomLevel = recoveryRoomLevel;
                playerModel.PlayerRecreationRoomLevel = recreationRoomLevel;
                
                dbConnection.Update(playerModel);
                
                Debug.Log("Building levels updated successfully.");
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to update building levels: " + ex.Message);
            return false;
        }
    }
    
    public bool UpdatePlayerDay(string playerId, int currentDay)
    {
        try
        {
            var playerModel = dbConnection.Table<PlayerModel>().FirstOrDefault(p => p.PlayerId == playerId);
            if (playerModel != null)
            {
                playerModel.PlayerDDay = currentDay;
                dbConnection.Update(playerModel);
                Debug.Log("Player day updated successfully.");
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to update player day: " + ex.Message);
            return false;
        }
    }
    
    public bool UpdatePlayerStats(string playerId, float hp, float stress, float attack, float defense)
    {
        try
        {
            // PlayerId로 플레이어 데이터를 가져옴
            var playerModel = dbConnection.Table<PlayerModel>().FirstOrDefault(p => p.PlayerId == playerId);
            if (playerModel != null)
            {
                // HP, 스트레스, 공격력, 방어력을 업데이트
                playerModel.PlayerHp = hp;
                playerModel.PlayerStress = stress;
                playerModel.PlayerAttack = attack;
                playerModel.PlayerDefense = defense;

                // 데이터베이스에 업데이트
                dbConnection.Update(playerModel);

                Debug.Log("Player stats updated successfully.");
                return true;
            }
            else
            {
                Debug.LogWarning("Player not found for updating stats.");
                return false;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to update player stats: " + ex.Message);
            return false;
        }
    }
    
    public bool UpdateTechLearnedStatus(string playerId, int tech01IsLearned, int tech02IsLearned, int tech03IsLearned)
    {
        try
        {
            // 플레이어 데이터를 ID로 검색
            var playerModel = dbConnection.Table<PlayerModel>().FirstOrDefault(p => p.PlayerId == playerId);
            if (playerModel != null)
            {
                // 기술 상태 업데이트
                playerModel.PlayerTech01IsLearned = tech01IsLearned;
                playerModel.PlayerTech02IsLearned = tech02IsLearned;
                playerModel.PlayerTech03IsLearned = tech03IsLearned;

                // 데이터베이스에 저장
                dbConnection.Update(playerModel);

                Debug.Log("Tech learned status updated successfully.");
                return true;
            }
            else
            {
                Debug.LogWarning("Player not found for updating tech learned status.");
                return false;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to update tech learned status: " + ex.Message);
            return false;
        }
    }
    
    public bool DeletePlayer()
    {
        try
        {
            // 플레이어 테이블의 모든 데이터 삭제
            dbConnection.Execute("DELETE FROM player");
            
            Debug.Log("All player data deleted from the database.");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to delete player data: " + ex.Message);
            return false;
        }
    }

    
} // end class
