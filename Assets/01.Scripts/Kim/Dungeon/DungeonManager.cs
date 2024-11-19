using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : SceneSingleton<DungeonManager>
{
    [Header("Player Data")] 
    public PlayerData playerData;
    public CardDatabase cardDatabase;

    [Header("던전에서 관리할 플레이어의 정보들")]
    public float hp;
    public float attack;
    public float defense;

    [Header("UI MVP 패턴")] 
    [SerializeField] private DungeonUIView dungeonUIView;
    public DungeonUIPresenter dungeonUIPresenter;

    private void Start()
    {
        dungeonUIPresenter = new DungeonUIPresenter(dungeonUIView);
    }

    public void SetStats(float newHp, float newDefence, float newAttack)
    {
        if (dungeonUIPresenter == null)
        {
            dungeonUIPresenter = new DungeonUIPresenter(dungeonUIView);
        }

        dungeonUIPresenter.SetStats(newHp, newDefence, newAttack);
    }
}
