using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum GoalState
{
    MovingToTarget,
    MovingToFinalPoint,
    Completed
}

public class TargetGame : MonoBehaviour
{
    public SamplePlayerMovement playerMovement; // 플레이어의 이동 스크립트
    public GameObject target;
    public Transform finalPoint;
    
    [SerializeField]
    private bool hasTarget = false;  // 플레이어가 목표물을 가지고 있는지 여부
    [SerializeField]
    private bool reachedFinalPoint = false; // 마지막 지점에 도착했는지 여부
    
    [Header("샘플 타겟 핼스")]
    private float playerHealth = 100f;    // 목표물 체력
    private float targetHealth = 100f;    // 목표물 체력
    
    [Header("UI 요소, 나중에 변경")] 
    public TextMeshProUGUI textStatus;
    
    private GoalState currentGoal;
    
    // Start is called before the first frame update
    void Start()
    {
        // 델리게이트 연결. 추후 매니저로 가거나 바인딩으로 갈 수도 있음.
        playerMovement.OnPickupTarget += () => SetNewGoal(GoalState.MovingToFinalPoint);
        playerMovement.OnReachedDestination += HandleDestinationReached;
        
        // 초기 목표
        SetNewGoal(GoalState.MovingToTarget);
    }
    
    void Update()
    {
        if (playerHealth <= 0)
        {
            Debug.Log("플레이어가 죽었습니다. 패배.");
            return;
        }

        if (targetHealth <= 0)
        {
            Debug.Log("목표물이 죽었습니다. 구출 실패.");
            return;
        }
    }
    
    // 목적지 도착 시 처리
    private void HandleDestinationReached()
    { 
        if (currentGoal == GoalState.MovingToFinalPoint)
        {
            // 최종 목적지에 도착하면 완료 상태로 변경
            SetNewGoal(GoalState.Completed);
        }
    }
    
    // UI 텍스트 업데이트 함수
    private void UpdateStatusUI(string message)
    {
        if (textStatus != null)
        {
            textStatus.text = message;
        }
    }
    
    // 목표물 픽업 함수 (목표물에 도착했을 때 호출)
    public void PickUpTarget()
    {
        hasTarget = true;
        Debug.Log("목표물을 픽업했습니다.");
    }
    
    private void SetNewGoal(GoalState newGoal)
    {
        currentGoal = newGoal;
        switch (currentGoal)
        {
            case GoalState.MovingToTarget:
                playerMovement.SetDestination(target.transform.position);
                UpdateStatusUI("Current Goal : Take Target");
                break;
            
            case GoalState.MovingToFinalPoint:
                // TODO : Target 오브젝트를 어떻게 할것인지, 일단은 없애고 위에 아이콘으로 알려주기..
                target.SetActive(false);
                
                playerMovement.SetDestination(finalPoint.position);
                UpdateStatusUI("Current Goal : Move To Final Point");
                break;
            
            case GoalState.Completed:
                UpdateStatusUI("Mission Complete!");
                
                break;
        }
    }
    
} // end class
