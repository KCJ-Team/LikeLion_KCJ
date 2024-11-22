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
    
    private bool isDungeonCleared = false; // 던전 클리어 상태를 확인하는 플래그

    private void Awake()
    {
        //지정 위치에 플레이어 생성
        GameManager.Instance.Player = Instantiate(GameManager.Instance.playerData.Character, playerSpawnPoint.position, Quaternion.identity);
            
        // 몬스터 생성 코루틴 시작
        StartCoroutine(SpawnMonsterRoutine());
    }

    private void Update()
    {
        if (spawner.IsBossSpawned && spawner.BossMonster == null && !isDungeonCleared)
        {
            if (DungeonManager.Instance.isPossibleLegendWeapon)
            {
                DungeonLegendClear();
            }
            else
            {
                DungeonClear();
            }
            
            isDungeonCleared = true; // 던전 클리어 처리 중인 상태로 변경

        }
    }

    private IEnumerator SpawnMonsterRoutine()
    {
        while (!spawner.IsBossSpawned && !isDungeonCleared) // 클리어 상태에서는 실행 중단
        {
            for (int i = 0; i < monstersPerWave; i++)
            {
                if (spawner.RemainingMonsters > 0)
                {
                    spawner.SpawnMonster();
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void DungeonClear()
    {
        if (isDungeonCleared) return; // 이미 클리어 상태라면 실행하지 않음

        dungeonClearPanel.SetActive(true);
    
        if (possibleWeapons != null && possibleWeapons.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, possibleWeapons.Length);
            CardObject rewardWeapon = possibleWeapons[randomIndex];

            Card card = rewardWeapon.CreateCard();
    
            // 인벤토리에 무기 추가
            GameManager.Instance.playerData.inventory.AddItem(card, 1);
            GameManager.Instance.CardDatabase.InitializeIDs();
        
            GameSceneDataManager.Instance.DungeonClearReward();
        }
    }

    
    // hyuna. 레전드 보상 무기
    private void DungeonLegendClear()
    {
        if (isDungeonCleared) return; // 이미 클리어 상태라면 실행하지 않음

        dungeonClearPanel.SetActive(true);
    
        if (possibleLegendWeapons != null && possibleLegendWeapons.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, possibleLegendWeapons.Length);
            CardObject rewardWeapon = possibleLegendWeapons[randomIndex];

            Card card = rewardWeapon.CreateCard();
            GameManager.Instance.playerData.inventory.AddItem(card, 1);

            GameManager.Instance.CardDatabase.InitializeIDs();

            GameSceneDataManager.Instance.DungeonClearReward();
        
            Debug.Log("고급 무기 여부 확인, 고급 무기 제공");
        }
    }
}