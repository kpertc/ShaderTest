using UnityEngine;
using UnityEditor;

public class LoggingTest : EditorWindow
{
    //Menu Item + Hot Key + Init Window
    [MenuItem("Custom/EditorWindows/Logings")]
    static void Init() => GetWindow<LoggingTest>("Logings");

    private void OnGUI()
    {
        if (GUILayout.Button("Debug.log"))
            Debug.Log("Debug.log");

        if (GUILayout.Button("Debug.LogWarning"))
            Debug.LogWarning("Debug.LogWarning");

        if (GUILayout.Button("Debug.LogError"))
            Debug.LogError("Debug.LogError");


        GUILayout.Space(40);
        GUILayout.Label("Not Avaliable yet");
        GUILayout.Label("Check out log type for more info");
        GUILayout.Label("https://docs.unity3d.com/ScriptReference/LogType.html");

    
        if (GUILayout.Button("Debug.Assert"))
        { }

        if (GUILayout.Button("Debug.LogException"))
        { }



    }
}
