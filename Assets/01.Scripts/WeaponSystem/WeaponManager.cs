using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class WeaponManager : MonoBehaviour
{
    public PlayerData playerData;
    public Transform RifleHolder;
    public Transform PistolHolder;
    public Transform ShotgunHolder;
    public Transform SniperHolder;
    
    private GameObject currentWeaponObject;
    private bool isFiring = false;
    private bool isReloading = false;
    private Transform currentWeaponHolder;
    public Transform firePoint;
    
    private Animator _animator;
    private int currentAmmo; // 현재 탄약
    
    private string reloadLayerName = "Reload"; // 리로드 애니메이션이 있는 레이어 이름
    private int reloadLayerIndex; // 레이어 인덱스를 캐시
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        reloadLayerIndex = _animator.GetLayerIndex(reloadLayerName);
    }

    private void Start()
    {
        UpdateWeapon();
        
        if (reloadLayerIndex != -1)
        {
            _animator.SetLayerWeight(reloadLayerIndex, 0f);
        }
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
            
            if (currentWeaponHolder.childCount > 0)
            {
                firePoint = currentWeaponHolder.GetChild(0);
            }
            else
            {
                firePoint = currentWeaponHolder;
                Debug.LogWarning($"Weapon holder {currentWeaponHolder.name} has no child objects!");
            }
            
            currentWeaponObject = Instantiate(playerData.currentWeapon.characterDisplay, currentWeaponHolder);
            currentAmmo = playerData.currentWeapon.Ammo; // 무기 교체시 탄약 초기화
        }
    }

    public void Attack()
    {
        if (playerData.currentWeapon == null || isReloading) return;

        if (!isFiring && currentAmmo > 0)
        {
            StartCoroutine(FireRoutine());
        }
        else if (currentAmmo <= 0)
        {
            StartReload(); // 탄약이 없으면 자동 장전
        }
    }

    private IEnumerator FireRoutine()
    {
        isFiring = true;

        while (Input.GetMouseButton(0) && currentAmmo > 0 && !isReloading)
        {
            FireWeapon();
            currentAmmo--;
            yield return new WaitForSeconds(1f / playerData.currentWeapon.attackSpeed);
        }

        isFiring = false;
    }

    public void StartReload()
    {
        if (!isReloading && currentAmmo < playerData.currentWeapon.Ammo)
        {
            StartCoroutine(ReloadRoutine());
        }
    }

    private void Update()
    {
        // R키를 누르면 수동 재장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartReload();
        }
        
        _animator.SetBool("IsReload",isReloading);
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

    // UI 표시용 현재 탄약 정보 가져오기
    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int GetMaxAmmo()
    {
        return playerData.currentWeapon?.Ammo ?? 0;
    }

    public bool IsReloading()
    {
        return isReloading;
    }
    
    private IEnumerator ReloadRoutine()
    {
        isReloading = true;
        
        // 리로드 레이어 활성화
        if (reloadLayerIndex != -1)
        {
            _animator.SetLayerWeight(reloadLayerIndex, 1f);
        }
        
        _animator.SetBool("IsReload", true);
        
        // 재장전 시간
        float reloadTime = 2f;
        yield return new WaitForSeconds(reloadTime);
        
        // 리로드 레이어 비활성화
        if (reloadLayerIndex != -1)
        {
            _animator.SetLayerWeight(reloadLayerIndex, 0f);
        }
        
        _animator.SetBool("IsReload", false);
        currentAmmo = playerData.currentWeapon.Ammo;
        isReloading = false;
    }
}