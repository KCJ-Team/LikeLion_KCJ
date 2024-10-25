using UnityEngine;
using Newtonsoft.Json;
using System;

// MultiplayerCharacterController는 멀티플레이어 환경에서 캐릭터의 움직임을 제어하고 동기화하는 클래스입니다.
public class MultiplayerCharacterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    
    private Vector3 movement;
    private Transform cameraTransform;
    private string playerId;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        playerId = Guid.NewGuid().ToString();
        NetworkManager.Instance.OnMessageReceived += OnNetworkMessageReceived;
    }

    private void Update()
    {
        HandleInput();
        Move();
        Rotate();
        SyncPosition();
    }

    // InputManager를 통해 입력을 처리합니다.
    private void HandleInput()
    {
        Vector2 input = InputManager.Instance.GetMovementInput();
        movement = new Vector3(input.x, 0f, input.y);
    }

    // 캐릭터를 이동시킵니다.
    private void Move()
    {
        if (movement.magnitude >= 0.1f)
        {
            Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward);
            Vector3 moveDirection = cameraForward * movement.z + cameraRight * movement.x;

            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }

    // 캐릭터를 회전시킵니다.
    private void Rotate()
    {
        if (movement != Vector3.zero)
        {
            Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward);
            Vector3 targetDirection = cameraForward * movement.z + cameraRight * movement.x;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    // 캐릭터의 위치를 서버와 동기화합니다.
    private void SyncPosition()
    {
        PlayerState state = new PlayerState
        {
            PlayerId = playerId,
            Position = transform.position,
            Rotation = transform.rotation
        };
        string json = JsonConvert.SerializeObject(state);
        NetworkManager.Instance.SendMessage(json);
    }

    // 네트워크 메시지를 받아 다른 플레이어의 위치를 업데이트합니다.
    private void OnNetworkMessageReceived(string message)
    {
        PlayerState state = JsonConvert.DeserializeObject<PlayerState>(message);
        if (state.PlayerId != playerId)
        {
            // 다른 플레이어의 위치와 회전을 업데이트합니다.
            // 더 부드러운 움직임을 위해 보간을 구현할 수 있습니다.
            transform.position = state.Position;
            transform.rotation = state.Rotation;
        }
    }
}