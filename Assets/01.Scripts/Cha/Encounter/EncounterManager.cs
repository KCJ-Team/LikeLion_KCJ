using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class EncounterManager : SceneSingleton<EncounterManager>
{
    [SerializeField] private EncounterData encounterData;
    [SerializeField] private List<Encounter> unresolvedEncounters;

    [Header("UI MVP 패턴")] 
    [SerializeField] 
    private EncounterUIView encounterUIView;
    private EncounterUIPresenter encounterUIPresenter;
    
    private Encounter currentEncounter; // 현재 표시 중인 인카운터

    private void Start()
    {
        if (encounterData == null)
        {
            Debug.LogError("EncounterData가 할당되지 않았습니다.");
            return;
        }

        // 매니저에 사용할 인카운터 넘기기. Call by Value..
        // TODO : DB..
        unresolvedEncounters = encounterData.encounters.ToList();

        encounterUIPresenter = new EncounterUIPresenter(encounterUIView);
    }
    
    // 다음 인카운터 오브젝트를 활성화
    public void ActivateNextEncounter()
    {
        encounterUIPresenter.ActivateNextEncounter();
    }
    
    // 랜덤 인카운터 UI에 Print하는 것까지..
    public void PrintRandomEncounter()
    {
        if (unresolvedEncounters.Count == 0)
        {
            Debug.Log("모든 인카운터가 해결되었습니다.");
            return;
        }

        // 랜덤으로 해결되지 않은 인카운터 선택
        int randomIndex = Random.Range(0, unresolvedEncounters.Count);
        currentEncounter = unresolvedEncounters[randomIndex];
        
        // 선택된 인카운터를 리스트에서 제거
        unresolvedEncounters.RemoveAt(randomIndex);
        
        // Presenter를 통해 UI 업데이트
        encounterUIPresenter.OpenEncounterPopup(currentEncounter);
    }

    // Choice 1 선택 시 호출되는 메서드
    public void ChooseOption1()
    {
        if (currentEncounter == null) return;
        
        // Choice2 결과를 처리
        // 자원과 팩션 변화
        // 자원 변경
        GameResourceManager.Instance.AddResource(ResourceType.Energy, currentEncounter.choice1RewardEnergy);
        GameResourceManager.Instance.AddResource(ResourceType.Food, currentEncounter.choice1RewardFood);
        GameResourceManager.Instance.AddResource(ResourceType.Fuel, currentEncounter.choice1RewardFuel);
        GameResourceManager.Instance.AddResource(ResourceType.Workforce, currentEncounter.choice1RewardWorkforce);

        // 팩션 수치 변경
        if (currentEncounter.choice1Faction != FactionType.None)
        {
            FactionManager.Instance.ChangeFactionSupport(currentEncounter.choice1Faction, currentEncounter.choice1FactionSupport);
        }
        
        string strResult = $"Encounter Name: {currentEncounter.name}\n" +
                           $"choice1Result: {currentEncounter.choice1Result}\n" +
                           $"choice1Reward_Energy: {currentEncounter.choice1RewardEnergy}\n" +
                           $"choice1Reward_Food: {currentEncounter.choice1RewardFood}\n +" +
                           $"choice1Reward_Fuel: {currentEncounter.choice1RewardFuel}\n +" +
                           $"choice1Reward_Workforce: {currentEncounter.choice1RewardWorkforce}\n +" +
                           $"choice1Faction: {currentEncounter.choice1Faction}\n +" +
                           $"choice1FactionSupport: {currentEncounter.choice1FactionSupport}\n";
        
        // Presenter를 통해 결과 UI 업데이트
        // encounterUIPresenter.UpdateUIResult(currentEncounter, 1);
    }

    // Choice 2 선택 시 호출되는 메서드
    public void ChooseOption2()
    {
        if (currentEncounter == null) return;

        // Choice2 결과를 처리
        // 자원과 팩션 변화
        // 자원 변경
        GameResourceManager.Instance.AddResource(ResourceType.Energy, currentEncounter.choice2RewardEnergy);
        GameResourceManager.Instance.AddResource(ResourceType.Food, currentEncounter.choice2RewardFood);
        GameResourceManager.Instance.AddResource(ResourceType.Fuel, currentEncounter.choice2RewardFuel);
        GameResourceManager.Instance.AddResource(ResourceType.Workforce, currentEncounter.choice2RewardWorkforce);

        // 팩션 수치 변경
        if (currentEncounter.choice2Faction != FactionType.None)
        {
            FactionManager.Instance.ChangeFactionSupport(currentEncounter.choice2Faction, currentEncounter.choice2FactionSupport);
        }
        
        string strResult = $"Encounter Name: {currentEncounter.name}\n" +
                           $"choice2Result: {currentEncounter.choice2Result}\n" +
                           $"choice2Reward_Energy: {currentEncounter.choice2RewardEnergy}\n" +
                           $"choice2Reward_Food: {currentEncounter.choice2RewardFood}\n +" +
                           $"choice2Reward_Fuel: {currentEncounter.choice2RewardFuel}\n +" +
                           $"choice2Reward_Workforce: {currentEncounter.choice2RewardWorkforce}\n +" +
                           $"choice2Faction: {currentEncounter.choice2Faction}\n +" +
                           $"choice2FactionSupport: {currentEncounter.choice2FactionSupport}\n";

        // Presenter를 통해 결과 UI 업데이트
        // encounterUIPresenter.UpdateUIResult(currentEncounter, 1);
    }
} // end class