using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 로비메뉴, 로비에서 관리하는 정보들 매니저
/// </summary>
public class LobbyMenuManager : SceneSingleton<LobbyMenuManager>
{
    [Header("SO Data..")] 
    public PlayerData playerData;
    public CardDatabase cardDatabase;
    public StoreData storeData;

    [Header("로비에서 관리할 플레이어의 정보들")]
    public float hp;
    public float stress;
    public float attack;
    public float defense;

    [Header("UI MVP 패턴")] 
    [SerializeField] private LobbyMenuUIView lobbyMenuUIView;
    public LobbyMenuUIPresenter lobbyMenuUIPresenter;
    
    private void Start()
    {
        lobbyMenuUIPresenter = new LobbyMenuUIPresenter(lobbyMenuUIView);
    }
    
    public void SetHpAndStress(float newHp, float newStress)
    {
        if (lobbyMenuUIPresenter == null)
        {
            lobbyMenuUIPresenter = new LobbyMenuUIPresenter(lobbyMenuUIView);
        }
        
        lobbyMenuUIPresenter.SetHpAndStress(newHp, newStress);
    }
    
    public void ChangeHp(float amount)
    {
        lobbyMenuUIPresenter.ChangeHp(amount);
    }

    public void ChangeStress(float amount)
    {
        lobbyMenuUIPresenter.ChangeStress(amount);
    }

    public void SetAttackAndDefense(float newAttack, float newDefense)
    {
        lobbyMenuUIPresenter.SetAttackAndDefense(newAttack, newDefense);
    }
    
} // end class