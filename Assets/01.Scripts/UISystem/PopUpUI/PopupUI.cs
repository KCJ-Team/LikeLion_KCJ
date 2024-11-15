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
    public SerializedDictionary<string, GameObject> contentObjs = new(); // 콘텐츠 에서 active할 오브젝트들

    
    private IPopupData popupData; // 공통 데이터 인터페이스로 보관
    
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        OnFocus?.Invoke();
    }
    
    // 데이터 Get
    public IPopupData GetData()
    {
        return popupData;
    }
    
    // 데이터 설정 메서드
    public void SetData(IPopupData data)
    {
        popupData = data;
    }
    
    // 헤더 설정
    public void SetHeader(string title, Sprite icon = null)
    {
        if (headerText != null)
        {
            headerText.text = title;
        }
    }

    // 특정 Content 텍스트, 아이콘, 버튼을 설정 (딕셔너리 키 사용)
    public void SetContent(string key, string text = null, Sprite icon = null)
    {
        // text 설정
        if (contentTexts.ContainsKey(key) && text != null)
        {
            contentTexts[key].text = text;
            contentTexts[key].gameObject.SetActive(true);
        }
        
        // icon 설정
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
    
    // 특정 key의 활성화 상태를 토글하거나 설정
    public void ToggleActiveState(string key, bool? setActive = null)
    {
        // 텍스트 오브젝트의 활성화 상태를 변경
        if (contentTexts.ContainsKey(key))
        {
            GameObject targetObject = contentTexts[key].gameObject;
            targetObject.SetActive(setActive ?? !targetObject.activeSelf);
        }
        
        // 아이콘 오브젝트의 활성화 상태를 변경
        if (contentIcons.ContainsKey(key))
        {
            GameObject targetObject = contentIcons[key].gameObject;
            targetObject.SetActive(setActive ?? !targetObject.activeSelf);
        }
    }
} // end class