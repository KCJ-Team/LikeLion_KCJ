using System;
using System.Collections;
using System.Collections.Generic;
using PlayerInfo;
using UnityEngine;

/// <summary>
/// Lobby에서 3D 오브젝트들을 관리하는 매니저
/// </summary>
public class ObjectManager : SceneSingleton<ObjectManager>
{
    [Header("로비에서 사용할 Player 모델 오브젝트들")] 
    public GameObject malePlayerObj;
    public GameObject femalePlayerObj;

    [Header("던전에서 사용할 Player 프리팹 ")] 
    public GameObject maleChracterPrefab;
    public GameObject femaleChracterPrefab;
    
    [Header("Map 모델 오브젝트들")] 
    public GameObject planets; // 상위 오브젝트
    public List<GameObject> planetObjs = new(); // 하위 오브젝트 자동 추가 리스트
    
    public GameObject mapEliminationObj;

    private void Start()
    {
        // planetObjs 리스트 초기화 - 하위 오브젝트 자동 추가
        ClearPlanetObjects(false);
    }
    
    public void ClearPlanetObjects(bool isActive)
    {
        foreach (Transform child in planets.transform)
        {
            // planetObjs에 없는 경우에만 추가
            if (!planetObjs.Contains(child.gameObject))
            {
                planetObjs.Add(child.gameObject);
            }
            
            // 모든 오브젝트 비활성화
            // child.gameObject.SetActive(isActive);
        }

        Debug.Log("Planet objects initialized, and all deactivated.");
    }
    
    public void SetPlanetObjectsActive(bool isActive)
    {
        foreach (var planet in planetObjs)
        {
            planet.SetActive(isActive);
            mapEliminationObj.SetActive(isActive);
        }
    
        Debug.Log($"All planet objects set to active: {isActive}");
    }
    
    public void ActivatePlayerModel(PlayerModelType playerType)
    {
        // 모든 플레이어 오브젝트 비활성화
        malePlayerObj.SetActive(false);
        femalePlayerObj.SetActive(false);

        // playerType에 따라 특정 오브젝트 활성화
        switch (playerType)
        {
            case PlayerModelType.Male:
                // 11.19 hyuna 플레이어 데이터 스크립터블 오브젝트에 설정
                malePlayerObj.SetActive(true);
                LobbyMenuManager.Instance.playerData.Character = maleChracterPrefab;
                Debug.Log("Male player object activated.");
                break;
                
            case PlayerModelType.Female:
                femalePlayerObj.SetActive(true);
                LobbyMenuManager.Instance.playerData.Character = femaleChracterPrefab;
                Debug.Log("Female player object activated.");
                break;

            default:
                Debug.LogWarning("Unknown PlayerModelType.");
                break;
        }
    }
    
} // end class
