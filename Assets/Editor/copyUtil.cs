using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class copyUtil : EditorWindow
{
    [MenuItem("Custom/Sync Utility")]
    static void Init() => GetWindow<copyUtil>("Sync Utility");

    private void OnEnable() => Selection.selectionChanged += updateSelection;
    private void OnDisable () => Selection.selectionChanged -= updateSelection;

    string currentSelected = "";
    string destProjectPath;

    void updateSelection()
    {
        currentSelected = "";

        foreach (string GUID in Selection.assetGUIDs)
        {
            currentSelected += (AssetDatabase.GUIDToAssetPath(GUID) + "\n");
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Current Selected: ");

 
        GUILayout.TextArea(currentSelected, GUILayout.Height(50));

        if (GUILayout.Button("Set Dest Project")) destProjectPath = EditorUtility.OpenFolderPanel("Select Project Path", "", "");
   
        GUILayout.TextField(destProjectPath);

        if (GUILayout.Button("Copy"))
        {
            //Debug.Log(Application.dataPath);
            foreach (string GUID in Selection.assetGUIDs)
            {
                System.IO.File.Copy(Application.dataPath.Replace("Assets", "") + AssetDatabase.GUIDToAssetPath(GUID), destProjectPath + "/" + AssetDatabase.GUIDToAssetPath(GUID), true);
                
                //FileUtil.CopyFileOrDirectory(AssetDatabase.GUIDToAssetPath(GUID), destProjectPath + "/" + AssetDatabase.GUIDToAssetPath(GUID));
            }

            string lastFile = destProjectPath + "/" + AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[Selection.assetGUIDs.Length - 1]);
       
            //System.Diagnostics.Process.Start("explore.exe", lastFile);
            EditorUtility.RevealInFinder(lastFile);
        }

    }



}
