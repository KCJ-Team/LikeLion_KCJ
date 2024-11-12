using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 엔딩을 관리하는 매니저
/// </summary>
public class EndingManager : SceneSingleton<EndingManager>
{
    [SerializeField] private SerializedDictionary<EndingType, EndingData> endings; // 엔딩 데이터 리스트
    
    [Header("UIs")]
    public GameObject panelEnding; // 엔딩 패널
    public Text textEndingTitle; // 엔딩 타이틀
    public TextMeshProUGUI textEndingTitleKr; // 엔딩 타이틀 한국어
    public Text textEnding; // 엔딩 텍스트 UI
    public Image iconEndingFaction; // 엔딩 Faction 아이콘 UI
    public Text textLeftDay;
    public Image imageEndingBackground; 
    
    private void Start()
    {
        if (panelEnding != null)
        {
            panelEnding.SetActive(false); // 시작 시 엔딩 패널 숨기기
        }
    }
    
    public void ShowEnding(EndingType endingType, int daySurvived, Sprite factionIcon = null)
    {
        if (endings.TryGetValue(endingType, out EndingData ending))
        {
            DisplayEnding(ending, daySurvived, factionIcon);
        }
        else
        {
            Debug.LogWarning($"Ending data not found for ending type: {endingType}");
        }
    }
   
    private void DisplayEnding(EndingData ending, int daySurvived, Sprite factionIcon = null)
    {
        if (panelEnding != null)
        {
            panelEnding.SetActive(true); // 엔딩 패널 표시
            
            textEndingTitle.text = ending.endingTitle;
            textEndingTitleKr.text = ending.endingTitleKr; 
            textEnding.text = ending.endingText; // 엔딩 텍스트 설정
            imageEndingBackground.sprite = ending.endingImage; // 엔딩 백그라운드 이미지 설정
            textLeftDay.text = $"당신은 {daySurvived}일 까지 버텼습니다.";

            // 아이콘 설정 및 알파값 조절
            if (factionIcon != null)
            {
                iconEndingFaction.sprite = factionIcon;
                iconEndingFaction.color = new Color(iconEndingFaction.color.r, iconEndingFaction.color.g, iconEndingFaction.color.b, 1f); // 알파값 1
            }
            else
            {
                iconEndingFaction.sprite = null;
                iconEndingFaction.color = new Color(iconEndingFaction.color.r, iconEndingFaction.color.g, iconEndingFaction.color.b, 0f); // 알파값 0
            }
            
            // 게임 시간 멈춤
            GameTimeManager.Instance.TogglePauseTime();
        }
    }
} // end class
