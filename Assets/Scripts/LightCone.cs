using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCone : MonoBehaviour {
    public float height = 2f;
    public float angle = 10f;
    
    [Range(2,20)]
    public int segments = 20;

    private MeshFilter _meshFilter;
    private float _previousHeight;
    private float _previousAngle;
    private int _previousSegments;

    void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        UpdateMesh();
    }

    void Update()
    {
        if (height != _previousHeight || angle != _previousAngle || segments != _previousSegments)
        {
            UpdateMesh();
        }
    }

    void UpdateMesh()
    {
        _meshFilter.mesh = CreateConeMesh(height, angle, segments);
        _previousHeight = height;
        _previousAngle = angle;
        _previousSegments = segments;
    }

    Mesh CreateConeMesh(float height, float angle, int segments)
    {
        Mesh mesh = new Mesh();

        float radius = Mathf.Tan(Mathf.Deg2Rad * angle) * height;

        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero; // Tip of the cone
        for (int i = 0; i <= segments; i++)
        {
            float theta = (float)i / segments * 2.0f * Mathf.PI;
            float x = radius * Mathf.Cos(theta);
            float z = radius * Mathf.Sin(theta);
            vertices[i + 1] = new Vector3(x, -height, z); // Adjusted to have the tip at (0,0,0)
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2 == segments + 1 ? 1 : i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}