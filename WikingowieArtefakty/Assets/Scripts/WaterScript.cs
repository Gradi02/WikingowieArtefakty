using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterScript : MonoBehaviour
{
    public int segmentsX = 100; // liczba segmentów wzd³u¿ osi X
    public int segmentsY = 100; // liczba segmentów wzd³u¿ osi Y

    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            Mesh mesh = meshFilter.mesh;
            mesh.Clear();

            mesh.vertices = GenerateVertices();
            mesh.triangles = GenerateTriangles();
            mesh.RecalculateNormals();
        }
    }

    Vector3[] GenerateVertices()
    {
        Vector3[] vertices = new Vector3[(segmentsX + 1) * (segmentsY + 1)];

        for (int y = 0; y <= segmentsY; y++)
        {
            for (int x = 0; x <= segmentsX; x++)
            {
                float normalizedX = x / (float)segmentsX;
                float normalizedY = y / (float)segmentsY;

                vertices[x + y * (segmentsX + 1)] = new Vector3(normalizedX, 0, normalizedY);
            }
        }

        return vertices;
    }

    int[] GenerateTriangles()
    {
        int[] triangles = new int[segmentsX * segmentsY * 6];

        int index = 0;
        for (int y = 0; y < segmentsY; y++)
        {
            for (int x = 0; x < segmentsX; x++)
            {
                int vertexIndex = x + y * (segmentsX + 1);

                triangles[index] = vertexIndex;
                triangles[index + 1] = vertexIndex + 1;
                triangles[index + 2] = vertexIndex + segmentsX + 1;

                triangles[index + 3] = vertexIndex + 1;
                triangles[index + 4] = vertexIndex + segmentsX + 2;
                triangles[index + 5] = vertexIndex + segmentsX + 1;

                index += 6;
            }
        }

        return triangles;
    }
}
