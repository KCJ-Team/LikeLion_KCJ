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
    public void SendPlayerPosition(TestPlayer player, Vector3 position, float speed)
    {
        var playerPosition = new PlayerInfo.PlayerInfo()
        {
            PlayerId = player.playerId,
            X = position.x,
            Y = position.y,
            Z = position.z,
            Speed = speed
        };

        GameMessage message = new GameMessage
        {
            // PlayerPosition = playerPosition
        };
        
        networkManager.SendMessage(message);
        Debug.Log("Sent player position to server: " + position);
    }

    // 서버에서 받은 메시지 처리
    public void HandleReceivedMessage(GameMessage message)
    {
        // 받은 메시지를 파싱하여 플레이어의 상태나 위치를 업데이트하는 로직 추가
        Debug.Log("Received message from server: " + message);
    }
}