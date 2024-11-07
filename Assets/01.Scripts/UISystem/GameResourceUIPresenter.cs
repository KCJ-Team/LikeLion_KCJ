using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI요소와 플레이어 정보 업데이트 스크립트
public class GameResourceUIPresenter
{
    private PlayerInfo.PlayerInfo playerInfo;
    private GameResourceUIView uiView;

    public GameResourceUIPresenter(GameResourceUIView uiView)
    {
        this.uiView = uiView;
        UpdateView();
    }
    
    public void UpdateView()
    {
        //uiView.UpdateUI(playerInfo.PlayerId);
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
   
} // end class