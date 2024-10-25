using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FactionType
{
    Red, // 질서와 규율
    Black, // 감시
    Green, // 구조와 봉사
    Yellow // 자유
}

[Serializable]
public class Faction
{
    public string factionId;
    public string name;
    public FactionType type;
    public string description;
    public Sprite icon;

    [Range(0f, 1f)] public float supportLevel; // 지지도 (0 ~ 1)
    
    // 팩션 지지도 변화 메서드
    public void ChangeSupport(float amount)
    {
        supportLevel = Mathf.Clamp01(supportLevel + amount); // 0과 1 사이로 제한
        Debug.Log($"{name} ({type}) support changed by {amount}. Current support: {supportLevel}");
    }
}
