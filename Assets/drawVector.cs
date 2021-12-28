using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawVector : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //draw sphere, radus = 1f
        Gizmos.DrawWireSphere(new Vector3(0,0,0), 1f);

        Gizmos.color = Color.red;
        //draw Vector
        Gizmos.DrawLine(new Vector3(0,0,0), new Vector3(1, 1, 1).normalized);
    }
}
