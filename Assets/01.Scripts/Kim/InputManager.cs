using UnityEngine;

// InputManager는 전체 게임에서 입력을 관리하는 싱글톤 클래스입니다.
public class InputManager : DD_Singleton<InputManager>
{
    // 이동 입력을 가져오는 메서드
    public Vector2 GetMovementInput()
    {
        // 수평 및 수직 입력을 가져옵니다.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        
        return new Vector2(horizontal, vertical).normalized;
    }
}