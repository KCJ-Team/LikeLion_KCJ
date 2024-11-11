using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pull : Skill
{
    [SerializeField] private GameObject ProjectilePrefab;
    [SerializeField] private float projectileLifetime = 5f;
    private Transform firePoint;

    private void Start()
    {
        firePoint = GameManager.Instance.Player.GetComponent<WeaponManager>().firePoint;
    }

    public void ShootProjectile()
    {
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
        
        GameObject projectile = Instantiate(ProjectilePrefab, firePoint.position, Quaternion.LookRotation(direction));
        
        PullProjectile pr = projectile.GetComponent<PullProjectile>();
            
        if (projectile != null)
        {
            pr.Initialize(direction, damage);
        }
        
        Destroy(projectile, projectileLifetime);
    }
    
    public override SkillState GetInitialState()
    {
        return new PullFiringState(this);
    }
}