using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Encounter
{
    public int encounterId;
    public string name;
    public string description;
    
    // Choice 1 필드
    public string choice1Text;
    public string choice1Result;
    public int choice1RewardEnergy;
    public int choice1RewardFood;
    public int choice1RewardFuel;
    public int choice1RewardWorkforce;
    public FactionType choice1Faction;
    public float choice1FactionSupport;

    // Choice 2 필드
    public string choice2Text;
    public string choice2Result;
    public int choice2RewardEnergy;
    public int choice2RewardFood;
    public int choice2RewardFuel;
    public int choice2RewardWorkforce;
    public FactionType choice2Faction;
    public float choice2FactionSupport;
}