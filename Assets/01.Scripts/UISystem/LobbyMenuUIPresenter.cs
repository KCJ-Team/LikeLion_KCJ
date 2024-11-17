using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMenuUIPresenter
{
    private readonly LobbyMenuUIView uiView;
  
    public LobbyMenuUIPresenter(LobbyMenuUIView uiView)
    {
        this.uiView = uiView;
     
        // 이벤트 등록
        uiView.btnProfile.onClick.AddListener(OnProfileButtonClicked);
        uiView.btnDeck.onClick.AddListener(OnDeckButtonClicked);
        uiView.btnStore.onClick.AddListener(OnStoreButtonClicked);
    }

    private void OnProfileButtonClicked()
    {
        uiView.HideAllPanels();
        uiView.ShowPanel(uiView.profilePanel);
    }
    
    private void OnDeckButtonClicked()
    {
        uiView.HideAllPanels();
        uiView.ShowPanel(uiView.deckPanel);
        uiView.ShowPanel(uiView.inventoryPopup);

        UpdateInventoryAndDeck();
    }

    private void OnStoreButtonClicked()
    {
        uiView.HideAllPanels();
        uiView.ShowPanel(uiView.storePopup);
    }
    
    public void SetHpAndStress(float newHp, float newStress)
    {
        LobbyMenuManager.Instance.hp = Mathf.Clamp(newHp, 0.0f, LobbyMenuManager.Instance.playerData.BaseHP);
        LobbyMenuManager.Instance.stress = Mathf.Clamp(newStress, 0.0f, 100.0f);
        
        uiView.UpdateHpAndStressUI();
    }

    public void ChangeHp(float amount)
    {
        LobbyMenuManager.Instance.hp = Mathf.Clamp(LobbyMenuManager.Instance.hp + amount, 0.0f, LobbyMenuManager.Instance.playerData.BaseHP);
        
        Debug.Log($"ChangeHp: HP changed to {LobbyMenuManager.Instance}");
        uiView.UpdateHpAndStressUI();
    }

    public void ChangeStress(float amount)
    {
        LobbyMenuManager.Instance.stress = Mathf.Clamp(LobbyMenuManager.Instance.stress - amount, 0.0f, 100.0f);
        
        Debug.Log($"ChangeStress: Stress changed to {LobbyMenuManager.Instance.stress}");
        uiView.UpdateHpAndStressUI();
    }

    public void SetAttackAndDefense(float newAttack, float newDefense)
    {
        LobbyMenuManager.Instance.attack = newAttack;
        LobbyMenuManager.Instance.defense = newDefense;

        uiView.UpdateAttackAndDefenseUI();
    }
    
    private void UpdateInventoryAndDeck()
    {
        // 여기에 inventory와 deck 관련 UI 업데이트 로직 추가
        Debug.Log("Inventory and Deck updated.");
        
        // playerData에서 인벤토리 데이터 가져오기
       // LobbyMenuManager.Instance.playerData.inventory.GetSlots[0;

        
        
        
    }
} // end class
