using System;
using UnityEngine;

public class Grab : Skill
{
    [SerializeField] private GameObject pullProjectilePrefab;
    [SerializeField] private float projectileSpeed = 20f;
    [SerializeField] private float pullForce = 40f;
    [SerializeField] private float projectileDamage = 10f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private float pullToDistance = 3f; // 플레이어 앞으로 당겨올 거리

    private void Awake()
    {
        firePoint = GameManager.Instance.Player.transform;
    }

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
            
            GameObject projectileObj = Instantiate(pullProjectilePrefab, firePosition, Quaternion.LookRotation(direction));
            PullProjectile projectile = projectileObj.GetComponent<PullProjectile>();
            
            if (projectile != null)
            {
                projectile.SetSpeed(projectileSpeed);
                projectile.SetPullForce(pullForce);
                projectile.SetPullToDistance(pullToDistance);
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
        return new GrabFiringState(this);
    }
}