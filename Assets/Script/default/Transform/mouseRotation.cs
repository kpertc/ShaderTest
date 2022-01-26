using UnityEngine;

[AddComponentMenu("Common Scripts/Mouse Rotation")]
public class mouseRotation : MonoBehaviour
{
    Vector3 rotateResult;
    Vector3 currentRotation;
    Vector3 rotationOffset;

    public float mouseX;
    public float mouseY;
    public bool invertY;

    [Range(0,1)] public float sensitivity = 0.5f;

    public Vector3 initialRot;

    void Start()
    {
        transform.eulerAngles = initialRot;
        rotationOffset = new Vector3(mouseY, mouseX) * sensitivity - initialRot;
    }

    void Update()
    {
        mouseX = Input.mousePosition.x;
        mouseY = Input.mousePosition.y;

        if (invertY) rotateResult = new Vector3(mouseY + rotationOffset.x, -mouseX + rotationOffset.y, rotationOffset.z) * sensitivity;

        else rotateResult = new Vector3(mouseY + rotationOffset.x, mouseX + rotationOffset.y, rotationOffset.z) * sensitivity;

        transform.eulerAngles = rotateResult;
    }
}
