using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShadowClone : Skill
{
    private GameObject player;
    private GameObject clone;
    public float spawnRadius = 5f;

    private void Awake()
    {
        player = GameManager.Instance.Player;
    }

    private void Start()
    {
        spawnRadius = GameManager.Instance.playerData.currentWeapon.attackRange;
    }
    
    public void SpawnClone()
    {
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(0f, spawnRadius);
        
        float angleRad = angle * Mathf.Deg2Rad;
        
        Vector2 spawnPosition = new Vector2(
            transform.position.x + Mathf.Cos(angleRad) * distance,
            transform.position.y + Mathf.Sin(angleRad) * distance
        );
        
        clone = Instantiate(player, spawnPosition, Quaternion.identity);
        DestroyComponents();
        
        Destroy(gameObject);
    }

    private void DestroyComponents()
    {
        Destroy(clone.GetComponent<PlayerController>());
        Destroy(clone.GetComponent<PlayerSkillController>());
        Destroy(clone.GetComponent<PlayerDecteting>());
    }
    
    public override SkillState GetInitialState()
    {
        return new ShadowCloneState(this);
    }
}