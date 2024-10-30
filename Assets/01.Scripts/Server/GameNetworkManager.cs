using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using Google.Protobuf;
using Messages;
using UnityEngine;

public class GameNetworkManager : SceneSingleton<GameNetworkManager>
{
    private TcpClient client;
    private NetworkStream stream;
    private bool isRunning = false;
    
    [SerializeField]
    private NetworkConfig networkConfig;
    
    private ConcurrentQueue<GameMessage> receiveQueue = new ConcurrentQueue<GameMessage>(); // 작업큐
    
    private void Start()
    {
        ConnectToServer();
    }

    private void ConnectToServer()
    {
        try
        {
            // 에디터와 빌드 환경에 따라 다른 IP 설정
            string serverIP;
        
#if UNITY_EDITOR
            serverIP = networkConfig.localServerIP;  // Unity 에디터에서 사용할 로컬 IP
#else
            serverIP = networkConfig.releaseServerIP;  // 빌드된 애플리케이션에서 사용할 릴리스 서버 IP
#endif
            
            client = new TcpClient(serverIP, networkConfig.serverPort);
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
            // 비동기적으로 4바이트 길이 정보를 읽음
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
                Debug.LogWarning("Disconnected from server.");
                return;
            }

            byte[] lengthBuffer = (byte[])ar.AsyncState;
            int messageLength = BitConverter.ToInt32(lengthBuffer, 0);

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
                Debug.LogWarning("Disconnected from server.");
                return;
            }

            byte[] messageBuffer = (byte[])ar.AsyncState;
            GameMessage gameMessage = GameMessage.Parser.ParseFrom(messageBuffer);

            // 수신 큐에 메시지를 추가
            receiveQueue.Enqueue(gameMessage);

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
            JobQueue.Enqueue(() => ProcessMessage(gameMessage));
        }
    }
    
    private void ProcessMessage(GameMessage message)
    {
        Debug.Log($"Processed message: {message}");
    }

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

    private void OnDisable()
    {
        isRunning = false;
        stream?.Close();
        client?.Close();
    }
}
