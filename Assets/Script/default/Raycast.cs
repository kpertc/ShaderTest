using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//[ExecuteAlways]
public class Raycast : MonoBehaviour
{
    public bool isCasted;
    public bool isRandomColor;

    public string CastedObject = "Nothing";

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


    private void Start()
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
            if (CastedObject != hit.collider.name) Debug.Log("Enter!");

            CastedObject = hit.collider.name;

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
            if (hit.collider == null) Debug.Log("Leave!");

            //else if (CastedObject != hit.collider.name) Debug.Log("Leave!");

            CastedObject = "Nothing";

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
