using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossoverRegion : MonoBehaviour
{
    [SerializeField]
    private float a = 0.1f;
    [SerializeField]
    private float c = 0.1f;
    [SerializeField]
    private float angleResolution = 15f;
    [SerializeField]
    private float maxDistance = 10f;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Render(Vector3[] points, Vector3[] directions, bool circle)
    {
        int stepsAmount = 0;

        Line[] lines = new Line[circle ? points.Length : points.Length - 1];
        for (int i = 0; i < lines.Length; i++)
        {
            int i2 = (i + 1) % points.Length;
            lines[i] = new Line(new Vector2(points[i].x, points[i].z), new Vector2(points[i2].x, points[i2].z));
        }

        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> triangles = new List<int>();

        float angle = 0f;

        if (points.Length >= 4)
        {
            for (int i = 0; i < points.Length; i++)
            {
                Vector3 toRight;
                if (circle)
                {
                    Vector3 nextPoint = points[(i + 1) % points.Length];
                    Vector3 prevPoint = points[i - 1 < 0 ? points.Length - 1 : i - 1];

                    toRight = (nextPoint - prevPoint).normalized;

                    angle = 0f;// Vector3.Angle(points[i] - prevPoint, nextPoint - points[i]);

                    for (int j = -2; j <= 2; j++)
                    {
                        int prevJ = (j + i - 1) < 0 ? (points.Length + (j + i - 1)) : ((j + i - 1) % points.Length);
                        int nextJ = (j + i + 1) < 0 ? (points.Length + (j + i + 1)) : ((j + i + 1) % points.Length);
                        int atJ = (j + i) < 0 ? (points.Length + (j + i)) : ((j + i) % points.Length);

                        float angleHere = Vector3.Angle(points[atJ] - points[prevJ], points[nextJ] - points[atJ]);

                        float factor = 0f;
                        if (Mathf.Abs(j) == 2)
                        {
                            factor = 0.06f;
                        }
                        else if (Mathf.Abs(j) == 1)
                        {
                            factor = 0.23f;
                        }
                        else
                        {
                            factor = 0.42f;
                        }

                        angle += (angleHere * factor);
                    }

                    Vector2 forward = new Vector2(points[i].x - prevPoint.x, points[i].z - prevPoint.z);
                    Ray2D rayF = new Ray2D(new Vector2(prevPoint.x, prevPoint.z), forward);

                    if ((Utils.PointRightTo(rayF, new Vector2(nextPoint.x, nextPoint.z))) == (Vector2.Angle(new Vector2(-directions[i].z, directions[i].x), rayF.direction) >= 90f))
                    {
                        angle = 0f;
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        toRight = (points[i + 1] - points[0]).normalized;
                    }
                    else if (i == points.Length - 1)
                    {
                        toRight = (points[points.Length - 1] - points[points.Length - 2]).normalized;
                    }
                    else
                    {
                        Vector3 nextPoint = points[i + 1];
                        Vector3 prevPoint = points[i - 1];

                        toRight = (nextPoint - prevPoint).normalized;
                    }
                }

                Vector3 down = Vector3.Cross(toRight, directions[i]).normalized;
                if (down.y > 0f)
                {
                    down *= -1f;
                }

                float minDistance = float.MaxValue;
                int lineIndex = -1;
                Line thisLine = new Line(new Vector2(points[i].x + directions[i].normalized.x * 0.05f, points[i].z + directions[i].normalized.z * 0.05f), new Vector2(points[i].x + directions[i].normalized.x, points[i].z + directions[i].normalized.z));
                for (int j = 0; j < lines.Length; j++)
                {
                    int j1 = j == 0 ? lines.Length - 1 : j - 1;
                    int j2 = (j + 1) % lines.Length;
                    Vector2 intersectPoint;
                    if (j1 != i && j != i && j2 != i && lines[i].Intersects(thisLine, out intersectPoint))
                    {
                        if (Vector2.Distance(intersectPoint, new Vector2(points[i].x, points[i].z)) < minDistance)
                        {
                            minDistance = Vector2.Distance(intersectPoint, new Vector2(points[i].x, points[i].z));
                            lineIndex = j;
                        }
                    }
                }

                if (lineIndex != -1)
                {
                    if (minDistance < 0.1f)
                    {
                        minDistance = 0.1f;
                    }

                    int lineIndex2 = (lineIndex + 1) % points.Length;
                    Vector3 lowestPoint = points[lineIndex];
                    if (lowestPoint.y > points[lineIndex2].y)
                    {
                        lowestPoint = points[lineIndex2];
                    }

                    directions[i] = new Vector3(directions[i].normalized.x, (lowestPoint.y - points[i].y) * (1f / minDistance), directions[i].normalized.z);
                }

                uvs.Add(new Vector2(points[i].x, points[i].z));
                vertices.Add(points[i]);

                int counter = 0;
                float x = 0f;
                while (stepsAmount == 0 ? x < maxDistance : counter < stepsAmount)
                {
                    float[] func = fallFunction(angleResolution * counter);

                    x = func[0];

                    Vector3 vert = points[i] + directions[i].normalized * func[0] * Mathf.Min((1f / (angle / 4.5f)), 1f) + down * func[1];

                    //vertices.Add(new Vector3(vert.x, (GeneratedTrack.GetTensorHeight(vert) - 1.2f) * (x / maxDistance) + ((maxDistance - x) / maxDistance) * vert.y, vert.z));
                    vertices.Add(new Vector3(vert.x, (GeneratedTrack.GetTensorHeight(vert)) + (x / maxDistance) * -1.2f, vert.z));
                    uvs.Add(new Vector2(vertices[vertices.Count - 1].x, vertices[vertices.Count - 1].z));

                    counter++;
                }

                if (stepsAmount == 0)
                {
                    stepsAmount = counter;
                }


                if (i > 0)
                {
                    for (int j = 0; j < stepsAmount - 1; j++)
                    {
                        if (Vector3.Cross((vertices[vertices.Count - (j + 1)] - vertices[vertices.Count - (j + 1 + stepsAmount + 1)]), (vertices[vertices.Count - (j + 1 + stepsAmount + 1)] - vertices[vertices.Count - (j + 2 + stepsAmount + 1)])).y > 0f)
                        {
                            triangles.Add(vertices.Count - (j + 1 + stepsAmount + 1));
                            triangles.Add(vertices.Count - (j + 2 + stepsAmount + 1));
                            triangles.Add(vertices.Count - (j + 1));
                        }
                        else
                        {
                            triangles.Add(vertices.Count - (j + 1 + stepsAmount + 1));
                            triangles.Add(vertices.Count - (j + 1));
                            triangles.Add(vertices.Count - (j + 2 + stepsAmount + 1));
                        }

                        if (Vector3.Cross((vertices[vertices.Count - (j + 2)] - vertices[vertices.Count - (j + 1)]), (vertices[vertices.Count - (j + 1)] - vertices[vertices.Count - (j + 2 + stepsAmount + 1)])).y > 0f)
                        {
                            triangles.Add(vertices.Count - (j + 1));
                            triangles.Add(vertices.Count - (j + 2 + stepsAmount + 1));
                            triangles.Add(vertices.Count - (j + 2));
                        }
                        else
                        {
                            triangles.Add(vertices.Count - (j + 1));
                            triangles.Add(vertices.Count - (j + 2));
                            triangles.Add(vertices.Count - (j + 2 + stepsAmount + 1));
                        }
                    }
                }
                if (i == points.Length - 1 && circle)
                {
                    for (int j = 0; j < stepsAmount - 1; j++)
                    {
                        if (Vector3.Cross((vertices[stepsAmount - (j + 1)] - vertices[vertices.Count - (j + 1)]), (vertices[vertices.Count - (j + 1)] - vertices[vertices.Count - (j + 2)])).y > 0f)
                        {
                            triangles.Add(vertices.Count - (j + 1));
                            triangles.Add(vertices.Count - (j + 2));
                            triangles.Add(stepsAmount - (j + 1));
                        }
                        else
                        {
                            triangles.Add(vertices.Count - (j + 1));
                            triangles.Add(stepsAmount - (j + 1));
                            triangles.Add(vertices.Count - (j + 2));
                        }

                        if (Vector3.Cross((vertices[stepsAmount - (j + 2)] - vertices[stepsAmount - (j + 1)]), (vertices[stepsAmount - (j + 1)] - vertices[vertices.Count - (j + 2)])).y > 0f)
                        {
                            triangles.Add(stepsAmount - (j + 1));
                            triangles.Add(vertices.Count - (j + 2));
                            triangles.Add(stepsAmount - (j + 2));
                        }
                        else
                        {
                            triangles.Add(stepsAmount - (j + 1));
                            triangles.Add(stepsAmount - (j + 2));
                            triangles.Add(vertices.Count - (j + 2));
                        }


                    }
                }
            }


            GetComponent<MeshFilter>().mesh.Clear();
            GetComponent<MeshFilter>().mesh.vertices = vertices.ToArray();

            for (int i = 0; i < uvs.Count; i++)
            {
                //uvs[i] = uvs[i] / 15f;
            }

            GetComponent<MeshFilter>().mesh.uv = uvs.ToArray();

            GetComponent<MeshFilter>().mesh.subMeshCount = 1;

            GetComponent<MeshFilter>().mesh.SetTriangles(triangles.ToArray(), 0);

            GetComponent<MeshFilter>().mesh.RecalculateNormals();

            GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh;
        }

        
    }

    public GeneratedTrack GeneratedTrack
    {
        get;
        set;
    }

    private float[] fallFunction(float angle)
    {
        if (angle == 0)
        {
            return new float[] { 0f, 0f };
        }

        float x = Mathf.Tan((angle * Mathf.PI) / 180f) / (2f * a);
        float y = a * x * x + c;

        return new float[] { x, y };
    }
}
