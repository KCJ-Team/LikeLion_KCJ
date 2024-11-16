using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Tech
{
    public int techId;
    public string techName;
    public string techDesciption;
    public int techCost;
    public Sprite techIcon;
    public bool isLearned;
    
    public void SetTechCost(int productionOutput)
    {
        techCost = productionOutput * 4; // 기본 4배
    }
}
