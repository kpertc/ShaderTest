using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DG.Tweening;

//[ExecuteAlways]
public class RaycastControl : MonoBehaviour
{
    [Header("Raycast Parameters (ReadOnly)")]
    [readOnlyAttribute] public bool isCasted;
    [readOnlyAttribute] public string CastedObject = "Nothing";
    [readOnlyAttribute] public GameObject lastCastedObject;

    [Space(10)]
    private GameObject CastedObjectDelay;
    private GameObject castedGameObject;

    [HideInInspector] public Vector3 hitPosition;
    [HideInInspector] public Vector3 hitPositionNormal;
    [HideInInspector] public Vector3 hitPositionNormalRotation;
    [HideInInspector] public float hitPointDistance;
    [HideInInspector] public Vector3 outPutPos;

    [Header("Position Settings")]
    #region smoothPos

    [Range(0f, 1f)] public float smoothTime = 0.3F;
    [Range(0f, 1f)] public float directPosSurfaceNormalOffset;

    [readOnlyAttribute] public Vector3 directPos;
    [readOnlyAttribute] public Vector3 smoothPos;
        
    //[ReadOnly] public Vector3 Offset = new Vector3(0,0,0);
    [readOnlyAttribute] public float distance;
    [readOnlyAttribute] public Vector3 movingVector;
    private Vector3 velocity = Vector3.zero;
    
    public void smoothPos_Update() // Lerp Integration
    {
        //Vector3 targetPosition = directPos.TransformPoint(Offset);

        movingVector = directPos - smoothPos;

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

    [Space(5)]
    [Header("Hover Effect Control")]
    
    public bool enableTranslation;
    public bool enableRotation;

    [Range(0f, 1f)] public float translationIntensity = 0.3f;
    [Range(0f, 10f)]public float rotationIntensity = 1;

    //Events
    public event Action<GameObject> onRayCastEnter;
    public event Action<GameObject> onRayCasting;
    public event Action<GameObject> onRayCastLeave;

    void onEnterAnimation (GameObject obj)
    {
        obj.transform.DOKill();

        if (obj.GetComponent<selfPos>().enableAdsorption) DOTween.To(() => outPutPos, x => outPutPos = x, obj.transform.position, 0.2f);

        else outPutPos = smoothPos;

        if (obj.tag == "UI")
        {
            obj.transform.DOLocalMoveX(0.5f, .2f).SetEase(Ease.InOutSine);
            obj.transform.DOScale(new Vector3(0.1f, 4 * 1.1f, 3 * 1.1f), .2f).SetEase(Ease.OutSine);
            //obj.transform.DOScale(new Vector3(0.1f, 4 * 1.05f, 3 * 1.05f), .2f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        
            obj.GetComponent<Renderer>().material.DOFloat(0.08f, "_OutlineWidth", 0.2f).SetEase(Ease.OutSine);
        }
        
        //Turn-on Hightlight
        obj.GetComponent<Renderer>().material.DOFloat(0.5f, "_intensity", 0.2f).SetEase(Ease.OutSine);
    }

    void onLeaveAnimation (GameObject obj)
    {
        if (obj.GetComponent<selfPos>().enableAdsorption) DOTween.To(() => outPutPos, x => outPutPos = x, smoothPos, 0.2f);

        obj.transform.DOKill(); // Stop the hover loop

        if (obj.tag == "UI")
        {
            obj.transform.DOLocalRotate(new Vector3(0, 0, 0), .05f).SetEase(Ease.InSine); // Stop Rotation first

            obj.transform.DOLocalMove(obj.GetComponent<selfPos>().initPos, .3f).SetEase(Ease.InOutSine);

            obj.transform.DOScale(new Vector3(0.1f, 4, 3), .2f).SetEase(Ease.OutSine);

            obj.GetComponent<Renderer>().material.DOFloat(0.0f, "_OutlineWidth", 0.2f).SetEase(Ease.OutSine);
        }
        
        //Turn-off HighLight
        obj.GetComponent<Renderer>().material.DOFloat(0f, "_intensity", 0.2f).SetEase(Ease.OutSine);
    }

    void onAnimation(GameObject obj)
    {
        if (obj.GetComponent<selfPos>().enableAdsorption)
        {
            outPutPos = obj.transform.position;
        }

        else outPutPos = smoothPos;

        if (obj.tag == "UI")
        {
            // Get smoothPos relative to obj direction
            Vector3 rotDir = (smoothPos - obj.transform.position).normalized; //x is height // Y Z is moving
        
            // Translation
            if (enableTranslation) obj.transform.DOLocalMove(obj.GetComponent<selfPos>().initPos + new Vector3(0, rotDir.y, rotDir.z) * translationIntensity, .02f).SetEase(Ease.InSine);
            
            //rotation
            if (enableRotation) obj.transform.DOLocalRotate(new Vector3(0,- rotDir.z, rotDir.y) * rotationIntensity, .05f).SetEase(Ease.InSine);
        
            // HighLight
            obj.GetComponent<Renderer>().material.DOVector(hitPosition, "_inputWS",0.05f);
        }
        
        else if (obj.tag == "Panel")
        {
            // HighLight
            obj.GetComponent<Renderer>().material.DOVector(smoothPos, "_inputWS",0.05f);
        }
    }

    private void OnEnable()
    {
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
        }

        else
        {
            //update objects
            castedGameObject = null;
            CastedObject = "Nothing";

            //check object to trigger enter&leave event
            castedObjectRecord();
        }
        
        // Update SmoothPos
        smoothPos_Update();
        
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

    void OnDrawGizmos()
    {
        if (isCasted)
        {
            if (showHitNormal) {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(hitPosition, hitPosition + hitPositionNormal);
            }

            if (showHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(hitPosition, gizmoSphereSize);
            }

            if (showDirectPos)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(directPos, gizmoSphereSize);
            }

            if (showSmoothPos)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(smoothPos, gizmoSphereSize);
            }
        }

        

        //Show Controller Object Dir
        if (showDir)
        {
            //Forward
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 10);

            //Upward
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.up);

            //right
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + transform.right);
        }
    }    
}