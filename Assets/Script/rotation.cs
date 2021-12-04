using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{
    public Vector3 speed = new Vector3(0,0,0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate (
            speed.x * Time.deltaTime,
            speed.y * Time.deltaTime,
            speed.z * Time.deltaTime
            
        );
    }
}
