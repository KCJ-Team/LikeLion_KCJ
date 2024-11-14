using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SQLite;
using UnityEngine;

public class DatabaseManager : DD_Singleton<DatabaseManager>
{
    private static DatabaseManager _instance;
    public static DatabaseManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // DatabaseManager 인스턴스가 존재하지 않으면 생성
                GameObject obj = new GameObject("DatabaseManager");
                _instance = obj.AddComponent<DatabaseManager>();
                DontDestroyOnLoad(obj);
            }
            return _instance;
        }
    }

    private SQLiteConnection dbConnection;
    private const string DB_NAME = "kcjdb.db";

    private void Awake()
    {
        // Singleton pattern: 중복되는 인스턴스가 있으면 파괴
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않음
            InitializeDatabase();
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // 기존 인스턴스가 있으면 새로운 인스턴스는 파괴
        }
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

        // string persistentDbPath = Path.Combine(Application.persistentDataPath, DB_NAME);
        // string streamingDbPath = Path.Combine(Application.streamingAssetsPath, DB_NAME);
        //
        // // 매번 StreamingAssets에서 복사하여 덮어쓰기
        // File.Copy(streamingDbPath, persistentDbPath, true);
        // Debug.Log("Database copied to persistentDataPath.");
        //
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
