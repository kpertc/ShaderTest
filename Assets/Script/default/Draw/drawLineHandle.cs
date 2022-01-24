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
    [Range(0f,1f)] public float gizmoSize = 0;

    public CurvePoints curve1;


    [System.Serializable]
    public class CurvePoints
    {
        public Vector3 anchor1;
        public Vector3 controlPoint1;

        public Vector3 anchor2;
        public Vector3 controlPoint2;
    }

    private void OnDrawGizmos()
    {

        Handles.DrawBezier(GetPos(0), GetPos(3), GetPos(1), GetPos(2), Color.white, EditorGUIUtility.whiteTexture, 1f);

        /* for (int i = 0; i < positions.Length; i++) {
             positions[i].position = Handles.DoPositionHandle(GetPos(i), Quaternion.identity);
         }*/

        visualizePoints(gizmoSize);

        GetBezierPoint(t);

        Gizmos.DrawSphere(GetBezierPoint(t), 0.5f);
    }
    

    // Useful Functions
    Vector3 GetPos(int i) => positions[i].position;

    Vector3 GetBezierPoint (float t) {

        Vector3 a = Vector3.Lerp(GetPos(0), GetPos(1), t);
        Vector3 b = Vector3.Lerp(GetPos(1), GetPos(2), t);
        Vector3 c = Vector3.Lerp(GetPos(2), GetPos(3), t);

        Vector3 d = Vector3.Lerp(a, b, t);
        Vector3 e = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(a, b, t);
    }

    void visualizePoints(float size) {
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
    }

    

}