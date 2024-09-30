using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    public void Attack()
    {
        Debug.Log("검으로 공격!");
    }

    public float GetDamage()
    {
        return 10f;
    }

    public float GetAttackSpeed()
    {
        return 1.5f;
    }
}
