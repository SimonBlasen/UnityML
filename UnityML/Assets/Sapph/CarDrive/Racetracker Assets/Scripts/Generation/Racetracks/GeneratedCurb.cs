using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedCurb
{
    private float minOuterOffset = 0.08f;
    private float curbDistanceFactor = 0.3f;

    private List<Vector3> renderVertices = new List<Vector3>();
    private List<Vector2> renderUVs = new List<Vector2>();
    private List<int> renderTriangles = new List<int>();

    public GeneratedCurb(Vector3[] vertices, Vector3[] directions, float width, float height, float endAngle, Vector3 prevT, Vector3 afteT)
    {
        Width = width;

        height = 0.1f;

        Vector3[] innerVerts = new Vector3[vertices.Length];
        Vector3[] outerVerts = new Vector3[vertices.Length];
        Vector2[] borderLine = new Vector2[vertices.Length];

        float wentDistance = 0f;

        innerVerts[0] = vertices[0];

        Vector3 toForwardFirst = Vector3.Cross(directions[0], Vector3.up);
        Vector3 goOffsetFirst = Vector3.Cross(directions[0], toForwardFirst).normalized;
        if (goOffsetFirst.y < 0f)
        {
            goOffsetFirst *= -1f;
        }

        Vector3 newDirFirst = (directions[0].normalized * width + goOffsetFirst * minOuterOffset).normalized;

        outerVerts[0] = vertices[0] + directions[0].normalized * width;
        borderLine[0] = new Vector2(outerVerts[0].x, outerVerts[0].z);

        for (int i = 1; i < vertices.Length; i++)
        {
            innerVerts[i] = vertices[i];

            Vector3 toForward = Vector3.Cross(directions[i], Vector3.up);
            Vector3 goOffset = Vector3.Cross(toForward, directions[i]).normalized;

            Vector3 newDir = (directions[i].normalized * width + goOffset * minOuterOffset).normalized;

            outerVerts[i] = vertices[i] + directions[i].normalized * width;
            borderLine[i] = new Vector2(outerVerts[i].x, outerVerts[i].z);
        }

        List<RenderVertexShortcut> scs = GeneratedTrack.calculateShortcuts(borderLine, true);

        for (int i = 0; i < scs.Count; i++)
        {
            RenderVertexShortcut rvs = scs[i];
            for (int j = rvs.indexBefore + 1; j < rvs.indexAfter; j++)
            {
                // The 0.2f offset results of that the inner of a sharp curve may always be a little higher on the curbs
                outerVerts[j] = new Vector3(rvs.intersectPoint.x, (outerVerts[rvs.indexBefore].y + outerVerts[rvs.indexAfter].y) * 0.5f + 0.2f, rvs.intersectPoint.y);
            }
        }



        wentDistance = 0f;

        renderVertices.Add(new Vector3(innerVerts[0].x, 0f, innerVerts[0].z));
        renderUVs.Add(new Vector2(0f, 0f));

        toForwardFirst = Vector3.Cross(directions[0], Vector3.up);
        goOffsetFirst = Vector3.Cross(directions[0], toForwardFirst).normalized;
        if (goOffsetFirst.y < 0f)
        {
            goOffsetFirst *= -1f;
        }

        newDirFirst = (directions[0].normalized * width + goOffsetFirst * minOuterOffset).normalized;

        int plus3 = 0;

        if (height > 0f)
        {
            Vector3 v1 = renderVertices[renderVertices.Count - 1] + ((new Vector3(outerVerts[0].x, 0f, outerVerts[0].z)) - renderVertices[renderVertices.Count - 1]) * 0.25f + goOffsetFirst * (height * (2f / 3f));
            Vector3 v2 = renderVertices[renderVertices.Count - 1] + ((new Vector3(outerVerts[0].x, 0f, outerVerts[0].z)) - renderVertices[renderVertices.Count - 1]) * 0.5f + goOffsetFirst * (height * (1f));
            Vector3 v3 = renderVertices[renderVertices.Count - 1] + ((new Vector3(outerVerts[0].x, 0f, outerVerts[0].z)) - renderVertices[renderVertices.Count - 1]) * 0.75f + goOffsetFirst * (height * (2f / 3f));

            renderVertices.Add(v1);
            renderVertices.Add(v2);
            renderVertices.Add(v3);
            renderUVs.Add(new Vector2(0.25f, 0f));
            renderUVs.Add(new Vector2(0.5f, 0f));
            renderUVs.Add(new Vector2(0.75f, 0f));

            plus3 = 4;
        }

        renderVertices.Add(new Vector3(outerVerts[0].x, 0f, outerVerts[0].z));
        renderUVs.Add(new Vector2(1f, 0f));

        for (int i = 1; i < vertices.Length; i++)
        {
            wentDistance += Vector3.Distance(outerVerts[i - 1], outerVerts[i]);

            renderVertices.Add(new Vector3(innerVerts[i].x, 0f, innerVerts[i].z));
            renderUVs.Add(new Vector2(0f, wentDistance * curbDistanceFactor));

            Vector3 toForward = Vector3.Cross(directions[i], Vector3.up);
            Vector3 goOffset = Vector3.Cross(toForward, directions[i]).normalized;

            if (goOffset.y < 0f)
            {
                goOffset *= -1f;
            }

            if (height > 0f)
            {
                Vector3 v1 = renderVertices[renderVertices.Count - 1] + ((new Vector3(outerVerts[i].x, 0f, outerVerts[i].z)) - renderVertices[renderVertices.Count - 1]) * 0.25f + goOffset * (height * (2f / 3f));
                Vector3 v2 = renderVertices[renderVertices.Count - 1] + ((new Vector3(outerVerts[i].x, 0f, outerVerts[i].z)) - renderVertices[renderVertices.Count - 1]) * 0.5f + goOffset * (height * (1f));
                Vector3 v3 = renderVertices[renderVertices.Count - 1] + ((new Vector3(outerVerts[i].x, 0f, outerVerts[i].z)) - renderVertices[renderVertices.Count - 1]) * 0.75f + goOffset * (height * (2f / 3f));

                renderVertices.Add(v1);
                renderVertices.Add(v2);
                renderVertices.Add(v3);
                renderUVs.Add(new Vector2(0.25f, wentDistance * curbDistanceFactor));
                renderUVs.Add(new Vector2(0.5f, wentDistance * curbDistanceFactor));
                renderUVs.Add(new Vector2(0.75f, wentDistance * curbDistanceFactor));
            }

            renderVertices.Add(new Vector3(outerVerts[i].x, 0f, outerVerts[i].z));
            renderUVs.Add(new Vector2(1f, wentDistance * curbDistanceFactor));


            /*renderTriangles.Add(renderVertices.Count - 4 - plus3);
            if (Vector3.Cross(renderVertices[renderVertices.Count - 4 - plus3] - renderVertices[renderVertices.Count - 3 - plus3], renderVertices[renderVertices.Count - 2] - renderVertices[renderVertices.Count - 4 - plus3]).y < 0)
            {
                renderTriangles.Add(renderVertices.Count - 3 - plus3);
                renderTriangles.Add(renderVertices.Count - 2);
            }
            else
            {
                renderTriangles.Add(renderVertices.Count - 2);
                renderTriangles.Add(renderVertices.Count - 3 - plus3);
            }*/

            if (plus3 == 0)
            {
                renderTriangles.Add(renderVertices.Count - 4 - plus3);
                if (Vector3.Cross(renderVertices[renderVertices.Count - 4 - plus3] - renderVertices[renderVertices.Count - 3 - plus3], renderVertices[renderVertices.Count - 2] - renderVertices[renderVertices.Count - 4 - plus3]).y < 0)
                {
                    renderTriangles.Add(renderVertices.Count - 3 - plus3);
                    renderTriangles.Add(renderVertices.Count - 2);
                }
                else
                {
                    renderTriangles.Add(renderVertices.Count - 2);
                    renderTriangles.Add(renderVertices.Count - 3 - plus3);
                }

                renderTriangles.Add(renderVertices.Count - 2);
                if (Vector3.Cross(renderVertices[renderVertices.Count - 2] - renderVertices[renderVertices.Count - 1], renderVertices[renderVertices.Count - 3 - plus3] - renderVertices[renderVertices.Count - 2]).y < 0)
                {
                    renderTriangles.Add(renderVertices.Count - 1);
                    renderTriangles.Add(renderVertices.Count - 3 - plus3);
                }
                else
                {
                    renderTriangles.Add(renderVertices.Count - 3 - plus3);
                    renderTriangles.Add(renderVertices.Count - 1);
                }
            }

            else
            {
                int newPlus1 = plus3 - 1;

                for (int j = 0; j < plus3; j++)
                {
                    renderTriangles.Add(renderVertices.Count - 4 - newPlus1 - j);
                    if (Vector3.Cross(renderVertices[renderVertices.Count - 4 - newPlus1 - j] - renderVertices[renderVertices.Count - 3 - newPlus1 - j], renderVertices[renderVertices.Count - 2 - j] - renderVertices[renderVertices.Count - 4 - newPlus1 - j]).y < 0)
                    {
                        renderTriangles.Add(renderVertices.Count - 3 - newPlus1 - j);
                        renderTriangles.Add(renderVertices.Count - 2 - j);
                    }
                    else
                    {
                        renderTriangles.Add(renderVertices.Count - 2 - j);
                        renderTriangles.Add(renderVertices.Count - 3 - newPlus1 - j);
                    }

                    renderTriangles.Add(renderVertices.Count - 2 - j);
                    if (Vector3.Cross(renderVertices[renderVertices.Count - 2 - j] - renderVertices[renderVertices.Count - 1 - j], renderVertices[renderVertices.Count - 3 - newPlus1 - j] - renderVertices[renderVertices.Count - 2 - j]).y < 0)
                    {
                        renderTriangles.Add(renderVertices.Count - 1 - j);
                        renderTriangles.Add(renderVertices.Count - 3 - newPlus1 - j);
                    }
                    else
                    {
                        renderTriangles.Add(renderVertices.Count - 3 - newPlus1 - j);
                        renderTriangles.Add(renderVertices.Count - 1 - j);
                    }
                }

                
            }

            
        }

        renderVertices.Add(new Vector3(prevT.x, 0f, prevT.z));
        renderUVs.Add(new Vector2(0f, Vector3.Distance(renderVertices[renderVertices.Count - 1], renderVertices[0]) * -curbDistanceFactor));
        renderVertices.Add(new Vector3(afteT.x, 0f, afteT.z));
        renderUVs.Add(new Vector2(0f, (wentDistance + Vector3.Distance(renderVertices[renderVertices.Count - 1], renderVertices[renderVertices.Count - 4])) * curbDistanceFactor));
        
        if (plus3 == 0)
        {
            renderTriangles.Add(renderVertices.Count - 2);
            if (Vector3.Cross(renderVertices[renderVertices.Count - 2] - renderVertices[0], renderVertices[1] - renderVertices[renderVertices.Count - 2]).y < 0)
            {
                renderTriangles.Add(0);
                renderTriangles.Add(1);
            }
            else
            {
                renderTriangles.Add(1);
                renderTriangles.Add(0);
            }

            renderTriangles.Add(renderVertices.Count - 1);
            if (Vector3.Cross(renderVertices[renderVertices.Count - 1] - renderVertices[renderVertices.Count - 4], renderVertices[renderVertices.Count - 3] - renderVertices[renderVertices.Count - 1]).y < 0)
            {
                renderTriangles.Add(renderVertices.Count - 4);
                renderTriangles.Add(renderVertices.Count - 3);
            }
            else
            {
                renderTriangles.Add(renderVertices.Count - 3);
                renderTriangles.Add(renderVertices.Count - 4);
            }
        }

        else
        {
            for (int j = 0; j < plus3; j++)
            {
                renderTriangles.Add(renderVertices.Count - 2);
                if (Vector3.Cross(renderVertices[renderVertices.Count - 2] - renderVertices[0 + j], renderVertices[1 + j] - renderVertices[renderVertices.Count - 2]).y < 0)
                {
                    renderTriangles.Add(0 + j);
                    renderTriangles.Add(1 + j);
                }
                else
                {
                    renderTriangles.Add(1 + j);
                    renderTriangles.Add(0 + j);
                }

                renderTriangles.Add(renderVertices.Count - 1);
                if (Vector3.Cross(renderVertices[renderVertices.Count - 1] - renderVertices[renderVertices.Count - 7 + j], renderVertices[renderVertices.Count - 6 + j] - renderVertices[renderVertices.Count - 1]).y < 0)
                {
                    renderTriangles.Add(renderVertices.Count - 7 + j);
                    renderTriangles.Add(renderVertices.Count - 6 + j);
                }
                else
                {
                    renderTriangles.Add(renderVertices.Count - 6 + j);
                    renderTriangles.Add(renderVertices.Count - 7 + j);
                }
            }


        }
    }


    public Vector3[] RenderVertices
    {
        get
        {
            return renderVertices.ToArray();
        }
    }

    public Vector2[] RenderUVs
    {
        get
        {
            return renderUVs.ToArray();
        }
    }

    public int[] RenderTriangles
    {
        get
        {
            return renderTriangles.ToArray();
        }
    }

    public float Width { get; private set; }
}
