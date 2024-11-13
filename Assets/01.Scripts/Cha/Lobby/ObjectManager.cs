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
    [Header("Player 모델 오브젝트들")] 
    public GameObject malePlayerObj;
    public GameObject femalePlayerObj;

    [Header("Map 모델 오브젝트들")] 
    public GameObject planets; // 상위 오브젝트
    public List<GameObject> planetObjs = new(); // 하위 오브젝트 자동 추가 리스트
    
    public GameObject mapEliminationObj;

    private void Start()
    {
        // planetObjs 리스트 초기화 - 하위 오브젝트 자동 추가
        DisablePlanetObjects(false);
    }
    
    private void DisablePlanetObjects(bool isActive)
    {
        foreach (Transform child in planets.transform)
        {
            // planetObjs에 없는 경우에만 추가
            if (!planetObjs.Contains(child.gameObject))
            {
                planetObjs.Add(child.gameObject);
            }
            
            // 모든 오브젝트 비활성화
            child.gameObject.SetActive(isActive);
        }

        Debug.Log("Planet objects initialized, and all deactivated.");
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
                malePlayerObj.SetActive(true);
                Debug.Log("Male player object activated.");
                break;
                
            case PlayerModelType.Female:
                femalePlayerObj.SetActive(true);
                Debug.Log("Female player object activated.");
                break;

            default:
                Debug.LogWarning("Unknown PlayerModelType.");
                break;
        }
    }
    
} // end class
