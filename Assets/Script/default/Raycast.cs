using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteAlways]
public class Raycast : MonoBehaviour
{
    public bool isCasted;

    public string CastedObject;

    private Vector3 hitPosition;
    public Vector3 hitPositionNormal;
    public Vector3 hitPositionNormalRotation;

//[Range(0.2f, 0.8f)] public float drawSphereSize = 0.5f;

    [Header("DrawGizmo")]
    public bool isDrawSeldDir;
    public bool isDrawHitGizmo;
    public bool isDrawHitNormal;

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        isCasted = Physics.Raycast(ray, out RaycastHit hit);

        if (isCasted) {
            CastedObject = hit.collider.name;
            hitPosition = hit.point;
            hitPositionNormal = hit.normal;
        }

        else CastedObject = "Nothing";
    }

    void OnDrawGizmos()
    {
        if (isDrawSeldDir) drawSelfDir();

        if (isCasted)
        {
            if (isDrawHitGizmo) Gizmos.DrawIcon(hitPosition, "aim_1024.png", true);

            if (isDrawHitNormal) {
                Gizmos.color = Color.blue;
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
