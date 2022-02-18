using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

[RequireComponent(typeof(raycastControlGizmo))]
public class RaycastControl : MonoBehaviour
{
    [Header("Raycast Parameters (ReadOnly)")]
    [readOnlyAttribute] public bool isCasted;
    [readOnlyAttribute] public string CastedObject = "Nothing";
    [readOnlyAttribute] public GameObject lastCastedObject;
    [Range(0, 10f)] public float defaultDistance;

    [Space(10)]
    private GameObject CastedObjectDelay;
    private GameObject castedGameObject;

    [HideInInspector] public Vector3 hitPosition;
    [HideInInspector] public Vector3 hitPositionNormal;
    [HideInInspector] public Vector3 hitPositionNormalRotation;
    [HideInInspector] public float hitPointDistance;
    
    //snapped curve control
    //[HideInInspector] 
    //[HideInInspector]
    [HideInInspector] public Vector3 outPutPos;

    [Header("Position Settings")]
    #region smoothPos

    [Range(0f, 1f)] public float smoothTime = 0.3F;
    [Range(0f, 1f)] public float directPosSurfaceNormalOffset;

    [readOnlyAttribute] public Vector3 directPos;
    [readOnlyAttribute] public Vector3 smoothPos;
        
    
    //[ReadOnly] public Vector3 Offset = new Vector3(0,0,0);
    [Header("Bezier Curve Control")]
    [Range(0f, 1f)] public float FrontBackWeight = 0.5f;
    [Range(0f, 0.3f), Tooltip("")] public float curveStiffness = 0.1f;
    [Range(0f, 10f)] public float curveControlPointXOffset;

    [readOnlyAttribute] public float distance;
    [readOnlyAttribute] public Vector3 movingVector;
    
    //
    private CurveSegment _curve;
    private Vector3 velocity = Vector3.zero;
    
    public void smoothPos_Update() // Lerp Integration
    {
        //Vector3 targetPosition = directPos.TransformPoint(Offset);

        movingVector = (directPos - smoothPos).normalized;

        smoothPos = Vector3.SmoothDamp(smoothPos, directPos, ref velocity, smoothTime);

        distance = Vector3.Distance(smoothPos, directPos);

        //smoothPos.eulerAngles = directPos.eulerAngles;  
    }
    
    #endregion

    [Header("Debug Gizmo Visual")]
    public bool showDir;
    public bool showHit;
    public bool showHitNormal;

    [Space(5)]
    [Range(0f, 1f)] public float gizmoSphereSize = 0.3f;
    public bool showDirectPos;
    public bool showSmoothPos;

    //Events
    public event Action<GameObject> onRayCastEnter;
    public event Action<GameObject> onRayCasting;
    public event Action<GameObject> onRayCastLeave;

    void onEnterAnimation (GameObject obj)
    {
        if (obj.layer != 5) return; //if collided obj is not UI
        
        // Get info
        UIControl _UIControl = obj.GetComponent<UIControl>();
        
        obj.transform.DOKill();

        if (_UIControl._UIType == UIControl.UIType.UI)
        {
            obj.transform.DOLocalMoveX(0.5f, _UIControl.animTime).SetEase(Ease.InOutSine);
            obj.transform.DOScale(new Vector3(0.1f, 4 * 1.1f, 3 * 1.1f), _UIControl.animTime).SetEase(Ease.OutSine);
            obj.GetComponent<Renderer>().material.DOFloat(_UIControl.enableOutlineHover, "_OutlineWidth", _UIControl.animTime).SetEase(Ease.OutSine);
        }
        
        //Turn-on Hightlight
        if (_UIControl.enableHightlight) obj.GetComponent<Renderer>().material.DOFloat(0.5f, "_intensity", _UIControl.animTime).SetEase(Ease.OutSine);
    }

    void onLeaveAnimation (GameObject obj)
    {   
        if (obj.layer != 5) return; //if collided obj is not UI
        
        // Get info
        UIControl _UIControl = obj.GetComponent<UIControl>();
        
        // Snapping
        if (obj.GetComponent<UIControl>().enableSnapping) DOTween.To(() => outPutPos, x => outPutPos = x, smoothPos, smoothTime).SetEase(Ease.InExpo);

        obj.transform.DOKill(); // Stop the hover loop

        if (_UIControl._UIType == UIControl.UIType.UI)
        {
            obj.transform.DOLocalRotate(new Vector3(0, 0, 0), _UIControl.animTime).SetEase(Ease.InExpo); // Stop Rotation first

            obj.transform.DOLocalMove(obj.GetComponent<UIControl>().initPos, _UIControl.animTime).SetEase(Ease.InExpo);

            obj.transform.DOScale(new Vector3(0.1f, 4, 3), _UIControl.animTime).SetEase(Ease.InExpo);

            obj.GetComponent<Renderer>().material.DOFloat(_UIControl.enableOutlineIdle, "_OutlineWidth", _UIControl.animTime).SetEase(Ease.InExpo);
        }
        
        //Turn-off HighLight
        obj.GetComponent<Renderer>().material.DOFloat(0f, "_intensity", _UIControl.animTime).SetEase(Ease.OutSine);
    }

    void onAnimation(GameObject obj)
    {
        if (obj.layer != 5) return; //if collided obj is not UI
            
        // Get info
        UIControl _UIControl = obj.GetComponent<UIControl>();
        
        // Snapping
        if (_UIControl.enableSnapping)
        {
            DOTween.To(() => outPutPos, x => outPutPos = x, obj.transform.position, smoothTime);

            //smoothPos is already smoothed, not need for another tweening
            _curve.anchor1 = outPutPos; 
            _curve.controlPoint1 = smoothPos + ((1 - FrontBackWeight) * distance * movingVector * curveStiffness) + new Vector3(curveControlPointXOffset, 0, 0); ;

            _curve.anchor2 =  transform.position;
            _curve.controlPoint2 = _curve.anchor2 + (FrontBackWeight * distance * movingVector * curveStiffness);
        }
        else
        {
            outPutPos = smoothPos;
            defaultCurveControl();
        }

        if (_UIControl._UIType == UIControl.UIType.UI)
        {
            // Get smoothPos relative to obj direction
            Vector3 rotDir = (smoothPos - obj.transform.position).normalized; //x is height // Y Z is moving
            // Translation
            if (_UIControl.enableTranslation) obj.transform.DOLocalMove(_UIControl.initPos + new Vector3(0, rotDir.y, rotDir.z) * _UIControl.translationSensitivity, _UIControl.animTime).SetEase(Ease.InSine);
            //rotation
            if (_UIControl.enableRotation) obj.transform.DOLocalRotate(new Vector3(0,- rotDir.z, rotDir.y) * _UIControl.rotationSensitivity, _UIControl.animTime).SetEase(Ease.InSine);
            // HighLight
            obj.GetComponent<Renderer>().material.DOVector(hitPosition, "_inputWS",_UIControl.animTime);
        }
        
        else if (_UIControl._UIType == UIControl.UIType.Panel)
        {
            // HighLight
            obj.GetComponent<Renderer>().material.DOVector(smoothPos, "_inputWS",_UIControl.animTime);
        }
    }

    private void OnEnable()
    {
        _curve = GetComponent<bezierCurve>().curve1;
        
        onRayCastEnter += (objName) => { if (objName != null && objName.layer == 5) onEnterAnimation(objName); };
        onRayCasting += (objName) => { if (objName != null && objName.layer == 5) onAnimation(objName); };
        onRayCastLeave += (objName) => { if (objName != null && objName.layer == 5) onLeaveAnimation(objName); };
    }
    private void OnDestroy()
    {

    }

    void Update()
    {
        // Updates
        Ray ray = new Ray(transform.position, transform.forward);

        isCasted = Physics.Raycast(ray, out RaycastHit hit);
        
        if (isCasted)
        {
            //update objects
            castedGameObject = hit.collider.gameObject;
            CastedObject = castedGameObject.name;
            
            
            //check object to trigger enter&leave event
            castedObjectRecord ();

            // OnRayCasting
            if (onRayCasting != null) onRayCasting(hit.collider.gameObject);

            // hit & Gizmos 
            hitPosition = hit.point;
            hitPositionNormal = hit.normal;
            hitPositionNormalRotation = Quaternion.LookRotation(- hitPositionNormal, Vector3.up).eulerAngles;

            // Distance
            hitPointDistance = Vector3.Distance(transform.position, hitPosition);

            // directPos Normal Offset
            directPos = hit.point + hit.normal * directPosSurfaceNormalOffset;

            smoothPos_Update();

            outPutPos = smoothPos;
        }

        else
        {
            // update objects
            castedGameObject = null;
            CastedObject = "Nothing";

            // check object to trigger enter&leave event
            castedObjectRecord();
            
            // default Raycast to space
            directPos = transform.position + transform.forward * hitPointDistance;

            smoothPos_Update();

            defaultCurveControl();

            outPutPos = smoothPos;
        }
    }

    void castedObjectRecord ()
    {
        // CastedObject Changed
        if (castedGameObject != CastedObjectDelay)      
        {
            lastCastedObject = CastedObjectDelay;

            //Trigger Leave
            if (onRayCastLeave != null) onRayCastLeave(CastedObjectDelay);

            //Trigger Enter
            if (onRayCastEnter != null)

                if (isCasted) onRayCastEnter(castedGameObject);

                else onRayCastEnter(null);
        }

        CastedObjectDelay = castedGameObject;
    }

    //update Position
    void defaultCurveControl()
    {
        DOTween.To(() => _curve.anchor1, x => _curve.anchor1 = x, outPutPos, smoothTime);
        //Vector3 CCP1 = _curve.anchor1 + ((1 - FrontBackWeight) * distance * movingVector * curveStiffness);
        //DOTween.To(() => _curve.controlPoint1, x => _curve.controlPoint1 = x, CCP1, smoothTime);

        _curve.controlPoint1 = _curve.anchor1 + ((1 - FrontBackWeight) * distance * movingVector * curveStiffness);

        DOTween.To(() => _curve.anchor2, x => _curve.anchor2 = x, transform.position, smoothTime);
        Vector3 CCP2 = _curve.anchor2 + (FrontBackWeight * distance * movingVector * curveStiffness);
        DOTween.To(() => _curve.controlPoint2, x => _curve.controlPoint2 = x, CCP2, smoothTime);
    }
}