using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class CSVToScriptableObject : MonoBehaviour
{
    // CSV 파일을 ScriptableObject로 변환하는 메서드
    public static void ConvertCSV<T>(string csvFileName, string outputFolder) where T : ScriptableObject
    {
        string filePath = $"Assets/Resources/Data/{csvFileName}.csv"; // CSV 파일 경로
        string[] lines = File.ReadAllLines(filePath);

        // CSV 첫 줄을 헤더로 간주하고, 데이터를 읽어온다
        for (int i = 1; i < lines.Length; i++)
        {
            string[] row = lines[i].Split(',');

            // ScriptableObject 생성
            T scriptableObject = ScriptableObject.CreateInstance<T>();

            // CSV 데이터를 ScriptableObject에 반영 (반사 사용)
            FieldInfo[] fields = typeof(T).GetFields();
            for (int j = 0; j < fields.Length; j++)
            {
                if (fields[j].FieldType == typeof(int))
                {
                    fields[j].SetValue(scriptableObject, int.Parse(row[j]));
                }
                else if (fields[j].FieldType == typeof(float))
                {
                    fields[j].SetValue(scriptableObject, float.Parse(row[j]));
                }
                else if (fields[j].FieldType.IsEnum)
                {
                    // 열거형인 경우 문자열을 Enum으로 변환
                    object enumValue = Enum.Parse(fields[j].FieldType, row[j]);
                    fields[j].SetValue(scriptableObject, enumValue);
                }
                else
                {
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


    public static void ConvertEncounterCSVToList<T>(string csvFileName, string csvFolderName, string outputFolder)
    {
         string filePath = $"Assets/Resources/Data/{csvFolderName}.csv"; // CSV 파일 경로
        if (!File.Exists(filePath))
        {
            Debug.LogError($"CSV 파일을 찾을 수 없습니다: {filePath}");
            return;
        }

        string[] lines = File.ReadAllLines(filePath);

        if (lines.Length < 2)
        {
            Debug.LogError("CSV 파일이 비어 있거나 데이터가 부족합니다.");
            return;
        }

        // CSV의 첫 번째 줄에서 필드 이름을 가져옵니다.
        string[] headers = lines[0].Split(',');

        // EncounterData 생성
        EncounterData encounterData = ScriptableObject.CreateInstance<EncounterData>();
        encounterData.encounters = new();

        // 각 줄을 반복하여 Encounter 객체를 생성
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            if (values.Length < 18)
            {
                Debug.LogWarning($"라인 {i}의 데이터가 불완전합니다. 건너뜁니다.");
                continue;
            }
            
            // 데이터 파싱 시 유효성 검사 추가
            int encounterId = 0;
            int.TryParse(values[0], out encounterId);

            float choice1FactionSupport = 0f;
            float.TryParse(values[10], out choice1FactionSupport);

            float choice2FactionSupport = 0f;
            float.TryParse(values[18], out choice2FactionSupport);

            // FactionType 열거형으로 변환
            FactionType choice1Faction = FactionType.None;
            Enum.TryParse(values[9], true, out choice1Faction);

            FactionType choice2Faction = FactionType.None;
            Enum.TryParse(values[17], true, out choice2Faction);

            // Encounter 객체 생성 및 값 설정
            Encounter encounter = new Encounter
            {
                encounterId = encounterId,
                name = values[1],
                description = values[2],
                choice1Text = values[3],
                choice1Result = values[4],
                choice1RewardEnergy = int.TryParse(values[5], out int rewardEnergy1) ? rewardEnergy1 : 0,
                choice1RewardFood = int.TryParse(values[6], out int rewardFood1) ? rewardFood1 : 0,
                choice1RewardFuel = int.TryParse(values[7], out int rewardFuel1) ? rewardFuel1 : 0,
                choice1RewardWorkforce = int.TryParse(values[8], out int rewardWorkforce1) ? rewardWorkforce1 : 0,
                choice1Faction = choice1Faction,
                choice1FactionSupport = choice1FactionSupport,
                choice2Text = values[11],
                choice2Result = values[12],
                choice2RewardEnergy = int.TryParse(values[13], out int rewardEnergy2) ? rewardEnergy2 : 0,
                choice2RewardFood = int.TryParse(values[14], out int rewardFood2) ? rewardFood2 : 0,
                choice2RewardFuel = int.TryParse(values[15], out int rewardFuel2) ? rewardFuel2 : 0,
                choice2RewardWorkforce = int.TryParse(values[16], out int rewardWorkforce2) ? rewardWorkforce2 : 0,
                choice2Faction = choice2Faction,
                choice2FactionSupport = choice2FactionSupport
            };

            // EncounterData의 리스트에 추가
            encounterData.encounters.Add(encounter);
        }

        // ScriptableObject를 에셋으로 저장
        string assetPath = $"{outputFolder}/{csvFileName}.asset";
        AssetDatabase.CreateAsset(encounterData, assetPath);

        // 에셋 데이터베이스 저장 및 갱신
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"{encounterData.encounters.Count}개의 Encounter가 {outputFolder}에 생성된 EncounterData에 저장되었습니다.");
    }
    
} // end class