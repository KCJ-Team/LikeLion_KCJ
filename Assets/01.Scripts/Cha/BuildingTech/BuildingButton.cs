using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public Button btnBuilding;
    public BaseBuilding buildingPrefab; // 생성할 빌딩 프리팹
    private BuildingStateMachine stateMachine;  // 상태 전환을 관리할 StateMachine
    
    private void Start()
    {
        stateMachine = new BuildingStateMachine();
        
        btnBuilding.onClick.AddListener(Build);
    }

    private void Build()
    {
        stateMachine.Build(buildingPrefab);
    }
} // end class
