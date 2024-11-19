using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SQLite;
using UnityEngine;

public class InventoryService
{
    private SQLiteConnection dbConnection;

    public InventoryService()
    {
        dbConnection = DatabaseManager.Instance.GetConnection();
    }

    /// <summary>
    /// 캐릭터 생성시 기본 인벤토리 아이템들을 cardDatabase에 가져와서, 넣어주고, 인벤과 SO를 초기화한후 인벤만 집어넣는다.
    /// </summary>
    public bool InitialInventoryAndEquipment()
    {
        CardDatabase cardDatabase = MainMenuManger.Instance.CardDatabase;

        try
        {
            // 노말 권총
            int normalPisotlNum = 0;

            InventoryModel inventoryModel = new InventoryModel()
            {
                InventoryId = normalPisotlNum
            };

            dbConnection.Insert(inventoryModel);
            
            // 기본 스킬 카드 추가 (ID: 12 ~ 16)
            for (int skillId = 12; skillId <= 16; skillId++)
            {
                InventoryModel skillCard = new InventoryModel
                {
                    InventoryId = skillId,
                };
                
                dbConnection.Insert(skillCard);
                Debug.Log($"Skill card {skillId} added to inventory.");
            }
            
            // 버프 카드 추가 (ID: 21 ~ 24)
            for (int buffId = 21; buffId <= 24; buffId++)
            {
                InventoryModel buffCard = new InventoryModel
                {
                    InventoryId = buffId,
                };
                dbConnection.Insert(buffCard);
                Debug.Log($"Buff card {buffId} added to inventory.");
            }
            
            // 인벤토리 테이블에 카드를 추가했으면 곧바로 가지고 와서, 
            // 전체 인벤토리를 가져옴
            var inventoryList = dbConnection.Table<InventoryModel>().ToList();
            
            // 기존 인벤토리, 장비창 초기화
            PlayerData playerData = MainMenuManger.Instance.playerData;
            playerData.inventory.Clear();
            playerData.equipment.Clear();
            
            Debug.Log("Player inventory cleared.");
            
            // 각 InventoryModel을 Card로 변환하고 플레이어 인벤토리에 추가
            foreach (var inventoryItem in inventoryList)
            {
                // CardObject를 Card로 변환
                CardObject cardObject = cardDatabase.CardObjects[inventoryItem.InventoryId];
                if (cardObject != null)
                {
                    Card card = new Card(cardObject);

                    bool result = playerData.inventory.AddItem(card, 1); // 수량은 1로 설정
                    Debug.Log($"{result} 결과! Added card {cardObject.name} (ID: {card.Id}) to player inventory.");
                }
                else
                {
                    Debug.LogWarning($"CardObject with ID {inventoryItem.InventoryId} not found in CardDatabase.");
                }
            }
            
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("인벤토리 저장 안됨!");
            return false;
        }
    }
    
    /// <summary>
    /// 전체 인벤토리를 가져오는 메서드
    /// </summary>
    /// <returns>List of InventoryModel</returns>
    public List<InventoryModel> GetAllInventory()
    {
        try
        {
            // 데이터베이스에서 모든 인벤토리 레코드를 가져옴
            var inventoryList = dbConnection.Table<InventoryModel>().ToList();
            CardDatabase cardDatabase = LobbyMenuManager.Instance.cardDatabase;
            
            // 각 InventoryModel을 Card로 변환하고 플레이어 인벤토리에 추가
            foreach (var inventoryItem in inventoryList)
            {
                // CardObject를 Card로 변환
                CardObject cardObject = cardDatabase.CardObjects[inventoryItem.InventoryId]; // 맞추지 않앗나?
                if (cardObject != null)
                {
                    Card card = new Card(cardObject);
                    
                    bool result = LobbyMenuManager.Instance.playerData.inventory.AddItem(card, 1); // 수량은 1로 설정
                    Debug.Log($"{result} 결과 ! Added card {cardObject.name} (ID: {card.Id}) to player inventory.");
                }
                else
                {
                    Debug.LogWarning($"CardObject with ID {inventoryItem.InventoryId} not found in CardDatabase.");
                }
            }
            
            Debug.Log($"Loaded {inventoryList.Count} inventory items.");
            return inventoryList;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error fetching inventory: {e.Message}");
            return new List<InventoryModel>();
        }
    }

    // TODO : id가 cardDataBase의 id가 맞느지 확인필요
    public void UpdateInventory()
    {
        try
        {
            // 기존 InventoryModel 데이터 삭제
            dbConnection.Execute("DELETE FROM inventory");
            Debug.Log("Cleared old inventory data from the database.");

            // PlayerData에서 inventory 가져오기
            var inventorySlots = LobbyMenuManager.Instance.playerData.inventory.GetSlots;

            foreach (var slot in inventorySlots)
            {
                //if (slot.card.Id >= 0) // 유효한 카드만 처리
               // {
                    InventoryModel inventoryItem = new InventoryModel
                    {
                        InventoryId = slot.card.Id // 카드의 ID를 InventoryModel로 저장
                    };
                    
                    dbConnection.Insert(inventoryItem);
                    Debug.Log($"Saved card with ID {slot.card.Id} to database.");
               // }
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error updating inventory: {e.Message}");
        }
    }
} // end class
