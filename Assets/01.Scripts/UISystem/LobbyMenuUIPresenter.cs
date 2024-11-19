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
        // 이거 PopupManager에 있는걸로 하면 되잖아 근데..
        
        uiView.HideAllPanels();
        uiView.ShowPanel(uiView.profilePanel);
    }
    
    private void OnDeckButtonClicked()
    {
        uiView.HideAllPanels(uiView.popupInventory);
        uiView.ShowPanel(uiView.deckPanel);
      //  uiView.ShowPanel(uiView.inventoryPopup);
        uiView.ShowPopup(uiView.popupInventory);
    }

    private void OnStoreButtonClicked()
    {
        uiView.HideAllPanels(uiView.popupStore);
        // uiView.ShowPanel(uiView.storePopup);
        uiView.ShowPopup(uiView.popupStore);
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
} // end class
