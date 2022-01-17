using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyProperties))]
public class SerializedEditor : Editor
{
    SerializedProperty sp_intExample;
    
    SerializedProperty sp_stringArrayExmple;
    SerializedProperty sp_stringListExample;
    void OnEnable()
    {
        sp_intExample = serializedObject.FindProperty("intExample");
        
        sp_stringArrayExmple = serializedObject.FindProperty("stringArrayExmple");
        sp_stringListExample = serializedObject.FindProperty("stringListExample");
        
    }
    
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(sp_intExample, new GUIContent("intExample"));
        
        EditorGUILayout.PropertyField(sp_stringArrayExmple, new GUIContent("arrayString"));
        EditorGUILayout.PropertyField(sp_stringListExample, new GUIContent("listString"));

        GUILayout.Button("Example Button");
    }
}
