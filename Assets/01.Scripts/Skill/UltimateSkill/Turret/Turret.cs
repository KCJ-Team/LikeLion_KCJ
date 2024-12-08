using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Skill
{
    [SerializeField] private GameObject turretPrefab;
    public float spawnRadius;
    private GameObject currentTurret;
    
    private void Start()
    {
        spawnRadius = GameManager.Instance.playerData.currentWeapon.attackRange;
    }
    
    public void TurretInstall()
    {
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(0f, spawnRadius);
        
        float angleRad = angle * Mathf.Deg2Rad;
        
        Vector2 spawnPosition = new Vector2(
            transform.position.x + Mathf.Cos(angleRad) * distance,
            transform.position.y + Mathf.Sin(angleRad) * distance
        );
        
        currentTurret = Instantiate(turretPrefab, spawnPosition, Quaternion.identity);
        //SoundManager.Instance.PlaySFX(SFXSoundType.Skill_Mine);
        
        Vector3 direction = Vector3.zero;
        
        Projectile pr = currentTurret.GetComponent<Projectile>();
    
        if (pr != null)
        {
            pr.Initialize(direction, damage);
        }
        
        Destroy(gameObject);
    }
    
    public override SkillState GetInitialState()
    {
        return new TurretInstallState(this);
    }
}