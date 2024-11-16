using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using UnityEngine;

/// <summary>
/// Encounter DB와 커넥션 하는 Service
/// </summary>
public class EncounterService
{
    private SQLiteConnection dbConnection;

    public EncounterService()
    {
        dbConnection = DatabaseManager.Instance.GetConnection();
    }

    /// <summary>
    /// EncounterData의 모든 인카운터 ID를 DB에 저장
    /// </summary>
    public bool CreateEncounters()
    {
        EncounterData encounterData = MainMenuManger.Instance.encounterData;

        try
        {
            // 모든 Encounter ID를 DB에 삽입
            foreach (Encounter encounter in encounterData.encounters)
            {
                var encounterModel = new EncounterModel
                {
                    EncounterId = encounter.encounterId
                };

                dbConnection.Insert(encounterModel);
            }
            
            Debug.Log("All encounters saved to DB.");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to insert encounters: " + ex.Message);
            return false;
        }
    }
    
    /// <summary>
    /// DB에서 모든 Encounter ID를 가지고와 EncounterData와 비교해서 맞는 EncounterList 반환
    /// </summary>
    public List<Encounter> GetEncounters()
    {
        try
        {
            // DB에서 모든 Encounter ID 가져오기
            List<EncounterModel> encounterModels = dbConnection.Table<EncounterModel>().ToList();
        
            // EncounterData와 비교하여 일치하는 Encounter 객체만 반환
            List<Encounter> matchedEncounters = EncounterManager.Instance.EncounterData.encounters
                .Where(encounter => encounterModels.Any(em => em.EncounterId == encounter.encounterId))
                .ToList();
        
            Debug.Log($"Retrieved {matchedEncounters.Count} matching encounters from the database.");
           
            return matchedEncounters;
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to retrieve encounters from the database: " + ex.Message);
            return null;
        }
    }
    
    /// <summary>
    /// 기존 인카운터 리스트들을 삭제하고, 인카운터 리스트를 저장하기
    /// </summary>
    /// <param name="encounters"></param>
    /// <returns></returns>
    public bool UpdateEncounters(List<Encounter> encounters)
    {
        try
        {
            // 기존 인카운터 데이터를 모두 삭제
            dbConnection.Execute("DELETE FROM encounter");

            // 새로운 인카운터 리스트 삽입
            foreach (Encounter encounter in encounters)
            {
                var encounterModel = new EncounterModel
                {
                    EncounterId = encounter.encounterId
                };

                dbConnection.Insert(encounterModel);
            }
        
            Debug.Log("Encounters updated successfully in the database.");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to update encounters: " + ex.Message);
            return false;
        }
    }

    public bool DeleteEncounters()
    {
        try
        {
            // 인카운터 테이블의 모든 데이터 삭제
            dbConnection.Execute("DELETE FROM encounter");
            
            Debug.Log("All encounter data deleted from the database.");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Failed to delete encounter data: " + ex.Message);
            return false;
        }
    }
} // end class
