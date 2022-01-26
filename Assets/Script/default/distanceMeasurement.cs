using UnityEngine;

[ExecuteAlways]
public class distanceMeasurement : MonoBehaviour
{
    public Transform _objToMeasure;

    public float distance;

    void Update()
    {
        if (_objToMeasure) distance = Vector3.Distance(_objToMeasure.position, transform.position);
    }
}
