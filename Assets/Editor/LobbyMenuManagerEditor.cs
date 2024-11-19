using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LobbyMenuManager))]
public class LobbyMenuManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 대상 객체 가져오기
        LobbyMenuManager lobbyMenuManager = (LobbyMenuManager)target;

        // 기본 인스펙터 표시
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Custom Controls", EditorStyles.boldLabel);

        // HP 슬라이더
        EditorGUILayout.LabelField("HP", EditorStyles.boldLabel);
        lobbyMenuManager.hp = EditorGUILayout.Slider("HP Value", lobbyMenuManager.hp, 0.0f, lobbyMenuManager.playerData.BaseHP);
        if (GUI.changed)
        {
            lobbyMenuManager.SetHpAndStress(lobbyMenuManager.hp, lobbyMenuManager.stress);
        }

        // Stress 슬라이더
        EditorGUILayout.LabelField("Stress", EditorStyles.boldLabel);
        lobbyMenuManager.stress = EditorGUILayout.Slider("Stress Value", lobbyMenuManager.stress, 0.0f, 100.0f);
        if (GUI.changed)
        {
            lobbyMenuManager.SetHpAndStress(lobbyMenuManager.hp, lobbyMenuManager.stress);
        }

        // Attack 슬라이더
        EditorGUILayout.LabelField("Attack", EditorStyles.boldLabel);
        lobbyMenuManager.attack = EditorGUILayout.FloatField("Attack Value", lobbyMenuManager.attack);
        if (GUI.changed)
        {
            //lobbyMenuManager.lobbyMenuUIPresenter.UpdateAttackAndDefenseUI();
        }

        // Defense 슬라이더
        EditorGUILayout.LabelField("Defense", EditorStyles.boldLabel);
        lobbyMenuManager.defense = EditorGUILayout.FloatField("Defense Value", lobbyMenuManager.defense);
        if (GUI.changed)
        {
            //lobbyMenuManager.lobbyMenuUIPresenter.UpdateAttackAndDefenseUI();
        }

        // 값이 변경된 경우 실시간 반영
        if (GUI.changed)
        {
            EditorUtility.SetDirty(lobbyMenuManager); // 변경 사항 저장
        }
    }
}