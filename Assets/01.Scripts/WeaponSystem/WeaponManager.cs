using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour
{
    public PlayerData playerData;
    public Transform RifleHolder;
    public Transform PistolHolder;
    public Transform ShotgunHolder;
    public Transform SniperHolder;
    
    private GameObject currentWeaponObject;
    private bool isFiring = false;
    private Transform currentWeaponHolder;
    public Transform firePoint; // 발사 위치를 저장할 변수
    
    private void Start()
    {
        UpdateWeapon();
    }

    private Transform GetWeaponHolder(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.Rifle:
                return RifleHolder;
            case WeaponType.Pistol:
                return PistolHolder;
            case WeaponType.ShotGun:
                return ShotgunHolder;
            case WeaponType.SniperRifle:
                return SniperHolder;
            default:
                return RifleHolder;
        }
    }

    public void UpdateWeapon()
    {
        if (currentWeaponObject != null)
        {
            Destroy(currentWeaponObject);
        }

        if (playerData.currentWeapon != null)
        {
            currentWeaponHolder = GetWeaponHolder(playerData.currentWeapon.weaponType);
            
            // 홀더의 첫 번째 자식 오브젝트를 firePoint로 설정
            if (currentWeaponHolder.childCount > 0)
            {
                firePoint = currentWeaponHolder.GetChild(0);
            }
            else
            {
                firePoint = currentWeaponHolder; // 자식이 없으면 홀더 자체를 사용
                Debug.LogWarning($"Weapon holder {currentWeaponHolder.name} has no child objects!");
            }
            
            currentWeaponObject = Instantiate(playerData.currentWeapon.characterDisplay, currentWeaponHolder);
        }
    }

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

        while (Input.GetMouseButton(0))
        {
            FireWeapon();
            yield return new WaitForSeconds(1f / playerData.currentWeapon.attackSpeed);
        }

        isFiring = false;
    }

    private void FireWeapon()
    {
        if (playerData.currentWeapon.projectilePrefab == null || firePoint == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 targetPoint;
        
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = new Vector3(hit.point.x, firePoint.position.y, hit.point.z);
        }
        else
        {
            targetPoint = ray.GetPoint(1000f);
            targetPoint.y = firePoint.position.y;
        }

        Vector3 direction = (targetPoint - firePoint.position).normalized;
        
        if (playerData.currentWeapon.weaponType == WeaponType.ShotGun)
        {
            FireShotgunProjectiles(direction);
        }
        else
        {
            FireSingleProjectile(direction);
        }
    }
    
    private void FireSingleProjectile(Vector3 shootDirection)
    {
        GameObject projectileObj = Instantiate(playerData.currentWeapon.projectilePrefab, firePoint.position, Quaternion.LookRotation(shootDirection));
            
        if (projectileObj.TryGetComponent<StandardProjectile>(out StandardProjectile projectile))
        {
            float finalDamage = playerData.currentWeapon.damage + playerData.AttackPower;
            projectile.Initialize(shootDirection, finalDamage);
        }
    }
    
    private void FireShotgunProjectiles(Vector3 baseDirection)
    {
        int pelletCount = 8;
        float spreadAngle = 20f;
        float damagePerPellet = (playerData.currentWeapon.damage + playerData.AttackPower) / pelletCount;
    
        for (int i = 0; i < pelletCount; i++)
        {
            float randomAngle = Random.Range(-spreadAngle / 2, spreadAngle / 2);
            Vector3 spreadDirection = Quaternion.Euler(0, randomAngle, 0) * baseDirection;
            
            GameObject projectileObj = Instantiate(playerData.currentWeapon.projectilePrefab, firePoint.position, Quaternion.LookRotation(spreadDirection));
                
            if (projectileObj.TryGetComponent<ShotgunPellet>(out ShotgunPellet pellet))
            {
                pellet.Initialize(spreadDirection, damagePerPellet);
            }
        }
    }
}