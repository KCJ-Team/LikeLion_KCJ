using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinePreparingState : SkillState
{
    private Mine _mine;
    private GameObject rangeIndicator;
    private Vector3 targetPosition;
    private LayerMask groundLayer;
    
    public MinePreparingState(Skill skill) : base(skill)
    {
        _mine = skill as Mine;
        groundLayer = LayerMask.GetMask("Ground");
    }

    public override void EnterState()
    {
        GameObject indicatorPrefab = Resources.Load<GameObject>("SkillRangeIndicator");
        rangeIndicator = GameObject.Instantiate(indicatorPrefab);
        rangeIndicator.transform.localScale = new Vector3(1f, 1f, 1f);
        rangeIndicator.SetActive(true);
    }

    public override void UpdateState()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            targetPosition = hit.point;
            rangeIndicator.transform.position = targetPosition;
            
            if (Input.GetMouseButtonDown(0))
            {
                skill.ChangeState(new MineInstallState(skill, targetPosition));
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelSkill();
        }
    }

    public override void ExitState()
    {
        if (rangeIndicator != null)
        {
            GameObject.Destroy(rangeIndicator);
        }
    }
    
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
