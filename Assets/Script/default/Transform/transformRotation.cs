using UnityEngine;

[AddComponentMenu("Common Scripts/Transform & Rotation")]
public class transformRotation : MonoBehaviour
{
    public Vector3 sin_Transform = new Vector3(0,0,0);

    public Vector3 rotation_speed = new Vector3(0,0,0);

    Vector3 _transfrom = new Vector3(0,0,0);
    Vector3 _rotation = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        _transfrom = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        _transfrom += new Vector3(
            Mathf.Sin(sin_Transform.x * Time.time) * Time.deltaTime,
            Mathf.Sin(sin_Transform.y * Time.time) * Time.deltaTime, 
            Mathf.Sin(sin_Transform.z * Time.time) * Time.deltaTime
        );
        
        transform.position = _transfrom;
            


        _rotation += new Vector3(
            rotation_speed.x * Time.deltaTime,
            rotation_speed.y * Time.deltaTime,
            rotation_speed.z * Time.deltaTime
        );

        transform.localRotation = Quaternion.Euler(
            _rotation
        );
    }
}
