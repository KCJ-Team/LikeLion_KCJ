using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 멀티게임룸을 관리하는 매니저. 방정보(플레이어정보). Room 클래스를 만들어서 관리해도됨.
/// </summary>
public class RoomManager : SceneSingleton<RoomManager>
{
    public string RoomId
    {
        get => roomId;
        set => roomId = value;
    }

    public List<TestPlayer> Players
    {
        get => players;
        set => players = value;
    }

    private string roomId;
    private List<TestPlayer> players = new();
    
    // Test 프리팹 오브젝트
    public GameObject playerPrefab;
    public GameObject otherPlayerPrefab;
    
    // 룸이 생성되었을때 정보를 받고 룸 초기화
    public void InitializeRoom(string roomId, List<TestPlayer> players)
    {
        this.roomId = roomId;
        this.players = players;
        Debug.Log($"Room initialized with ID: {roomId}, Player count: {players.Count}");
        
        // 모든 플레이어 오브젝트 생성
        foreach (var player in players)
        {
            CreatePlayerObject(player);
        }
    }
    
    // 플레이어 오브젝트 생성 메서드
    private void CreatePlayerObject(TestPlayer player)
    {
        // X, Y, Z 값이 초기화된 값(0)인 경우 기본값으로 0f 설정
        float x = player.x != 0 ? player.x : 0f;
        float y = player.y != 0 ? player.y : 0f;
        float z = player.z != 0 ? player.z : 0f;
        
        Vector3 initialPosition = new Vector3(x, y, z);

        GameObject newPlayer;
        
        // 나 자신이라면
        if (player.playerId == PlayerManager.Instance.Player.playerId)
        {
            newPlayer = Instantiate(playerPrefab, initialPosition, Quaternion.identity);
        }
        else
        {
            newPlayer = Instantiate(otherPlayerPrefab, initialPosition, Quaternion.identity);

        }
       
        newPlayer.name = $"Player_{player.playerId}";
        player.playerObject = newPlayer; // 생성된 오브젝트를 TestPlayer에 저장
        
        Debug.Log($"Created player object for Player ID: {player.playerId} at position: {initialPosition}");
    }

    // 플레이어 위치 정보 업데이트 메서드
    public void UpdateOtherPlayerPosition(string playerId, Vector3 newPosition, float newSpeed)
    {
        TestPlayer player = GetPlayerByPlayerId(playerId);
        
        if (player != null)
        {
            // 실제 게임 오브젝트의 위치를 업데이트
            player.playerObject.transform.position = newPosition;
            player.speed = newSpeed;
            
            Debug.Log($"Updated position for player {playerId}: ({newPosition.x}, {newPosition.y}, {newPosition.z}), Speed: {newSpeed}");
        }
        else
        {
            Debug.LogWarning($"Player with ID {playerId} not found in room.");
        }
    }
    
    // 특정 플레이어 ID로 플레이어 객체를 반환하는 메서드
    public TestPlayer GetPlayerByPlayerId(string playerId)
    {
        return players.Find(player => player.playerId == playerId);
    }
    
    // TODO : 룸파괴, 오브젝트파괴 메소드도 구현해야함

} // end class
