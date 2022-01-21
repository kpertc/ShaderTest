using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
//[ExecuteAlways]

public class shaderGizmo : MonoBehaviour
{
    //private Material[] MaterialList;
    
    [SerializeField] private Material[] materials;

    public string gizmoText = "";
    
    public Vector3 offset = new Vector3(0, 2f, 0);
    
    void Start()
    {
        
        GizmoText();
    }

    void GizmoText()
    {
        materials = GetComponent<Renderer>().sharedMaterials;

        gizmoText = "";
        
        foreach (Material material in materials)
        {
            gizmoText += (material.shader.name + "\n");
        }
    }

    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Handles.Label(transform.position,
            gizmoText);
    }
}
