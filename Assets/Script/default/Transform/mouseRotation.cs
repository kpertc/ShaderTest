using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseRotation : MonoBehaviour
{
    [HideInInspector] public Vector3 rotateResult;
    [HideInInspector] public Vector3 currentRotation;

    public float mouseX;
    public float mouseY;

    [Range(0,1)] public float sensitivity = 1;

    void Start()
    {
        
    }

    void Update()
    {
        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;

        //currentRotation = transform.rotation.eulerAngles;
        //rotateResult = currentRotation + new Vector3(mouseX, mouseY, 0);

        rotateResult = new Vector3(mouseY, mouseX) * sensitivity;

        transform.eulerAngles = rotateResult;
    }
}
