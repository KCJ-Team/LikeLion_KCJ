using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 캐릭터의 상태와 버프를 관리하는 클래스
/// </summary>
public class Status : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;   // 플레이어 기본 데이터
    
    // 현재 적용된 버프들을 관리하는 딕셔너리
    private Dictionary<string, Buff> activeBuffs = new Dictionary<string, Buff>();
    
    private float currentHealth;     // 현재 체력
    
    private void Start()
    {
        currentHealth = playerData.HP;
    }
    
    private void Update()
    {
        UpdateBuffs();  // 매 프레임마다 버프 상태 업데이트
    }
    
    /// <summary>
    /// 모든 활성화된 버프의 지속시간을 감소시키고 만료된 버프 제거
    /// </summary>
    private void UpdateBuffs()
    {
        List<string> expiredBuffs = new List<string>();
        
        // 만료된 버프 찾기
        foreach (var buff in activeBuffs.Values)
        {
            buff.duration -= Time.deltaTime;
            if (buff.duration <= 0)
            {
                expiredBuffs.Add(buff.id);
            }
        }
        
        // 만료된 버프 제거
        foreach (string buffId in expiredBuffs)
        {
            RemoveBuff(buffId);
        }
    }
    
    /// <summary>
    /// 새로운 버프 추가
    /// </summary>
    public void AddBuff(Buff buff)
    {
        // 같은 ID의 버프가 이미 있다면 제거
        if (activeBuffs.ContainsKey(buff.id))
        {
            RemoveBuff(buff.id);
        }
        
        // 새 버프 추가 및 효과 적용
        activeBuffs[buff.id] = buff;
        ApplyBuff(buff);
    }
    
    /// <summary>
    /// 버프 제거
    /// </summary>
    public void RemoveBuff(string buffId)
    {
        if (activeBuffs.TryGetValue(buffId, out Buff buff))
        {
            RemoveBuffEffect(buff);      // 버프 효과 제거
            activeBuffs.Remove(buffId);  // 활성 버프 목록에서 제거
        }
    }
    
    /// <summary>
    /// 버프 효과 적용
    /// 각 효과 타입에 따라 해당하는 스탯 수정
    /// </summary>
    private void ApplyBuff(Buff buff)
    {
        foreach (var effect in buff.effects)
        {
            switch (effect.type)
            {
                case BuffType.MoveSpeed:
                    playerData.MoveSpeed *= (1 + effect.value);  // 이동속도 증가
                    break;
                case BuffType.MaxHealth:
                    playerData.HP *= (1 + effect.value);         // 최대체력 증가
                    break;
                case BuffType.AttackPower:
                    // 공격력 증가 (구현 필요)
                    break;
                case BuffType.Defense:
                    // 방어력 증가 (구현 필요)
                    break;
            }
        }
    }
    
    /// <summary>
    /// 버프 효과 제거
    /// 적용된 효과를 원래대로 되돌림
    /// </summary>
    private void RemoveBuffEffect(Buff buff)
    {
        foreach (var effect in buff.effects)
        {
            switch (effect.type)
            {
                case BuffType.MoveSpeed:
                    playerData.MoveSpeed /= (1 + effect.value);  // 이동속도 감소
                    break;
                case BuffType.MaxHealth:
                    playerData.HP /= (1 + effect.value);         // 최대체력 감소
                    break;
                case BuffType.AttackPower:
                    // 공격력 감소 (구현 필요)
                    break;
                case BuffType.Defense:
                    // 방어력 감소 (구현 필요)
                    break;
            }
        }
    }
    
    // 현재 스탯 값을 반환하는 getter 메서드들
    public float GetCurrentMoveSpeed() => playerData.MoveSpeed;
    public float GetCurrentMaxHealth() => playerData.HP;
    public float GetCurrentHealth() => currentHealth;
}