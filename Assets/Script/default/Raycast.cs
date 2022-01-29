using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

//[ExecuteAlways]
public class Raycast : MonoBehaviour
{
    /*
    //Singleton Method
    public static Raycast current;

    public void Awake() => current = this; 
    */

    public bool isCasted;
    public bool isRandomColor;

    public string CastedObject = "Nothing";
    public GameObject castedGameObject;

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
    private bool isRayCasting;

    public event Action onRayCastEnter;
    public event Action onRayCasting;
    public event Action onRayCastLeave;

    private void OnEnable()
    {
        onRayCastEnter += () => Debug.Log("Enter");
        onRayCasting += () => Debug.Log("on");
        onRayCastLeave += () => Debug.Log("Leave");
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
            // OnRayCastEnter
            if (!isRayCasting)
            {
                isRayCasting = true;

                if (onRayCastEnter != null) onRayCastEnter();
            }

            // OnRayCasting
            if (onRayCasting != null) onRayCasting();
            //Debug.Log("OnRayCasting");

            CastedObject = hit.collider.name;

            castedGameObject = GameObject.Find(CastedObject);

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
            //OnRayCastLeave
            if (isRayCasting)
            {
                isRayCasting = false;
                if (onRayCastLeave != null) onRayCastLeave();
            }

            CastedObject = "Nothing";

            castedGameObject = null;

            UISprite_renderer.enabled = false;
        }
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
