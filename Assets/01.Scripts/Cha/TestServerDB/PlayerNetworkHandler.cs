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
    public void SendPlayerPosition(MessageType messageType, TestPlayer player, Vector3 position, float speed, int hp)
    {
         // RoomManager에서 roomId와 플레이어 목록 가져오기
         string roomId = RoomManager.Instance.RoomId;
         
         // RoomPlayerUpdate 객체 생성
         // TODO : 모델, 웨폰 프리팹도 초기화 시켜주어야함
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
                 Hp = hp,
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
    
    
    // 플레이어 HP 전송
    public void SendPlayerHp(MessageType messageType, TestPlayer player, int hp)
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
                Hp = hp,
            }
        };
        
        GameMessage message = new GameMessage
        {
            MessageType = messageType, // 메시지 타입 설정\
            RoomPlayerUpdate = roomPlayerUpdate
        };
        
        // 디버깅 정보 출력
        Debug.Log("\n Sending player position to server:");
        
        networkManager.SendMessage(message);
    }
    
    // 플레이어 무기 변경시 무기타입 전송.
    // TODO : 기존 웨폰타입 PlayerWeapontype으로 변경.. 
    public void SendPlayerWeaponType(MessageType messageType, TestPlayer player, PlayerWeaponType weaponType)
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
                PrefabWeaponType = weaponType
                
            }
        };
        
        GameMessage message = new GameMessage
        {
            MessageType = messageType, // 메시지 타입 설정\
            RoomPlayerUpdate = roomPlayerUpdate
        };
        
        // 디버깅 정보 출력
        Debug.Log("\n Sending player position to server:");
        
        networkManager.SendMessage(message);
    }
    
    // 플레이어 애니메이션 파라미터값 변경시 값 전송
    public void SendPlayerWeaponType(MessageType messageType, TestPlayer player, float isRunning, bool isAim, float movementX, float movementY, int weaponType)
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
                AnimParams =
                {
                    IsRunning = isRunning,
                    IsAim = isAim,
                    MovementX = movementX,
                    MovementY = movementY,
                    WeaponType = weaponType
                }
                
            }
        };
        
        GameMessage message = new GameMessage
        {
            MessageType = messageType, // 메시지 타입 설정\
            RoomPlayerUpdate = roomPlayerUpdate
        };
        
        // 디버깅 정보 출력
        Debug.Log("\n Sending player position to server:");
        
        networkManager.SendMessage(message);
    }
} // end class