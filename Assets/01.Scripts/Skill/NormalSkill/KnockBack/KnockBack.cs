using UnityEngine;

public class KnockBack : Skill
{
    [SerializeField] private GameObject knockbackProjectilePrefab;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float knockbackForce = 10f;
    [SerializeField] private float projectileDamage = 10f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask targetLayers; // 충돌 대상 레이어 추가
    
    //private float currentCooldown = 0f;
    
    private void Update()
    {
        if (currentCooldown > 0)
        {
            currentCooldown -= Time.deltaTime;
        }
    }
    
    public void FireProjectile()
    {
        firePoint = GameManager.Instance.Player.transform;
        
        if (currentCooldown <= 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector3 targetPosition;
            
            if (Physics.Raycast(ray, out hit))
            {
                targetPosition = hit.point;
            }
            else
            {
                targetPosition = ray.GetPoint(100f);
            }
            
            Vector3 firePosition = firePoint != null ? firePoint.position : transform.position;
            Vector3 direction = (new Vector3(targetPosition.x, firePosition.y, targetPosition.z) - firePosition).normalized;
            
            GameObject projectileObj = Instantiate(knockbackProjectilePrefab, firePosition, Quaternion.LookRotation(direction));
            KnockbackProjectile projectile = projectileObj.GetComponent<KnockbackProjectile>();
            
            if (projectile != null)
            {
                projectile.SetSpeed(projectileSpeed);
                projectile.SetKnockbackForce(knockbackForce);
                projectile.SetTargetLayers(targetLayers);
                projectile.Initialize(direction, projectileDamage);
            }
            
            currentCooldown = cooldown;
        }
    }
    
    public bool CanUseSkill()
    {
        return currentCooldown <= 0;
    }
    
    public override SkillState GetInitialState()
    {
        return new KnockBackFiringState(this);
    }
}