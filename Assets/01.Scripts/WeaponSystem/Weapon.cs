using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    None,
    Melee,
    Range
}

public enum WeaponType
{
    Sword,
    Pistol,
    Rifle,
    SniperRifle,
    RocketLauncher,
    Shotgun
}

[CreateAssetMenu(fileName = "New Weapon", menuName = "SO/Weapon")]
public class Weapon : ScriptableObject
{
    public AttackType attackType;
    public WeaponType weaponType;

    public float damage;
    public float AttackDistance;
    public float AttackSpeed;
    public float Magazine;
}