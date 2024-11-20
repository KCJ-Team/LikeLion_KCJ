using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    private PlayerData _playerData;
    
    protected override void Start()
    {
        base.Start();
        _playerData = GameManager.Instance.playerData;
        maxHealth = _playerData.BaseHP;
    }
}