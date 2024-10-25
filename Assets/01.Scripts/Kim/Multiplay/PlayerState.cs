using UnityEngine;

// PlayerState는 플레이어의 상태를 나타내는 직렬화 가능한 클래스입니다.
[System.Serializable]
public class PlayerState
{
    public string PlayerId;  // 플레이어의 고유 식별자
    public Vector3 Position; // 플레이어의 3D 위치
    public Quaternion Rotation; // 플레이어의 회전
}