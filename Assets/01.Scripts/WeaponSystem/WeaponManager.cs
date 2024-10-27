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
        switch (playerData.currentWeapon.weaponType)
        {
            case WeaponType.Sword:
                MeleeAttack();
                break;
            default:
                RangedAttack();
                break;
        }

        // 이펙트 생성
        if (playerData.currentWeapon.effectPrefab != null)
        {
            Instantiate(playerData.currentWeapon.effectPrefab, transform.position, transform.rotation);
        }
    }

    // 근접 공격
    private void MeleeAttack()
    {
        // 근접 공격 로직 구현
    }

    // 원거리 공격
    private void RangedAttack()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            var position = weaponHolder.position;
            Vector3 shootDirection = new Vector3(hit.point.x, position.y, hit.point.z);

            GameObject projectile = Instantiate(playerData.currentWeapon.projectilePrefab, position, Quaternion.LookRotation(shootDirection));
            
            projectile.GetComponent<Rigidbody>().velocity = shootDirection * 10f; // 발사체 속도 조정
        }
    }
}