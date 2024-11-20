using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 기술 연구 매니저
/// </summary>
public class TechManager : SceneSingleton<TechManager>
{
    [Header("기술 리스트")] 
    public List<Tech> techs;
    
    private void Start()
    {
        InitializeTechCosts();
    }
    
    private void InitializeTechCosts()
    {
        BaseBuilding researchLab = BuildingManager.Instance.GetBuilding(BuildingType.ResearchLab);

        if (researchLab != null)
        {
            int productionOutput = researchLab.GetBuildingData().productionOutput;

            foreach (var tech in techs)
            {
                tech.SetTechCost(productionOutput);
            }
        }
        else
        {
            foreach (var tech in techs)
            {
                tech.SetTechCost(20); // 기본값
            }
        }
    }
    
    /// <summary>
    /// 각 테트리 기술..
    /// </summary>
    /// <param name="tech"></param>
    public void LearnTech(Tech tech)
    {
        // 연구 포인트 소모
        GameResourceManager.Instance.ConsumeResource(ResourceType.Research, tech.techCost);

        // Tech 상태 업데이트
        tech.isLearned = true;

        // 각 테크에 따라 로직 실행
        switch (tech.techId)
        {
            case 0: // 신체 강화
                ApplyPhysicalEnhancement();
                break;

            case 1: // 자원 증폭
                ApplyResourceAmplification();
                break;

            case 2: // 탐험 보상 고급 무기 획득률 증가
                ApplyExplorationEnhancement();
                break;

            default:
                Debug.LogWarning($"Unhandled tech ID: {tech.techId}");
                break;
        }
    }
    
    // 신체 강화
    private void ApplyPhysicalEnhancement()
    {
        // Attack과 Defense를 2배로 증가
        float newAttack = LobbyMenuManager.Instance.attack * 2;
        float newDefense = LobbyMenuManager.Instance.defense * 2;

        LobbyMenuManager.Instance.SetAttackAndDefense(newAttack, newDefense);

        Debug.Log("신체 강화 적용: Attack과 Defense 값이 2배로 증가했습니다.");
    }
    
    // 매일 오전 12시에 자원을 한번 더 생산
    private void ApplyResourceAmplification()
    {
        // 새로운 자원 생산 스케줄 추가
        GameTimeManager.Instance.hasProducedAt12PM = true;// 12시에 자원 추가 생산

        Debug.Log("자원 증폭 적용: 매일 12시에 추가 자원 생산이 실행됩니다.");
    }

    // 탐험 보상에서 고급 무기 획득률 증가
    private void ApplyExplorationEnhancement()
    {
        // DB의 데이터만 true로 날려주면 됨.
        PlayerService playerService = new PlayerService();
        string playerId = playerService.GetPlayer().PlayerId;
        
        // bool 값을 int로 변환
        int techLearned1 = techs[0].isLearned ? 1 : 0;
        int techLearned2 = techs[1].isLearned ? 1 : 0;
        int techLearned3 = techs[2].isLearned ? 1 : 0;
        
        // DB에 저장. 
        playerService.UpdateTechLearnedStatus(playerId, techLearned1, techLearned2, techLearned3);

        Debug.Log("탐험 보상 강화 적용: 고급 무기 획득 확률이 증가했습니다.");
    }
} // end class
