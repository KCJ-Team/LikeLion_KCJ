using System.Collections;
using System.Collections.Generic;
using Messages;
using Playerinfo;
using UnityEngine;

/// <summary>
/// Test용 플레이어매니저. 매니저단에서는 각 기능 관련 로직을 담당,
/// NetworkHandler에서는 네트워크매니저와의 로직을 담당할지 말지 합칠지 그냥..
/// </summary>
public class PlayerManager : SceneSingleton<PlayerManager>
{
    // TODO : NetworkHandler 단을 구현하는게 좋을지 말지?
    // private PlayerNetworkHandler networkHandler;

    // 플레이어 위치 업데이트 요청을 네트워크 핸들러에 전달
    public void SendPlayerPosition(Vector3 position, Vector3 forward, float speed)
    {
        var tempPosition = new PlayerPosition
        {
            PlayerId = "Player1", // 예시 ID
            X = position.x,
            Y = position.y,
            Z = position.z,
            Speed = speed
        };

        // GameMessage에 담아서 전송
        var message = new GameMessage
        {
            PlayerPosition = tempPosition
        };

        GameNetworkManager.Instance.SendMessage(message); // NetworkManager를 통해 메시지를 전송
    }
} // end class