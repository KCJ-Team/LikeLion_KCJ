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
        /*// 마우스 위치를 월드 좌표로 변환
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        
        if (groundPlane.Raycast(ray, out float distance))
        {
            // 광선과 평면의 교차점 구하기
            Vector3 targetPoint = ray.GetPoint(distance);
            
            // 발사 방향 계산 (y축은 0으로 고정하여 수평 이동)
            Vector3 direction = targetPoint - transform.position;
            direction.y = 0; // y축 변화 제거
            direction.Normalize();
            
            // ProjectilePrefab 인스턴스화
            GameObject projectile = Instantiate(ProjectilePrefab, transform.position, Quaternion.LookRotation(direction));
            
            PullProjectile pr = projectile.GetComponent<PullProjectile>();
            
            if (projectile != null)
            {
                pr.Initialize(direction, damage);
            }
            
            // 일정 시간 후 발사체 제거
            Destroy(projectile, projectileLifetime);
        }*/
        
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