using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUIManager : SceneSingleton<PopupUIManager>
{
    /* 팝업 UI
    public PopupUI _inventoryPopup;
    public PopupUI _characterInfoPopup;
    public PopupUI _shopPopup;
    public PopupUI _SettingPopup;
    public PopupUI _SoundPopup;
    */
    
    //키 바인딩 - 예시
    public KeyCode _escapeKey = KeyCode.Escape;
    public KeyCode buildingKey = KeyCode.I;
    public KeyCode incounterKey  = KeyCode.C;
    public KeyCode storeKey = KeyCode.V;
    
    [SerializeField]
    private LinkedList<PopupUI> _activePopupLList;  //실시간 팝업 관리 LinkedList
    [SerializeField]
    private List<PopupUI> _allPopupList;            //전체 팝업 목록 List

    // hyuna
    [Header("PopUIs")]
    public PopupUI buildingUpgradePopup;
    public PopupUI incounterPopup;
    public PopupUI storePopup;
    
    private void Awake()
    {
        base.Awake();
        
        _activePopupLList = new LinkedList<PopupUI>();
        Init();
        Invoke("InitCloseAll", 0.01f);
    }

    private void Update()
    {
        // ESC 누를 경우 링크드리스트의 First 닫기
        if (Input.GetKeyDown(_escapeKey))
        {
            if (_activePopupLList.Count > 0)
            {
                ClosePopup(_activePopupLList.First.Value);
            }
            else
            {
                OpenSettingPopup();
            }
        }

        // 단축키 조작 - 예시
        ToggleKeyDownAction(buildingKey, buildingUpgradePopup);
        ToggleKeyDownAction(incounterKey,  incounterPopup);
        ToggleKeyDownAction(storeKey,  storePopup);
    }
    
    //일반 메소드
    private void Init()
    {
        // 1. 리스트 초기화
        _allPopupList = new List<PopupUI>()
        {
            //_inventoryPopup, _characterInfoPopup, _shopPopup, _SettingPopup, _SoundPopup
            buildingUpgradePopup//, incounterPopup, storePopup
        };

        // 2. 모든 팝업에 이벤트 등록
        foreach (var popup in _allPopupList)
        {
            // 헤더 포커스 이벤트
            popup.OnFocus += () =>
            {
                _activePopupLList.Remove(popup);
                _activePopupLList.AddFirst(popup);
                RefreshAllPopupDepth();
            };

            // 닫기 버튼 이벤트
            popup._closeButton.onClick.AddListener(() => ClosePopup(popup));
        }
        
        OpenPopup(buildingUpgradePopup);
        //OpenPopup(_characterInfoPopup);
    }
    
    //시작 시 모든 팝업 닫기
    private void InitCloseAll()
    {
        foreach (var popup in _allPopupList)
        {
            ClosePopup(popup);
        }
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
    
    //팝업을 열고 LinkedList 상단의 추가
    public void OpenPopup(PopupUI popup)
    {
        _activePopupLList.AddFirst(popup);
        popup.gameObject.SetActive(true);
        RefreshAllPopupDepth();
    }
    
    //팝업을 닫고 LinkedList에서 제거
    private void ClosePopup(PopupUI popup)
    {
        _activePopupLList.Remove(popup);
        popup.gameObject.SetActive(false);
        RefreshAllPopupDepth();
    }
    
    //LinkedList내 모든 팝업의 자식 순서 재배치
    private void RefreshAllPopupDepth()
    {
        foreach (var popup in _activePopupLList)
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
}
