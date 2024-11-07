using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;

public class PopupUI : MonoBehaviour, IPointerDownHandler
{
    [Header("Common")]
    public Button closeButton;
    public event Action OnFocus;

    [Header("Header and Content Elements")]
    public Text headerText; // 팝업 헤더 텍스트
    public SerializedDictionary<string, Text> contentTexts = new(); // 콘텐츠 텍스트 맵핑
    public SerializedDictionary<string, Image> contentIcons = new(); // 콘텐츠 아이콘 맵핑

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        OnFocus?.Invoke();
    }

    // 헤더 설정
    public void SetHeader(string title, Sprite icon = null)
    {
        if (headerText != null)
        {
            headerText.text = title;
        }
    }

    // 특정 Content 텍스트와 아이콘을 설정 (딕셔너리 키 사용)
    public void SetContent(string key, string text = null, Sprite icon = null)
    {
        // text
        if (contentTexts.ContainsKey(key) && text != null)
        {
            contentTexts[key].text = text;
            contentTexts[key].gameObject.SetActive(true);
        }
        
        // icon
        if (contentIcons.ContainsKey(key))
        {
            if (icon != null)
            {
                contentIcons[key].sprite = icon;
                contentIcons[key].gameObject.SetActive(true);
            }
            else
            {
                // 아이콘이 null일 경우 아이콘 GameObject를 비활성화
                contentIcons[key].gameObject.SetActive(false);
            }
        }
    }

    // 특정 콘텐츠 요소 숨기기
    public void HideContent(BuildingType key)
    {
        // if (contentTexts.ContainsKey(key))
        // {
        //     contentTexts[key].gameObject.SetActive(false);
        // }
        // if (contentIcons.ContainsKey(key))
        // {
        //     // contentIcons[key].gameObject.SetActive(false);
        // }
    }
} // end class