using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuicklyEvent : EditorWindow
{
    [MenuItem("Tools/QuicklyTools/ShowQuicklyInfo")]
    public static void ShowQuicklyInfo()
    {
        var window = GetWindow<QuicklyEvent>();
        window.Show();
    }
    
    void OnGUI()
    {
        if(GUILayout.Button("Open Read Me"))
            OpenReadmeFile();
    }

    [MenuItem("Tools/QuicklyTools/OpenReadmeFile #o")]
    public static void OpenReadmeFile()
    {
        string readMePath = string.Format("{0}/README.md",Application.dataPath.Remove(Application.dataPath.Length - 7,7));
        EditorUtility.OpenWithDefaultApp(readMePath);
    }
}
