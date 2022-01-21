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




    [Range(0.2f, 0.8f)] public float drawSphereSize = 0.5f;





    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        isCasted = Physics.Raycast(ray, out RaycastHit hit);

        if (isCasted) {

            CastedObject = hit.collider.name;

            hitPosition = hit.point;
        }

        else
        {
            CastedObject = "Nothing";

            hitPosition = new Vector3 (0,0,0);
        }

        Handles.color = Color.red;

        Handles.DrawWireCube(new Vector3(0,0,0), new Vector3(1,1,1));

    }

    void OnDrawGizmos()
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

        //Gizmos.DrawSphere(hitPosition, drawSphereSize);

        Gizmos.DrawIcon(hitPosition, "aim_1024.png", true);
        
        //Gizmos.DrawCube(transform.position + transform.forward, new Vector3(1,1,1));

        //Debug.Log("transform.forward: " + transform.forward);
    }
}
