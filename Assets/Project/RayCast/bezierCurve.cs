using System;
using UnityEngine;
using UnityEditor;

//[ExecuteAlways]
//[ExecuteInEditMode]
public class bezierCurve : MonoBehaviour
{
    private RaycastControl _RaycastControl;
    private LineRenderer _LineRenderer;

    [Range(0f, 1f)] public float t = 0;

    [Space(10)]
    [Header("Curve Settings")]
    [Range(0f, 1f)] public float FrontBackWeight = 0.5f;
    [Range(0f, 0.3f), Tooltip("")] public float curveStiffness = 0.1f;

    [Header("Gizmo Display")] 
    [Range(0f,5f)] public float gizmoSize = 0.1f;

    public bool showGizmoPoints;
    public bool showGizmoTPoint;
    public bool showBeizerCurve;
    public bool showSampling;

    [readOnlyAttribute] public CurveSegment curve1 = new CurveSegment();

    [HideInInspector] public Vector3[] lineRendererPos;
    
    private void OnDrawGizmos()
    {
        curve1.t = t;

        if (showGizmoPoints) curve1.vis_Gizmo_CurvePoints(gizmoSize); 
        if (showGizmoTPoint) curve1.vis_Handle_TPoint();
        if (showBeizerCurve) curve1.vis_handles_Bezier(gizmoSize);
        if (showSampling) curve1.vis_Gizmo_sampling(gizmoSize);

    }

    public void Start()
    {
        _RaycastControl = GetComponent<RaycastControl>();
        _LineRenderer = GetComponent<LineRenderer>();
    }

    public void Update()
    {
        float distance = _RaycastControl.distance;
        Vector3 movingVector = _RaycastControl.movingVector.normalized; //direction

        //update Position
        curve1.anchor1 = _RaycastControl.outPutPos;
        curve1.controlPoint1 = curve1.anchor1 + ((1 - FrontBackWeight) * distance * movingVector * curveStiffness);

        curve1.anchor2 =  _RaycastControl.transform.position;
        curve1.controlPoint2 = curve1.anchor2 + (FrontBackWeight * distance * movingVector * curveStiffness);

        curve1.sampling();
        LRUpdate();
    }

    private void LRUpdate() //Line Renderer Update
    {
        CurveSegment.t_PosRot[] _samples = curve1.samples;
        lineRendererPos = new Vector3[_samples.Length];
        for (int i = 0; i < _samples.Length; i++) lineRendererPos[i] = _samples[i].pos;

        _LineRenderer.positionCount = _samples.Length; //set Count First
        _LineRenderer.SetPositions(lineRendererPos); //Set Positions
    }
}

[System.Serializable]
public class CurveSegment
{
    // Points
    public Vector3 anchor1;
    public Vector3 controlPoint1;

    public Vector3 anchor2;
    public Vector3 controlPoint2;

    // Samples
    [Range(1, 50)] public int sampleNum = 1;
    private float sampleInterval;
    public t_PosRot[] samples;

    // T-Related
    [Space(30), Header("t related")]
    public float t;

    // Constructor
    public CurveSegment() { }

    public CurveSegment(Vector3 _anchor1, Vector3 _anchor2, Vector3 _controlPoint1, Vector3 _controlPoint2)
    {
        this.anchor1 = _anchor1;
        this.controlPoint1 = _anchor2;

        this.anchor2 = _controlPoint1;
        this.controlPoint2 = _controlPoint2;
    }

    // t_PosRot dataStructure
    [System.Serializable]
    public struct t_PosRot
    {
        public Vector3 pos;
        public Quaternion rot;

        public t_PosRot (Vector3 _pos, Quaternion _rot)
        {
            this.pos = _pos;
            this.rot = _rot;
        }
    }

    // get T point and Normal
    t_PosRot GetTPoint(float _t)
    {
        Vector3 a = Vector3.Lerp(anchor1, controlPoint1, _t);
        Vector3 b = Vector3.Lerp(controlPoint1, controlPoint2, _t);
        Vector3 c = Vector3.Lerp(controlPoint2, anchor2, _t);

        Vector3 d = Vector3.Lerp(a, b, _t);
        Vector3 e = Vector3.Lerp(b, c, _t);

        //Vector3 pos = Vector3.Lerp(d, e, t);
        Vector3 tangent = (e - d).normalized;

        return new t_PosRot(Vector3.Lerp(d, e, _t), Quaternion.LookRotation(tangent));
    }

    // Sampling
    public void sampling()
    {
        sampleInterval = 1 / (float)sampleNum;
        //Debug.Log(smapleInterval);
        samples = new t_PosRot[sampleNum + 1];
        float tempT;

        for (int i = 0; i <= sampleNum; i++)
        {
            tempT = i * sampleInterval;

            samples[i] = GetTPoint(tempT);

            //Debug.Log("i: " + i.ToString() + ", tempT: " + tempT);
        }
    }

    // vis -> Visual Preview
    public void visMono(float gizmoSize)
    {
        vis_Gizmo_CurvePoints(gizmoSize);
        vis_Handle_TPoint();
        vis_handles_Bezier(gizmoSize);
    }

    public void visEditor()
    {
        vis_handles_DoPositionHandle();
    }

    public void vis_Gizmo_CurvePoints(float gizmoSize)
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

    public void vis_Gizmo_sampling(float gimzoSize)
    {
        foreach (t_PosRot sample in samples)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(sample.pos, gimzoSize);
        }
    }

    public void vis_Handle_TPoint() => Handles.DoPositionHandle(GetTPoint(t).pos, GetTPoint(t).rot);

    public void vis_handles_Bezier(float gizmoSize) => Handles.DrawBezier(anchor1, anchor2, controlPoint1, controlPoint2, Color.white, EditorGUIUtility.whiteTexture, gizmoSize * 10);

    public void vis_handles_DoPositionHandle()
    {
        anchor1 = Handles.DoPositionHandle(anchor1, Quaternion.identity);
        anchor2 = Handles.DoPositionHandle(anchor2, Quaternion.identity);
        controlPoint1 = Handles.DoPositionHandle(controlPoint1, Quaternion.identity);
        controlPoint2 = Handles.DoPositionHandle(controlPoint2, Quaternion.identity);
    }
}