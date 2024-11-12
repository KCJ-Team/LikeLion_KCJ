using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameTimeManager))]
public class GameTimeManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameTimeManager gameTimeManager = (GameTimeManager)target;

        // 기본 Inspector UI 표시
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Day Control", EditorStyles.boldLabel);

        // 슬라이더 추가 (0에서 설정된 startDay 값까지 조정 가능)
        int newDay = EditorGUILayout.IntSlider("Current Day", gameTimeManager.currentDay, 0, gameTimeManager.gameTimeSetting.startDay);

        // 슬라이더 값이 변경되었을 때 UI 업데이트
        if (newDay != gameTimeManager.currentDay)
        {
            gameTimeManager.currentDay = newDay;
            gameTimeManager.UpdateDayUI(); // UI 업데이트 호출

            EditorUtility.SetDirty(gameTimeManager); // 변경사항 적용
        }
        
        // EditorGUILayout.Space();
        // EditorGUILayout.LabelField("Time Speed Control", EditorStyles.boldLabel);
        //
        // EditorGUILayout.BeginHorizontal();
        //
        // // 왼쪽 버튼 (<) 원래 속도로 돌아감
        // if (GUILayout.Button("<"))
        // {
        //     gameTimeManager.enableXSpeed = false;
        //     gameTimeManager.ToggleDoubleTimeSpeed();
        //     Debug.Log("Time speed reset to normal.");
        // }
        //
        // // 오른쪽 버튼 (>) 4배속으로 증가
        // if (GUILayout.Button(">"))
        // {
        //     gameTimeManager.enableXSpeed = true;
        //     gameTimeManager.gameTimeSetting.xSpeed = 4;
        //     gameTimeManager.ToggleDoubleTimeSpeed();
        //     Debug.Log("Time speed set to 4x.");
        // }
        //
        // EditorGUILayout.EndHorizontal();
    }
} // end class