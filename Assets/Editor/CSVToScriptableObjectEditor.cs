using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CSVToScriptableObjectEditor : EditorWindow {
    [MenuItem("Tools/Convert CSV to ScriptableObject/BuildingData")]
    public static void ConvertBuildingData() {
        CSVToScriptableObject.ConvertCSV<BuildingData>("Building/Buildings", "Assets/Resources/Data/Building"); // CSV 파일명, SO가 생성될 디렉토리
    }
}
