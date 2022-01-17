using UnityEditor;
using UnityEngine;
 
public class CustomTools : EditorWindow
{
    [MenuItem("Window/Custom Tools/Enable")]
    public static void Enable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }
 
    [MenuItem("Window/Custom Tools/Disable")]
    public static void Disable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }
 
    private static void OnSceneGUI(SceneView sceneview)
    {
        Handles.BeginGUI();
 
        if (GUILayout.Button("Button"))
            Debug.Log("Button Clicked");
 
        Handles.EndGUI();
    }
}