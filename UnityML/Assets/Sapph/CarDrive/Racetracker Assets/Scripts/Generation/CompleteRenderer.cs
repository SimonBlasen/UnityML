using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteRenderer : MonoBehaviour
{
    public bool generate = false;
    public bool generateRandom = false;

    [Header("Info")]
    [SerializeField]
    private float trackLength = 0f;

    [Header("Terrain")]
    [SerializeField]
    private Terrain terrain;
    [SerializeField]
    private GenerationRenderer generationRenderer;
    [SerializeField]
    private ProceduralTerrainGenerator terrainGenerator;
    [SerializeField]
    public GameObject startFinishBow;

    [Header("Settings")]
    [SerializeField]
    private float maxTurnDegreeResolution = 90f / 8f;
    [SerializeField]
    private float width = 13f;
    [SerializeField]
    private long seed = 0;

    [SerializeField]
    private DiscreteGenerationMode generationMode;
    [SerializeField]
    private bool curbsEnabled = false;
    [SerializeField]
    private float param_AnglePeriod = 0.1f;
    [SerializeField]
    private float param_AngleAmplitude = 40f;
    [SerializeField]
    private float param_RadiusPeriod = 0.4f;
    [SerializeField]
    private float param_RadiusAmplitude = 140f;
    [SerializeField]
    private float param_RadiusOffset = 150f;
    [SerializeField]
    private float param_SegDecMin = -2f;
    [SerializeField]
    private float param_SegDecMax = 2f;
    [SerializeField]
    private float param_SegDecPeriod = 1f;


    [Header("Prefabs")]
    [SerializeField]
    private GameObject debugText;
    [SerializeField]
    private GameObject curbPrefab;
    [SerializeField]
    private GameObject wallPrefab;
    [SerializeField]
    private GameObject crossoverRegionPrefab;
    [SerializeField]
    private GameObject cubeTerrainModifierCP;
    [SerializeField]
    private GameObject grassPlanePrefab;
    [SerializeField]
    private GameObject mlCheckpointCubePrefab;

    [Header("Supplyers")]
    [SerializeField]
    private GeneratedTrackSupplyer[] supplyers;
    [SerializeField]
    private PlaneRenderer planeRenderer = null;

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    private List<Vector3> vertices = new List<Vector3>();
    private List<int> triangles = new List<int>();
    private List<Vector2> uvs = new List<Vector2>();

    private List<GameObject> instDebugObjects = new List<GameObject>();
    private List<GameObject> instCurbs = new List<GameObject>();
    private GameObject instWallLeft = null;
    private GameObject instWallRight = null;

    private GeneratedTrack currentRenderedTrack = null;

    private JobGenerateTrack jobGenerateTrack = null;
    private GeneratedTrack generatedTrack = null;

    private int jobListIndex = 0;
    private List<JobGenerateTrack> jobList = new List<JobGenerateTrack>();
    private List<GeneratedTrack> hcTracks = new List<GeneratedTrack>();
    private float minHcCurvature = 0f;
    private float minHcSpeed = 0f;

    private bool firstStart = true;

    private GameObject[,] tmcps = null;

    private TerrainModifier terrainModifier = null;

    // Use this for initialization
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (terrainModifier == null)
        {
            if (generationRenderer.TerrainModifier != null)
            {
                terrainModifier = generationRenderer.TerrainModifier;
            }
        }
    }

    public void ActionGenerateRandom()
    {
        seed = Utils.longRandom();

        Start();
    }

    public void ActionGenerateToSeed(long seed)
    {
        this.seed = seed;

        Start();
    }

    public void ActionGenerateToHCs(long startSeed, float hcCurvature, float hcSpeed)
    {
        minHcCurvature = hcCurvature;
        minHcSpeed = hcSpeed;

        hcTracks.Clear();
        for (int i = 0; i < jobList.Count; i++)
        {
            jobList[i].Abort();
        }
        jobList.Clear();


        jobListIndex = 0;

        for (int i = 0; i < 100; i++)
        {
            JobGenerateTrack job = new JobGenerateTrack();
            job.Seed = startSeed + i;
            job.Width = width;
            job.Terrain = terrain;
            job.TerrainModifier = terrainModifier;
            jobList.Add(job);
        }


        for (int i = 0; i < 20; i++)
        {
            jobList[i].Start();
        }
    }

    public GeneratedTrack[] HCTracks
    {
        get
        {
            return hcTracks.ToArray();
        }
    }

    public long Seed
    {
        get
        {
            return seed;
        }
        set
        {
            seed = value;
        }
    }

    public void DestryMesh()
    {
        vertices.Clear();
        triangles.Clear();
        uvs.Clear();
    }

    public bool tempRem = false;

    private List<GameObject> instMLStuff = new List<GameObject>();

    public void Render(GeneratedTrack track)
    {
        Render(track, false);
    }

    public void Render(GeneratedTrack track, bool mlStuff)
    {
        for (int i = 0; i < instMLStuff.Count; i++)
        {
            Destroy(instMLStuff[i]);
        }

        instMLStuff = new List<GameObject>();

        //if (planeRenderer != null)
        //{
        //    planeRenderer.RefreshPlane(track.TerrainModifier);
        //}

        trackLength = track.TrackLength;

        vertices.Clear();
        triangles.Clear();
        uvs.Clear();

        int debugCounter = 0;
        for (int i = 0; i < instDebugObjects.Count; i++)
        {
            Destroy(instDebugObjects[i]);
        }
        instDebugObjects.Clear();

        for (int i = 0; i < instCurbs.Count; i++)
        {
            Destroy(instCurbs[i]);
        }
        instCurbs.Clear();


        /*if (track.Elements[0].GetType() == typeof(GeneratedStraight))
        {
            GeneratedStraight straight = (GeneratedStraight)track.Elements[0];

            Vector3 toRight = (Quaternion.Euler(0f, (straight.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(straight.WidthStart, 0f, 0f));
            
            vertices.Add(straight.Position - toRight);
            uvs.Add(new Vector2(vertices[vertices.Count - 1].x, vertices[vertices.Count - 1].z));
            vertices.Add(straight.Position + toRight);
            uvs.Add(new Vector2(vertices[vertices.Count - 1].x, vertices[vertices.Count - 1].z));

        }
        else if (track.Elements[0].GetType() == typeof(GeneratedTurn))
        {
            GeneratedTurn turn = (GeneratedTurn)track.Elements[0];

            Vector3 toRight = (Quaternion.Euler(0f, (turn.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(turn.WidthStart, 0f, 0f));
            
            vertices.Add(turn.Position - toRight);
            uvs.Add(new Vector2(vertices[vertices.Count - 1].x, vertices[vertices.Count - 1].z));
            vertices.Add(turn.Position + toRight);
            uvs.Add(new Vector2(vertices[vertices.Count - 1].x, vertices[vertices.Count - 1].z));
        }
        else if (track.Elements[0].GetType() == typeof(GeneratedBezSpline))
        {
            GeneratedBezSpline bezSpline = (GeneratedBezSpline)track.Elements[0];
            
            vertices.Add(bezSpline.RenderVertsLeft[0]);
            uvs.Add(new Vector2(vertices[vertices.Count - 1].x, vertices[vertices.Count - 1].z));
            vertices.Add(bezSpline.RenderVertsRight[0]);
            uvs.Add(new Vector2(vertices[vertices.Count - 1].x, vertices[vertices.Count - 1].z));
        }


        for (int i = 0; i < track.Elements.Length; i++)
        {

            int verticesCountBefore = vertices.Count;

            //if (track.Elements[i].GetType() != typeof(GeneratedStraight))
            //{
                GameObject instDebugText = Instantiate(debugText);
                instDebugText.transform.position = track.Elements[i].Position;
                instDebugText.GetComponent<DebugPoint>().Text = debugCounter.ToString();

                instDebugObjects.Add(instDebugText);

                debugCounter++;
            //}

            if (track.Elements[i].GetType() == typeof(GeneratedStraight))
            {
                GeneratedStraight straight = (GeneratedStraight)track.Elements[i];

                Vector3 toRight = (Quaternion.Euler(0f, (straight.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(straight.WidthEnd, 0f, 0f));
                Vector3 toFront = (Quaternion.Euler(0f, (straight.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(0f, 0f, straight.Length));


                int segmentsAmount = (int)(straight.Length / 4f);

                for (int j = 1; j <= segmentsAmount; j++)
                {
                    float s = ((float)j) / segmentsAmount;

                    
                    vertices.Add(straight.Position + (toFront * s) - toRight);
                    uvs.Add(new Vector2(vertices[vertices.Count - 1].x, vertices[vertices.Count - 1].z));
                    vertices.Add(straight.Position + (toFront * s) + toRight);
                    uvs.Add(new Vector2(vertices[vertices.Count - 1].x, vertices[vertices.Count - 1].z));
                    

                    triangles.Add(vertices.Count - 4);
                    triangles.Add(vertices.Count - 2);
                    triangles.Add(vertices.Count - 3);

                    triangles.Add(vertices.Count - 3);
                    triangles.Add(vertices.Count - 2);
                    triangles.Add(vertices.Count - 1);
                }
            }
            else if (track.Elements[i].GetType() == typeof(GeneratedTurn))
            {
                GeneratedTurn turn = (GeneratedTurn)track.Elements[i];

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
                    
                    vertices.Add(segmentPos + toRightTurned * currentWidth * -1f + turn.Position);
                    uvs.Add(new Vector2(vertices[vertices.Count - 1].x, vertices[vertices.Count - 1].z));
                    vertices.Add(segmentPos + toRightTurned * currentWidth + turn.Position);
                    uvs.Add(new Vector2(vertices[vertices.Count - 1].x, vertices[vertices.Count - 1].z));


                    triangles.Add(vertices.Count - 4);
                    triangles.Add(vertices.Count - 2);
                    triangles.Add(vertices.Count - 3);

                    triangles.Add(vertices.Count - 3);
                    triangles.Add(vertices.Count - 2);
                    triangles.Add(vertices.Count - 1);
                }
            }
            else if (track.Elements[i].GetType() == typeof(GeneratedBezSpline))
            {
                GeneratedBezSpline bezierSpline = (GeneratedBezSpline)track.Elements[i];

                for (int j = 1; j < bezierSpline.RenderVertsLeft.Length; j++)
                {
                    vertices.Add(bezierSpline.RenderVertsLeft[j]);
                    uvs.Add(new Vector2(vertices[vertices.Count - 1].x, vertices[vertices.Count - 1].z));
                    vertices.Add(bezierSpline.RenderVertsRight[j]);
                    uvs.Add(new Vector2(vertices[vertices.Count - 1].x, vertices[vertices.Count - 1].z));


                    triangles.Add(vertices.Count - 4);
                    triangles.Add(vertices.Count - 2);
                    triangles.Add(vertices.Count - 3);
                    
                    triangles.Add(vertices.Count - 3);
                    triangles.Add(vertices.Count - 2);
                    triangles.Add(vertices.Count - 1);
                }
            }

            if (curbsEnabled && track.Elements[i].Curbs.Length > 0)
            {
                for (int j = 0; j < track.Elements[i].Curbs.Length; j++)
                {
                    GameObject instCurb = Instantiate(curbPrefab, transform);
                    instCurb.GetComponent<CurbRenderer>().ApplyCurb(track.Elements[i].Curbs[j], track.Elements[i]);
                    instCurb.transform.localPosition = Vector3.zero;
                    instCurbs.Add(instCurb);
                }
            }



            //List<Vector3> sectorVertices = new List<Vector3>();
            //for (int j = verticesCountBefore; j < vertices.Count; j++)
            //{
            //    sectorVertices.Add(vertices[j]);
            //}

            //track.TerrainModifier.FillSectorsByElements(sectorVertices, i);
        }
        */

        Debug.Log("Generate Mesh: " + Time.time);

        track.GenerateMesh();
        vertices = new List<Vector3>(track.RenderVerticesTrack);
        triangles = new List<int>(track.RenderTrianglesTrack);
        uvs = new List<Vector2>(track.RenderUVsTrack);

        if (mlStuff)
        {
            Vector3[] leftMLPoints = new Vector3[vertices.Count / 2];
            Vector3[] rightMLPoints = new Vector3[vertices.Count / 2];
            for (int i = 0; i < leftMLPoints.Length; i++)
            {
                leftMLPoints[i] = vertices[i * 2 + 0];
                rightMLPoints[i] = vertices[i * 2 + 1];

                GameObject cubeI = Instantiate(mlCheckpointCubePrefab);
                cubeI.transform.position = (leftMLPoints[i] + rightMLPoints[i]) * 0.5f;
                cubeI.transform.right = leftMLPoints[i] - rightMLPoints[i];
                cubeI.transform.localScale = new Vector3((leftMLPoints[i] - rightMLPoints[i]).magnitude, 6f, 0.1f);

                instMLStuff.Add(cubeI);
            }
            GameObject instWallLeftML = Instantiate(wallPrefab);
            instWallLeftML.transform.position = Vector3.zero;
            GameObject instWallRightML = Instantiate(wallPrefab);
            instWallRightML.transform.position = Vector3.zero;

            instWallLeftML.GetComponent<WallRenderer>().Points = leftMLPoints;
            instWallRightML.GetComponent<WallRenderer>().Points = rightMLPoints;

            instWallLeftML.name = "ML_Wall_Left";
            instWallLeftML.name = "ML_Wall_Right";

            instMLStuff.Add(instWallLeftML);
            instMLStuff.Add(instWallRightML);
        }




        //3,751



        int startFinishElementIndex = (int)track.AnalyzedTrack.startFinishLineIndex;
        float sStartFinishElement = track.AnalyzedTrack.startFinishLineIndex - startFinishElementIndex;
        Vector3 posStartFinish = Vector3.zero;
        float rotStartFinish = 0f;
        if (track.Elements[startFinishElementIndex].GetType() == typeof(GeneratedStraight))
        {
            GeneratedStraight straight = (GeneratedStraight) track.Elements[startFinishElementIndex];
            posStartFinish = straight.Position + (straight.EndPosition - straight.Position) * sStartFinishElement;
            rotStartFinish = straight.Direction;
        }
        else
        {
            GeneratedBezSpline spline = (GeneratedBezSpline)track.Elements[startFinishElementIndex];
            int vertSplineIndex = (int)(spline.RenderVertsLeft.Length * sStartFinishElement);
            posStartFinish = spline.RenderVertsLeft[vertSplineIndex] + 0.5f * (spline.RenderVertsRight[vertSplineIndex] - spline.RenderVertsLeft[vertSplineIndex]);
            rotStartFinish = Vector2.Angle(new Vector2((spline.RenderVertsRight[vertSplineIndex] - spline.RenderVertsLeft[vertSplineIndex]).x, (spline.RenderVertsRight[vertSplineIndex] - spline.RenderVertsLeft[vertSplineIndex]).z), new Vector2(0f, 1f));
            if (Vector2.Angle(new Vector2((spline.RenderVertsRight[vertSplineIndex] - spline.RenderVertsLeft[vertSplineIndex]).x, (spline.RenderVertsRight[vertSplineIndex] - spline.RenderVertsLeft[vertSplineIndex]).z), new Vector2(1f, 0f)) > 90f)
            {
                rotStartFinish = 360 - rotStartFinish;
            }

            rotStartFinish = rotStartFinish * Mathf.PI / 180f;
            rotStartFinish -= (Mathf.PI * 0.5f);
        }

        posStartFinish = new Vector3(posStartFinish.x, track.GetTensorHeight(posStartFinish), posStartFinish.z);

        startFinishBow.transform.position = posStartFinish;
        startFinishBow.transform.rotation = Quaternion.Euler(0f, rotStartFinish * 180f / Mathf.PI, 0f);


        //3,924

        

        Debug.Log("Generate Grass planes: " + Time.time);

        for (int j = 0; j < track.GrassPlanes.Length; j++)
        {
            Vector3[] grassLeftVerts = track.GrassPlanes[j].vertices;
            for (int i = 0; i < grassLeftVerts.Length; i++)
            {
                grassLeftVerts[i] = grassLeftVerts[i] + new Vector3(0f, track.GetTensorHeight(grassLeftVerts[i]), 0f);
            }


            GameObject grassLeft = Instantiate(grassPlanePrefab);
            grassLeft.transform.position = transform.position;
            grassLeft.GetComponent<MeshFilter>().mesh.Clear();
            grassLeft.GetComponent<MeshFilter>().mesh.vertices = grassLeftVerts;
            grassLeft.GetComponent<MeshFilter>().mesh.uv = track.GrassPlanes[j].uvs;
            grassLeft.GetComponent<MeshFilter>().mesh.subMeshCount = 1;
            grassLeft.GetComponent<MeshFilter>().mesh.SetTriangles(track.GrassPlanes[j].triangles, 0);
            grassLeft.GetComponent<MeshFilter>().mesh.RecalculateNormals();
            grassLeft.GetComponent<MeshCollider>().sharedMesh = grassLeft.GetComponent<MeshFilter>().mesh;

            instDebugObjects.Add(grassLeft);


            if (track.GrassPlanes[j].AllowBordercross)
            {
                List<Vector3> borderVerts = new List<Vector3>();
                List<Vector3> borderDirs = new List<Vector3>();
                for (int k = 0; k < track.GrassPlanes[j].verticesOutside.Length; k++)
                {
                    int k0 = (k - 1) < 0 ? track.GrassPlanes[j].verticesOutside.Length - 1 : (k - 1);
                    int k2 = (k + 1) % track.GrassPlanes[j].verticesOutside.Length;

                    if (Vector3.Distance(track.GrassPlanes[j].verticesOutside[k], track.GrassPlanes[j].verticesOutside[k2]) >= 0.01f)
                    {
                        borderVerts.Add(track.GrassPlanes[j].verticesOutside[k] + new Vector3(0f, track.GetTensorHeight(track.GrassPlanes[j].verticesOutside[k]), 0f));
                        borderDirs.Add(track.GrassPlanes[j].directionsOutside[k]);
                    }
                }



                GameObject instLeftCrossover = Instantiate(crossoverRegionPrefab, transform);
                instLeftCrossover.transform.localPosition = Vector3.zero;
                instLeftCrossover.GetComponent<CrossoverRegion>().GeneratedTrack = track;
                instLeftCrossover.GetComponent<CrossoverRegion>().Render(borderVerts.ToArray(), borderDirs.ToArray(), true);

                instDebugObjects.Add(instLeftCrossover);
            }
        }

        if (track.Circles != null)
        {
            for (int i = 0; i < track.Circles.Length; i++)
            {
                GameObject instDebugCircle = Instantiate(debugText);
                instDebugCircle.transform.position = new Vector3(track.Circles[i].Midpoint.x, 0f, track.Circles[i].Midpoint.y);

                instDebugObjects.Add(instDebugCircle);
            }
        }


        //5,544

        


        for (int i = 0; i < vertices.Count; i++)
        {
            //int currentElement = elementsVertices[i];
            vertices[i] = vertices[i] + new Vector3(0f, track.GetTensorHeight(vertices[i]), 0f);
        }




        //5,560

        


        //
        // Cross over region
        //


        Vector3[] leftVertices = new Vector3[vertices.Count / 2];
        Vector3[] rightVertices = new Vector3[vertices.Count / 2];
        Vector3[] leftDirections = new Vector3[vertices.Count / 2];
        Vector3[] rightDirections = new Vector3[vertices.Count / 2];

        for (int i = 0; i < vertices.Count; i++)
        {
            if (i % 2 == 0)
            {
                leftVertices[i / 2] = vertices[i];
                leftDirections[i / 2] = vertices[i] - vertices[i + 1];
            }
            else
            {
                rightVertices[(i - 1) / 2] = vertices[vertices.Count - i];
                rightDirections[(i - 1) / 2] = vertices[vertices.Count - i] - vertices[vertices.Count - i - 1];
            }
        }
        

        Debug.Log("Generate Curbs: " + Time.time);

        for (int i = 0; i < track.Curbs.Length; i++)
        {
            GameObject instCurb = Instantiate(curbPrefab);
            instCurb.transform.position = new Vector3(0f, 0.5f, 0f);
            instCurb.GetComponent<CurbRenderer>().ApplyCurb(track.Curbs[i], track.TerrainModifier);
            instDebugObjects.Add(instCurb);
        }
        


        if (instWallLeft != null)
        {
            Destroy(instWallLeft);
            instWallLeft = null;
        }
        if (instWallRight != null)
        {
            Destroy(instWallRight);
            instWallRight = null;
        }

        instWallLeft = Instantiate(wallPrefab);
        instWallLeft.transform.position = Vector3.zero;
        instWallRight = Instantiate(wallPrefab);
        instWallRight.transform.position = Vector3.zero;

        instWallLeft.GetComponent<WallRenderer>().Points = track.BorderGenerator.WallLeft;
        instWallRight.GetComponent<WallRenderer>().Points = track.BorderGenerator.WallRight;


        //5,730


        


        Debug.Log("Set track mesh: " + Time.time);



        meshFilter.mesh.Clear();
        meshFilter.mesh.vertices = vertices.ToArray();
        meshFilter.mesh.uv = uvs.ToArray();

        meshFilter.mesh.subMeshCount = 1;

        meshFilter.mesh.SetTriangles(triangles.ToArray(), 0);

        meshFilter.mesh.RecalculateNormals();

        meshCollider.sharedMesh = meshFilter.mesh;


        //5,743


        Debug.Log("Apply grass: " + Time.time);

        terrainModifier.ApplyGrass(vertices.ToArray());

        //61,596

        

        Debug.Log("Adjust terrain to track: " + Time.time);

        terrainGenerator.AdjustTerrainToTrack(vertices.ToArray());
        

        currentRenderedTrack = track;

    

        tempRem = false;

        //Debug.Log("Finished: " + Time.time);
    }

    public void ResetTerrain()
    {
        terrainGenerator.ResetTerrainToSaved();
    }

    public GeneratedTrack CurrentRenderedTrack
    {
        get
        {
            return currentRenderedTrack;
        }
    }

    public TerrainModifier TerrainModifier
    {
        get
        {
            return terrainModifier;
        }
    }
}
