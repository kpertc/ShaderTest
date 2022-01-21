using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[ExecuteAlways]
[ExecuteInEditMode]
public class drawLineHandle : MonoBehaviour
{
    public float shieldArea = 5.0f;
}

[CustomEditor(typeof(drawLineHandle))]
public class ExampleEditor : Editor
{
    Vector3 pos;

    protected virtual void OnSceneGUI()
    {
        drawLineHandle handleExample = (drawLineHandle)target;

        if (handleExample == null)
        {
            return;
        }

        Handles.color = Color.yellow;

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.green;

        Vector3 position = handleExample.transform.position + Vector3.up * 2f;
        string posString = position.ToString();

        Handles.Label(position,
            posString + "\nShieldArea: " +
            handleExample.shieldArea.ToString(),
            style
        );

        Handles.BeginGUI();
        if (GUILayout.Button("Reset Area", GUILayout.Width(100)))
        {
            handleExample.shieldArea = 5;
        }
        Handles.EndGUI();

        Handles.DrawWireArc(
            handleExample.transform.position,
            handleExample.transform.up,
            -handleExample.transform.right,
            180,
            handleExample.shieldArea);

        handleExample.shieldArea =
            Handles.ScaleValueHandle(handleExample.shieldArea,
                handleExample.transform.position + handleExample.transform.forward * handleExample.shieldArea,
                handleExample.transform.rotation,
                1, Handles.ConeHandleCap, 1);

        //Vector3 p1 = handleTransform.TransformPoint(new Vector3(0,3,0));

        pos = Handles.DoPositionHandle(pos, Quaternion.identity);

        Debug.Log(pos);
    }

    

}