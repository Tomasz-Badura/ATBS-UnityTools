using UnityEngine;
using UnityEditor;
using System.IO;

public class CsvConverterWindow : EditorWindow
{
    private string delimiter = ",";
    private string filePath = "";
    private string savePath = "";
    private ScriptableObject converter; 

    [MenuItem("CsvSystem/Converter")]
    static void OpenWindow()
    {
        Debug.Log(Application.dataPath);
        CsvConverterWindow window = (CsvConverterWindow)GetWindow(typeof(CsvConverterWindow));
        window.minSize = new Vector2(400, 600);
        window.maxSize = new Vector2(400, 600);
        window.Show();
    }

    void OnGUI()
    {
        // DELIMITER FIELD
        delimiter = EditorGUILayout.TextField("Delimiter", delimiter);
        // SAVEPATH FIELD
        savePath = EditorGUILayout.TextField("Save path", savePath);
        // FILEPATH FIELD
        filePath = EditorGUILayout.TextField("File path", filePath);
        // CONVERTER FIELD
        converter = (ScriptableObject) EditorGUILayout.ObjectField("SoConverter", converter, typeof(ScriptableObject), false);
        
        // DELIMITER CHECK
        if(string.IsNullOrEmpty(delimiter) || delimiter.Length > 1)
        {
            EditorGUILayout.HelpBox("Delimiter must include one letter / character.", MessageType.Warning);
        }
        // CONVERTER CHECK
        else if((ISOConverter) this.converter == null)
        {
            EditorGUILayout.HelpBox("Converter ScriptableObject must implement ISOConverter interface", MessageType.Warning);
        }
        // IMPORT BUTTON
        else if(GUILayout.Button("Import", GUILayout.Width(Screen.width), GUILayout.Height(50)))
        {
            SaveSO((ISOConverter) this.converter);
        }
    }

    private void SaveSO(ISOConverter converter)
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + "/" + filePath);
        foreach(string s in allLines)
        {
            string[] data = s.Split(delimiter);
            ScriptableObject so = converter.Convert(data);
            AssetDatabase.CreateAsset(so, $"Assets/{savePath}/{data[0]}.asset");
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}