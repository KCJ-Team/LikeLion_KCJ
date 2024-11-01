using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UI 조작 관련 스크립트
public class UIManager : MonoBehaviour
{
    public UIView _UIView;
    private UIPresenter _uiPresenter;
    public PlayerInfo.PlayerInfo _playerInfo;

    private void Start()
    {
        _uiPresenter = new UIPresenter(_playerInfo, _UIView);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _uiPresenter.UpdateName("Pressed k");
        }
    }
}
