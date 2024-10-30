using System.Collections;
using System.Collections.Generic;
using Messages;
using Playerinfo;
using UnityEngine;

public class PlayerNetworkHandler
{
    // 플레이어 위치 정보를 서버로 전송하는 메서드
    // public void SendPlayerPosition(
    //     string playerId, float x, float y, float z, float vx, float vy, float vz, float speed)
    // {
    //     // PlayerPosition 메시지를 생성
    //     var position = new PlayerPosition
    //     {
    //         PlayerId = playerId,
    //         X = x,
    //         Y = y,
    //         Z = z,
    //         Speed = speed
    //     };
    //
    //     // GameMessage에 담아서 전송
    //     var message = new GameMessage
    //     {
    //         PlayerPosition = position
    //     };
    //
    //     GameNetworkManager.Instance.SendMessage(message); // NetworkManager를 통해 메시지를 전송
    // }
    
}
