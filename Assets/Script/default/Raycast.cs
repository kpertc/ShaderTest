using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

//[ExecuteAlways]
public class Raycast : MonoBehaviour
{
    public bool isCasted;
    public bool isRandomColor;

    public string CastedObject = "Nothing";
    private string CastedObjectDelay;
    public GameObject castedGameObject;
    public string lastCastedObject;

    private Vector3 hitPosition;
    private Vector3 hitPositionNormal;
    private Vector3 hitPositionNormalRotation;
    public float hitPointDistance;

    public GameObject rayCastedPoint;


    [Header("DrawGizmo")]
    public bool isDrawSeldDir;
    public bool isDrawHitGizmo;
    public bool isDrawHitNormal;

    [Header("UI_Sprite")]
    public GameObject UISprite;
    [Range(0f, 1f)]  public float UISprite_surfaceOffset;
    Renderer UISprite_renderer;

    //For RayCasting Event
    private bool isRayCasting = false;

    public event Action<string> onRayCastEnter;
    public event Action<string> onRayCasting;
    public event Action<string> onRayCastLeave;

    //public bool hasCasted;

    //public delegate void raycasting (GameObject CastedObj);

    private void OnEnable()
    {
        onRayCastEnter += (objName) => Debug.Log("Enter: " + objName);
        //onRayCasting += (objName) => Debug.Log("On: " + objName);
        onRayCastLeave += (objName) => Debug.Log("Leave: " + objName);
    }

    private void OnDestroy()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // inits
        UISprite_renderer = UISprite.GetComponent<Renderer>();

        // Updates
        Ray ray = new Ray(transform.position, transform.forward);

        isCasted = Physics.Raycast(ray, out RaycastHit hit);
        
        if (isCasted)
        {
            castedGameObject = hit.collider.gameObject;

            //still casting, but changed Object
            if (CastedObject != hit.collider.name)
            {
                //Leave
                /*
                if (CastedObject != "Nothing")
                {
                    if (onRayCastLeave != null) onRayCastLeave(lastCastedObject);
                }
                */
                //Enter
                if (onRayCastEnter != null) onRayCastEnter(CastedObject);
            }

            // OnRayCastEnter
            else if (!isRayCasting && CastedObject == "Nothing") 
            {
                if (isRayCasting = true && onRayCastEnter != null) onRayCastEnter(hit.collider.name);
            }

            CastedObject = hit.collider.name;
            castedObjectRecord ();

            // OnRayCasting
            if (onRayCasting != null) onRayCasting(CastedObject);
            //Debug.Log("OnRayCasting");

            // hit & Gizmos 
            hitPosition = hit.point;
            hitPositionNormal = hit.normal;
            hitPositionNormalRotation = Quaternion.LookRotation(- hitPositionNormal, Vector3.up).eulerAngles;

            // Distance
            hitPointDistance = Vector3.Distance(transform.position, hitPosition);

            // UISprite
            UISprite_renderer.enabled = true;
            UISprite.transform.position = hit.point + hit.normal * UISprite_surfaceOffset;
            UISprite.transform.eulerAngles = hitPositionNormalRotation;
        }

        else
        {
            castedGameObject = null;

            //OnRayCastLeave
            /*
            if (isRayCasting)
            {
                isRayCasting = false;

                if (onRayCastLeave != null) onRayCastLeave(CastedObjectDelay);
            }
            */

            CastedObject = "Nothing";
            castedObjectRecord ();
            

            UISprite_renderer.enabled = false;
        }
    }

    void castedObjectRecord ()
    {
        // CastedObject Changed
        if (CastedObject != CastedObjectDelay)      
        {
            lastCastedObject = CastedObjectDelay;

            if (onRayCastLeave != null) onRayCastLeave(CastedObjectDelay);
        }

        CastedObjectDelay = CastedObject;
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
