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
    [ReadOnly] public bool isCasted;
    [ReadOnly] public string CastedObject = "Nothing";
    [ReadOnly] public GameObject lastCastedObject;

    [Space(10)]
    private GameObject CastedObjectDelay;
    private GameObject castedGameObject;

    [HideInInspector] public Vector3 hitPosition;
    [HideInInspector] public Vector3 hitPositionNormal;
    [HideInInspector] public Vector3 hitPositionNormalRotation;
    [HideInInspector] public float hitPointDistance;

    [Header("Position Settings")]
    public GameObject directPos;
    public GameObject smoothPos;
    [Range(0f, 1f)] public float directPos_surfaceNormalOffset;

    [Header("Visual_Gizmo Toggles")]
    public bool isDrawSeldDir;
    public bool isDrawHitGizmo;
    public bool isDrawHitNormal;

    [Space(5)]
    public bool showdirectPosMesh;
    public bool showSmoothPosMesh;

    [Space(5)]
    [Header("Hover Effect Control")]
    [Range(0f, 10f)]public float rotationIntensity = 1;

    public bool enableTranslation;
    public bool enableRotation;

    //Events
    public event Action<GameObject> onRayCastEnter;
    public event Action<GameObject> onRayCasting;
    public event Action<GameObject> onRayCastLeave;

    void onEnterAnimation (GameObject obj)
    {
        obj.transform.DOKill();
        
        obj.transform.DOLocalMoveX(0.5f, .2f).SetEase(Ease.InOutSine);
        obj.transform.DOScale(new Vector3(0.1f, 4 * 1.1f, 3 * 1.1f), .2f).SetEase(Ease.OutSine);
        //obj.transform.DOScale(new Vector3(0.1f, 4 * 1.05f, 3 * 1.05f), .2f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        
        obj.GetComponent<Renderer>().material.SetFloat("_OutlineWidth", 0.1f);
    }

    void onLeaveAnimation (GameObject obj)
    {
        obj.transform.DOKill(); // Stop the hover loop
        obj.transform.DOLocalRotate(new Vector3(0,0,0), .05f).SetEase(Ease.InSine); // Stop Rotation first
        
        obj.transform.DOLocalMove(obj.GetComponent<selfPos>().initPos, .3f).SetEase(Ease.InOutSine);
        
        obj.transform.DOScale(new Vector3(0.1f, 4, 3), .2f).SetEase(Ease.OutSine);
        
        obj.GetComponent<Renderer>().material.SetFloat("_OutlineWidth", 0.0f);
    }

    void onAnimation(GameObject obj)
    {
        // Get smoothPos relative to obj direction
        Vector3 rotDir = (smoothPos.transform.position - obj.transform.position).normalized; //x is height // Y Z is moving
        
        // Translation
        if (enableTranslation) obj.transform.DOLocalMove(obj.GetComponent<selfPos>().initPos + new Vector3(0, rotDir.y, rotDir.z) * 0.3f, .1f).SetEase(Ease.InSine);
            
        //rotation
        if (enableRotation) obj.transform.DOLocalRotate(new Vector3(0,- rotDir.z, rotDir.y) * rotationIntensity, .05f).SetEase(Ease.InSine);
 
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

    // Update is called once per frame
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
            directPos.transform.position = hit.point + hit.normal * directPos_surfaceNormalOffset;
        }

        else
        {
            //update objects
            castedGameObject = null;
            CastedObject = "Nothing";

            //check object to trigger enter&leave event
            castedObjectRecord();
        }

        visualControl(isCasted);
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

    void visualControl (bool isCasted)
    {
        if(showdirectPosMesh) directPos.GetComponent<Renderer>().enabled = isCasted;
        if(showSmoothPosMesh) smoothPos.GetComponent<Renderer>().enabled = isCasted;
    }

    void OnDrawGizmos()
    {
        if (isDrawSeldDir) drawSelfDir();

        if (isCasted)
        {
            if (isDrawHitGizmo) Gizmos.DrawIcon(hitPosition, "aim_1024.png", true);

            if (isDrawHitNormal) {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(hitPosition, hitPosition + hitPositionNormal);
            }
        }
    }

    void drawSelfDir()
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



// ----------------------[ReadOnly]Drawer-------------------------------------
public class ReadOnlyAttribute : PropertyAttribute
{

}

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property,
                                            GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position,
                               SerializedProperty property,
                               GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}
