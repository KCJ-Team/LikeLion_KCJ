using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

public enum BuildingUIType
{
    CraftingButton,     // 크래프팅 영역
    BuildingButton, // 빌딩 이미지 영역
    ProcessImage, // 6, 18시 자원이 생산되었을때 줄 이미지
    EnableUpgradeImage, // 업그레이드 가능한 상태일때 줄 이미지
    UseButton,           // 병원/오락 Use 영역
}

//UI 요소 관리 스크립트
public class BuildingUIView : MonoBehaviour
{
    [Header("빌딩 업그레이드 팝업창 업그레이드(빌드) 버튼")] 
    public Button buildUpgradeButton;
    
    [Header("빌딩에 관련된 UI 목록")]
    public SerializedDictionary<BuildingType, SerializedDictionary<BuildingUIType, GameObject>> buildingUIs;

    [Header("UseButton의 아이콘")] 
    public Sprite iconUse;
    public Sprite iconUnused;
} // end class