using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class copyFileToClipBoard : EditorWindow
{
    [MenuItem("Custom/Independent Functions/Copy File to Clipboard")]
    public static void _copyFileToClipBoard() {
        string[] currentSelection = Selection.assetGUIDs;

        foreach (string select in currentSelection)
        {
            string fileSystemPath = Application.dataPath.Replace("Assets","") + AssetDatabase.GUIDToAssetPath(select);

            GUIUtility.systemCopyBuffer = fileSystemPath;
        }
    }
}
