using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private IWeapon currentWeapon;
    
    void Start()
    {
        // 기본 무기를 검으로 설정
        currentWeapon = new Sword();
    }

    void Update()
    {
        // 예: 스페이스바를 누르면 공격
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }

        // 무기 변경 예시
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeWeapon(new Sword());
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeWeapon(new Bow());
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeWeapon(new Pistol());
    }

    void Attack()
    {
        currentWeapon.Attack();
        Debug.Log($"데미지: {currentWeapon.GetDamage()}, 공격속도: {currentWeapon.GetAttackSpeed()}");
    }

    void ChangeWeapon(IWeapon newWeapon)
    {
        currentWeapon = newWeapon;
        Debug.Log($"무기를 {newWeapon.GetType().Name}으로 변경했습니다.");
    }
}
