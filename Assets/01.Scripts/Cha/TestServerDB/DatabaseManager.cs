using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SQLite;
using UnityEngine;

public class DatabaseManager : SceneSingleton<DatabaseManager>
{
    private SQLiteConnection dbConnection;
    private const string DB_NAME = "kcjdb.db";
    
    private void Start()
    {
        InitializeDatabase();
    }
    
    private void InitializeDatabase()
    {
        string persistentDbPath = Path.Combine(Application.persistentDataPath, DB_NAME);
        
        // DB가 존재하지 않으면 StreamingAssets에서 복사
        if (!File.Exists(persistentDbPath))
        {
            string streamingDbPath = Path.Combine(Application.streamingAssetsPath, DB_NAME);
            File.Copy(streamingDbPath, persistentDbPath);
            Debug.Log("Database copied to persistentDataPath.");
        }

        // DB 연결
        dbConnection = new SQLiteConnection(persistentDbPath);
        
        // DB 연결 성공 로그
        if (dbConnection != null)
        {
            Debug.Log("Database connection successful: " + persistentDbPath);
        }
        else
        {
            Debug.LogError("Failed to connect to the database.");
        }
    }
    
    public SQLiteConnection GetConnection()
    {
        return dbConnection;
    }

    private void OnDestroy()
    {
        dbConnection?.Close();
    }
} // end class
