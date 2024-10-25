using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class CSVToScriptableObjectConverter : AssetPostprocessor
{
    private static readonly string csvFolderPath = "Assets/CSV";
    private static readonly string outputFolderPath = "Assets/SO";

    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        Debug.Log("CSV files converted");
        
        foreach (string assetPath in importedAssets)
        {
            if (Path.GetExtension(assetPath).ToLower() == ".csv" && assetPath.StartsWith(csvFolderPath))
            {
                ConvertCSVToScriptableObject(assetPath);
            }
        }
    }

    private static void ConvertCSVToScriptableObject(string csvPath)
    {
        string fileName = Path.GetFileNameWithoutExtension(csvPath);
        string outputPath = $"{outputFolderPath}/{fileName}.asset";

        if (!Directory.Exists(outputFolderPath))
        {
            Directory.CreateDirectory(outputFolderPath);
        }

        CSVDataContainer2 container = AssetDatabase.LoadAssetAtPath<CSVDataContainer2>(outputPath);
        if (container == null)
        {
            container = ScriptableObject.CreateInstance<CSVDataContainer2>();
            AssetDatabase.CreateAsset(container, outputPath);
        }

        string[] lines = File.ReadAllLines(csvPath);
        if (lines.Length < 2)
        {
            Debug.LogError($"CSV file {csvPath} is empty or has no data rows.");
            return;
        }

        container.headers = lines[0].Split(',');
        container.dataRows = new List<CSVDataRow2>();

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            if (values.Length != container.headers.Length)
            {
                Debug.LogWarning($"Skipping row {i} in {csvPath} due to mismatched column count.");
                continue;
            }

            CSVDataRow2 row = new CSVDataRow2();
            row.index = Int32.Parse(values[0]);
            row.hp = Int32.Parse(values[1]);
            row.mp = Int32.Parse(values[2]);
            row.attackPower = Int32.Parse(values[3]);
            container.dataRows.Add(row);
        }

        EditorUtility.SetDirty(container);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"CSV conversion completed for {csvPath}. ScriptableObject saved at {outputPath}");
    }
}

[System.Serializable]
public class CSVDataRow2
{
    public int index;
    public int hp;
    public int mp;
    public int attackPower;
}

public class CSVDataContainer2 : ScriptableObject
{
    public string[] headers;
    public List<CSVDataRow2> dataRows;
}