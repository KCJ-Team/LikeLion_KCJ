using System;
using System.Collections;
using System.Collections.Generic;
using Messages;
using Playerinfo;
using UnityEngine;

/// <summary>
/// Test용 플레이어매니저. 매니저단에서는 각 기능 관련 로직을 담당,
/// </summary>
public class PlayerManager : SceneSingleton<PlayerManager>
{
    // 일단 테스트로 Player 넣어둠
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
        // db에 플레이어 가져오기
        player = playerService.GetPlayer();

        // 만약 없으면 플레이어 만들기(Create)
        if (player == null)
        {
            player = playerService.CreatePlayer();

            // 실패시 로직 처리해야함
            if (player == null) return;

            Debug.Log($"Create New Player : {player.playerId}");
        }
        else
        {
            Debug.Log($"Get Current Player : {player.playerId}");
        }

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

    // 플레이어 위치 업데이트 요청을 네트워크 핸들러에 전달
    public void SendPlayerPosition(Vector3 position, Vector3 forward, float speed)
    {
        playerNetworkHandler.SendPlayerPosition(player, position, speed);
    }

    private void OnApplicationQuit()
    {
        Logout();
        Debug.Log("Application is quitting, player logged out.");
    }
} // end class