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

    // 추후 UI를 분리하던가 해야겠음
    [Header("Test UI")] public GameObject panelEncounter;
    public TextMeshProUGUI textEncounterName;
    public TextMeshProUGUI textEncounterDescription;
    public TextMeshProUGUI textChoice1Text;
    public TextMeshProUGUI textChoice2Text;
    public TextMeshProUGUI textRemaining;

    public GameObject panelEncounterResult;
    public TextMeshProUGUI textEncounterResult;

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

        // UI Update
        UpdateUI();
    }

    // UI를 업데이트
    private void UpdateUI()
    {
        if (textEncounterName != null)
        {
            textEncounterName.text = currentEncounter.name;
        }

        if (textEncounterDescription != null)
        {
            textEncounterDescription.text = currentEncounter.description;
        }

        if (textChoice1Text != null)
        {
            textChoice1Text.text = currentEncounter.choice1Text;
        }

        if (textChoice2Text != null)
        {
            textChoice2Text.text = currentEncounter.choice2Text;
        }

        if (textRemaining != null)
        {
            textRemaining.text = $"Remaining : {unresolvedEncounters.Count.ToString()}";
        }

        // 모든 텍스트를 하나의 로그로 출력
        Debug.Log($"Encounter Name: {currentEncounter.name}\n" +
                  $"Description: {currentEncounter.description}\n" +
                  $"Choice 1: {currentEncounter.choice1Text}\n" +
                  $"Choice 2: {currentEncounter.choice2Text}");

        panelEncounter.SetActive(true);
        panelEncounterResult.SetActive(false);
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

        // UI Update
        UpdateUIResult(strResult);
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

        // UI Update
        UpdateUIResult(strResult);
    }

    private void UpdateUIResult(string strResult)
    {
        Debug.Log(strResult);

        textEncounterResult.text = strResult;

        panelEncounter.SetActive(false);
        panelEncounterResult.SetActive(true);
    }
} // end class