using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class drawDebugDrawRay : MonoBehaviour
{
    public bool isDraw = false;

    [Range(0.1f, 10f)]
    public float duration = 2;

    [Range(-10f, 10f)]
    public float Lenght = 1;

    private void Update()
    {
        if (isDraw == true)
        {
            Debug.DrawRay(transform.position, transform.forward * Lenght, Random.ColorHSV(), duration);

            isDraw = false;
        }
    }
}
