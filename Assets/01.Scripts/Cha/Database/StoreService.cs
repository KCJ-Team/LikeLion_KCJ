using System;
using System.Collections;
using System.Collections.Generic;
using SQLite;
using UnityEngine;

public class StoreService
{
    private SQLiteConnection dbConnection;

    public StoreService()
    {
        dbConnection = DatabaseManager.Instance.GetConnection();
    }
    
    public void InitialStore()
    {
        CardDatabase cardDatabase = MainMenuManager.Instance.CardDatabase;
        
        try
        {
            // 상점은 기본 무기 3개(라이플, 스나이퍼, 샷건) 1,2,3
            for (int i = 1; i <= 3; i++)
            {
                StoreModel store = new StoreModel
                {
                    StoreId = i,
                };
                
                dbConnection.Insert(store);
                Debug.Log($"Skill card {store.StoreId} added to inventory.");
            }
            
            // 궁극기 (4개) 17,18,19,20
            for (int i = 17; i <= 20; i++)
            {
                StoreModel store = new StoreModel
                {
                    StoreId = i,
                };
                
                dbConnection.Insert(store);
                Debug.Log($"Skill card {store.StoreId} added to inventory.");
            }
            
            // 전체 스토어를 가져와서, SO에 넣어주기
            List<StoreModel> storeModels = dbConnection.Table<StoreModel>().ToList();
        
            // 기존 스토어 SO를 초기화
            StoreData storeData = MainMenuManager.Instance.storeData;
            storeData.ClearAllItems();
            
            Debug.Log("StoreData cleared.");
            
            // 각 StoreModel을 id로 찾아 Card로 변환하고 스토어에 추가
            foreach (StoreModel store in storeModels)
            {
                CardObject cardObject = cardDatabase.CardObjects[store.StoreId];
                if (cardObject != null)
                {
                    Card card = new Card(cardObject);

                    Store storeItem = new Store
                    {
                        storeItemId = store.StoreId,
                        storeCard = cardObject
                    };

                    storeData.AddItem(storeItem);
                    
                    Debug.Log($"Added card {cardObject.name} (ID: {card.Id}) to 상점.");
                }
                else
                {
                    Debug.LogWarning($"CardObject with ID {store.StoreId} not found in CardDatabase.");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving equipment: {e.Message}");
        }
    }

    public void DeleteStore()
    {
        try
        {
            // 데이터베이스에서 전체 StoreModel 데이터를 삭제
            dbConnection.Execute("DELETE FROM store");

            // StoreData 초기화
            LobbyMenuManager.Instance.storeData.ClearAllItems();

            Debug.Log("Store data deleted successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Error deleting store: {e.Message}");
        }
    }
} // end class
