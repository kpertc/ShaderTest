using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastControlGizmo : MonoBehaviour
{
    private RaycastControl _RaycastControl;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    } 
    
    void OnDrawGizmos()
    {
        _RaycastControl = GetComponent<RaycastControl>();
        
        if (_RaycastControl.isCasted)
        {
            if (_RaycastControl.showHitNormal) {
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(_RaycastControl.hitPosition, _RaycastControl.hitPosition + _RaycastControl.hitPositionNormal);
            }

            if (_RaycastControl.showHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(_RaycastControl.hitPosition, _RaycastControl.gizmoSphereSize);
            }

            if (_RaycastControl.showDirectPos)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(_RaycastControl.directPos, _RaycastControl.gizmoSphereSize);
            }

            if (_RaycastControl.showSmoothPos)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_RaycastControl.smoothPos, _RaycastControl.gizmoSphereSize);
            }
        }

        

        //Show Controller Object Dir
        if (_RaycastControl.showDir)
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
