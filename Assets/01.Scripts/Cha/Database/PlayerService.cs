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

        if (existingPlayer != null)
        {
            // 기존 플레이어 데이터 삭제
            dbConnection.Delete(existingPlayer);
            Debug.Log("Existing player data deleted.");
        }
        
        // guid 생성
        string id = Guid.NewGuid().ToString();
        
        // 플레이어 모델 생성
        PlayerModel playerModel = new PlayerModel
        {
            PlayerId = id,
            PlayerType = (int)type,
            PlayerEnergy = MainMenuMaanger.Instance.resourceDatas[ResourceType.Energy].initAmount,
            PlayerFood = MainMenuMaanger.Instance.resourceDatas[ResourceType.Food].initAmount,
            PlayerWorkforce = MainMenuMaanger.Instance.resourceDatas[ResourceType.Workforce].initAmount,
            PlayerFuel = MainMenuMaanger.Instance.resourceDatas[ResourceType.Fuel].initAmount,
            PlayerDDay = MainMenuMaanger.Instance.gameTimeData.startDay
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
        
        // // PlayerModel -> TestPlayer 직접매핑
        // TestPlayer newPlayer = new TestPlayer
        // {
        //     playerId = playerModel.PlayerId
        // };
        
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
    /// <param name="playerId"></param>
    /// <param name="energy"></param>
    /// <param name="food"></param>
    /// <param name="workforce"></param>
    /// <param name="fuel"></param>
    /// <returns></returns>
    public bool UpsertPlayerResources(string playerId, int energy, int food, int workforce, int fuel)
    {
        try
        {
            // 플레이어 정보가 있는지 확인
            var playerModel = dbConnection.Table<PlayerModel>().FirstOrDefault(p => p.PlayerId == playerId);

            if (playerModel != null)
            {
                // 데이터가 있으면 업데이트
                playerModel.PlayerEnergy = energy;
                playerModel.PlayerFood = food;
                playerModel.PlayerWorkforce = workforce;
                playerModel.PlayerFuel = fuel;
                dbConnection.Update(playerModel);
                Debug.Log("Player resources updated successfully.");
            }
            else
            {
                // 데이터가 없으면 새로 삽입
                playerModel = new PlayerModel
                {
                    PlayerId = playerId,
                    PlayerEnergy = energy,
                    PlayerFood = food,
                    PlayerWorkforce = workforce,
                    PlayerFuel = fuel
                };
                dbConnection.Insert(playerModel);
                Debug.Log("New player inserted with resources.");
            }

            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to upsert player resources: " + ex.Message);
            return false;
        }
    }
} // end class
