using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreManager : SceneSingleton<StoreManager>
{
    public Action<Store> OnItemPurchased;
    private Store currentStore;
    
    /// <summary>
    /// Buy버튼을 눌렀을때, 구매 팝업을 띄우기
    /// </summary>
    public void OnBuyButtonClicked(Store storeItem)
    {
        PopupUI popup = PopupUIManager.Instance.GetPopup(PopupType.Noti);
        
        // 기본 UI 요소 설정
        popup.SetContent("Description", "카드를 구매하시겠습니까?");
        popup.SetContent("ButtonOkText", "BUY");
        popup.contentObjs["Currency"].SetActive(true);
        
        // 구매시도시 재화를 검사해서 충분치 않으면 xx
        bool canBuy = GameResourceManager.Instance.IsResourceSufficient(ResourceType.Currency, storeItem.cost);
        popup.contentObjs["ButtonOk"].SetActive(canBuy);

        currentStore = storeItem;
        
        // 팝업 열기
        PopupUIManager.Instance.OpenPopup(popup);
    }

  
    /// <summary>
    /// 팝업에서 Buy 버튼을 눌렀을때
    /// </summary>
    public void OnBuyCard()
    {
        // 가격 -200 을 빼고, 인벤토리에 추가한다.
        if (currentStore != null)
        {
            if (GameResourceManager.Instance.ConsumeResource(ResourceType.Currency, currentStore.cost))
            {
                // 인벤토리에 추가
                // 추가할때 슬롯을 만들어줘야함..! 
                LobbyMenuManager.Instance.playerData.inventory.AddItem(currentStore.storeCard.cardData, 1);

                // 상점에서 지우기, 오브젝트도 지워줘야함..
                LobbyMenuManager.Instance.storeData.RemoveItem(currentStore);
                OnItemPurchased?.Invoke(currentStore);
                
                // 팝업닫기
                PopupUI popup = PopupUIManager.Instance.GetPopup(PopupType.Noti);
                PopupUIManager.Instance.ClosePopup(popup);
                
                // 상위 팝업 상점 팝업도 닫기
                PopupUI popupStore = PopupUIManager.Instance.GetPopup(PopupType.Store);
                PopupUIManager.Instance.ClosePopup(popupStore);
            }
        }
    }
}
