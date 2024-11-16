using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 미사일 발사 전 조준 상태를 구현한 클래스
/// 마우스로 범위를 지정하고 Q키를 한번 더 누르면 발사 상태로 전환
/// </summary>
public class MissilePreparingState : SkillState
{
    private MissileBombing _missileBombing;
    private GameObject rangeIndicator;        // 스킬 범위 표시기
    private Vector3 targetPosition;           // 현재 조준 위치
    private LayerMask groundLayer;           // 지면 레이어
    
    public MissilePreparingState(Skill skill) : base(skill)
    {
        _missileBombing = skill as MissileBombing;
        groundLayer = LayerMask.GetMask("Ground");
    }
    
    // 상태 시작 시 범위 표시기 생성
    public override void EnterState()
    {
        // 범위 표시기 프리팹 로드 및 생성
        GameObject indicatorPrefab = Resources.Load<GameObject>("SkillRangeIndicator");
        rangeIndicator = GameObject.Instantiate(indicatorPrefab);
        rangeIndicator.transform.localScale = new Vector3(1f, 1f, 1f);
        rangeIndicator.SetActive(true);
    }
    
    // 마우스 위치에 따라 범위 표시기 업데이트 및 Q키 입력 감지
    public override void UpdateState()
    {
        // 마우스 위치로 레이캐스트하여 지면 위치 확인
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            targetPosition = hit.point;
            rangeIndicator.transform.position = targetPosition;
            
            // Q키를 다시 누르면 발사 상태로 전환
            if (Input.GetMouseButtonDown(0))
            {
                // 타겟 위치를 MissileFiringState에 전달하기 위해 생성자 수정
                skill.ChangeState(new MissileFiringState(skill, targetPosition));
            }
        }
        
        // ESC키를 누르면 스킬 취소
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelSkill();
        }
    }
    
    // 범위 표시기 제거
    public override void ExitState()
    {
        if (rangeIndicator != null)
        {
            GameObject.Destroy(rangeIndicator);
        }
    }
    
    // 스킬 취소
    private void CancelSkill()
    {
        // 범위 표시기 제거
        if (rangeIndicator != null)
        {
            GameObject.Destroy(rangeIndicator);
        }
        
        // 기본 상태로 돌아가기
        skill.ChangeState(skill.GetInitialState());
    }
}