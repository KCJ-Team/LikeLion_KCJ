using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using Google.Protobuf;
using Messages;
using TMPro;
using UnityEngine;

public class GameNetworkManager : SceneSingleton<GameNetworkManager>
{
    private TcpClient client;
    private NetworkStream stream;
    private bool isRunning = false;
    
    [SerializeField]
    private NetworkConfig networkConfig;
    
    // 수신된 메세지를 일시적으로 저장
    private ConcurrentQueue<GameMessage> receiveQueue = new ConcurrentQueue<GameMessage>(); 
  
    // 일단 테스트로 보여줄 텍스트 넣어두기
    public TextMeshProUGUI textResult;

    private void Start()
    {
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        try
        {
            // 에디터와 빌드 환경에 따라 다른 IP 설정
           // string serverIP;
        
// // #if UNITY_EDITOR
//             serverIP = networkConfig.localServerIP;  // Unity 에디터에서 사용할 로컬 IP
// // #else
//             // serverIP = networkConfig.releaseServerIP;  // 빌드된 애플리케이션에서 사용할 릴리스 서버 IP
// // #endif
//             
            client = new TcpClient(networkConfig.serverIP, networkConfig.serverPort);
            stream = client.GetStream();
            isRunning = true;
            
            Debug.Log("Connected to server.");

            // 처음 데이터 수신 시작
            StartReceiving();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error connecting to server: {e.Message}");
        }
    }

    private void StartReceiving()
    {
        if (stream != null && isRunning)
        {
            byte[] lengthBuffer = new byte[4];
            Debug.Log("Attempting to receive packet length...");
            stream.BeginRead(lengthBuffer, 0, lengthBuffer.Length, OnLengthReceived, lengthBuffer);
        }
    }

    private void OnLengthReceived(IAsyncResult ar)
    {
        try
        {
            int bytesRead = stream.EndRead(ar);
            if (bytesRead == 0)
            {
                Debug.LogWarning("Disconnected from server during length read.");
                isRunning = false;
                return;
            }

            byte[] lengthBuffer = (byte[])ar.AsyncState;
            int messageLength = BitConverter.ToInt32(lengthBuffer, 0);
            Debug.Log($"Packet length received: {messageLength} bytes");
            
            // 메시지 본문을 읽기 위한 버퍼 준비
            byte[] messageBuffer = new byte[messageLength];
            stream.BeginRead(messageBuffer, 0, messageLength, OnMessageReceived, messageBuffer);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error receiving message length: {e.Message}");
            isRunning = false;
        }
    }

    private void OnMessageReceived(IAsyncResult ar)
    {
        try
        {
            int bytesRead = stream.EndRead(ar);
            if (bytesRead == 0)
            {
                Debug.LogWarning("Disconnected from server during message read.");
                isRunning = false;
                return;
            }

            byte[] messageBuffer = (byte[])ar.AsyncState;
            Debug.Log($"Message received with {bytesRead} bytes out of expected {messageBuffer.Length} bytes");
            
            // 메시지 길이 검증
            if (bytesRead != messageBuffer.Length)
            {
                Debug.LogError($"Expected message length {messageBuffer.Length}, but received {bytesRead}");
                return;
            }
            
            GameMessage gameMessage = GameMessage.Parser.ParseFrom(messageBuffer);
            Debug.Log("Parsed GameMessage successfully.");
            
            // 수신 큐에 메시지를 추가
            receiveQueue.Enqueue(gameMessage);
            Debug.Log($"receiveQueue count: {receiveQueue.Count}");

            // 다음 메시지를 기다리기 위해 StartReceiving 호출
            StartReceiving();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error receiving message: {e.Message}");
            isRunning = false;
        }
    }

    private void Update()
    {
        // 메인 스레드에서 큐에 있는 메시지를 처리
        while (receiveQueue.TryDequeue(out GameMessage gameMessage))
        {
            Debug.Log("Message dequeued from receiveQueue.");
            JobQueue.Enqueue(() => ProcessMessage(gameMessage));
        }
    }
    
    // TODO : try-catch로 오류처리..
    public void SendMessage(GameMessage message)
    {
        if (client != null && client.Connected)
        {
            byte[] messageBytes = message.ToByteArray();
            byte[] lengthBytes = BitConverter.GetBytes(messageBytes.Length);

            stream.Write(lengthBytes, 0, 4);
            stream.Write(messageBytes, 0, messageBytes.Length);
        }
    }
    
    /// <summary>
    /// 서버에서 보낸 패킷을 메세지로 받아 타입에 맞춰 로직을 짜준다.
    /// </summary>
    /// <param name="message"></param>
    private void ProcessMessage(GameMessage message)
    {
        if (message == null) return;
        // 왜 Type이 안오지??
        Debug.Log($"Processed message: {message.ToString()}");
        
        // Test로 message 텍스트 화면에 쏴주기..
        textResult.text = message.ToString();
        
        // TODO : messageType을 보고 처리 로직 구현하기
        switch (message.MessageType) {
            case MessageType.SessionLogin :
                Debug.Log($"로그인 타입 / Processed message: {message.ToString()}");
    
                break;
            
            case MessageType.SessionLogout :

                break;
            case MessageType.MatchmakingStart :
                if (message.MessageCase == GameMessage.MessageOneofCase.Response)
                {
                    // 방 생성시 클라이언트 로직 처리
                    if (message.Response.RoomInfo != null)
                    {
                        var roomInfo = message.Response.RoomInfo;
                        var players = new List<TestPlayer>();
                        
                        // RoomInfo의 플레이어를 list속에 플레이어 넣기
                        foreach (var player in roomInfo.Players)
                        {
                            Debug.Log($"Player ID: {player.PlayerId}, Position: ({player.X}, {player.Y}, {player.Z})");

                            // 플레이어 정보를 생성하고 List에 추가
                            players.Add(new TestPlayer(
                                player.PlayerId,
                                player.X,  // X 좌표
                                player.Y,  // Y 좌표
                                player.Z,  // Z 좌표
                                player.Speed,
                                (int)player.Health    // Health를 int로 변환
                            ));
                        }

                        // RoomManager에서 방 초기화 및 플레이어 생성 호출
                        RoomManager.Instance.InitializeRoom(roomInfo.RoomId, players);
                    }
                }
               
                break;
            
            case MessageType.PlayerPositionUpdate:
                if (message.MessageCase == GameMessage.MessageOneofCase.RoomPlayerUpdate)
                {
                    var roomPlayerUpdate = message.RoomPlayerUpdate;

                    // 해당 플레이어의 ID와 위치 정보를 사용하여 위치 업데이트 메서드 호출
                    Vector3 newPosition = new Vector3(
                        roomPlayerUpdate.PlayerInfo.X,
                        roomPlayerUpdate.PlayerInfo.Y,
                        roomPlayerUpdate.PlayerInfo.Z
                    );
                    
                    float newSpeed = roomPlayerUpdate.PlayerInfo.Speed;

                    // RoomManager에서 바뀐 플레이어의 포지션을 업데이트
                    RoomManager.Instance.UpdateOtherPlayerPosition(
                        roomPlayerUpdate.PlayerInfo.PlayerId,
                        newPosition,
                        newSpeed
                    );

                    Debug.Log($"Player position updated for {roomPlayerUpdate.PlayerInfo.PlayerId}");
                }
                
                break;
        }
    }

    private void OnDisable()
    {
        isRunning = false;
        stream?.Close();
        client?.Close();
    }
}
