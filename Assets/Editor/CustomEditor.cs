using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomEditor : EditorWindow
{
    //Menu Item + Hot Key + Init Window
    [MenuItem("Custom/Custom")]
    static void Init() => GetWindow<CustomEditor>("The Title");

    public enum things 
    {
        cat,dog,fish
    }
    
    things mythings;

    string Car_ID = "12345";

    private void OnGUI()
    {
        
        using (new GUILayout.VerticalScope(EditorStyles.helpBox)) //safe fucntion
        {
            //Label
            GUILayout.Label("This is a Label");
            
            //Button
            if (GUILayout.Button("This is a button"))
                Debug.Log("111");
            
            //Space
            GUILayout.Space(20);
            
            //EnumPop
            mythings = (things)EditorGUILayout.EnumPopup("Choose a animal: ", mythings);
            
            //TextField
            Car_ID = GUILayout.TextField(Car_ID, 25);
            GUILayout.Label("Result: " + Car_ID);
        }
    }
}
