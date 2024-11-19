using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum StoreType
{
    Weapon,
    Skill
}

public class StoreInterface : MonoBehaviour
{
    public StoreType storeType;
    public GameObject slotPrefab;
    public Dictionary<Store, GameObject> storeSlots = new();
    public Transform content;
    private Store currentStore;
    
    // Start is called before the first frame update
    void Start()
    {
        // 기존 슬롯 제거
        foreach (var slot in storeSlots.Values)
        {
            Destroy(slot);
        }
        storeSlots.Clear();
        
        // StoreData에서 아이템 가져오기
        StoreData storeData = LobbyMenuManager.Instance.storeData;

        if (storeData == null || storeData.storeItems == null || storeData.storeItems.Count == 0)
        {
            Debug.LogWarning("StoreData is empty or not assigned.");
            return;
        }

        // 스토어 데이터를 순회하며 UI 생성
        foreach (Store storeItem in storeData.storeItems)
        {
            if (storeType == StoreType.Weapon && storeItem.storeCard.type != CardType.Weapon)
                continue;
            if (storeType == StoreType.Skill && storeItem.storeCard.type != CardType.Skill)
                continue;
            
            // 1. 슬롯 프리팹을 Content 하위에 생성
            GameObject slot = Instantiate(slotPrefab, content);

            // 2. 슬롯의 하위에 storeCard 오브젝트 생성
            Transform cardParent = slot.transform.GetChild(0); // 슬롯 프리팹의 자식 중 카드가 들어갈 부모
            GameObject storeCard = Instantiate(storeItem.storeCard.characterPrefab, cardParent);

            // 3. storeCard의 RectTransform 앵커와 피벗 설정
            RectTransform cardRect = storeCard.GetComponent<RectTransform>();
            if (cardRect != null)
            {
                cardRect.anchorMin = new Vector2(0.5f, 0.5f);
                cardRect.anchorMax = new Vector2(0.5f, 0.5f);
                cardRect.pivot = new Vector2(0.5f, 0.5f);
                cardRect.anchoredPosition = Vector2.zero; // 가운데로 위치 조정
            }
            
            // 4. 슬롯의 버튼 가져오기
            Button buyButton = slot.GetComponentInChildren<Button>();
            if (buyButton != null)
            {
                // 현재 StoreItem 정보를 전달하도록 이벤트 등록
                Store currentStoreItem = storeItem;
                buyButton.onClick.AddListener(() => StoreManager.Instance.OnBuyButtonClicked(currentStoreItem));
            }
            
            // 딕셔너리에 추가
            storeSlots.Add(storeItem, slot);
        }

        Debug.Log($"{storeSlots.Count} store items loaded into {storeType} store.");
    }
    
    private void OnEnable()
    {
        // 델리게이트 구독
        StoreManager.Instance.OnItemPurchased += RemoveStoreItem;
    }

    private void OnDisable()
    {
        // 델리게이트 구독 해제
        StoreManager.Instance.OnItemPurchased -= RemoveStoreItem;
    }
    
    /// <summary>
    /// 특정 아이템을 제거하는 메서드
    /// </summary>
    public void RemoveStoreItem(Store storeItem)
    {
        if (storeSlots.TryGetValue(storeItem, out GameObject slotToRemove))
        {
            storeSlots.Remove(storeItem);
            Destroy(slotToRemove);
            Debug.Log($"Removed store item: {storeItem.storeCard.name}");
        }
        else
        {
            Debug.LogWarning($"Store item not found in storeSlots: {storeItem.storeCard.name}");
        }
    }
    
} // end class