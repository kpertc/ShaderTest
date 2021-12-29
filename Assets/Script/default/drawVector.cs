using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawVector : MonoBehaviour
{
    public Vector3 Vector = new Vector3(0,0,0);

    public bool isNormalize = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //draw sphere, radus = 1f
        Gizmos.DrawWireSphere(new Vector3(0,0,0), 1f);

        Gizmos.color = Color.red;

        //Vector3 tempVector = isNormalize? Vector.normalized : Vector;

        //draw Vector
        Gizmos.DrawLine(new Vector3(0,0,0), isNormalize ? Vector.normalized : Vector);
    }
}
