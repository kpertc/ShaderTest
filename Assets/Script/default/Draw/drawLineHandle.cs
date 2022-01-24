using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[ExecuteAlways]
[ExecuteInEditMode]
public class drawLineHandle : MonoBehaviour
{
    [Range(0f,1f)] public float t = 0;

    public Transform[] positions;

    [Header("Gizmo Size")] 
    [Range(0f,2f)] public float gizmoSize = 0;

    public CurvePoints curve1;


    [System.Serializable]
    public struct CurvePoints
    {
        public Vector3 anchor1;
        public Vector3 controlPoint1;

        public Vector3 anchor2;
        public Vector3 controlPoint2;
    }

    private void OnDrawGizmos()
    {
        handlesDrawBezier(curve1);

        visualizeCurvePoints(curve1, gizmoSize);

        Gizmos.DrawSphere(GetBezierPoint(curve1, t), gizmoSize);
    }
    

    // Useful Functions
    static void handlesDrawBezier(CurvePoints cps) => Handles.DrawBezier(cps.anchor1, cps.anchor2, cps.controlPoint1, cps.controlPoint2, Color.white, EditorGUIUtility.whiteTexture, 1f);
    static Vector3 GetBezierPoint (CurvePoints cps, float t) {

        Vector3 a = Vector3.Lerp(cps.anchor1, cps.controlPoint1, t);
        Vector3 b = Vector3.Lerp(cps.controlPoint1, cps.controlPoint2, t);
        Vector3 c = Vector3.Lerp(cps.controlPoint2, cps.anchor2, t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(d, e, t);
    }

    static void visualizeCurvePoints(CurvePoints cps, float size) {
        //Anchors
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(cps.anchor1, size);
        Gizmos.DrawSphere(cps.anchor2, size);

        //ControlPoint
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(cps.controlPoint1, size * 0.5f);
        Gizmos.DrawSphere(cps.controlPoint2, size * 0.5f);

        //Connect Points & Handles
        Gizmos.DrawLine(cps.anchor1, cps.controlPoint1);
        Gizmos.DrawLine(cps.anchor2, cps.controlPoint2);
    }

    public static void handlesDoPositionHandle(CurvePoints cps)
    {
        cps.anchor1 = Handles.DoPositionHandle(cps.anchor1, Quaternion.identity);
        cps.anchor2 = Handles.DoPositionHandle(cps.anchor2, Quaternion.identity);
        cps.controlPoint1 = Handles.DoPositionHandle(cps.controlPoint1, Quaternion.identity);
        cps.controlPoint2 = Handles.DoPositionHandle(cps.controlPoint2, Quaternion.identity);
    }
}


public static class drawLineUtil {


/*    void visualizePoints(float size)
    {
        //Point
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(GetPos(0), size);
        Gizmos.DrawSphere(GetPos(3), size);

        //Control Handle
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(GetPos(1), size * 0.5f);
        Gizmos.DrawSphere(GetPos(2), size * 0.5f);

        //Connect Points & Handles
        Gizmos.DrawLine(GetPos(0), GetPos(1));
        Gizmos.DrawLine(GetPos(2), GetPos(3));
    }*/

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