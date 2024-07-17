using UnityEngine;

namespace World {
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class CustomCube : MonoBehaviour {

        private void Start() {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            Mesh mesh = new Mesh();
            mesh.name = "CustomCube";

            // Define vertices for each face
            Vector3[][] faceVertices = {
                new[] // Front face
                {
                    new Vector3(-0.5f, -0.5f, 0.5f),
                    new Vector3(0.5f, -0.5f, 0.5f),
                    new Vector3(0.5f, 0.5f, 0.5f),
                    new Vector3(-0.5f, 0.5f, 0.5f),
                },
                new[] // Back face
                {
                    new Vector3(-0.5f, -0.5f, -0.5f),
                    new Vector3(0.5f, -0.5f, -0.5f),
                    new Vector3(0.5f, 0.5f, -0.5f),
                    new Vector3(-0.5f, 0.5f, -0.5f),
                },
                new[] // Left face
                {
                    new Vector3(-0.5f, -0.5f, -0.5f),
                    new Vector3(-0.5f, 0.5f, -0.5f),
                    new Vector3(-0.5f, 0.5f, 0.5f),
                    new Vector3(-0.5f, -0.5f, 0.5f),
                },
                new[] // Right face
                {
                    new Vector3(0.5f, -0.5f, -0.5f),
                    new Vector3(0.5f, -0.5f, 0.5f),
                    new Vector3(0.5f, 0.5f, 0.5f),
                    new Vector3(0.5f, 0.5f, -0.5f),
                },
                new[] // Top face
                {
                    new Vector3(-0.5f, 0.5f, 0.5f),
                    new Vector3(0.5f, 0.5f, 0.5f),
                    new Vector3(0.5f, 0.5f, -0.5f),
                    new Vector3(-0.5f, 0.5f, -0.5f),
                },
                new[] // Bottom face
                {
                    new Vector3(-0.5f, -0.5f, 0.5f),
                    new Vector3(0.5f, -0.5f, 0.5f),
                    new Vector3(0.5f, -0.5f, -0.5f),
                    new Vector3(-0.5f, -0.5f, -0.5f),
                },
            };

            // Define triangles for each face
            int[][] faceTriangles = {
                new[] { 0, 1, 2, 0, 2, 3 }, // Back face
                new[] { 0, 2, 1, 0, 3, 2 }, // Front face
                new[] { 0, 3, 2, 0, 2, 1 }, // Left face
                new[] { 0, 2, 1, 0, 3, 2 }, // Right face
                new[] { 0, 1, 2, 0, 2, 3 }, // Bottom face
                new[] { 0, 2, 1, 0, 3, 2 }, // Top face
            };

            // Define normals for each face
            Vector3[] faceNormals = {
                Vector3.forward, Vector3.back, Vector3.left, Vector3.right, Vector3.up, Vector3.down,
            };

            // Initialize arrays for vertices, normals, and submeshes
            Vector3[] vertices = new Vector3[24];
            Vector3[] normals = new Vector3[24];
            int[][] submeshTriangles = new int[6][];
            Vector2[] uv = new Vector2[24];

            for (int i = 0; i < 6; i++) {
                for (int j = 0; j < 4; j++) {
                    vertices[i * 4 + j] = faceVertices[i][j];
                    normals[i * 4 + j] = faceNormals[i];
                    uv[i * 4 + j] = new Vector2(vertices[i * 4 + j].x, vertices[i * 4 + j].y);
                }

                submeshTriangles[i] = new int[6];
                for (int k = 0; k < 6; k++) {
                    submeshTriangles[i][k] = faceTriangles[i][k] + i * 4;
                }
            }

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.uv = uv;

            // Assign submeshes
            mesh.subMeshCount = 6;
            for (int i = 0; i < 6; i++) {
                mesh.SetTriangles(submeshTriangles[i], i);
            }

            meshFilter.mesh = mesh;

            // Assign a material to the mesh renderer
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            Material[] materials = new Material[6];
            for (int i = 0; i < 6; i++) {
                materials[i] = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            }
            meshRenderer.materials = materials;

            // Set the color of the faces
            SetFaceColor(0, Color.red); // Front
            SetFaceColor(1, Color.blue); // Back
            SetFaceColor(2, Color.green); // Left
            SetFaceColor(3, Color.yellow); // Right
            SetFaceColor(4, Color.cyan); // Top
            SetFaceColor(5, Color.magenta); // Bottom
        }


        public void SetFaceColor(int faceIndex, Color color) {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (faceIndex >= 0 && faceIndex < meshRenderer.materials.Length) {
                meshRenderer.materials[faceIndex].color = color;
            }
        }
    }
}