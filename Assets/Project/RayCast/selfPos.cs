using UnityEngine;

public class selfPos : MonoBehaviour
{
    public Vector3 initPos;

    public bool enableAdsorption;
    void Start()
    {
        initPos = transform.position;
    }
}
