using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshGen : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Procedural Quad";

        List<Vector3> vertexs = new List<Vector3>(){
            new Vector3(-1,-1),
            new Vector3(-1,1),
            new Vector3(1,1),
            new Vector3(1,-1)
        };

        int[] triIndices = new int[]
        {
            2, 0, 1,
            2, 3, 0
        };
        
        mesh.SetVertices(vertexs);
        mesh.triangles = triIndices;
        
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
