using System.Collections.Generic;
using Google.Protobuf.Collections;
using UnityEngine;
using Messages;
using PlayerInfo;

public class PlayerNetworkHandler
{
    private readonly GameNetworkManager networkManager;

    public PlayerNetworkHandler()
    {
        networkManager = GameNetworkManager.Instance;
    }

    // 플레이어 ID 전송
    public void SendPlayerId(string playerId, MessageType messageType)
    {
        // GameMessage 생성 및 메시지 타입과 playerId 설정
        GameMessage message = new GameMessage
        {
            MessageType = messageType, // 메시지 타입 설정
            PlayerInfo = new PlayerInfo.PlayerInfo()
            {
                PlayerId = playerId
            }
        };
        
        networkManager.SendMessage(message);
        Debug.Log($"Sent playerId to server : MessageType : {messageType}, PlayerId : {playerId}");
    }
    
    // 플레이어 위치 정보 전송
    public void SendPlayerPosition(MessageType messageType, TestPlayer player, Vector3 position, float speed, int health)
    {
         // RoomManager에서 roomId와 플레이어 목록 가져오기
         string roomId = RoomManager.Instance.RoomId;
         
         // RoomPlayerUpdate 객체 생성
         var roomPlayerUpdate = new RoomInfo.RoomPlayerUpdate()
         {
             RoomId = roomId,
             PlayerInfo = new PlayerInfo.PlayerInfo()
             {
                 PlayerId = player.playerId,
                 X = position.x,
                 Y = position.y,
                 Z = position.z,
                 Speed = speed,
                 Health = health,
             }
         };
        
         GameMessage message = new GameMessage
         {
             MessageType = messageType, // 메시지 타입 설정\
             RoomPlayerUpdate = roomPlayerUpdate
         };
        
         // 디버깅 정보 출력
         Debug.Log("\n Sending player position to server:");
         // Debug.Log($"Player ID: {player.playerId}");
         // Debug.Log($"Position - X: {position.x}, Y: {position.y}, Z: {position.z}");
         // Debug.Log($"Speed: {speed}");
         // Debug.Log($"Health: {health} \n");
        
        networkManager.SendMessage(message);
    }

    // 서버에서 받은 메시지 처리
    public void HandleReceivedMessage(GameMessage message)
    {
        // 받은 메시지를 파싱하여 플레이어의 상태나 위치를 업데이트하는 로직 추가
        Debug.Log("Received message from server: " + message);
    }
} // end class