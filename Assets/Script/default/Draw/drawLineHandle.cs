using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[ExecuteAlways]
[ExecuteInEditMode]
public class drawLineHandle : MonoBehaviour
{
    [Range(0f,1f)] public float t = 0;

    [Header("Gizmo Size")] 
    [Range(0f,2f)] public static float gizmoSize = 1;

    public CurveSegment curve1;

    private void OnDrawGizmos()
    {
        curve1.t = t;
        curve1.vis(gizmoSize);
    }
}


public static class drawLineUtil {


}


[System.Serializable]
public class CurveSegment
{
    public Vector3 anchor1;
    public Vector3 controlPoint1;

    public Vector3 anchor2;
    public Vector3 controlPoint2;

    [Space(30), Header("t related")]
    public float t;
    public Vector3 t_pos;
    public Quaternion t_rot;

    // Constructor
    public CurveSegment(Vector3 _anchor1, Vector3 _anchor2, Vector3 _controlPoint1, Vector3 _controlPoint2)
    {
        this.anchor1 = _anchor1;
        this.controlPoint1 = _anchor2;

        this.anchor2 = _controlPoint1;
        this.controlPoint2 = _controlPoint2;
    }

    // get T point and Normal
    void GetTPoint()
    {
        Vector3 a = Vector3.Lerp(anchor1, controlPoint1, t);
        Vector3 b = Vector3.Lerp(controlPoint1, controlPoint2, t);
        Vector3 c = Vector3.Lerp(controlPoint2, anchor2, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        //Vector3 pos = Vector3.Lerp(d, e, t);
        Vector3 tangent = (e - d).normalized;

        this.t_pos = Vector3.Lerp(d, e, t);
        this.t_rot = Quaternion.LookRotation(tangent);
    }

    // vis -> Visual Preview
    public void vis(float gizmoSize)
    {
        GetTPoint();
        vis_Gizmo_CurvePoints(gizmoSize);
        vis_Handle_TPoint();
        vis_handles_Bezier(gizmoSize);
        vis_handles_DoPositionHandle();
    }

    void vis_Gizmo_CurvePoints(float gizmoSize)
    {
        //Anchors
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(anchor1, gizmoSize);
        Gizmos.DrawSphere(anchor2, gizmoSize);

        //ControlPoint
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(controlPoint1, gizmoSize * 0.5f);
        Gizmos.DrawSphere(controlPoint2, gizmoSize * 0.5f);

        //Connect Points & Handles
        Gizmos.DrawLine(anchor1, controlPoint1);
        Gizmos.DrawLine(anchor2, controlPoint2);
    }

    void vis_Handle_TPoint() => Handles.DoPositionHandle(t_pos, t_rot);

    void vis_handles_Bezier(float gizmoSize) => Handles.DrawBezier(anchor1, anchor2, controlPoint1, controlPoint2, Color.white, EditorGUIUtility.whiteTexture, gizmoSize);

    void vis_handles_DoPositionHandle()
    {
        anchor1 = Handles.DoPositionHandle(anchor1, Quaternion.identity);
        anchor2 = Handles.DoPositionHandle(anchor2, Quaternion.identity);
        controlPoint1 = Handles.DoPositionHandle(controlPoint1, Quaternion.identity);
        controlPoint2 = Handles.DoPositionHandle(controlPoint2, Quaternion.identity);
    }
}






[CustomEditor(typeof(drawLineHandle))]
public class drawLineHandleEditor : Editor
{
    

    protected virtual void OnSceneGUI()
    {
        drawLineHandle handleExample = (drawLineHandle)target;

        if (handleExample == null) return;

        Handles.color = Color.yellow;

        GUIStyle style = new GUIStyle();
        style.normal.textColor = Color.green;

        Vector3 position = handleExample.transform.position + Vector3.up * 2f;
        string posString = position.ToString();

        /*        Handles.Label(position,
                    posString + "\nShieldArea: " +
                    handleExample.shieldArea.ToString(),
                    style
                );*/

        /*Handles.BeginGUI();
        if (GUILayout.Button("Reset Area", GUILayout.Width(100)))
            Handles.EndGUI();*/

        /*        foreach (Vector3 _position in handleExample.positions)
                {
                    //_position = Handles.DoPositionHandle(_position, Quaternion.identity);
                }*/

        //Debug.Log(pos);

        //drawLineHandle.handlesDoPositionHandle(handleExample.curve1);

        //handleExample.curve1.anchor1 = Handles.DoPositionHandle(handleExample.curve1.anchor1, Quaternion.identity);

        handleExample.curve1.anchor1 = Handles.DoPositionHandle(handleExample.curve1.anchor1, Quaternion.identity);
        handleExample.curve1.anchor2 = Handles.DoPositionHandle(handleExample.curve1.anchor2, Quaternion.identity);
        handleExample.curve1.controlPoint1 = Handles.DoPositionHandle(handleExample.curve1.controlPoint1, Quaternion.identity);
        handleExample.curve1.controlPoint2 = Handles.DoPositionHandle(handleExample.curve1.controlPoint2, Quaternion.identity);
    }



}