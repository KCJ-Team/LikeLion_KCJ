using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class CSVToScriptableObject : MonoBehaviour
{
    // CSV 파일을 ScriptableObject로 변환하는 메서드
    public static void ConvertCSV<T>(string csvFileName, string outputFolder) where T : ScriptableObject {
        string filePath = $"Assets/Resources/Data/{csvFileName}.csv"; // CSV 파일 경로
        string[] lines = File.ReadAllLines(filePath);

        // CSV 첫 줄을 헤더로 간주하고, 데이터를 읽어온다
        for (int i = 1; i < lines.Length; i++) {
            string[] row = lines[i].Split(',');

            // ScriptableObject 생성
            T scriptableObject = ScriptableObject.CreateInstance<T>();

            // CSV 데이터를 ScriptableObject에 반영 (반사 사용)
            FieldInfo[] fields = typeof(T).GetFields();
            for (int j = 0; j < fields.Length; j++) {
                if (fields[j].FieldType == typeof(int)) {
                    fields[j].SetValue(scriptableObject, int.Parse(row[j]));
                } else if (fields[j].FieldType == typeof(float)) {
                    fields[j].SetValue(scriptableObject, float.Parse(row[j]));
                } else if (fields[j].FieldType.IsEnum) {
                    // 열거형인 경우 문자열을 Enum으로 변환
                    object enumValue = Enum.Parse(fields[j].FieldType, row[j]);
                    fields[j].SetValue(scriptableObject, enumValue);
                } else {
                    fields[j].SetValue(scriptableObject, row[j]);
                }
            }

            // ScriptableObject 저장
            string assetPath = $"{outputFolder}/{row[2]}.asset"; // 타입 이름을 사용해 파일명 설정
            AssetDatabase.CreateAsset(scriptableObject, assetPath);
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"CSV to ScriptableObject conversion completed for {csvFileName}");
    }
}
