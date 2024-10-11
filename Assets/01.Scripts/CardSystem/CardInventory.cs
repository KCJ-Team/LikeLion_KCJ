/*using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class CardInventory : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform viewPortTransform;
    public RectTransform contentPanelTransform;
    public HorizontalLayoutGroup HLG;

    public PlayerInventory playerInventory;
    public GameObject cardPrefab;
    public GameObject emptyCardPrefab; // 빈 카드를 위한 프리팹

    private List<RectTransform> ItemList = new List<RectTransform>();

    private Vector2 OldVelocity;
    private bool isUpdated;

    private void Start()
    {
        OldVelocity = Vector2.zero;
        isUpdated = false;
        
        InitializeItemList();
        
        if (ItemList.Count == 0)
        {
            HandleEmptyInventory();
            return;
        }

        int ItemsToAdd = Mathf.CeilToInt(viewPortTransform.rect.width / (ItemList[0].rect.width + HLG.spacing));

        for (int i = 0; i < ItemsToAdd; i++)
        {
            RectTransform RT = Instantiate(ItemList[i % ItemList.Count], contentPanelTransform);
            RT.SetAsLastSibling();
        }
        
        for (int i = 0; i < ItemsToAdd; i++)
        {
            int num = ItemList.Count - i - 1;

            while (num < 0)
            {
                num += ItemList.Count;
            }
            
            RectTransform RT = Instantiate(ItemList[num], contentPanelTransform);
            RT.SetAsFirstSibling();
        }

        contentPanelTransform.localPosition = new Vector3((0 - (ItemList[0].rect.width + HLG.spacing) * ItemsToAdd),
            contentPanelTransform.localPosition.y,
            contentPanelTransform.localPosition.z);
    }

    private void InitializeItemList()
    {
        if (playerInventory.cards == null || playerInventory.cards.Count == 0)
        {
            Debug.Log("Player inventory is empty.");
            return;
        }

        foreach (Card card in playerInventory.cards)
        {
            GameObject cardObj = Instantiate(cardPrefab, contentPanelTransform);
            Card cardComponent = cardObj.GetComponent<Card>();
            cardComponent.sprite = card.sprite;
            cardComponent.cardType = card.cardType;
            cardComponent.InitCard();
            ItemList.Add(cardObj.GetComponent<RectTransform>());
        }
    }

    private void HandleEmptyInventory()
    {
        Debug.Log("Handling empty inventory");
        GameObject emptyCard = Instantiate(emptyCardPrefab, contentPanelTransform);
        RectTransform emptyCardRT = emptyCard.GetComponent<RectTransform>();
        ItemList.Add(emptyCardRT);

        // 뷰포트를 채우기 위해 빈 카드를 복제
        int ItemsToAdd = Mathf.CeilToInt(viewPortTransform.rect.width / (emptyCardRT.rect.width + HLG.spacing));
        for (int i = 1; i < ItemsToAdd; i++)
        {
            RectTransform RT = Instantiate(emptyCardRT, contentPanelTransform);
            RT.SetAsLastSibling();
        }

        // 스크롤 기능 비활성화
        scrollRect.enabled = false;
    }

    private void Update()
    {
        if (ItemList.Count == 0) return; // 아이템이 없으면 업데이트 로직 스킵

        if (isUpdated)
        {
            isUpdated = false;
            scrollRect.velocity = OldVelocity;
        }
        
        if (contentPanelTransform.localPosition.x > 0)
        {
            Canvas.ForceUpdateCanvases();
            OldVelocity = scrollRect.velocity;
            contentPanelTransform.localPosition -= new Vector3(ItemList.Count * (ItemList[0].rect.width + HLG.spacing), 0, 0);
            isUpdated = true;
        }

        if (contentPanelTransform.localPosition.x < 0 - (ItemList.Count * (ItemList[0].rect.width + HLG.spacing)))
        {
            Canvas.ForceUpdateCanvases();
            OldVelocity = scrollRect.velocity;
            contentPanelTransform.localPosition += new Vector3(ItemList.Count * (ItemList[0].rect.width + HLG.spacing), 0, 0);
            isUpdated = true;
        }
    }
}*/