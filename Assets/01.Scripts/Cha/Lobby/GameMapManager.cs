using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMapManager : SceneSingleton<GameMapManager>
{
    [Header("카메라 설정")]
    public Camera objectCamera; // Object Camera를 Inspector에서 연결
    
    [Header("맵 3D 오브젝트, UIs")] 
    public GameObject mapElimination; // 섬멸전
    // public GameObject mapConquest; // 점령전

    public Button btnStartGame; // 싱글 게임 시작
    // public Button btnMatchGame; // 멀티 매치메이킹

    [Header("맵 정보 패널")] 
    public GameObject panelMapInfo;
    public Text textMapName;
    public Image imageMapIcon;
    
    private MapData selectedMapData; // 선택된 맵의 데이터를 저장

    private void Start()
    {
        // Start 버튼에 클릭 이벤트 추가
        btnStartGame.onClick.AddListener(LoadMapScene);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = objectCamera.ScreenPointToRay(Input.mousePosition);
        
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject mapObject = hit.collider.gameObject;
                Debug.Log("Clicked on: " + mapObject.name); // 디버그 출력으로 클릭된 오브젝트 확인

                IMap map = mapObject.GetComponent<IMap>();
            
                if (map != null)
                {
                    selectedMapData = map.GetMapData();
                    UpdateUI(selectedMapData);
                    
                    if (!panelMapInfo.activeSelf) panelMapInfo.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("IMap interface not found on clicked object.");
                }
            }
            else
            {
                if (panelMapInfo.activeSelf) panelMapInfo.SetActive(false);
                Debug.Log("No object hit by Raycast.");
            }
        }
    }
    
    // UI를 업데이트하는 메서드
    private void UpdateUI(MapData mapData)
    {
        textMapName.text = mapData.mapName;
        imageMapIcon.sprite = mapData.mapIcon;
    }
    
    // Start 버튼 클릭 시 선택된 맵의 씬 로드
    private void LoadMapScene()
    {
        if (selectedMapData != null && !string.IsNullOrEmpty(selectedMapData.mapSceneName))
        {
            SceneManager.LoadScene(selectedMapData.mapSceneName);
        }
        else
        {
            Debug.LogWarning("No map data selected or scene name is empty.");
        }
    }

} // end class
