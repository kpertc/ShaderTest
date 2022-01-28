using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class move : MonoBehaviour
{
    public Vector3 direction = new Vector3(1,0,0);

    public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("space"))
        {
            _scale();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + direction);
    }

    public void _move() => transform.Translate(direction * speed * Time.deltaTime, Space.World);

    public void _scale() => transform.localScale = new Vector3(2,2,2);

}
