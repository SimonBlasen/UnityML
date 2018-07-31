using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedTrack
{
    private List<GeneratedElement> elements;
    private DiscreteTrack discreteTrack;
    private TerrainModifier terrainModifier = null;
    private BorderGenerator borderGenerator = null;
    private HistoProfile curvatureProfile;
    private HistoProfile speedProfile;
    private HistoProfile curvatureIdeallineProfile;
    private CurbsGenerator curbsGenerator;

    public GeneratedTrack()
    {
        elements = new List<GeneratedElement>();
    }

    public void AddElement(GeneratedElement element)
    {
        if (float.IsNaN(element.Position.x) || float.IsNaN(element.Position.z) || float.IsNaN(element.Position.y) ||
            float.IsNaN(element.EndPosition.x) || float.IsNaN(element.EndPosition.z) || float.IsNaN(element.EndPosition.y))
        {
            Faulty = true;
        }
        elements.Add(element);
    }

    public void Analyze()
    {
        discreteTrack = new DiscreteTrack(this, terrainModifier);

        speedProfile = new HistoProfile(17);
        speedProfile.SetBorders(10f, 90f);
        for (int i = 0; i < discreteTrack.speedAnalyzer.Vs.Length; i++)
        {
            speedProfile.AddValue(discreteTrack.speedAnalyzer.Vs[i]);
        }


        curvatureProfile = new HistoProfile(17);
        curvatureProfile.SetBorders(-0.1f, 0.1f);
        for (int i = 0; i < discreteTrack.curvatureAnalyzerTrack.Curvature.Length; i++)
        {
            curvatureProfile.AddValue(discreteTrack.curvatureAnalyzerTrack.Curvature[i]);
        }


        curvatureIdeallineProfile = new HistoProfile(17);
        curvatureIdeallineProfile.SetBorders(-0.07f, 0.07f);
        for (int i = 0; i < discreteTrack.curvatureIdealLineAnalyzer.Curvature.Length; i++)
        {
            curvatureIdeallineProfile.AddValue(discreteTrack.curvatureIdealLineAnalyzer.Curvature[i]);
        }
    }

    private float maxTurnDegreeResolution = 90f / 8f;

    public Vector3[] RenderVerticesTrack
    {
        get;
        set;
    }
    public int[] RenderTrianglesTrack
    {
        get;
        set;
    }
    public Vector2[] RenderUVsTrack
    {
        get;
        set;
    }

    public Vector3[] RenderVerticesGrassLeft
    {
        get;
        set;
    }
    public int[] RenderTrianglesGrassLeft
    {
        get;
        set;
    }
    public Vector2[] RenderUVsGrassLeft
    {
        get;
        set;
    }

    public Vector3[] RenderVerticesGrassRight
    {
        get;
        set;
    }
    public int[] RenderTrianglesGrassRight
    {
        get;
        set;
    }
    public Vector2[] RenderUVsGrassRight
    {
        get;
        set;
    }

    public GeneratedGrassPlane[] GrassPlanes
    {
        get;set;
    }

    public long Seed { get; set; }

    public void GenerateMesh()
    {
        // right Track Vertices
        List<Vector2> rT = new List<Vector2>();
        // left Track Vertices
        List<Vector2> lT = new List<Vector2>();
        // right Grass Vertices
        List<Vector2> rG = new List<Vector2>();
        // right Grass Vertices
        List<Vector2> lG = new List<Vector2>();
        
        
        /*if (Elements[0].GetType() == typeof(GeneratedStraight))
        {
            GeneratedStraight straight = (GeneratedStraight)Elements[0];

            Vector3 toRight = (Quaternion.Euler(0f, (straight.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(straight.WidthStart, 0f, 0f));

            lT.Add(new Vector2((straight.Position - toRight).x, (straight.Position - toRight).z));
            rT.Add(new Vector2((straight.Position + toRight).x, (straight.Position + toRight).z));

        }
        else if (Elements[0].GetType() == typeof(GeneratedTurn))
        {
            GeneratedTurn turn = (GeneratedTurn)Elements[0];

            Vector3 toRight = (Quaternion.Euler(0f, (turn.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(turn.WidthStart, 0f, 0f));

            lT.Add(new Vector2((turn.Position - toRight).x, (turn.Position - toRight).z));
            rT.Add(new Vector2((turn.Position + toRight).x, (turn.Position + toRight).z));
        }
        else if (Elements[0].GetType() == typeof(GeneratedBezSpline))
        {
            GeneratedBezSpline bezSpline = (GeneratedBezSpline)Elements[0];

            lT.Add(new Vector2((bezSpline.RenderVertsLeft[0]).x, (bezSpline.RenderVertsLeft[0]).z));
            rT.Add(new Vector2((bezSpline.RenderVertsRight[0]).x, (bezSpline.RenderVertsRight[0]).z));
        }*/


        for (int i = 0; i < Elements.Length; i++)
        {
            if (Elements[i].GetType() == typeof(GeneratedStraight))
            {
                GeneratedStraight straight = (GeneratedStraight)Elements[i];

                Vector3 toRight = (Quaternion.Euler(0f, (straight.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(straight.WidthEnd, 0f, 0f));
                Vector3 toFront = (Quaternion.Euler(0f, (straight.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(0f, 0f, straight.Length));


                int segmentsAmount = (int)(straight.Length / DiscreteTrack.straightSegmentingFactorMesh);

                for (int j = 1; j <= segmentsAmount; j++)
                {
                    float s = ((float)j) / segmentsAmount;


                    lT.Add(new Vector2((straight.Position + (toFront * s) - toRight).x, (straight.Position + (toFront * s) - toRight).z));
                    rT.Add(new Vector2((straight.Position + (toFront * s) + toRight).x, (straight.Position + (toFront * s) + toRight).z));

                    /*
                     *  -2    -1
                     *   x-----x   
                     *   |\    |  
                     *   | \   |  / \
                     *   |  \  |   |  Drive Direction
                     *   |   \ |   |
                     *   |    \|   
                     *   x-----x   
                     *  -4    -3
                     * 
                     * */
                }
            }
            else if (Elements[i].GetType() == typeof(GeneratedTurn))
            {
                GeneratedTurn turn = (GeneratedTurn)Elements[i];

                bool rightTurn = turn.Degree >= 0f;

                Vector3 toRight = (Quaternion.Euler(0f, (turn.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(1f, 0f, 0f));
                Vector3 toFront = (Quaternion.Euler(0f, (turn.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(0f, 0f, 1f));

                Vector3 middlePoint = toRight * turn.Radius * (rightTurn ? 1f : -1f);

                int segmentsAmount = ((int)(Mathf.Abs(turn.Degree) / maxTurnDegreeResolution)) + 1;

                for (int j = 1; j <= segmentsAmount; j++)
                {
                    Vector3 toRightTurned = (Quaternion.Euler(0f, (turn.Degree / ((float)segmentsAmount)) * j, 0f)) * toRight;

                    Vector3 segmentPos = middlePoint + (toRightTurned * (rightTurn ? -1f : 1f) * turn.Radius);

                    float currentWidth = (turn.WidthEnd - turn.WidthStart) * (float)((float)j / (float)segmentsAmount) + turn.WidthStart;

                    lT.Add(new Vector2((segmentPos + toRightTurned * currentWidth * -1f + turn.Position).x, (segmentPos + toRightTurned * currentWidth * -1f + turn.Position).z));
                    rT.Add(new Vector2((segmentPos + toRightTurned * currentWidth + turn.Position).x, (segmentPos + toRightTurned * currentWidth + turn.Position).z));
                    
                }
            }
            else if (Elements[i].GetType() == typeof(GeneratedBezSpline))
            {
                GeneratedBezSpline bezierSpline = (GeneratedBezSpline)Elements[i];

                for (int j = 1; j < bezierSpline.RenderVertsLeft.Length; j++)
                {
                    lT.Add(new Vector2((bezierSpline.RenderVertsLeft[j]).x, (bezierSpline.RenderVertsLeft[j]).z));
                    rT.Add(new Vector2((bezierSpline.RenderVertsRight[j]).x, (bezierSpline.RenderVertsRight[j]).z));
                }
            }
        }



        // Cutting of the overlapping segments

        List<RenderVertexShortcut> lSCs = calculateShortcuts(lT);
        List<RenderVertexShortcut> rSCs = calculateShortcuts(rT);

        for (int i = 0; i < lSCs.Count; i++)
        {
            RenderVertexShortcut rvs = lSCs[i];
            for (int j = rvs.indexBefore + 1; j < (rvs.indexAfter < rvs.indexBefore ? lT.Count : rvs.indexAfter); j++)
            {
                lT[j] = rvs.intersectPoint;
            }
            for (int j = 0; j < (rvs.indexAfter < rvs.indexBefore ? rvs.indexAfter : 0); j++)
            {
                lT[j] = rvs.intersectPoint;
            }
        }
        for (int i = 0; i < rSCs.Count; i++)
        {
            RenderVertexShortcut rvs = rSCs[i];
            for (int j = rvs.indexBefore + 1; j < (rvs.indexAfter < rvs.indexBefore ? rT.Count : rvs.indexAfter); j++)
            {
                rT[j] = rvs.intersectPoint;
            }
            for (int j = 0; j < (rvs.indexAfter < rvs.indexBefore ? rvs.indexAfter : 0); j++)
            {
                rT[j] = rvs.intersectPoint;
            }
        }








        float grassWidth = 8f;

        List<Vector2> lGFull = new List<Vector2>();
        List<Vector2> rGFull = new List<Vector2>();

        for (int i = 0; i < lT.Count; i++)
        {
            int i0 = (i - 1) < 0 ? lT.Count - 1 : i - 1;
            int i2 = (i + 1) % lT.Count;

            Vector2 tangentLeft = lT[i2] - lT[i0];
            Vector2 newVLeft = (new Vector2(-tangentLeft.y, tangentLeft.x)).normalized * grassWidth + lT[i];
            Vector2 tangentRight = rT[i2] - rT[i0];
            Vector2 newVRight = (new Vector2(tangentRight.y, -tangentRight.x)).normalized * grassWidth + rT[i];

            Vector2 vLeft = (lT[i] - rT[i]).normalized * grassWidth + lT[i];
            Vector2 vRight = (rT[i] - lT[i]).normalized * grassWidth + rT[i];

            lG.Add(newVLeft);
            rG.Add(newVRight);
            lGFull.Add(newVLeft);
            rGFull.Add(newVRight);
        }

        int grassPlanesNewStartIndex = -1;

        if (discreteTrack != null)
        {
            Vector2 startFinishPos = new Vector2(elements[(int)discreteTrack.startFinishLineIndex].Position.x, elements[(int)discreteTrack.startFinishLineIndex].Position.z);


            float minDistance = float.MaxValue;

            for (int i = 0; i < lGFull.Count; i++)
            {
                if (Vector2.Distance(lGFull[i], startFinishPos) < minDistance)
                {
                    grassPlanesNewStartIndex = i;
                    minDistance = Vector2.Distance(lGFull[i], startFinishPos);
                }
            }

            Debug.Log("Grass start index: " + grassPlanesNewStartIndex);

            List<Vector2> lGFullNew = new List<Vector2>();
            List<Vector2> rGFullNew = new List<Vector2>();
            List<Vector2> rTNew = new List<Vector2>();
            List<Vector2> lTNew = new List<Vector2>();
            List<Vector2> rGNew = new List<Vector2>();
            List<Vector2> lGNew = new List<Vector2>();

            for (int i = 0; i < lGFull.Count; i++)
            {
                lGFullNew.Add(lGFull[(i + grassPlanesNewStartIndex) % lGFull.Count]);
                lTNew.Add(lT[(i + grassPlanesNewStartIndex) % lGFull.Count]);
                lGNew.Add(lG[(i + grassPlanesNewStartIndex) % lGFull.Count]);

                rGFullNew.Add(rGFull[(i + grassPlanesNewStartIndex) % lGFull.Count]);
                rTNew.Add(rT[(i + grassPlanesNewStartIndex) % lGFull.Count]);
                rGNew.Add(rG[(i + grassPlanesNewStartIndex) % lGFull.Count]);
            }

            lGFull = lGFullNew;
            rGFull = rGFullNew;
            rT = rTNew;
            lT = lTNew;
            lG = lGNew;
            rG = rGNew;
        }













        // Curbs


        curbsGenerator = new CurbsGenerator(lT.ToArray(), rT.ToArray(), grassPlanesNewStartIndex);
        
        curbsGenerator.CalculateCurbs(discreteTrack);







        /*lGFull.Add(lGFull[0]);
        rGFull.Add(rGFull[0]);
        lG.Add(lG[0]);
        rG.Add(rG[0]);
        lT.Add(lT[0]);
        rT.Add(rT[0]);*/




        // Generating the grass zones on the sides

        List<GeneratedGrassPlane> grassplanes = new List<GeneratedGrassPlane>();


        GeneratedPlaneElement[] leftGrassPlanes = generateGrassplanes(lGFull, lG, lT, 0, 0, 0);
        GeneratedPlaneElement[] rightGrassPlanes = generateGrassplanes(rGFull, rG, rT, 0, 0, 0);
        GeneratedPlaneElement[] combinedPlanes = new GeneratedPlaneElement[leftGrassPlanes.Length + rightGrassPlanes.Length];
        for (int i = 0; i < leftGrassPlanes.Length; i++)
        {
            combinedPlanes[i] = leftGrassPlanes[i];
        }
        for (int i = 0; i < rightGrassPlanes.Length; i++)
        {
            combinedPlanes[i + leftGrassPlanes.Length] = rightGrassPlanes[i];
        }






        // Track vertices

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        float rightUvCounter = 0f;
        float leftUvCounter = 0f;

        vertices.Add(new Vector3(lT[0].x, 0f, lT[0].y));
        uvs.Add(new Vector2(0f, leftUvCounter));
        vertices.Add(new Vector3(rT[0].x, 0f, rT[0].y));
        uvs.Add(new Vector2(1f, rightUvCounter));
        for (int i = 1; i < lT.Count; i++)
        {
            vertices.Add(new Vector3(lT[i].x, 0f, lT[i].y));
            leftUvCounter += (Vector3.Distance(vertices[vertices.Count - 3], vertices[vertices.Count - 1])) * (1f / 16f);
            uvs.Add(new Vector2(0f, leftUvCounter));
            vertices.Add(new Vector3(rT[i].x, 0f, rT[i].y));
            rightUvCounter += (Vector3.Distance(vertices[vertices.Count - 3], vertices[vertices.Count - 1])) * (1f / 16f);
            uvs.Add(new Vector2(1f, rightUvCounter));

            triangles.Add(vertices.Count - 4);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 3);
            triangles.Add(vertices.Count - 2);
            triangles.Add(vertices.Count - 1);
            triangles.Add(vertices.Count - 3);
        }


        vertices.Add(new Vector3(lT[0].x, 0f, lT[0].y));
        leftUvCounter += (Vector3.Distance(vertices[vertices.Count - 3], vertices[vertices.Count - 1])) * (1f / 16f);
        uvs.Add(new Vector2(0f, leftUvCounter));
        vertices.Add(new Vector3(rT[0].x, 0f, rT[0].y));
        rightUvCounter += (Vector3.Distance(vertices[vertices.Count - 3], vertices[vertices.Count - 1])) * (1f / 16f);
        uvs.Add(new Vector2(1f, rightUvCounter));

        triangles.Add(vertices.Count - 4);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 3);
        triangles.Add(vertices.Count - 2);
        triangles.Add(vertices.Count - 1);
        triangles.Add(vertices.Count - 3);






        RenderVerticesTrack = vertices.ToArray();
        RenderTrianglesTrack = triangles.ToArray();
        RenderUVsTrack = uvs.ToArray();


        // Grass vertices

        List<GeneratedGrassPlane> leftGrass = new List<GeneratedGrassPlane>();

        for (int i = 0; i < leftGrassPlanes.Length; i++)
        {
            GeneratedGrassPlane plane = new GeneratedGrassPlane();

            List<Vector3> verticesOutside = new List<Vector3>();
            List<Vector3> directionsOutside = new List<Vector3>();
            List<Vector3> verticesGrassLeft = new List<Vector3>();
            List<int> trianglesGrassLeft = new List<int>();
            List<Vector2> uvsGrassLeft = new List<Vector2>();

            if (leftGrassPlanes[i].GetType() == typeof(GeneratedGrassPlane2D))
            {
                GeneratedGrassPlane2D ggp2d = (GeneratedGrassPlane2D)leftGrassPlanes[i];

                verticesOutside.Add(new Vector3(ggp2d.vertices[0].x, 0f, ggp2d.vertices[0].y));

                verticesGrassLeft.Add(new Vector3(ggp2d.vertices[0].x, 0f, ggp2d.vertices[0].y));
                uvsGrassLeft.Add(ggp2d.vertices[0]);
                verticesGrassLeft.Add(new Vector3(lT[ggp2d.trackStartIndex + 0].x, 0f, lT[ggp2d.trackStartIndex + 0].y));
                verticesGrassLeft[verticesGrassLeft.Count - 1] = verticesGrassLeft[verticesGrassLeft.Count - 2] + ((verticesGrassLeft[verticesGrassLeft.Count - 1] - verticesGrassLeft[verticesGrassLeft.Count - 2]).normalized * (Vector3.Distance(verticesGrassLeft[verticesGrassLeft.Count - 1], verticesGrassLeft[verticesGrassLeft.Count - 2]) - curbsGenerator.LeftWidths[ggp2d.trackStartIndex + 0]));
                uvsGrassLeft.Add(lT[ggp2d.trackStartIndex + 0]);

                directionsOutside.Add((verticesGrassLeft[verticesGrassLeft.Count - 2] - verticesGrassLeft[verticesGrassLeft.Count - 1]).normalized);

                for (int j = 1; j < ggp2d.vertices.Count; j++)
                {
                    bool locked = false;
                    for (int k = 0; k < ggp2d.jumpIndices.Count; k++)
                    {
                        if (j + ggp2d.trackStartIndex >= ggp2d.jumpIndices[k] && j + ggp2d.trackStartIndex < ggp2d.jumpIndices[k] + ggp2d.jumpWidths[k])
                        {
                            locked = true;
                        }
                    }


                    if (!locked)
                    {
                        verticesOutside.Add(new Vector3(ggp2d.vertices[j].x, 0f, ggp2d.vertices[j].y));

                        verticesGrassLeft.Add(new Vector3(ggp2d.vertices[j].x, 0f, ggp2d.vertices[j].y));
                        uvsGrassLeft.Add(ggp2d.vertices[j]);
                        verticesGrassLeft.Add(new Vector3(lT[ggp2d.trackStartIndex + j].x, 0f, lT[ggp2d.trackStartIndex + j].y));
                        verticesGrassLeft[verticesGrassLeft.Count - 1] = verticesGrassLeft[verticesGrassLeft.Count - 2] + ((verticesGrassLeft[verticesGrassLeft.Count - 1] - verticesGrassLeft[verticesGrassLeft.Count - 2]).normalized * (Vector3.Distance(verticesGrassLeft[verticesGrassLeft.Count - 1], verticesGrassLeft[verticesGrassLeft.Count - 2]) - curbsGenerator.LeftWidths[ggp2d.trackStartIndex + j]));
                        uvsGrassLeft.Add(lT[ggp2d.trackStartIndex + j]);

                        directionsOutside.Add((verticesGrassLeft[verticesGrassLeft.Count - 2] - verticesGrassLeft[verticesGrassLeft.Count - 1]).normalized);

                        trianglesGrassLeft.Add(verticesGrassLeft.Count - 4);
                        trianglesGrassLeft.Add(verticesGrassLeft.Count - 2);
                        trianglesGrassLeft.Add(verticesGrassLeft.Count - 3);
                        trianglesGrassLeft.Add(verticesGrassLeft.Count - 2);
                        trianglesGrassLeft.Add(verticesGrassLeft.Count - 1);
                        trianglesGrassLeft.Add(verticesGrassLeft.Count - 3);
                    }
                }

                verticesOutside.Add(new Vector3(ggp2d.vertices[0].x, 0f, ggp2d.vertices[0].y));
                verticesGrassLeft.Add(new Vector3(ggp2d.vertices[0].x, 0f, ggp2d.vertices[0].y));
                uvsGrassLeft.Add(ggp2d.vertices[0]);
                verticesGrassLeft.Add(new Vector3(lT[ggp2d.trackStartIndex + 0].x, 0f, lT[ggp2d.trackStartIndex + 0].y));
                verticesGrassLeft[verticesGrassLeft.Count - 1] = verticesGrassLeft[verticesGrassLeft.Count - 2] + ((verticesGrassLeft[verticesGrassLeft.Count - 1] - verticesGrassLeft[verticesGrassLeft.Count - 2]).normalized * (Vector3.Distance(verticesGrassLeft[verticesGrassLeft.Count - 1], verticesGrassLeft[verticesGrassLeft.Count - 2]) - curbsGenerator.LeftWidths[ggp2d.trackStartIndex + 0]));
                uvsGrassLeft.Add(lT[ggp2d.trackStartIndex + 0]);
                directionsOutside.Add((verticesGrassLeft[verticesGrassLeft.Count - 2] - verticesGrassLeft[verticesGrassLeft.Count - 1]).normalized);
                trianglesGrassLeft.Add(verticesGrassLeft.Count - 4);
                trianglesGrassLeft.Add(verticesGrassLeft.Count - 2);
                trianglesGrassLeft.Add(verticesGrassLeft.Count - 3);
                trianglesGrassLeft.Add(verticesGrassLeft.Count - 2);
                trianglesGrassLeft.Add(verticesGrassLeft.Count - 1);
                trianglesGrassLeft.Add(verticesGrassLeft.Count - 3);




                plane.vertices = verticesGrassLeft.ToArray();
                plane.triangles = trianglesGrassLeft.ToArray();
                plane.uvs = uvsGrassLeft.ToArray();
                plane.AllowBordercross = true;
                plane.verticesOutside = verticesOutside.ToArray();
                plane.directionsOutside = directionsOutside.ToArray();

                leftGrass.Add(plane);
            }
            else
            {
                GeneratedPlaneBetween gpb = (GeneratedPlaneBetween)leftGrassPlanes[i];

                for (int j = 0; j < gpb.vertices.Count; j++)
                {
                    verticesGrassLeft.Add(new Vector3(gpb.vertices[j].x, 0f, gpb.vertices[j].y));
                    uvsGrassLeft.Add(gpb.vertices[j]);
                }

                plane.vertices = verticesGrassLeft.ToArray();
                plane.triangles = gpb.triangles.ToArray();
                plane.uvs = uvsGrassLeft.ToArray();

                leftGrass.Add(plane);
            }

        }

        for (int i = 0; i < rightGrassPlanes.Length; i++)
        {
            GeneratedGrassPlane plane = new GeneratedGrassPlane();

            List<Vector3> verticesOutside = new List<Vector3>();
            List<Vector3> directionsOutside = new List<Vector3>();
            List<Vector3> verticesGrassLeft = new List<Vector3>();
            List<int> trianglesGrassLeft = new List<int>();
            List<Vector2> uvsGrassLeft = new List<Vector2>();

            if (rightGrassPlanes[i].GetType() == typeof(GeneratedGrassPlane2D))
            {
                GeneratedGrassPlane2D ggp2d = (GeneratedGrassPlane2D)rightGrassPlanes[i];

                verticesOutside.Add(new Vector3(ggp2d.vertices[0].x, 0f, ggp2d.vertices[0].y));

                verticesGrassLeft.Add(new Vector3(ggp2d.vertices[0].x, 0f, ggp2d.vertices[0].y));
                uvsGrassLeft.Add(ggp2d.vertices[0]);
                verticesGrassLeft.Add(new Vector3(rT[ggp2d.trackStartIndex + 0].x, 0f, rT[ggp2d.trackStartIndex + 0].y));
                verticesGrassLeft[verticesGrassLeft.Count - 1] = verticesGrassLeft[verticesGrassLeft.Count - 2] + ((verticesGrassLeft[verticesGrassLeft.Count - 1] - verticesGrassLeft[verticesGrassLeft.Count - 2]).normalized * (Vector3.Distance(verticesGrassLeft[verticesGrassLeft.Count - 1], verticesGrassLeft[verticesGrassLeft.Count - 2]) - curbsGenerator.RightWidths[ggp2d.trackStartIndex + 0]));
                uvsGrassLeft.Add(rT[ggp2d.trackStartIndex + 0]);

                directionsOutside.Add((verticesGrassLeft[verticesGrassLeft.Count - 2] - verticesGrassLeft[verticesGrassLeft.Count - 1]).normalized);

                for (int j = 1; j < ggp2d.vertices.Count; j++)
                {
                    bool locked = false;
                    for (int k = 0; k < ggp2d.jumpIndices.Count; k++)
                    {
                        if (j + ggp2d.trackStartIndex >= ggp2d.jumpIndices[k] && j + ggp2d.trackStartIndex < ggp2d.jumpIndices[k] + ggp2d.jumpWidths[k])
                        {
                            locked = true;
                        }
                    }


                    if (!locked)
                    {
                        verticesOutside.Add(new Vector3(ggp2d.vertices[j].x, 0f, ggp2d.vertices[j].y));

                        verticesGrassLeft.Add(new Vector3(ggp2d.vertices[j].x, 0f, ggp2d.vertices[j].y));
                        uvsGrassLeft.Add(ggp2d.vertices[j]);
                        verticesGrassLeft.Add(new Vector3(rT[ggp2d.trackStartIndex + j].x, 0f, rT[ggp2d.trackStartIndex + j].y));
                        verticesGrassLeft[verticesGrassLeft.Count - 1] = verticesGrassLeft[verticesGrassLeft.Count - 2] + ((verticesGrassLeft[verticesGrassLeft.Count - 1] - verticesGrassLeft[verticesGrassLeft.Count - 2]).normalized * (Vector3.Distance(verticesGrassLeft[verticesGrassLeft.Count - 1], verticesGrassLeft[verticesGrassLeft.Count - 2]) - curbsGenerator.RightWidths[ggp2d.trackStartIndex + j]));
                        uvsGrassLeft.Add(rT[ggp2d.trackStartIndex + j]);

                        directionsOutside.Add((verticesGrassLeft[verticesGrassLeft.Count - 2] - verticesGrassLeft[verticesGrassLeft.Count - 1]).normalized);

                        trianglesGrassLeft.Add(verticesGrassLeft.Count - 4);
                        trianglesGrassLeft.Add(verticesGrassLeft.Count - 3);
                        trianglesGrassLeft.Add(verticesGrassLeft.Count - 2);
                        trianglesGrassLeft.Add(verticesGrassLeft.Count - 2);
                        trianglesGrassLeft.Add(verticesGrassLeft.Count - 3);
                        trianglesGrassLeft.Add(verticesGrassLeft.Count - 1);
                    }
                }
                verticesOutside.Add(new Vector3(ggp2d.vertices[0].x, 0f, ggp2d.vertices[0].y));
                verticesGrassLeft.Add(new Vector3(ggp2d.vertices[0].x, 0f, ggp2d.vertices[0].y));
                uvsGrassLeft.Add(ggp2d.vertices[0]);
                verticesGrassLeft.Add(new Vector3(rT[ggp2d.trackStartIndex + 0].x, 0f, rT[ggp2d.trackStartIndex + 0].y));
                verticesGrassLeft[verticesGrassLeft.Count - 1] = verticesGrassLeft[verticesGrassLeft.Count - 2] + ((verticesGrassLeft[verticesGrassLeft.Count - 1] - verticesGrassLeft[verticesGrassLeft.Count - 2]).normalized * (Vector3.Distance(verticesGrassLeft[verticesGrassLeft.Count - 1], verticesGrassLeft[verticesGrassLeft.Count - 2]) - curbsGenerator.RightWidths[ggp2d.trackStartIndex + 0]));
                uvsGrassLeft.Add(rT[ggp2d.trackStartIndex + 0]);
                directionsOutside.Add((verticesGrassLeft[verticesGrassLeft.Count - 2] - verticesGrassLeft[verticesGrassLeft.Count - 1]).normalized);
                trianglesGrassLeft.Add(verticesGrassLeft.Count - 4);
                trianglesGrassLeft.Add(verticesGrassLeft.Count - 3);
                trianglesGrassLeft.Add(verticesGrassLeft.Count - 2);
                trianglesGrassLeft.Add(verticesGrassLeft.Count - 2);
                trianglesGrassLeft.Add(verticesGrassLeft.Count - 3);
                trianglesGrassLeft.Add(verticesGrassLeft.Count - 1);



                plane.vertices = verticesGrassLeft.ToArray();
                plane.triangles = trianglesGrassLeft.ToArray();
                plane.uvs = uvsGrassLeft.ToArray();
                plane.AllowBordercross = true;
                plane.verticesOutside = verticesOutside.ToArray();
                plane.directionsOutside = directionsOutside.ToArray();

                leftGrass.Add(plane);
            }
            else
            {
                GeneratedPlaneBetween gpb = (GeneratedPlaneBetween)rightGrassPlanes[i];

                for (int j = 0; j < gpb.vertices.Count; j++)
                {
                    verticesGrassLeft.Add(new Vector3(gpb.vertices[j].x, 0f, gpb.vertices[j].y));
                    uvsGrassLeft.Add(gpb.vertices[j]);
                }
                for (int j = 0; j < gpb.triangles.Count; j += 3)
                {
                    trianglesGrassLeft.Add(gpb.triangles[j]);
                    trianglesGrassLeft.Add(gpb.triangles[j + 2]);
                    trianglesGrassLeft.Add(gpb.triangles[j + 1]);
                }

                plane.vertices = verticesGrassLeft.ToArray();
                plane.triangles = trianglesGrassLeft.ToArray();
                plane.uvs = uvsGrassLeft.ToArray();
                plane.AllowBordercross = false;

                leftGrass.Add(plane);
            }

        }

        GrassPlanes = leftGrass.ToArray();
    }

    private GeneratedPlaneElement[] generateGrassplanes(List<Vector2> lGsFull, List<Vector2> lG, List<Vector2> lT, int startIndex, int trackStartIndex, int recDepth)
    {
        List<GeneratedPlaneElement> planes = new List<GeneratedPlaneElement>();

        List<RenderVertexShortcut> lGSCs = calculateShortcuts(lG);

        List<int> jumpIndices = new List<int>();
        List<int> jumpWidths = new List<int>();

        for (int i = 0; i < lGSCs.Count; i++)
        {
            RenderVertexShortcut rvs = lGSCs[i];

            List<Vector2> innerLG = new List<Vector2>();

            for (int j = rvs.indexBefore + 1; j < rvs.indexAfter; j++)
            {
                innerLG.Add(lG[j]);
                lG[j] = rvs.intersectPoint;
            }
            /*for (int j = rvs.indexBefore + 1; j < (rvs.indexAfter < rvs.indexBefore ? lG.Count : rvs.indexAfter); j++)
            {
                innerLG.Add(lG[j]);
                lG[j] = rvs.intersectPoint;
            }
            for (int j = 0; j < (rvs.indexAfter < rvs.indexBefore ? rvs.indexAfter : 0); j++)
            {
                innerLG.Add(lG[j]);
                lG[j] = rvs.intersectPoint;
            }*/

            List<RenderVertexShortcut> innerLGSCs = calculateShortcuts(innerLG);


            bool intersectsSomewhere = false;
            for (int j = rvs.indexBefore + 1; j < rvs.indexAfter; j++)
            {
                Line throughField = new Line(rvs.intersectPoint, lT[(j + startIndex) % lT.Count] - (lT[(j + startIndex) % lT.Count] - rvs.intersectPoint).normalized * 0.4f);

                for (int k = rvs.indexBefore + 1; k < rvs.indexAfter; k++)
                {
                    if (k - 2 != j && k - 1 != j && k != j && k + 1 != j)
                    {
                        Line trackAlong = new Line(lT[(k + startIndex - 1) % lT.Count], lT[(k + startIndex) % lT.Count]);

                        Vector2 isecPt;
                        if (trackAlong.Intersects(throughField, out isecPt))
                        {
                            //Debug.Log("Isec: " + isecPt.ToString());
                            intersectsSomewhere = true;
                            break;
                        }
                    }
                }
            }

            if (innerLGSCs.Count > 0 && intersectsSomewhere)
            {
                List<Vector2> recLG = new List<Vector2>();
                List<Vector2> recLT = new List<Vector2>();
                
                for (int recI = innerLGSCs[0].indexBefore + 1; recI < (innerLGSCs[0].indexAfter + (lGsFull.Count / 2) <= innerLGSCs[0].indexBefore ? innerLGSCs[0].indexAfter + lGsFull.Count : innerLGSCs[0].indexAfter); recI++)
                {
                    recLG.Add(lGsFull[(recI + rvs.indexBefore + 1 + startIndex) % lGsFull.Count]);
                }

                if (recLG.Count >= 2)
                {
                    jumpIndices.Add(rvs.indexBefore + startIndex);
                    jumpWidths.Add((rvs.indexAfter > rvs.indexBefore ? rvs.indexAfter : rvs.indexAfter + lGsFull.Count) - rvs.indexBefore);

                    planes.AddRange(generateGrassplanes(lGsFull, recLG, lT, innerLGSCs[0].indexBefore + 1 + rvs.indexBefore + 1, startIndex, recDepth + 1));

                    Vector2 pointDown = rvs.intersectPoint;
                    Vector2 pointUp = innerLGSCs[0].intersectPoint;
                    Line lineMid = new Line(pointDown, pointUp);

                    GeneratedPlaneBetween gpb = new GeneratedPlaneBetween();
                    List<Vector2> leftVerts = new List<Vector2>();
                    for (int j = rvs.indexAfter; j >= innerLGSCs[0].indexAfter + rvs.indexBefore; j--)
                    {
                        leftVerts.Add(lT[(j + startIndex) % lT.Count]);
                    }
                    List<Vector2> rightVerts = new List<Vector2>();
                    for (int j = rvs.indexBefore - 1; j <= innerLGSCs[0].indexBefore + rvs.indexBefore + 2; j++)
                    {
                        rightVerts.Add(lT[(j + startIndex) % lT.Count]);
                    }

                    int leftEndIndex = innerLGSCs[0].indexAfter + rvs.indexBefore;
                    int rightEndIndex = innerLGSCs[0].indexBefore + rvs.indexBefore + 2;


                    //gpb.vertices.Add(leftVerts[0]);
                    //gpb.vertices.Add(rightVerts[0]);
                    for (int j = 0; j < leftVerts.Count; j++)
                    {
                        gpb.vertices.Add(leftVerts[j]);
                    }
                    for (int j = 0; j < rightVerts.Count; j++)
                    {
                        gpb.vertices.Add(rightVerts[j]);
                    }

                    int lI = 0;
                    int rI = 0;
                    
                    while (lI < leftVerts.Count && rI < rightVerts.Count)
                    {
                        if (lI < leftVerts.Count - 1 && rI < rightVerts.Count - 1)
                        {
                            if (Vector2.Distance(leftVerts[lI + 1], rightVerts[rI]) < Vector2.Distance(leftVerts[lI], rightVerts[rI + 1]))
                            {
                                lI++;
                                if (lI >= leftVerts.Count)
                                {
                                    break;
                                }

                                gpb.triangles.Add(lI);
                                gpb.triangles.Add(rI + leftVerts.Count);
                                gpb.triangles.Add(lI - 1);

                            }
                            else
                            {
                                rI++;
                                if (rI >= rightVerts.Count)
                                {
                                    break;
                                }

                                gpb.triangles.Add(rI + leftVerts.Count);
                                gpb.triangles.Add(rI + leftVerts.Count - 1);
                                gpb.triangles.Add(lI);
                            }
                        }
                        else if (lI == leftVerts.Count - 1)
                        {
                            rI++;
                            if (rI >= rightVerts.Count)
                            {
                                break;
                            }

                            gpb.triangles.Add(rI + leftVerts.Count);
                            gpb.triangles.Add(rI + leftVerts.Count - 1);
                            gpb.triangles.Add(lI);
                        }
                        else if (rI == rightVerts.Count - 1)
                        {
                            lI++;
                            if (lI >= leftVerts.Count)
                            {
                                break;
                            }

                            gpb.triangles.Add(lI);
                            gpb.triangles.Add(rI + leftVerts.Count);
                            gpb.triangles.Add(lI - 1);
                        }
                    }

                    gpb.vertices.Add(lGsFull[(leftEndIndex + startIndex) % lGsFull.Count]);
                    gpb.vertices.Add(lGsFull[(rightEndIndex + startIndex) % lGsFull.Count]);
                    gpb.triangles.Add(gpb.vertices.Count - 1);
                    gpb.triangles.Add(gpb.vertices.Count - 2);
                    gpb.triangles.Add(rightVerts.Count + leftVerts.Count - 1);
                    gpb.triangles.Add(gpb.vertices.Count - 1);
                    gpb.triangles.Add(rightVerts.Count + leftVerts.Count - 1);
                    gpb.triangles.Add(leftVerts.Count - 1);

                    gpb.triangles.Add(leftVerts.Count - 1);
                    gpb.triangles.Add(gpb.vertices.Count - 2);
                    gpb.triangles.Add(gpb.vertices.Count - 1);

                    gpb.triangles.Add(gpb.vertices.Count - 2);
                    gpb.triangles.Add(rightVerts.Count + leftVerts.Count - 1);
                    gpb.triangles.Add(gpb.vertices.Count - 1);

                    /*for (int j = 1; j < Mathf.Min(leftVerts.Count, rightVerts.Count); j++)
                    {
                        gpb.vertices.Add(leftVerts[j]);
                        gpb.vertices.Add(rightVerts[j]);
                        gpb.triangles.Add(gpb.vertices.Count - 4);
                        gpb.triangles.Add(gpb.vertices.Count - 2);
                        gpb.triangles.Add(gpb.vertices.Count - 3);
                        gpb.triangles.Add(gpb.vertices.Count - 2);
                        gpb.triangles.Add(gpb.vertices.Count - 1);
                        gpb.triangles.Add(gpb.vertices.Count - 3);
                    }

                    if (leftVerts.Count > rightVerts.Count)
                    {
                        for (int j = rightVerts.Count; j < leftVerts.Count; j++)
                        {
                            gpb.vertices.Add(leftVerts[j]);
                            gpb.triangles.Add(gpb.vertices.Count - 3);
                            gpb.triangles.Add(gpb.vertices.Count - 1);
                            gpb.triangles.Add(gpb.vertices.Count - 2);
                        }
                    }
                    else  if (leftVerts.Count < rightVerts.Count)
                    {
                        for (int j = leftVerts.Count; j < rightVerts.Count; j++)
                        {
                            gpb.vertices.Add(rightVerts[j]);
                            gpb.triangles.Add(gpb.vertices.Count - 3);
                            gpb.triangles.Add(gpb.vertices.Count - 1);
                            gpb.triangles.Add(gpb.vertices.Count - 2);
                        }
                    }*/

                    planes.Add(gpb);
                }

                // TODO recursive
            }
        }

        GeneratedGrassPlane2D ggp2d = new GeneratedGrassPlane2D();
        ggp2d.trackStartIndex = startIndex + trackStartIndex;
        ggp2d.vertices = lG;
        ggp2d.jumpIndices.AddRange(jumpIndices);
        ggp2d.jumpWidths.AddRange(jumpWidths);
        planes.Add(ggp2d);

        return planes.ToArray();
    }

    public static List<RenderVertexShortcut> calculateShortcuts(Vector2[] borderline)
    {
        return calculateShortcuts(borderline, false);
    }

    public static List<RenderVertexShortcut> calculateShortcuts(List<Vector2> borderline)
    {
        return calculateShortcuts(borderline.ToArray(), false);
    }

    public static List<RenderVertexShortcut> calculateShortcuts(Vector2[] borderline, bool noCircles)
    {
        List<RenderVertexShortcut> lSCs = new List<RenderVertexShortcut>();

        int startIndexLeft = -1;
        for (int i = 0; i < borderline.Length; i++)
        {
            int i2 = (i + 1) % borderline.Length;
            Line lineHere = new Line(borderline[i], borderline[i2]);

            bool intersectsSomewhere = false;

            for (int j = 1; j < borderline.Length - 2; j++)
            {
                int j1 = (j + i2) % borderline.Length;
                int j2 = (j1 + 1) % borderline.Length;
                Line lineTest = new Line(borderline[j1], borderline[j2]);

                Vector2 isecPoint;
                if (lineHere.Intersects(lineTest, out isecPoint))
                {
                    intersectsSomewhere = true;
                    break;
                }
            }

            if (!intersectsSomewhere)
            {
                startIndexLeft = i;
                break;
            }
        }

        for (int i = 1; i < borderline.Length; i++)
        {
            int i1 = (i + startIndexLeft) % borderline.Length;
            int i2 = (i1 + 1) % borderline.Length;

            Line lineHere = new Line(borderline[i1], borderline[i2]);

            for (int j = 1; j < borderline.Length - 2; j++)      //Perhaps -1 is working too
            {
                int j1 = (i1 - j) < 0 ? (i1 - j) + borderline.Length : (i1 - j);
                int j2 = (j1 - 1) < 0 ? (j1 - 1) + borderline.Length : (j1 - 1);


                if (noCircles == false || Mathf.Abs(j1 - j2) <= 1)
                {
                    if (j1 < 0 || j2 < 0 || j1 >= borderline.Length || j2 >= borderline.Length)
                    {
                        Debug.LogError("what: " + j1 + "," + j2 + " at " + borderline.Length);
                    }
                    else
                    {
                        Line lineOther = new Line(borderline[j1], borderline[j2]);

                        Vector2 isecPoint;
                        if (lineHere.Intersects(lineOther, out isecPoint))
                        {
                            RenderVertexShortcut rvs = new RenderVertexShortcut();
                            rvs.indexAfter = j1;
                            rvs.indexBefore = i1;
                            rvs.intersectPoint = isecPoint;
                            lSCs.Add(rvs);

                            int jumpedDistance = j1 - i1;
                            if (jumpedDistance < 0)
                            {
                                jumpedDistance = (j1 + borderline.Length) - i1;
                            }

                            i += (jumpedDistance - 1);
                        }
                    }
                }
            }
        }

        return lSCs;
    }

    public Circle[] Circles
    {
        get;set;
    }

    public void GenerateBorder()
    {
        borderGenerator = new BorderGenerator(terrainModifier, elements, discreteTrack.startFinishLineIndex);
    }

    public void SetTerrainModifier(TerrainModifier terrainMod)
    {
        terrainModifier = terrainMod;
    }

    public float[,] TerrainModifierCPs
    {
        get
        {
            if (terrainModifier != null)
            {
                return terrainModifier.Checkpoints;
            }

            return null;
        }
    }

    public float GetHeight(float x, float z)
    {
        if (terrainModifier == null)
        {
            return 0f;
        }
        else
        {
            return terrainModifier.GetHeight(x, z);
        }
    }

    public Vector3 GetHeightVector(int elementIndex, float s, float leftToRight)
    {
        return new Vector3(0f, terrainModifier.GetHeight(elementIndex, s, leftToRight), 0f);
    }

    public float GetHeight(Vector3 position)
    {
        return GetHeight(position.x, position.z);
    }

    public Vector3 GetHeightVector(Vector3 position)
    {
        return new Vector3(0f, GetHeight(position.x, position.z), 0f);
    }

    public float GetTensorHeight(Vector3 position)
    {
        return terrainModifier.GetTensorHeight(position.x, position.z);
    }

    public float GetTensorHeightBySector(Vector3 position, int sector)
    {
        return terrainModifier.GetTensorHeightSector(position.x, position.z, sector);
    }

    public GeneratedCurb[] Curbs
    {
        get
        {
            return curbsGenerator.Curbs;
        }
    }

    public HistoProfile CurvatureProfile
    {
        get
        {
            return curvatureProfile;
        }
    }

    public HistoProfile CurvatureIdeallineProfile
    {
        get
        {
            return curvatureIdeallineProfile;
        }
    }

    public HistoProfile SpeedProfile
    {
        get
        {
            return speedProfile;
        }
    }

    public DiscreteTrack AnalyzedTrack
    {
        get
        {
            return discreteTrack;
        }
    }

    public GeneratedElement[] Elements
    {
        get
        {
            return elements.ToArray();
        }
    }

    public void ReplaceElement(int index, GeneratedElement element)
    {
        if (index >= 0 && index < elements.Count)
        {
            elements[index] = element;
        }
    }

    public TerrainModifier TerrainModifier
    {
        get
        {
            return terrainModifier;
        }
    }

    public BorderGenerator BorderGenerator
    {
        get
        {
            return borderGenerator;
        }
    }

    public float TrackLength
    {
        get
        {
            float length = 0f;
            for (int i = 0; i < Elements.Length; i++)
            {
                length += Elements[i].Length;
            }
            return length;
        }
    }

    public GeneratedTrack Copy()
    {
        GeneratedTrack copy = new GeneratedTrack();
        for (int i = 0; i < Elements.Length; i++)
        {
            copy.AddElement(Elements[i].Copy());
        }

        copy.SetTerrainModifier(terrainModifier);

        copy.discreteTrack = discreteTrack.Copy(copy);

        copy.curvatureIdeallineProfile = curvatureIdeallineProfile;
        copy.curvatureProfile = curvatureProfile;
        copy.speedProfile = speedProfile;

        return copy;
    }

    public bool Faulty { get; set; }
}



public class RenderVertexShortcut
{
    public int indexBefore;
    public int indexAfter;
    public Vector2 intersectPoint;
}

public class GeneratedGrassPlane
{
    public Vector3[] verticesOutside;
    public Vector3[] directionsOutside;
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    public bool AllowBordercross;
}

public class GeneratedPlaneElement
{

}

public class GeneratedGrassPlane2D : GeneratedPlaneElement
{
    public List<Vector2> vertices;
    public int trackStartIndex;
    public List<int> jumpIndices;
    public List<int> jumpWidths;

    public GeneratedGrassPlane2D()
    {
        jumpIndices = new List<int>();
        jumpWidths = new List<int>();
    }
}

public class GeneratedPlaneBetween : GeneratedPlaneElement
{
    public List<Vector2> vertices;
    public List<int> triangles;

    public GeneratedPlaneBetween()
    {
        vertices = new List<Vector2>();
        triangles = new List<int>();
    }
}