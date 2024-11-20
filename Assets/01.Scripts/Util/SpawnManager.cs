using System;
using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("MonsterSpawn Settings")] 
    public Transform playerSpawnPoint;
    
    [Header("MonsterSpawn Settings")]
    public Spawner spawner;
    public int monstersPerWave = 5;    // 한 번에 생성할 몬스터 수
    public float spawnInterval = 10f;   // 생성 간격 (초)

    [Header("UI")] 
    public GameObject dungeonClearPanel;
    
    [Header("Rewards")]
    public CardObject[] possibleWeapons; 
    public CardObject[] possibleLegendWeapons; 

    private void Awake()
    {
        //지정 위치에 플레이어 생성
        GameManager.Instance.Player = Instantiate(GameManager.Instance.playerData.Character, playerSpawnPoint.position, Quaternion.identity);
            
        // 몬스터 생성 코루틴 시작
        StartCoroutine(SpawnMonsterRoutine());
    }

    private void Update()
    {
        if (spawner.IsBossSpawned && spawner.BossMonster == null)
        {
            if (DungeonManager.Instance.isPossibleLegendWeapon)
            {
                DungeonLegendClear();
            }
            else
            {
                DungeonClear();
            }
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

            GameSceneDataManager.Instance.DungeonClearReward();
        }
    }
    
    // hyuna. 레전드 보상 무기
    private void DungeonLegendClear()
    {
        dungeonClearPanel.SetActive(true);
        
        if (possibleLegendWeapons != null && possibleLegendWeapons.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, possibleLegendWeapons.Length);
            CardObject rewardWeapon = possibleLegendWeapons[randomIndex];
        
            // 인벤토리에 무기 추가
            GameManager.Instance.playerData.inventory.AddItem(new Card(rewardWeapon), 1);
            
            GameSceneDataManager.Instance.DungeonClearReward();
            
            Debug.Log("고급 무기 여부 확인, 고급 무기 제공");
        }
    }
}