using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 엔딩을 관리하는 매니저
/// </summary>
public class EndingManager : SceneSingleton<EndingManager>
{
    [SerializeField] private SerializedDictionary<EndingType, EndingData> endings; // 엔딩 데이터 리스트
    
    [Header("UIs")]
    [SerializeField] public GameObject endingPanel; // 엔딩 패널
    [SerializeField] public Text endingTextUI; // 엔딩 텍스트 UI
    [SerializeField] public Image endingIconUI; // 엔딩 아이콘 UI
    
    private void Start()
    {
        if (endingPanel != null)
        {
            endingPanel.SetActive(false); // 시작 시 엔딩 패널 숨기기
        }
    }
    
    public void ShowEnding(EndingType endingType)
    {
        if (endings.TryGetValue(endingType, out EndingData ending))
        {
            DisplayEnding(ending);
        }
        else
        {
            Debug.LogWarning($"Ending data not found for ending type: {endingType}");
        }
    }
   
    private void DisplayEnding(EndingData ending)
    {
        if (endingPanel != null)
        {
            endingPanel.SetActive(true); // 엔딩 패널 표시
            endingTextUI.text = ending.endingText; // 엔딩 텍스트 설정
            endingIconUI.sprite = ending.endingImage; // 엔딩 아이콘 설정
        }
    }
    
} // end class
