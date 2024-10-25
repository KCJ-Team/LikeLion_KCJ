using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;
    private Dictionary<string, Buff> activeBuffs = new Dictionary<string, Buff>();
    
    private float currentHealth;
    
    private void Start()
    {
        currentHealth = playerData.HP;
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
                    playerData.MoveSpeed *= (1 + effect.value);
                    break;
                case BuffType.MaxHealth:
                    playerData.HP *= (1 + effect.value);
                    break;
                case BuffType.AttackPower:
                    // AttackPower가 PlayerData에 없다면 추가 필요
                    // playerData.AttackPower *= (1 + effect.value);
                    break;
                case BuffType.Defense:
                    // Defense가 PlayerData에 없다면 추가 필요
                    // playerData.Defense *= (1 + effect.value);
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
                    playerData.MoveSpeed /= (1 + effect.value);
                    break;
                case BuffType.MaxHealth:
                    playerData.HP /= (1 + effect.value);
                    break;
                case BuffType.AttackPower:
                    // AttackPower가 PlayerData에 없다면 추가 필요
                    // playerData.AttackPower /= (1 + effect.value);
                    break;
                case BuffType.Defense:
                    // Defense가 PlayerData에 없다면 추가 필요
                    // playerData.Defense /= (1 + effect.value);
                    break;
            }
        }
    }
    
    // 현재 스탯 가져오기
    public float GetCurrentMoveSpeed() => playerData.MoveSpeed;
    public float GetCurrentMaxHealth() => playerData.HP;
    public float GetCurrentHealth() => currentHealth;
}