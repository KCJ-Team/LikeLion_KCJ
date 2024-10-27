using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CSVToScriptableObjectEditor : EditorWindow {
    
    [MenuItem("Tools/Convert CSV to ScriptableObject/BuildingData")]
    public static void ConvertBuildingData() 
    {
        CSVToScriptableObject.ConvertCSV<BuildingData>("Building/Buildings", "Assets/Resources/Data/Building"); // CSV 파일명, SO가 생성될 디렉토리
    }
    
    // TODO : 추후 test -> Encounters로 바꾸기. 일단 테스트 데이터로 시험.
    [MenuItem("Tools/Convert CSV to ScriptableObject/EncounterData")]
    public static void ConvertEncounterData() 
    {
        CSVToScriptableObject.ConvertEncounterCSVToList<EncounterData>("TestEncounters", "Encounter/TestEncounters", "Assets/Resources/Data/Encounter"); // CSV 파일명, SO가 생성될 디렉토리
    }
} // end class
