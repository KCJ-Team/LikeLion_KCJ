using UnityEngine;

public class KnockBack : Skill
{
    [SerializeField] private GameObject knockbackProjectilePrefab;
    [SerializeField] private float projectileLifetime = 5f;
    private Transform firePoint;
    
    private void Start()
    {
        firePoint = GameManager.Instance.Player.GetComponent<WeaponManager>().firePoint;
    }
    
    public void FireProjectile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Vector3 targetPosition;
            
        if (Physics.Raycast(ray, out hit))
        {
            targetPosition = new Vector3(hit.point.x, firePoint.position.y, hit.point.z);
        }
        else
        {
            targetPosition = ray.GetPoint(1000f);
            targetPosition.y = firePoint.position.y;
        }
            
        Vector3 direction = (targetPosition - firePoint.position).normalized;
            
        GameObject projectileObj = Instantiate(knockbackProjectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
            
        KnockbackProjectile projectile = projectileObj.GetComponent<KnockbackProjectile>();
            
        if (projectile != null)
        {
            projectile.Initialize(direction, damage);
        }
            
        Destroy(projectile, projectileLifetime);
    }
    
    public override SkillState GetInitialState()
    {
        return new KnockBackFiringState(this);
    }
}