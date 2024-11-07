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
    
    private LayerMask enemyLayer; // Enemy 레이어 마스크

    private void Start()
    {
        // Enemy 레이어의 마스크 설정
        enemyLayer = LayerMask.GetMask("Enemy");
        UpdateWeapon();
    }

    // 무기 타입에 따른 홀더 가져오기
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
                return RifleHolder; // 기본값
        }
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
            // 무기 타입에 따른 홀더 설정
            currentWeaponHolder = GetWeaponHolder(playerData.currentWeapon.weaponType);
            
            // 해당 홀더 위치에 무기 생성
            currentWeaponObject = Instantiate(playerData.currentWeapon.characterDisplay, currentWeaponHolder);
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
    }

    // 원거리 공격
    private void RangedAttack()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 targetPoint;
        
        // 레이가 어떤 물체에 부딪히든 말든 방향을 구함
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = new Vector3(hit.point.x, currentWeaponHolder.position.y, hit.point.z);
        }
        else
        {
            targetPoint = ray.GetPoint(1000f);
            targetPoint.y = currentWeaponHolder.position.y;
        }

        Vector3 direction = (targetPoint - currentWeaponHolder.position).normalized;
        
        if (playerData.currentWeapon.weaponType == WeaponType.ShotGun)
        {
            FireShotgunPattern(direction);
        }
        else
        {
            FireSingleRay(direction);
        }
    }
    
    private void FireSingleRay(Vector3 shootDirection)
    {
        RaycastHit weaponHit;
        if (Physics.Raycast(currentWeaponHolder.position, shootDirection, out weaponHit, playerData.currentWeapon.range, enemyLayer))
        {
            Debug.DrawLine(currentWeaponHolder.position, weaponHit.point, Color.red, 0.5f);
            
            // 데미지 처리
            if (weaponHit.collider.TryGetComponent<IDamageable>(out IDamageable target))
            {
                float finalDamage = playerData.currentWeapon.damage + playerData.AttackPower;
                target.TakeDamage(finalDamage);
            }
        }
        else
        {
            Vector3 endPoint = currentWeaponHolder.position + shootDirection * playerData.currentWeapon.range;
            Debug.DrawLine(currentWeaponHolder.position, endPoint, Color.green, 0.5f);
        }
    }
    
    private void FireShotgunPattern(Vector3 baseDirection)
    {
        int pelletCount = 8;
        float spreadAngle = 20f;
        float damagePerPellet = (playerData.currentWeapon.damage + playerData.AttackPower) / pelletCount; // 총 데미지를 펠릿 수로 나눔
    
        for (int i = 0; i < pelletCount; i++)
        {
            float randomAngle = Random.Range(-spreadAngle / 2, spreadAngle / 2);
            Vector3 spreadDirection = Quaternion.Euler(0, randomAngle, 0) * baseDirection;
        
            RaycastHit weaponHit;
            if (Physics.Raycast(currentWeaponHolder.position, spreadDirection, out weaponHit, playerData.currentWeapon.range, enemyLayer))
            {
                Debug.DrawLine(currentWeaponHolder.position, weaponHit.point, Color.red, 0.5f);
                
                // 데미지 처리
                if (weaponHit.collider.TryGetComponent<IDamageable>(out IDamageable target))
                {
                    target.TakeDamage(damagePerPellet);
                }
            }
            else
            {
                Vector3 endPoint = currentWeaponHolder.position + spreadDirection * playerData.currentWeapon.range;
                Debug.DrawLine(currentWeaponHolder.position, endPoint, Color.green, 0.5f);
            }
        }
    }
}