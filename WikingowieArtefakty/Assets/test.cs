using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public int width = 10; // Liczba wierzcho³ków w szerokoœci
    public int length = 10; // Liczba wierzcho³ków w d³ugoœci

    Vector3[] newVertices;
    Vector2[] newUV;
    int[] newTriangles;

    void Awake()
    {
        GenerateMesh();
    }

    void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        int numVertices = (width + 1) * (length + 1);
        newVertices = new Vector3[numVertices];
        newUV = new Vector2[numVertices];

        for (int z = 0; z <= length; z++)
        {
            for (int x = 0; x <= width; x++)
            {
                int index = x + z * (width + 1);
                float xPos = (float)x / width;
                float zPos = (float)z / length;
                newVertices[index] = new Vector3(xPos, 0, zPos);
                newUV[index] = new Vector2(xPos, zPos);
            }
        }

        int numTriangles = width * length * 6;
        newTriangles = new int[numTriangles];
        int t = 0;
        for (int z = 0; z < length; z++)
        {
            for (int x = 0; x < width; x++)
            {
                int vertexIndex = x + z * (width + 1);
                newTriangles[t] = vertexIndex;
                newTriangles[t + 1] = vertexIndex + width + 1;
                newTriangles[t + 2] = vertexIndex + 1;
                newTriangles[t + 3] = vertexIndex + 1;
                newTriangles[t + 4] = vertexIndex + width + 1;
                newTriangles[t + 5] = vertexIndex + width + 2;
                t += 6;
            }
        }

        mesh.vertices = newVertices;
        mesh.uv = newUV;
        mesh.triangles = newTriangles;
    }
}
