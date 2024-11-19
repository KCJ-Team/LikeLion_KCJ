using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StoreData", menuName = "Store/Store Data")]
public class StoreData : ScriptableObject
{
    public List<Store> storeItems;
    
    // 전체 스토어 가져오기
    public List<Store> GetAllStoreItems()
    {
        return storeItems; // storeItems의 복사본 반환
    }
    
    public void AddItem(Store item)
    {
        if (item != null && !storeItems.Contains(item))
        {
            storeItems.Add(item);
            Debug.Log($"Item added to store: {item.storeItemId}");
        }
        else
        {
            Debug.LogWarning("Failed to add item: Item is null or already exists in the store.");
        }
    }
    
    public void RemoveItem(Store item)
    {
        if (storeItems.Contains(item))
        {
            storeItems.Remove(item);
            Debug.Log($"Item removed from store: {item.storeItemId}");
        }
        else
        {
            Debug.LogWarning("Failed to remove item: Item not found in the store.");
        }
    }
    
    public void ClearAllItems()
    {
        storeItems.Clear();
        Debug.Log("All items have been cleared from the store.");
    }
    
}
