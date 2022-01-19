using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rayCastWindow : MonoBehaviour
{
    public Transform ObjTF;


    

    // Start is called before the first frame update
    void Start()
    {
        ObjTF = transform;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(ObjTF.position, ObjTF.forward);

       //if (Physics.Raycast (ray, out RaycastHit hit))
    }
}
