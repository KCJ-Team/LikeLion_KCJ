using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DungeonManager : SceneSingleton<DungeonManager>
{
    [Header("Player Data")] 
    public PlayerData playerData;
    public CardDatabase cardDatabase;
    private WeaponManager _weaponManager;
    private PlayerCharacterController _characterController;
    public Spawner _Spawner;

    [Header("던전에서 관리할 플레이어의 정보들")]
    public float hp;
    public float attack;
    public float defense;
    public bool isPossibleLegendWeapon; // 연구실에서 고급 무기 보상을 연구했다면,

    [Header("UI MVP 패턴")] 
    [SerializeField] 
    public DungeonUIView dungeonUIView;
    public DungeonUIPresenter dungeonUIPresenter;

    private void Start()
    {
        _weaponManager = GameManager.Instance.Player.GetComponent<WeaponManager>();
        _characterController = GameManager.Instance.Player.GetComponent<PlayerCharacterController>();
        dungeonUIPresenter = new DungeonUIPresenter(dungeonUIView);
    }

    public void Update()
    {
        UpdateAmmo(_weaponManager.GetCurrentAmmo(),_weaponManager.GetMaxAmmo());
        UpdateRemainingMonsters(_Spawner.RemainingMonsters);
        SetBossSpawned(_Spawner.IsBossSpawned);

        SetSkillCooldownBySlotNumber(0,_characterController._skillController.GetSkillCooldown(KeyCode.Q));
        SetSkillCooldownBySlotNumber(1,_characterController._skillController.GetSkillCooldown(KeyCode.E));
        SetSkillCooldownBySlotNumber(2,_characterController._skillController.GetSkillCooldown(KeyCode.F));
        
    }

    public void SetStats(float newHp, float newDefence, float newAttack)
    {
        if (dungeonUIPresenter == null)
        {
            dungeonUIPresenter = new DungeonUIPresenter(dungeonUIView);
        }

        dungeonUIPresenter.SetStats(newHp, newDefence, newAttack);
    }
    
    public void UpdateAmmo(int currentAmmo, int maxAmmo)
    {
        dungeonUIPresenter.SetAmmo(currentAmmo, maxAmmo);
    }

    public void UpdateRemainingMonsters(int remaining)
    {
        dungeonUIPresenter.SetRemainingMonsters(remaining);
    }

    public void SetBossSpawned(bool isSpawned)
    {
        dungeonUIPresenter.SetBossSpawned(isSpawned);
    }

    public void SetSkillCooldownBySlotNumber(int slotNum, float coolDown)
    {
        dungeonUIPresenter.SetSkillCooldownBySlotNumber(slotNum, coolDown);
    }
}
