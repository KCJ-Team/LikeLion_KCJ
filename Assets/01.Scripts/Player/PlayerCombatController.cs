using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController
{
    private readonly PlayerCharacterController _player;
    private readonly WeaponManager _weaponManager;
    private WeaponType _currentWeaponType = WeaponType.None;

    public PlayerCombatController(PlayerCharacterController player, WeaponManager weaponManager)
    {
        _player = player;
        _weaponManager = weaponManager;
    }

    public void HandleCombat()
    {
        UpdateWeaponAnimation();
        HandleAttack();
    }

    public void UpdateWeaponAnimation()
    {
        _currentWeaponType = _player.playerData.currentWeapon?.weaponType ?? WeaponType.None;
        _player.GetComponent<Animator>().SetInteger("WeaponType", (int)_currentWeaponType);
    }

    private void HandleAttack()
    {
        if (Input.GetMouseButton(1) && Input.GetMouseButton(0))
        {
            //_weaponManager.Attack();
        }
    }
}
