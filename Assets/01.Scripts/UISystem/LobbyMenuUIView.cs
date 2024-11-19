using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMenuUIView : MonoBehaviour
{
    [Header("Down 패널 버튼들")]
    public Button btnProfile;
    public Button btnDeck;
    public Button btnStore;

    [Header("Down 버튼 누르면 열릴 UI 팝업과 패널들")]
    public GameObject profilePanel;
    public GameObject deckPanel;
    // public GameObject inventoryPopup;
    // public GameObject storePopup;
    public PopupUI popupInventory;
    public PopupUI popupStore;

    [Header("HP와 Stress UI")]
    public Slider hpSlider;
    public Text hpText;
    public Slider stressSlider;
    public Text stressText;
    public Text attackText;
    public Text defenseText;

    public void UpdateHpAndStressUI()
    {
        hpSlider.DOValue(LobbyMenuManager.Instance.hp, 0.5f).OnUpdate(() =>
        {
            hpText.text = $"{Mathf.Round(hpSlider.value)}/{LobbyMenuManager.Instance.playerData.BaseHP}";
        });
        
        stressSlider.DOValue(LobbyMenuManager.Instance.stress, 0.5f).OnUpdate(() =>
        {
            stressText.text = $"{Mathf.Round(stressSlider.value)}/100";
        });
    }

    public void UpdateAttackAndDefenseUI()
    {
        attackText.text = $"{LobbyMenuManager.Instance.attack}";
        defenseText.text = $"{LobbyMenuManager.Instance.defense}";
    }

    public void HideAllPanels(PopupUI exceptionPopup = null)
    {
        // deckPanel.SetActive(false);
        // // inventoryPopup.SetActive(false);
        // // storePopup.SetActive(false);
        //
        // PopupUIManager.Instance.ClosePopup(popupInventory);
        // PopupUIManager.Instance.ClosePopup(popupStore);
        
        // 패널 닫기
        deckPanel.SetActive(false);
        profilePanel.SetActive(false);

        // 팝업 닫기 (예외 팝업은 닫지 않음)
        if (popupInventory != exceptionPopup)
            PopupUIManager.Instance.ClosePopup(popupInventory);

        if (popupStore != exceptionPopup)
            PopupUIManager.Instance.ClosePopup(popupStore);
    }

    public void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void ShowPopup(PopupUI popup)
    {
        PopupUIManager.Instance.OpenPopup(popup);
    }
}
