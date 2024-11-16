using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopupType
{
    BuildingUpgrade, Encounter, Store, Lab, Deck
}

public class PopupUIManager : SceneSingleton<PopupUIManager>
{
    //키 바인딩 - 예시
    public KeyCode escapeKey = KeyCode.Escape;
    public KeyCode storeKey = KeyCode.C;
    public KeyCode labKey = KeyCode.V;
    public KeyCode deckKey = KeyCode.X;
    
    [SerializeField]
    private LinkedList<PopupUI> activePopupLList;  //실시간 팝업 관리 LinkedList
    
    [SerializeField]
    private List<PopupUI> allPopupList;            //전체 팝업 목록 List

    // hyuna
    [Header("PopUIs")]
    public PopupUI buildingUpgradePopup;
    public PopupUI EncounterPopup;
    public PopupUI labPopup;
    public PopupUI storePopup;
    public PopupUI deckPopup;
    
    private void Awake()
    {
        base.Awake();
        
        activePopupLList = new LinkedList<PopupUI>();
        Init();
        Invoke("InitCloseAll", 0.01f);
    }
    
    //일반 메소드
    private void Init()
    {
        // 1. 리스트 초기화
        allPopupList = new List<PopupUI>()
        {
            buildingUpgradePopup, EncounterPopup, labPopup, storePopup
        };

        // 2. 모든 팝업에 이벤트 등록
        foreach (var popup in allPopupList)
        {
            // 헤더 포커스 이벤트
            popup.OnFocus += () =>
            {
                activePopupLList.Remove(popup);
                activePopupLList.AddFirst(popup);
                RefreshAllPopupDepth();
            };

            // 닫기 버튼 이벤트
            popup.closeButton.onClick.AddListener(() => ClosePopup(popup));
        }
    }
    
    // 활성화된 팝업이 있는지 확인하는 메서드
    public bool IsAnyPopupOpen()
    {
        return activePopupLList.Count > 0;
    }
    
    //시작 시 모든 팝업 닫기
    private void InitCloseAll()
    {
        foreach (var popup in allPopupList)
        {
            ClosePopup(popup);
        }
    }
    
    private void Update()
    {
        // ESC 누를 경우 링크드리스트의 First 닫기
        if (Input.GetKeyDown(escapeKey))
        {
            if (activePopupLList.Count > 0)
            {
                ClosePopup(activePopupLList.First.Value);
            }
            else
            {
                OpenSettingPopup();
            }
        }

        // 단축키 조작
        ToggleKeyDownAction(labKey,  labPopup);
        ToggleKeyDownAction(storeKey,  storePopup);
    }
    
    //단축 키 입력에 따라 팝업 열고 닫기
    private void ToggleKeyDownAction(in KeyCode key, PopupUI popup)
    {
        if (Input.GetKeyDown(key))
        {
            ToggleOpenClosePopup(popup);
        }
    }
    
    //팝업 상태에 따라 열고 닫기
    private void ToggleOpenClosePopup(PopupUI popup)
    {
        if (!popup.gameObject.activeSelf) OpenPopup(popup);
        else ClosePopup(popup);
    }
    
    // 팝업 가져오기
    public PopupUI GetPopup(PopupType type)
    {
        switch (type)
        {
            case PopupType.BuildingUpgrade:
                return buildingUpgradePopup;
            case PopupType.Encounter:
                return EncounterPopup;
            case PopupType.Store:
                return storePopup;
            case PopupType.Lab:
                return labPopup;
            case PopupType.Deck:
                return deckPopup;
            default:
                Debug.LogWarning("Invalid PopupType requested.");
                return null;
        }
    }
    
    //팝업을 열고 LinkedList 상단의 추가
    public void OpenPopup(PopupUI popup)
    {
        activePopupLList.AddFirst(popup);
        popup.gameObject.SetActive(true);
        RefreshAllPopupDepth();
    }
    
    //팝업을 닫고 LinkedList에서 제거
    public void ClosePopup(PopupUI popup)
    {
        activePopupLList.Remove(popup);
        popup.gameObject.SetActive(false);
        RefreshAllPopupDepth();
    }
    
    //LinkedList내 모든 팝업의 자식 순서 재배치
    private void RefreshAllPopupDepth()
    {
        foreach (var popup in activePopupLList)
        {
            popup.transform.SetAsFirstSibling();
        }
    }
    
    private void OpenSettingPopup()
    {
        // if (_SettingPopup != null)
        // {
        //     _SettingPopup.gameObject.SetActive(true);
        //     _activePopupLList.AddFirst(_SettingPopup);
        //     RefreshAllPopupDepth();
        // }
    }

    public void CloseSettingPopup()
    {
        // _activePopupLList.Remove(_SettingPopup);
        // _SettingPopup.gameObject.SetActive(false);
        // RefreshAllPopupDepth();
    }

  
} // end class
