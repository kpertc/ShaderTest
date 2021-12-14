using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delegate : MonoBehaviour
{

    public delegate void _voidDelegate();

    _voidDelegate myfunction;
    void Start()
    {
        myfunction += beginFunction;
    }

    void beginFunction ()
    {
        Debug.Log("Begain");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log(myfunction.Method.ToString());
        }
            
    }
}
