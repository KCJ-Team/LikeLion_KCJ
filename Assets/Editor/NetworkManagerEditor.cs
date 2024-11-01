using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NetworkConfig))]
public class NetworkConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NetworkConfig config = (NetworkConfig)target;

        // 볼드체로 "Select Server IP:" 라벨 표시
        EditorGUILayout.LabelField("Select Server IP:", EditorStyles.boldLabel);

        // Server IP Type 선택 옵션과 한 칸의 여백 추가
        config.selectedIPType = (NetworkConfig.IPType)EditorGUILayout.EnumPopup("Server IP Type", config.selectedIPType);
        EditorGUILayout.Space(); // 한 줄 띄우기

        // 선택한 타입에 따라 IP를 설정
        switch (config.selectedIPType)
        {
            case NetworkConfig.IPType.Local:
                config.serverIP = config.localServerIP;
                break;
            case NetworkConfig.IPType.Release:
                config.serverIP = config.releaseServerIP;
                break;
        }

        // 볼드체로 "Current Server IP:" 라벨 표시하고 한 줄 띄우기
        EditorGUILayout.LabelField("Current Server IP:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField(config.serverIP);
        EditorGUILayout.Space(); // 한 줄 띄우기

        // 기본 인스펙터 드로잉
        DrawDefaultInspector();
    }
}