using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    [SerializeField]
    private Button btnBuilding;
    public BaseBuilding buildingPrefab; // 생성할 빌딩 프리팹
    private bool isCreated; // TODO : 추후엔 이거 저장해서(DB) 불러와야함
    
    private void Start()
    {
        btnBuilding.onClick.AddListener(Build);
    }

    private void Build()
    {
        // 현재 이미지가 비활성화면(불투명이면), Create, 그 외는 Upgrade
        BuildingManager.Instance.Build(buildingPrefab, isCreated);
        
        // 빌딩 최초 생성 후에는 업그레이드 상태로 전환
        if (!isCreated)
        {
            isCreated = true;
        }
    }
} // end class
