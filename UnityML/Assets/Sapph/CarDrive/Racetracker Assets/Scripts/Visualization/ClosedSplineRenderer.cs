using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedSplineRenderer : MonoBehaviour
{
    private int resolution = 500;
    private float width = 1f;
    private ClosedSpline<Vector3> spline = null;
    
    private MeshFilter meshFilter = null;

    // Use this for initialization
    void Start ()
    {
        meshFilter = GetComponent<MeshFilter>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void render()
    {
        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
        }

        if (spline != null)
        {
            List<Vector3> verts = new List<Vector3>();
            List<int> trias = new List<int>();
            List<Vector2> uvs = new List<Vector2>();

            Vector3 tangentF = spline.TangentAt(0f)[1] - spline.TangentAt(0f)[0];
            Vector3 toRightF = ((Quaternion.Euler(0f, 90f, 0f)) * tangentF).normalized;
            verts.Add(spline.SplineAt(0f) - toRightF * width);
            verts.Add(spline.SplineAt(0f) + toRightF * width);
            uvs.Add(new Vector2((spline.SplineAt(0f) - toRightF * width).x, (spline.SplineAt(0f) - toRightF * width).z));
            uvs.Add(new Vector2((spline.SplineAt(0f) + toRightF * width).x, (spline.SplineAt(0f) + toRightF * width).z));

            for (int i = 1; i < resolution; i++)
            {
                float s = ((float)i) / resolution;


                Vector3 tangent = spline.TangentAt(s)[1] - spline.TangentAt(s)[0];
                Vector3 toRight = ((Quaternion.Euler(0f, 90f, 0f)) * tangent).normalized;
                verts.Add(spline.SplineAt(s) - toRight * width);
                verts.Add(spline.SplineAt(s) + toRight * width);
                uvs.Add(new Vector2((spline.SplineAt(s) - toRight * width).x, (spline.SplineAt(s) - toRight * width).z));
                uvs.Add(new Vector2((spline.SplineAt(s) + toRight * width).x, (spline.SplineAt(s) + toRight * width).z));

                trias.Add(verts.Count - 4);
                trias.Add(verts.Count - 2);
                trias.Add(verts.Count - 3);

                trias.Add(verts.Count - 2);
                trias.Add(verts.Count - 1);
                trias.Add(verts.Count - 3);
            }

            meshFilter.mesh.Clear();
            meshFilter.mesh.vertices = verts.ToArray();
            meshFilter.mesh.uv = uvs.ToArray();

            meshFilter.mesh.subMeshCount = 1;

            meshFilter.mesh.SetTriangles(trias.ToArray(), 0);

            meshFilter.mesh.RecalculateNormals();
        }

    }

    public ClosedSpline<Vector3> ClosedSpline
    {
        get
        {
            return spline;
        }
        set
        {
            spline = value;

            render();
        }
    }

    public float Width
    {
        get
        {
            return width;
        }
        set
        {
            width = value;

            render();
        }
    }
}
