using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    //추후에 PlayerData에서 가져오기
    public float maxHealth = 100f;
    public float moveSpeed = 5f;
    public float attackPower = 10f;
    public float defense = 5f;
}

// 캐릭터의 스탯과 상태를 관리하는 클래스
public class Status : MonoBehaviour
{
    [SerializeField]
    private Stats baseStats;
    private Stats currentStats;
    
    private float currentHealth;
    private Dictionary<string, Buff> activeBuffs = new Dictionary<string, Buff>();
    
    private void Awake()
    {
        // 초기 스탯 설정
        currentStats = new Stats();
        ResetStats();
    }
    
    private void Start()
    {
        currentHealth = currentStats.maxHealth;
    }
    
    private void Update()
    {
        UpdateBuffs();
    }
    
    // 버프 업데이트 및 만료된 버프 제거
    private void UpdateBuffs()
    {
        List<string> expiredBuffs = new List<string>();
        
        foreach (var buff in activeBuffs.Values)
        {
            buff.duration -= Time.deltaTime;
            if (buff.duration <= 0)
            {
                expiredBuffs.Add(buff.id);
            }
        }
        
        foreach (string buffId in expiredBuffs)
        {
            RemoveBuff(buffId);
        }
    }
    
    // 스탯 초기화
    public void ResetStats()
    {
        currentStats.maxHealth = baseStats.maxHealth;
        currentStats.moveSpeed = baseStats.moveSpeed;
        currentStats.attackPower = baseStats.attackPower;
        currentStats.defense = baseStats.defense;
    }
    
    // 버프 적용
    public void AddBuff(Buff buff)
    {
        // 이미 같은 종류의 버프가 있다면 제거
        if (activeBuffs.ContainsKey(buff.id))
        {
            RemoveBuff(buff.id);
        }
        
        activeBuffs[buff.id] = buff;
        ApplyBuff(buff);
    }
    
    // 버프 제거
    public void RemoveBuff(string buffId)
    {
        if (activeBuffs.TryGetValue(buffId, out Buff buff))
        {
            RemoveBuffEffect(buff);
            activeBuffs.Remove(buffId);
        }
    }
    
    // 버프 효과 적용
    private void ApplyBuff(Buff buff)
    {
        foreach (var effect in buff.effects)
        {
            switch (effect.type)
            {
                case BuffType.MoveSpeed:
                    currentStats.moveSpeed *= (1 + effect.value);
                    break;
                case BuffType.MaxHealth:
                    currentStats.maxHealth *= (1 + effect.value);
                    break;
                case BuffType.AttackPower:
                    currentStats.attackPower *= (1 + effect.value);
                    break;
                case BuffType.Defense:
                    currentStats.defense *= (1 + effect.value);
                    break;
            }
        }
    }
    
    // 버프 효과 제거
    private void RemoveBuffEffect(Buff buff)
    {
        foreach (var effect in buff.effects)
        {
            switch (effect.type)
            {
                case BuffType.MoveSpeed:
                    currentStats.moveSpeed /= (1 + effect.value);
                    break;
                case BuffType.MaxHealth:
                    currentStats.maxHealth /= (1 + effect.value);
                    break;
                case BuffType.AttackPower:
                    currentStats.attackPower /= (1 + effect.value);
                    break;
                case BuffType.Defense:
                    currentStats.defense /= (1 + effect.value);
                    break;
            }
        }
    }
    
    // 현재 스탯 가져오기
    public float GetCurrentMoveSpeed() => currentStats.moveSpeed;
    public float GetCurrentMaxHealth() => currentStats.maxHealth;
    public float GetCurrentAttackPower() => currentStats.attackPower;
    public float GetCurrentDefense() => currentStats.defense;
    public float GetCurrentHealth() => currentHealth;
}