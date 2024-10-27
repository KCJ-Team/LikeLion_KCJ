using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CSV_Data_Paser : EditorWindow
{   
    private string csvFilePath = "";
    private string outputFolder = "Assets/ScriptableObjects";
    private string fileName = "CSVDataContainer";

    [MenuItem("Tools/CSV to ScriptableObject Converter %#C")] // Ctrl+Shift+C (Windows) or Cmd+Shift+C (Mac)
    public static void ShowWindow()
    {
        GetWindow<CSV_Data_Paser>("CSV Converter");
    }

    private void OnGUI()
    {
        GUILayout.Label("CSV to ScriptableObject Converter", EditorStyles.boldLabel);

        csvFilePath = EditorGUILayout.TextField("CSV File Path", csvFilePath);
        if (GUILayout.Button("Select CSV File"))
        {
            csvFilePath = EditorUtility.OpenFilePanel("Select CSV File", "", "csv");
        }

        outputFolder = EditorGUILayout.TextField("Output Folder", outputFolder);
        if (GUILayout.Button("Select Output Folder"))
        {
            outputFolder = EditorUtility.SaveFolderPanel("Select Output Folder", "Assets", "");
            outputFolder = outputFolder.Replace(Application.dataPath, "Assets");
        }

        fileName = EditorGUILayout.TextField("File Name", fileName);

        if (GUILayout.Button("Convert CSV to ScriptableObject"))
        {
            ConvertCSVToScriptableObject();
        }
    }

    private void ConvertCSVToScriptableObject()
    {
        if (string.IsNullOrEmpty(csvFilePath) || !File.Exists(csvFilePath))
        {
            Debug.LogError("Invalid CSV file path.");
            return;
        }

        string[] lines = File.ReadAllLines(csvFilePath);
        if (lines.Length < 2)
        {
            Debug.LogError("CSV file is empty or has no data rows.");
            return;
        }

        CSVDataContainer container = ScriptableObject.CreateInstance<CSVDataContainer>();
        container.headers = lines[0].Split(',');
        container.dataRows = new List<CSVDataRow>();

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            if (values.Length != container.headers.Length)
            {
                Debug.LogWarning($"Skipping row {i} due to mismatched column count.");
                continue;
            }

            CSVDataRow row = new CSVDataRow();
            row.values = values;
            container.dataRows.Add(row);
        }

        string assetPath = $"{outputFolder}/{fileName}.asset";
        AssetDatabase.CreateAsset(container, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"CSV conversion completed. ScriptableObject saved at {assetPath}");
    }
}

[System.Serializable]
public class CSVDataRow
{
    public string[] values;
}

public class CSVDataContainer : ScriptableObject
{
    public string[] headers;
    public List<CSVDataRow> dataRows;

    public string GetValue(int rowIndex, string headerName)
    {
        if (rowIndex < 0 || rowIndex >= dataRows.Count)
        {
            Debug.LogError($"Row index {rowIndex} is out of range.");
            return null;
        }

        int columnIndex = System.Array.IndexOf(headers, headerName);
        if (columnIndex == -1)
        {
            Debug.LogError($"Header '{headerName}' not found.");
            return null;
        }

        return dataRows[rowIndex].values[columnIndex];
    }
}