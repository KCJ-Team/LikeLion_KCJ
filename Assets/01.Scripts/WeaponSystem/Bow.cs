using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour,IWeapon
{
    
    
    public void Attack()
    {
        Debug.Log("활로 공격!");
    }

    public float GetDamage()
    {
        return 8f;
    }

    public float GetAttackSpeed()
    {
        return 1.2f;
    }
}