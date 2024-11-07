using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI요소와 플레이어 정보 업데이트 스크립트
public class UIPresenter : MonoBehaviour
{
    private PlayerInfo.PlayerInfo _playerInfo;
    private UIView _uiView;

    public UIPresenter(PlayerInfo.PlayerInfo playerInfo, UIView uiView)
    {
        _playerInfo = playerInfo;
        _uiView = uiView;
        UpdateView();
    }
    
    public void UpdateView()
    {
        _uiView.UpdateUI(_playerInfo.PlayerId);
    }

    public void UpdateName(string name)
    {
        _playerInfo.PlayerId = name;
        UpdateView();
    }
}