using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : Skill
{
    [SerializeField] private GameObject MinePrefab;
    public float spawnRadius;
    private GameObject currentMine;

    private void Start()
    {
        spawnRadius = GameManager.Instance.playerData.currentWeapon.attackRange;
    }
    
    public void MineInstall()
    {
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(0f, spawnRadius);
        
        float angleRad = angle * Mathf.Deg2Rad;
        
        Vector2 spawnPosition = new Vector2(
            transform.position.x + Mathf.Cos(angleRad) * distance,
            transform.position.y + Mathf.Sin(angleRad) * distance
        );
        
        currentMine = Instantiate(MinePrefab, spawnPosition, Quaternion.identity);
        //SoundManager.Instance.PlaySFX(SFXSoundType.Skill_Mine);
        
        Vector3 direction = Vector3.zero;
        
        Projectile pr = currentMine.GetComponent<Projectile>();
    
        if (pr != null)
        {
            pr.Initialize(direction, damage);
        }
        
        Destroy(gameObject);
    }
    
    public override SkillState GetInitialState()
    {
        return new MineInstallState(this);
    }
}
