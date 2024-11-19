using PlayerInfo;
using UnityEditor;
using UnityEngine;

public class PlayerDataEditor : EditorWindow
{
    private PlayerModelType selectedType = PlayerModelType.Male; // 기본값 설정

    [MenuItem("Tools/Player Data Generator")]
    public static void ShowWindow()
    {
        GetWindow<PlayerDataEditor>("Player Data Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("Player Initial Data Generator", EditorStyles.boldLabel);

        // PlayerModelType 선택
        selectedType = (PlayerModelType)EditorGUILayout.EnumPopup("Player Model Type", selectedType);

        // 데이터 저장 버튼
        if (GUILayout.Button("Save Initial Game Data"))
        {
            SaveInitialGameData();
        }
    }

    private void SaveInitialGameData()
    {
        // MainMenuManager 인스턴스 가져오기
        MainMenuManager mainMenuManager = FindObjectOfType<MainMenuManager>();

        if (mainMenuManager == null)
        {
            Debug.LogError("MainMenuManager instance not found in the scene.");
            return;
        }

        // SaveInitGameData 메서드 호출
        bool result = mainMenuManager.SaveInitGameData(selectedType);

        if (result)
        {
            Debug.Log("Player data saved successfully.");
            EditorUtility.DisplayDialog("Success", "Player data has been saved!", "OK");
        }
        else
        {
            Debug.LogError("Failed to save player data.");
            EditorUtility.DisplayDialog("Error", "Failed to save player data.", "OK");
        }
    }
}