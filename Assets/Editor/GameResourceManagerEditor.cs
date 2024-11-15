using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameResourceManager))]
public class GameResourceManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // "에디터 변경" Header 추가
        EditorGUILayout.Space(); // 위에 여백 추가
        EditorGUILayout.LabelField("에디터에서 값 변경", EditorStyles.boldLabel); // Header 볼드체로 추가
        EditorGUILayout.Space(); // 아래 여백 추가
        
        DrawDefaultInspector(); // 기본 인스펙터 그리기

        GameResourceManager manager = (GameResourceManager)target;

        // resources 딕셔너리를 순회하여 각 GameResource의 CurrentAmount를 인스펙터에서 실시간 수정 가능하게 설정
        foreach (var keyValuePair in manager.resources)
        {
            GameResource resource = keyValuePair.Value;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(resource.ResourceData.resourceName);

            // CurrentAmount 편집 가능 필드 설정
            int newAmount = EditorGUILayout.IntField(resource.CurrentAmount);

            if (newAmount != resource.CurrentAmount)
            {
                // GameResourceManager 객체에 Undo 적용
                Undo.RecordObject(manager, $"Change {resource.ResourceData.resourceName} Amount");

                // CurrentAmount를 범위 내에서 설정하고 변경된 내용 저장
                resource.CurrentAmount = Mathf.Clamp(newAmount, 0, resource.ResourceData.maxAmount);

                // 변경 사항을 Unity에 알림
                EditorUtility.SetDirty(manager);

                // UI 업데이트 메서드 호출로 변경 사항 적용
                manager.SetResourceAmount(keyValuePair.Key, resource.CurrentAmount);
                manager.gameResourceUIPresneter.UpdateResourceUI(); // UI 업데이트 호출
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}