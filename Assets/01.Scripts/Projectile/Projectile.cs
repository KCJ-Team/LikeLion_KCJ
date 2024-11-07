using UnityEngine;

// 발사체의 기본 동작을 정의하는 추상 클래스
// State 패턴을 사용하여 발사체의 상태를 관리
public abstract class Projectile : MonoBehaviour
{
    protected ProjectileState currentState;   // 현재 발사체의 상태
    protected float damage;                   // 발사체가 가하는 데미지
    public float speed;                    // 발사체의 이동 속도
    
    
    // 발사체 초기화 메서드
    public virtual void Initialize(Vector3 direction, float damage)
    {
        this.damage = damage;
        ChangeState(GetInitialState());   // 초기 상태로 설정
    }
    
    // 발사체의 초기 상태를 반환하는 추상 메서드
    protected abstract ProjectileState GetInitialState();
    
    public float GetDamage()
    {
        return damage;
    }
    
    // 발사체의 상태를 변경하는 메서드
    public void ChangeState(ProjectileState newState)
    {
        // 현재 상태가 있다면 종료
        if (currentState != null)
            currentState.ExitState();
        
        // 새로운 상태로 변경하고 시작
        currentState = newState;
        currentState.EnterState();
    }
    
    // 매 프레임마다 현재 상태 업데이트
    protected virtual void Update()
    {
        if (currentState != null)
            currentState.UpdateState();
    }
}