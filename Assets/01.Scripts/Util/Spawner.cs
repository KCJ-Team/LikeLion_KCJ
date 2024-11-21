using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [Header("Monster Settings")]
    public GameObject[] monsterPrefabs; // 여러 종류의 몬스터 프리팹 배열
    public Transform[] spawnPoints;
    public GameObject bossPrefab;
    public Transform BossSpawnPoints;

    [Header("Settings")]
    public int totalMonsters = 50; // 보스가 나타나기 전까지의 총 몬스터 수
    public float bossSpawnTime = 60f; // 보스 생성까지 남은 시간 (초)

    private int spawnedMonsters = 0;
    public float countdown = 1;
    private bool bossSpawned = false;
    private bool bossStart = false;
    private GameObject bossMonster;

    // 외부 접근을 위한 프로퍼티
    public int RemainingMonsters => totalMonsters - spawnedMonsters;
    public float RemainingBossTime => countdown;
    public bool IsBossSpawned => bossSpawned;
    public GameObject BossMonster => bossMonster;
    public bool IsBossStart => bossStart;

    void Start()
    {
        countdown = bossSpawnTime;
    }

    void Update()
    {
        if (bossStart && countdown > 0)
        {
            countdown -= Time.deltaTime;
        }
        
    }

    // 몬스터를 스폰하는 메서드 (여러 종류)
    public void SpawnMonster()
    {
        if (spawnedMonsters < totalMonsters)
        {
            int spawnIndex = Random.Range(0, spawnPoints.Length);
            int monsterIndex = Random.Range(0, monsterPrefabs.Length); // 무작위로 몬스터 선택
            Instantiate(monsterPrefabs[monsterIndex], spawnPoints[spawnIndex].position, Quaternion.identity);
            
            spawnedMonsters++;

            if (RemainingMonsters == 0 && !bossSpawned)
            {
                bossStart = true;
                
                if (bossStart)
                {
                    Invoke("SpawnBoss", bossSpawnTime);
                }
            }
        }
    }

    // 보스 스폰 메서드
    private void SpawnBoss()
    {
        bossMonster = Instantiate(bossPrefab, BossSpawnPoints.position, Quaternion.identity);
        bossSpawned = true;
    }

    // 외부 스크립트에서 접근 가능한 메서드
    public void ResetSpawner()
    {
        spawnedMonsters = 0;
        countdown = bossSpawnTime;
        bossSpawned = false;
    }
}