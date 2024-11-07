using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    private PlayerData _playerData;

    private void Awake()
    {
        _playerData = GameManager.Instance.playerData;
    }

    protected override void Start()
    {
        maxHealth = _playerData.BaseHP;
        base.Start();
    }
}