using System.Collections;
using System.Collections.Generic;
using SQLite;
using UnityEngine;

public class EquipmentService
{
    private SQLiteConnection dbConnection;

    public EquipmentService()
    {
        dbConnection = DatabaseManager.Instance.GetConnection();
    }
    
    /// <summary>
    /// Equipment 테이블에 있는 데이터를 모두 삭제하고 playerData.equipment에 있는 데이터를 저장
    /// </summary>
    public void SaveEquipment()
    {
        try
        {
            // 테이블 초기화
            dbConnection.Execute("DELETE FROM equipment");
            
            // playerData.equipment에서 데이터 저장
            foreach (var equipmentSlot in LobbyMenuManager.Instance.playerData.equipment.GetSlots)
            {
                if (equipmentSlot.card.Id >= 0) // 유효한 카드만 저장
                {
                    EquipmentModel equipmentModel = new EquipmentModel
                    {
                        EquipmentId = equipmentSlot.card.Id
                    };
                    dbConnection.Insert(equipmentModel);
                    Debug.Log($"Saved equipment {equipmentModel.EquipmentId}");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving equipment: {e.Message}");
        }
    }
    
    /// <summary>
    /// Equipment 테이블의 데이터를 playerData.equipment에 로드
    /// </summary>
    public void LoadEquipment()
    {
        try
        {
            var equipmentList = dbConnection.Table<EquipmentModel>().ToList();
            CardDatabase cardDatabase = LobbyMenuManager.Instance.cardDatabase;

            LobbyMenuManager.Instance.playerData.equipment.Clear(); // 기존 데이터를 비움

            foreach (var equipment in equipmentList)
            {
                CardObject cardObject = cardDatabase.CardObjects[equipment.EquipmentId];
                if (cardObject != null)
                {
                    Card card = new Card(cardObject);
                    LobbyMenuManager.Instance.playerData.equipment.GetSlots[equipment.EquipmentId].UpdateSlot(card, 1);
                    Debug.Log($"Loaded equipment {cardObject.name} into slot {equipment.EquipmentId}");
                }
                else
                {
                    Debug.LogWarning($"CardObject with ID {equipment.EquipmentId} not found in CardDatabase.");
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading equipment: {e.Message}");
        }
    }
} // end class
