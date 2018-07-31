using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallWall : WallRenderer
{
    private float wallHeight = 3f;
    private float wallWidth = 1f;

    protected override void refreshMesh()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        float overtake = 0f;

        for (int i = 0; i < controlPoints.Length; i++)
        {
            int i0 = (i == 0 ? controlPoints.Length - 1 : (i - 1));
            int i2 = (i + 1) % controlPoints.Length;

            Vector3 toRight = controlPoints[i2] - controlPoints[i0];
            toRight = (new Vector3(toRight.z, 0f, -toRight.x)).normalized;

            vertices.Add(controlPoints[i] + toRight * wallWidth * 0.5f + new Vector3(0f, -2f, 0f));
            vertices.Add(controlPoints[i] + new Vector3(0f, wallHeight, 0f) + toRight * wallWidth * 0.5f);
            vertices.Add(controlPoints[i] + new Vector3(0f, wallHeight, 0f) - toRight * wallWidth * 0.5f);
            vertices.Add(controlPoints[i] - toRight * wallWidth * 0.5f + new Vector3(0f, -2f, 0f));

            uvs.Add(new Vector2(overtake, 0f));
            uvs.Add(new Vector2(overtake, 1f/3f));
            uvs.Add(new Vector2(overtake, 2f/3f));
            uvs.Add(new Vector2(overtake, 1f));

            overtake += ((Vector3.Distance(controlPoints[i], controlPoints[i2])) / wallHeight);

            if (i >= 1)
            {
                triangles.Add(vertices.Count - 8);
                triangles.Add(vertices.Count - 7);
                triangles.Add(vertices.Count - 3);
                triangles.Add(vertices.Count - 8);
                triangles.Add(vertices.Count - 3);
                triangles.Add(vertices.Count - 4);

                triangles.Add(vertices.Count - 7);
                triangles.Add(vertices.Count - 6);
                triangles.Add(vertices.Count - 2);
                triangles.Add(vertices.Count - 7);
                triangles.Add(vertices.Count - 2);
                triangles.Add(vertices.Count - 3);

                triangles.Add(vertices.Count - 6);
                triangles.Add(vertices.Count - 5);
                triangles.Add(vertices.Count - 1);
                triangles.Add(vertices.Count - 6);
                triangles.Add(vertices.Count - 1);
                triangles.Add(vertices.Count - 2);

            }
        }


        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 3);
        triangles.Add(1);
        triangles.Add(vertices.Count - 4);
        triangles.Add(1);
        triangles.Add(0);

        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);
        triangles.Add(2);
        triangles.Add(vertices.Count - 3);
        triangles.Add(2);
        triangles.Add(1);

        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);
        triangles.Add(3);
        triangles.Add(vertices.Count - 2);
        triangles.Add(3);
        triangles.Add(2);




        GetComponent<MeshFilter>().mesh.Clear();
        GetComponent<MeshFilter>().mesh.vertices = vertices.ToArray();
        GetComponent<MeshFilter>().mesh.uv = uvs.ToArray();

        GetComponent<MeshFilter>().mesh.subMeshCount = 1;

        GetComponent<MeshFilter>().mesh.SetTriangles(triangles.ToArray(), 0);

        GetComponent<MeshFilter>().mesh.RecalculateNormals();

        GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
    }
}
