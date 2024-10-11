using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponGrade
{
    None,
    Normal,
    Epic,
    Legend
}

public class WeaponCard : Card
{
    public Weapon weapon;
    public GameObject weaponPrefab;
    public WeaponGrade weaponGrade;
    
    //등급을 랜덤으로 주어지는 메소드 추가
}