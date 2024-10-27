using UnityEngine;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

// NetworkManager는 서버와의 WebSocket 통신을 관리하는 싱글톤 클래스입니다.
public class NetworkManager : DD_Singleton<NetworkManager>
{
    private ClientWebSocket webSocket;
    private const string SERVER_URL = "ws://localhost:8080/ws";
    public event Action<string> OnMessageReceived;

    protected override void Awake()
    {
        base.Awake();
        ConnectToServer();
    }

    // 서버에 연결하는 비동기 메서드
    private async void ConnectToServer()
    {
        webSocket = new ClientWebSocket();
        await webSocket.ConnectAsync(new Uri(SERVER_URL), CancellationToken.None);
        Debug.Log("Connected to server");
        StartReceiving();
    }

    // 서버로부터 메시지를 지속적으로 받는 비동기 메서드
    private async void StartReceiving()
    {
        byte[] buffer = new byte[1024];
        while (webSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                OnMessageReceived?.Invoke(message);
            }
        }
    }

    // 서버로 메시지를 보내는 비동기 메서드
    public async void SendMessage(string message)
    {
        if (webSocket.State == WebSocketState.Open)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    // 객체가 파괴될 때 WebSocket 연결을 닫습니다.
    private void OnDestroy()
    {
        if (webSocket != null && webSocket.State == WebSocketState.Open)
        {
            webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Application closing", CancellationToken.None);
        }
    }
}