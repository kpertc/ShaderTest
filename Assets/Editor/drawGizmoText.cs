using UnityEngine;
using System.Collections;
using UnityEditor;

[UnityEditor.CustomEditor(typeof(shaderGizmo))]
public class drawGizmoText : Editor
{
    

    /*
    void OnSceneGUI()
    {
        shaderGizmo _shaderGizmo = (shaderGizmo)target;
        
        if (_shaderGizmo == null)
        {
            return;
        }

        Handles.Label(_shaderGizmo.transform.position + _shaderGizmo.offset,
            _shaderGizmo.gizmoText);
    }
    */
}
