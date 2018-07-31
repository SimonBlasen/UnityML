using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationRenderer : MonoBehaviour
{
    public bool generate = false;
    public bool generateRandom = false;

    [Header("Info")]
    [SerializeField]
    private float trackLength = 0f;

    [Header("References")]
    [SerializeField]
    public Terrain terrain;
    [SerializeField]
    private CompleteRenderer completeRenderer;
    [SerializeField]
    private CirclesGeneration circlesGeneration;

    [Header("Settings")]
    [SerializeField]
    private float maxTurnDegreeResolution = 90f / 8f;
    [SerializeField]
    public float width = 13f;
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
    private float param_AngleOffset = 4f;
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
    [SerializeField]
    private float param_goalLength = 4000f;
    //[SerializeField]
    //private bool param_Circles_UseCustom = false;
    //[SerializeField]
    //private Vector2[] param_Circles;
    //[SerializeField]
    //private float[] param_Circles_Radius;


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

    [Header("Supplyers")]
    [SerializeField]
    public GeneratedTrackSupplyer[] supplyers;
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
    public GeneratedTrack generatedTrack = null;

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
        completeRenderer.DestryMesh();

        DiscreteInt2Generator.param_AnglePeriod = param_AnglePeriod;
        DiscreteInt2Generator.param_AngleAmplitude = param_AngleAmplitude;
        DiscreteInt2Generator.param_AngleAmplitude = param_AngleAmplitude;
        DiscreteInt2Generator.param_RadiusPeriod = param_RadiusPeriod;
        DiscreteInt2Generator.param_RadiusAmplitude = param_RadiusAmplitude;
        DiscreteInt2Generator.param_RadiusOffset = param_RadiusOffset;
        DiscreteInt2Generator.param_SegDecMin = param_SegDecMin;
        DiscreteInt2Generator.param_SegDecMax = param_SegDecMax;
        DiscreteInt2Generator.param_SegDecPeriod = param_SegDecPeriod;
        DiscreteInt2Generator.generationMode = generationMode;
        //DiscreteInt2Generator.param_Circles = param_Circles;
        //DiscreteInt2Generator.param_Circles_Radius = param_Circles_Radius;
        //DiscreteInt2Generator.param_Circles_Use = param_Circles_UseCustom;

        if (circlesGeneration.CirclesMidpoints.Length == 0)
        {
            circlesGeneration.DoProc(seed, param_goalLength);
        }

        DiscreteInt2Generator.param_Circles = new Vector2[circlesGeneration.CirclesMidpoints.Length];
        DiscreteInt2Generator.param_Circles_Ins = new Vector2[circlesGeneration.CirclesMidpoints.Length];
        DiscreteInt2Generator.param_Circles_Outs = new Vector2[circlesGeneration.CirclesMidpoints.Length];
        DiscreteInt2Generator.param_Circles_Radius = new float[circlesGeneration.CirclesMidpoints.Length];
        DiscreteInt2Generator.param_Circles_Clockwise = new bool[circlesGeneration.CirclesMidpoints.Length];
        DiscreteInt2Generator.param_BetwLB = circlesGeneration.BetwLBs;
        DiscreteInt2Generator.param_BetwLT = circlesGeneration.BetwLTs;
        DiscreteInt2Generator.param_BetwRB = circlesGeneration.BetwRBs;
        DiscreteInt2Generator.param_BetwRT = circlesGeneration.BetwRTs;

        DiscreteInt2Generator.param_Circles_Use = circlesGeneration.CirclesMidpoints.Length > 0;

        for (int i = 0; i < circlesGeneration.CirclesMidpoints.Length; i++)
        {
            if (circlesGeneration.CirclesMidpoints[i].x != 0f && circlesGeneration.CirclesMidpoints[i].z != 0f)
            {
                DiscreteInt2Generator.param_Circles_Use = true;
            }

            DiscreteInt2Generator.param_Circles[i] = new Vector2(circlesGeneration.CirclesMidpoints[i].x, circlesGeneration.CirclesMidpoints[i].z);
            DiscreteInt2Generator.param_Circles_Ins[i] = new Vector2(circlesGeneration.CirclesIns[i].x, circlesGeneration.CirclesIns[i].z);
            DiscreteInt2Generator.param_Circles_Outs[i] = new Vector2(circlesGeneration.CirclesOuts[i].x, circlesGeneration.CirclesOuts[i].z);
            DiscreteInt2Generator.param_Circles_Radius[i] = circlesGeneration.CirclesRadius[i];
            DiscreteInt2Generator.param_Circles_Clockwise[i] = circlesGeneration.CirclesClockwise[i];
        }


         
        if (terrainModifier == null)
        {
            terrainModifier = new TerrainModifier(terrain);
        }

        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();

        if (jobGenerateTrack != null && jobGenerateTrack.IsDone == false)
        {
            jobGenerateTrack.Abort();
            jobGenerateTrack = null;
        }

        if (jobGenerateTrack == null || jobGenerateTrack.IsDone)
        {
            jobGenerateTrack = new JobGenerateTrack();
            if (firstStart)
            {
                firstStart = false;
                jobGenerateTrack.Seed = 58528152;
            }
            else
            {
                jobGenerateTrack.Seed = seed;
            }
            jobGenerateTrack.Width = width;
            jobGenerateTrack.Suppliers = supplyers;
            jobGenerateTrack.Terrain = terrain;
            jobGenerateTrack.TerrainModifier = terrainModifier;


            //jobGenerateTrack.Track = DiscreteInt2Generator.GenerateTrack(10f, width, Seed);

            jobGenerateTrack.Start();
        }
    }

    public void GenerateCircles()
    {
        circlesGeneration.DoProc(seed, param_goalLength);
    }

    public void GenerateCircles(int configurationIndex)
    {
        circlesGeneration.DoProc(configurationIndex, seed, param_goalLength);
    }

    // Update is called once per frame
    void Update()
    {
        if (generate)
        {
            generate = false;
            ActionGenerateToSeed(seed);
        }
        if (generateRandom)
        {
            generateRandom = false;
            ActionGenerateRandom();
        }

        if (jobGenerateTrack != null && jobGenerateTrack.IsDone)
        {
            generatedTrack = jobGenerateTrack.Track;

            //generatedTrack.Analyze();

            //generatedTrack.GenerateMesh();

            //generatedTrack.SetTerrainModifier(terrainModifier);
            //generatedTrack.GenerateBorder();

            if (tmcps == null)
            {

            }
            else
            {
                float[,] predef = new float[tmcps.GetLength(0), tmcps.GetLength(1)];
                for (int x = 0; x < predef.GetLength(0); x++)
                {
                    for (int y = 0; y < predef.GetLength(1); y++)
                    {
                        predef[x, y] = tmcps[x, y].transform.position.y;
                    }
                }
                //generatedTrack.ModifyTerrain(terrain, predef);
            }


            if (generatedTrack.TerrainModifierCPs != null && tmcps == null)
            {
                /*
                tmcps = new GameObject[generatedTrack.TerrainModifierCPs.GetLength(0), generatedTrack.TerrainModifierCPs.GetLength(1)];
                for (int x = 0; x < generatedTrack.TerrainModifierCPs.GetLength(0); x++)
                {
                    for (int y = 0; y < generatedTrack.TerrainModifierCPs.GetLength(1); y++)
                    {
                        GameObject inst = Instantiate(cubeTerrainModifierCP);
                        tmcps[x, y] = inst;
                        tmcps[x, y].transform.position = new Vector3(x * 10f - 200f * 10f * 0.5f, generatedTrack.TerrainModifierCPs[x, y], y * 10f - 200f * 10f * 0.5f);
                    }
                }*/
            }

            jobGenerateTrack = null;

            /*for (int i = 0; i < supplyers.Length; i++)
            {
                supplyers[i].Track = generatedTrack;
                supplyers[i].TrackUpdated();
            }*/

            Render(generatedTrack);
        }

        if (jobList.Count > 0 && jobListIndex < jobList.Count)
        {
            if (jobList[jobListIndex].IsDone)
            {
                if (jobList[jobListIndex].Track.CurvatureIdeallineProfile.HC >= minHcCurvature && jobList[jobListIndex].Track.SpeedProfile.HC >= minHcSpeed)
                {
                    //generatedTrack.SetTerrainModifier(terrainModifier);
                    //generatedTrack.GenerateBorder();
                    hcTracks.Add(jobList[jobListIndex].Track);
                }

                int nextToStart = jobListIndex + 20;
                if (nextToStart < jobList.Count)
                {
                    jobList[nextToStart].Start();
                }
                jobListIndex++;
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

    public void Render(GeneratedTrack track)
    {
        //if (planeRenderer != null)
        //{
        //    planeRenderer.RefreshPlane(track.TerrainModifier);
        //}

        trackLength = track.TrackLength;


        currentRenderedTrack = track;
    }

    public float TrackLength
    {
        get
        {
            return trackLength;
        }
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
