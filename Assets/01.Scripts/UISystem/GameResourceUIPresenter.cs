using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

//UI요소와 플레이어 정보 업데이트 스크립트
public class GameResourceUIPresenter
{
    private GameResourceUIView uiView;

    public GameResourceUIPresenter(GameResourceUIView uiView)
    {
        this.uiView = uiView;
    }
    
    public void UpdateResourceUI()
    {
        // GameResourceManager에서 각 자원의 양을 가져와 UI 업데이트
        uiView.UpdateResourceUI(
            GameResourceManager.Instance.GetResourceAmount(ResourceType.Energy),
            GameResourceManager.Instance.GetResourceAmount(ResourceType.Food),
            GameResourceManager.Instance.GetResourceAmount(ResourceType.Workforce),
            GameResourceManager.Instance.GetResourceAmount(ResourceType.Fuel),
            GameResourceManager.Instance.GetResourceAmount(ResourceType.Research),
            GameResourceManager.Instance.GetResourceAmount(ResourceType.Currency)
        );
    }

    public void ShowIconConsumeAt9PM()
    {
        uiView.ShowIconConsumeAt9PM();
    }

    public void WarningWorkforceResourceUI()
    {
        // 빨간색 강조 효과 추가 (예: DOTween으로 애니메이션 적용 가능)
        uiView.workforceText.color = Color.red;
        DOTween.Sequence()
            .Append(uiView.workforceText.DOColor(Color.red, 0.2f)) // 빨간색으로 변경
            .Append(uiView.workforceText.DOColor(Color.white, 0.2f)) // 흰색으로 복원
            .SetLoops(5, LoopType.Yoyo) // 5회 반복 (빨간색 -> 흰색 반복)
            .OnComplete(() =>
            {
                uiView.workforceText.color = Color.white; // 최종적으로 흰색으로 설정
            });
    }
} // end class