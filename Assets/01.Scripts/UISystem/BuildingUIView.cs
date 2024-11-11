using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Serialization;

//UI 요소 관리 스크립트
public class BuildingUIView : MonoBehaviour
{
    [Header("빌딩 업그레이드 팝업창 업그레이드(빌드) 버튼")] 
    public Button buildUpgradeButton;
    
    [Header("빌딩 건설 및 업그레이드 버튼 목록. 0은 크래프팅, 1은 빌딩 이미지")]
    public SerializedDictionary<BuildingType, Button[]> buildings; // 각 빌딩 타입에 대한 버튼 배열

} // end class