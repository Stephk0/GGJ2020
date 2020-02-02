using UnityEngine;
using System.Collections;

public class SetVertexColors : MonoBehaviour
{

    public Color color;
    public bool writeNormalsIntoColor;

    private Mesh sourceMesh;
    private Mesh smoothedMesh;

    public Transform smoothedMeshTransform;
    Vector3[] normals;

    void Start()
    {
        Mesh meshfilter = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = meshfilter.vertices;


        // Clone the cloth mesh to work on
        sourceMesh = new Mesh();
        // Get the sourceMesh from the originalSkinnedMesh
        sourceMesh = meshfilter;
        // Clone the sourceMesh 
        
        smoothedMesh = smoothedMeshTransform.GetComponent<MeshFilter>().mesh;
        
        //smoothedMesh.uv = null;
        smoothedMesh.RecalculateNormals();
        
        // Reference workingMesh to see deformations
        //meshfilter = workingMesh;

        // create new colors array where the colors will be created.
        Color[] colors = new Color[vertices.Length];
        Color[] normalsColor = new Color[vertices.Length];

        for (int i = 0; i < vertices.Length; i++)
        {
            colors[i] = color;
            //normalsColor[i] = new Color( smoothedMesh.normals[i].x, smoothedMesh.normals[i].y, smoothedMesh.normals[i].z) ;
            normalsColor[i] = smoothedMesh.colors32[i];
            //normalsColor[i] = mesh.normals;
        }

        // assign the array of colors to the Mesh.
        //meshfilter.colors = colors;
        normals = smoothedMesh.normals;
        if (writeNormalsIntoColor) meshfilter.colors = normalsColor;
        else meshfilter.colors = colors;
    }

    // Clone a mesh
    private static Mesh CloneMesh(Mesh mesh)
    {
        Mesh clone = new Mesh();
        clone.vertices = mesh.vertices;
        clone.normals = mesh.normals;
        clone.tangents = mesh.tangents;
        clone.triangles = mesh.triangles;
        clone.uv = mesh.uv;
        clone.uv2 = mesh.uv2;
        clone.uv2 = mesh.uv2;
        clone.bindposes = mesh.bindposes;
        clone.boneWeights = mesh.boneWeights;
        clone.bounds = mesh.bounds;
        clone.colors = mesh.colors;
        clone.name = mesh.name;
        //TODO : Are we missing anything?
        return clone;
    }
}