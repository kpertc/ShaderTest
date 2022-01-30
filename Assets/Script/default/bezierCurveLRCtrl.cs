using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bezierCurveLRCtrl : MonoBehaviour
{
    // Get Components
    private LineRenderer _LineRenderer;
    private bezierCurve _bezierCurvel;

    [HideInInspector] public Vector3[] lineRendererPos;

    void Start()
    {
        // Get Components
        _bezierCurvel = GetComponent<bezierCurve>();
        _LineRenderer = GetComponent<LineRenderer>();        
    }

    void Update()
    {
        updateBezierCurvePos();
    }

    private void updateBezierCurvePos()
    {
        CurveSegment.t_PosRot[] _samples = _bezierCurvel.curve1.samples;
        lineRendererPos = new Vector3[_samples.Length];
        for (int i = 0; i < _samples.Length; i++) lineRendererPos[i] = _samples[i].pos;

        _LineRenderer.positionCount = _samples.Length; //set Count First
        _LineRenderer.SetPositions(lineRendererPos); //Set Positions
    }
}
