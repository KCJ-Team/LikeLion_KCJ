using System;
using System.Collections;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    public Spawner spawner;
    public int monstersPerWave = 5;    // 한 번에 생성할 몬스터 수
    public float spawnInterval = 10f;   // 생성 간격 (초)
    
    [Header("UI")]
    public GameObject dungeonClearPanel;  // 던전 클리어 UI 패널

    [Header("Rewards")]
    public CardObject[] possibleWeapons; 

    private void Start()
    {
        // 몬스터 생성 코루틴 시작
        StartCoroutine(SpawnMonsterRoutine());
    }

    private void Update()
    {
        if (spawner.IsBossSpawned && spawner.BossMonster == null)
        {
            DungeonClear();
        }
    }

    private IEnumerator SpawnMonsterRoutine()
    {
        while (!spawner.IsBossSpawned)  // 보스가 생성되지 않은 동안 반복
        {
            // 한 번에 5마리씩 생성
            for (int i = 0; i < monstersPerWave; i++)
            {
                // 아직 생성할 몬스터가 남아있다면 생성
                if (spawner.RemainingMonsters > 0)
                {
                    spawner.SpawnMonster();
                }
            }

            // 10초 대기
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void DungeonClear()
    {
        dungeonClearPanel.SetActive(true);
        
        if (possibleWeapons != null && possibleWeapons.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, possibleWeapons.Length);
            CardObject rewardWeapon = possibleWeapons[randomIndex];
        
            // 인벤토리에 무기 추가
            GameManager.Instance.playerData.inventory.AddItem(new Card(rewardWeapon), 1);
        }
    }
}