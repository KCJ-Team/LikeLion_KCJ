using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Messages;

public class PlayerMovementController
{
    private readonly Transform _transform;
    private readonly Animator _animator;
    private readonly PlayerCharacterController _player;
    
    private Vector3 _movement;
    private Quaternion _targetRotation;
    private Transform _cameraTransform;
    private bool _isAimLocked;
    private bool _isSkillActive;
    private const float RotationSpeed = 10f;
    private const float AimSpeed = 2f;

    // 위치 업데이트를 위한 변수 추가
    private Vector3 _lastPosition;
    private Quaternion _lastRotation;
    private const float PositionUpdateThreshold = 0.01f; // 위치 변경 감지 임계값
    private const float RotationUpdateThreshold = 0.1f;  // 회전 변경 감지 임계값

    public MovementInfo MovementInfo => new MovementInfo
    {
        Movement = _movement,
        IsAimLocked = _isAimLocked,
        IsSkillActive = _isSkillActive
    };

    public PlayerMovementController(PlayerCharacterController player, Transform transform, Animator animator)
    {
        _player = player;
        _transform = transform;
        _animator = animator;
    }

    public void Initialize()
    {
        _cameraTransform = Camera.main.transform;
        _targetRotation = _transform.rotation;
        _lastPosition = _transform.position;
        _lastRotation = _transform.rotation;
    }

    public void HandleMovement()
    {
        HandleInput();
        HandleRotation();
        Move();
    }

    private void HandleInput()
    {
        Vector2 input = InputManager.Instance.GetMovementInput();
        _movement = new Vector3(input.x, 0f, input.y);
        _isAimLocked = Input.GetMouseButton(1);
        _isSkillActive = _animator.GetBool("IsSkill");
    }

    private void Move()
    {
        if (_movement.magnitude < 0.1f) return;

        Vector3 moveDirection = CalculateMoveDirection();
        float currentSpeed = (_isAimLocked || _isSkillActive) ? AimSpeed : _player.playerData.MoveSpeed;
        _transform.position += moveDirection * (currentSpeed * Time.deltaTime);
    }

    private Vector3 CalculateMoveDirection()
    {
        Vector3 cameraForward = Vector3.ProjectOnPlane(_cameraTransform.forward, Vector3.up).normalized;
        Vector3 cameraRight = Vector3.Cross(Vector3.up, cameraForward);
        return cameraForward * _movement.z + cameraRight * _movement.x;
    }

    private void HandleRotation()
    {
        if (_isAimLocked || _isSkillActive)
        {
            LookAtMouse();
            _transform.rotation = Quaternion.Slerp(_transform.rotation, _targetRotation, Time.deltaTime * RotationSpeed * 2f);
        }
        else if (_movement != Vector3.zero)
        {
            RotateTowardsMovement();
            _transform.rotation = Quaternion.Slerp(_transform.rotation, _targetRotation, Time.deltaTime * RotationSpeed);
        }
    }

    private void RotateTowardsMovement()
    {
        Vector3 moveDirection = CalculateMoveDirection();
        _targetRotation = Quaternion.LookRotation(moveDirection);
    }

    private void LookAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, _transform.position);

        if (groundPlane.Raycast(ray, out float rayDistance))
        {
            Vector3 pointToLook = ray.GetPoint(rayDistance);
            Vector3 direction = pointToLook - _transform.position;
            direction.y = 0f;
            
            if (direction.magnitude > 0.1f)
            {
                _targetRotation = Quaternion.LookRotation(direction.normalized);
            }
        }
    }

    // 서버에 위치 정보 업데이트
    private void UpdateServerPosition()
    {
        // 플레이어가 룸에 입장했는지 확인
        if (RoomManager.Instance == null || string.IsNullOrEmpty(RoomManager.Instance.RoomId))
        {
            return; // 룸에 입장하지 않은 상태면 위치 업데이트를 하지 않음
        }

        // 위치가 변경되었을 때만 서버에 업데이트
        if (Vector3.Distance(_transform.position, _lastPosition) > PositionUpdateThreshold)
        {
            float currentSpeed = (_isAimLocked || _isSkillActive) ? AimSpeed : _player.playerData.MoveSpeed;
            PlayerManager.Instance.SendPlayerPosition(MessageType.PlayerPositionUpdate, _transform.position, currentSpeed);
            _lastPosition = _transform.position;
        }
    }
}