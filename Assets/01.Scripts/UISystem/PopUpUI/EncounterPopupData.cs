using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterPopupData : IPopupData
{
    public Encounter encounter { get; set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    
    // Choice 1 필드
    public string Choice1Text { get; private set; }
    public string Choice1Result { get; private set; }
    public int Choice1RewardEnergy { get; private set; }
    public int Choice1RewardFood { get; private set; }
    public int Choice1RewardFuel { get; private set; }
    public int Choice1RewardWorkforce { get; private set; }
    public FactionType Choice1Faction { get; private set; }

    // Choice 2 필드
    public string Choice2Text { get; private set; }
    public string Choice2Result { get; private set; }
    public int Choice2RewardEnergy { get; private set; }
    public int Choice2RewardFood { get; private set; }
    public int Choice2RewardFuel { get; private set; }
    public int Choice2RewardWorkforce { get; private set; }
    public FactionType Choice2Faction { get; private set; }
    
    // Icon
    public Sprite EncounterChoiceIcon { get; private set; }
    public Sprite EncounterResultIcon { get; private set; }
    public Sprite FactionIcon { get; private set; }

    public EncounterPopupData(Encounter encounter)
    {
        this.encounter = encounter;
        
        // Title and Description 설정
        Title = encounter.name;
        Description = encounter.description;

        // Choice 1 관련 데이터 설정
        Choice1Text = encounter.choice1Text;
        Choice1Result = encounter.choice1Result;
        Choice1RewardEnergy = encounter.choice1RewardEnergy;
        Choice1RewardFood = encounter.choice1RewardFood;
        Choice1RewardFuel = encounter.choice1RewardFuel;
        Choice1RewardWorkforce = encounter.choice1RewardWorkforce;
        Choice1Faction = encounter.choice1Faction;

        // Choice 2 관련 데이터 설정
        Choice2Text = encounter.choice2Text;
        Choice2Result = encounter.choice2Result;
        Choice2RewardEnergy = encounter.choice2RewardEnergy;
        Choice2RewardFood = encounter.choice2RewardFood;
        Choice2RewardFuel = encounter.choice2RewardFuel;
        Choice2RewardWorkforce = encounter.choice2RewardWorkforce;
        Choice2Faction = encounter.choice2Faction;
        
        // 아이콘 설정
        EncounterChoiceIcon = Resources.Load<Sprite>("Data/Encounter/Icon_Choice");
        EncounterResultIcon = Resources.Load<Sprite>("Data/Encounter/Icon_Result");
    }
}