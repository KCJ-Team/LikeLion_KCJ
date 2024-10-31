using System;
using System.Collections;
using System.Collections.Generic;
using SQLite;
using UnityEngine;

/// <summary>
/// CRUD 서비스단 코드
/// </summary>
public class PlayerService
{
    private SQLiteConnection dbConnection;

    public PlayerService()
    {
        dbConnection = DatabaseManager.Instance.GetConnection();
    }

    public TestPlayer CreatePlayer()
    {
        // guid 생성
        string id = Guid.NewGuid().ToString();
        
        // 모델 생성
        PlayerModel playerModel = new PlayerModel
        {
            playerId = id
        };
        
        try
        {
            dbConnection.Insert(playerModel);
        }
        catch (Exception ex)
        {
            Debug.LogError("Player Insert Fail : " + ex.Message);
            return null;
        }
        
        // PlayerModel -> TestPlayer 직접매핑
        TestPlayer newPlayer = new TestPlayer
        {
            playerId = playerModel.playerId
        };
        
        return newPlayer;
    }

    public TestPlayer GetPlayer()
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
            
            // PlayerModel -> TestPlayer 직접매핑
            TestPlayer player = new TestPlayer
            {
                playerId = playerModel.playerId
            };
            
            return player; // 플레이어가 없으면 null 반환
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to get player ID: " + ex.Message);
            return null;
        }
    }
} // end class
