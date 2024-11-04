using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour
{
    public PlayerData playerData;
    public Transform weaponHolder; // 무기를 부착할 위치
    
    private GameObject currentWeaponObject;
    private bool isFiring = false;

    private void Start()
    {
        UpdateWeapon();
    }

    // 무기 업데이트 메서드
    public void UpdateWeapon()
    {
        if (currentWeaponObject != null)
        {
            Destroy(currentWeaponObject);
        }

        if (playerData.currentWeapon != null)
        {
            currentWeaponObject = Instantiate(playerData.currentWeapon.characterDisplay, weaponHolder);
        }
    }

    // 공격 메서드
    public void Attack()
    {
        if (playerData.currentWeapon == null) return;

        if (!isFiring)
        {
            StartCoroutine(FireRoutine());
        }
    }

    private IEnumerator FireRoutine()
    {
        isFiring = true;

        while (Input.GetMouseButton(0)) // 마우스 버튼을 누르고 있는 동안 계속 발사
        {
            FireWeapon();
            yield return new WaitForSeconds(1f / playerData.currentWeapon.attackSpeed);
        }

        isFiring = false;
    }

    private void FireWeapon()
    {
        RangedAttack();

        // 이펙트 생성
        // if (playerData.currentWeapon.effectPrefab != null)
        // {
        //     Instantiate(playerData.currentWeapon.effectPrefab, transform.position, transform.rotation);
        // }
    }

    // 원거리 공격
    private void RangedAttack()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
    
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 shootDirection = new Vector3(hit.point.x, weaponHolder.position.y, hit.point.z) - weaponHolder.position;

            GameObject projectile = Instantiate(playerData.currentWeapon.projectilePrefab, weaponHolder.position, Quaternion.LookRotation(shootDirection));
        
            projectile.GetComponent<Rigidbody>().velocity = shootDirection * 10f; // 발사체 속도 조정
        }
    }
}