using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MessageType = Messages.MessageType;

/// <summary>
/// Test용 플레이어매니저. 매니저단에서는 각 기능 관련 로직을 담당,
/// </summary>
public class PlayerManager : SceneSingleton<PlayerManager>
{
    // 일단 테스트로 넣어둠
    private TestPlayer player;
    private PlayerService playerService;
    private PlayerNetworkHandler playerNetworkHandler;

    public TestPlayer Player
    {
        get => player;
        set => player = value;
    }

    private void Start()
    {
        playerService = new PlayerService();
        playerNetworkHandler = new PlayerNetworkHandler();
    }

    public void Login()
    { 
        // 실제 로직, DB에서 플레이어 ID는 하나.
        // player = playerService.GetPlayer();
        // if (player == null)
        // {
        //     player = playerService.CreatePlayer();
        //     if (player == null) 
        //     {
        //         Debug.LogError("Failed to create new player.");
        //         return;
        //     }
        //     Debug.Log($"Create New Player : {player.playerId}");
        // }
        // else
        // {
        //     Debug.Log($"Get Current Player : {player.playerId}");
        // }
        
        // 테스트를 위해 매번 로그인마다 새로운 ID를 디비에 저장한다.
        player = playerService.CreatePlayer();
        if (player == null)
        {
            Debug.LogError("Failed to create new player.");
            return;
        }
        Debug.Log($"Create New Player in Build: {player.playerId}");
        
        // 이제 플레이어가 생겼으면 id를 가지고 세션 연결
        playerNetworkHandler.SendPlayerId(player.playerId, MessageType.SessionLogin); // playerId 전송
    }

    public void Logout()
    {
        if (player == null)
        {
            Debug.LogWarning("No player is currently logged in.");
            return;
        }
        
        // 세션에 로그아웃 요청
        playerNetworkHandler.SendPlayerId(player.playerId, MessageType.SessionLogout); // playerId 전송

        // 초기화 작업
        player = null;
        Debug.Log("Player has been logged out and local data cleared.");
    }
    
    // 매치메이킹 취소
    public void StartMatchmaking()
    {
        if (player == null)
        {
            Debug.LogWarning("No player is currently logged in.");
            return;
        }
        
        // 세션에 있는 플레이어에게 매치메이킹 시작 요청
        playerNetworkHandler.SendPlayerId(player.playerId, MessageType.MatchmakingStart);
    }
    
    // 매치메이킹 취소
    public void CancelMatchmaking()
    {
        if (player == null)
        {
            Debug.LogWarning("No player is currently logged in.");
            return;
        }
        
        // 세션에 있는 플레이어에게 매치메이킹 취소 요청
        playerNetworkHandler.SendPlayerId(player.playerId, MessageType.MatchmakingCancel);
    }
    
    // 플레이어 위치 업데이트 요청을 네트워크 핸들러에 전달
    public void SendPlayerPosition(MessageType messageType, Vector3 position, float speed, int health)
    {
        playerNetworkHandler.SendPlayerPosition(messageType, player, position, speed, health);
    }
    
    private void OnApplicationQuit()
    {
        Logout();
        Debug.Log("Application is quitting, player logged out.");
    }

   
} // end class