using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class meshGen_Brackeys : MonoBehaviour
{
    Mesh _mesh;

    Vector3[] vertices;
    int[] triangles;

    private void Start()
    {
        _mesh = new Mesh();

        GetComponent<MeshFilter>().mesh = _mesh;


    }


}
