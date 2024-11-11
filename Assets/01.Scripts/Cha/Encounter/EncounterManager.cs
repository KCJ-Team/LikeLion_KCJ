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

} // end class