using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour, IWeapon
{
    public void Attack()
    {
        Debug.Log("권총으로 공격!");
    }

    public float GetDamage()
    {
        return 15f;
    }

    public float GetAttackSpeed()
    {
        return 2f;
    }
}
