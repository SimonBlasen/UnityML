using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ProceduralTerrainGenerator : MonoBehaviour
{
    [SerializeField]
    private Terrain terrain;
    [SerializeField]
    private long seed = 0;
    [SerializeField]
    private int xStart = 0;
    [SerializeField]
    private int yStart = 0;
    [SerializeField]
    private int arrayLength = 10;
    [SerializeField]
    private float noiseAmplitude = 1f;
    [SerializeField]
    private float noisePeriod = 0.3f;
    [SerializeField]
    private bool generate = false;
    [SerializeField]
    private bool flatten = false;

    private PerlinNoise perlinNoise;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (generate)
        {
            perlinNoise = new PerlinNoise(seed);

            float[,] heights = new float[arrayLength, arrayLength];

            for (int x = 0; x < heights.GetLength(0); x++)
            {
                for (int y = 0; y < heights.GetLength(1); y++)
                {
                    heights[x, y] = (perlinNoise.noise2(x * noisePeriod, y * noisePeriod) + 1f) * 0.5f * noiseAmplitude;
                }
            }

            terrain.terrainData.SetHeights(xStart, yStart, heights);

            generate = false;
        }

        if (flatten)
        {
            float[,] heights = new float[arrayLength, arrayLength];

            for (int x = 0; x < heights.GetLength(0); x++)
            {
                for (int y = 0; y < heights.GetLength(1); y++)
                {
                    heights[x, y] = 0f;
                }
            }

            terrain.terrainData.SetHeights(xStart, yStart, heights);

            flatten = false;
        }
    }

    private float[,] savedHeights = null;

    public void AdjustTerrainToTrack(Vector3[] trackVertices)
    {
        Debug.Log("Adjusting Terrain");
        if (savedHeights == null)
        {
            savedHeights = terrain.terrainData.GetHeights(xStart, yStart, arrayLength, arrayLength);
        }

        float[,] newHeights = new float[arrayLength, arrayLength];
        for (int x = 0; x < arrayLength; x++)
        {
            for (int z = 0; z < arrayLength; z++)
            {
                newHeights[z, x] = savedHeights[z, x];
            }
        }

        for (int i = 0; i < trackVertices.Length; i += 2)
        {
            int indexNext = (i + 2) % trackVertices.Length;

            Vector2 min = new Vector2(trackVertices[i].x, trackVertices[i].z);
            Vector2 max = new Vector2(trackVertices[i].x, trackVertices[i].z);
            if (trackVertices[i + 1].x < min.x)
            {
                min.x = trackVertices[i + 1].x;
            }
            if (trackVertices[indexNext].x < min.x)
            {
                min.x = trackVertices[indexNext].x;
            }
            if (trackVertices[indexNext + 1].x < min.x)
            {
                min.x = trackVertices[indexNext + 1].x;
            }

            if (trackVertices[i + 1].z < min.y)
            {
                min.y = trackVertices[i + 1].z;
            }
            if (trackVertices[indexNext].z < min.y)
            {
                min.y = trackVertices[indexNext].z;
            }
            if (trackVertices[indexNext + 1].z < min.y)
            {
                min.y = trackVertices[indexNext + 1].z;
            }



            if (trackVertices[i + 1].x > max.x)
            {
                max.x = trackVertices[i + 1].x;
            }
            if (trackVertices[indexNext].x > max.x)
            {
                max.x = trackVertices[indexNext].x;
            }
            if (trackVertices[indexNext + 1].x > max.x)
            {
                max.x = trackVertices[indexNext + 1].x;
            }

            if (trackVertices[i + 1].z > max.y)
            {
                max.y = trackVertices[i + 1].z;
            }
            if (trackVertices[indexNext].z > max.y)
            {
                max.y = trackVertices[indexNext].z;
            }
            if (trackVertices[indexNext + 1].z > max.y)
            {
                max.y = trackVertices[indexNext + 1].z;
            }

            float transXMin = min.x - terrain.transform.position.x - 5f;
            transXMin = transXMin / (3000f / terrain.terrainData.heightmapResolution);

            int aabbXMin = (int)(transXMin + 1f);


            float transXMax = max.x - terrain.transform.position.x - 5f;
            transXMax = transXMax / (3000f / terrain.terrainData.heightmapResolution);

            int aabbXMax = (int)(transXMax + 0f);




            float transZMin = min.y - terrain.transform.position.z - 5f;
            transZMin = transZMin / (3000f / terrain.terrainData.heightmapResolution);

            int aabbZMin = (int)(transZMin + 1f);


            float transZMax = max.y - terrain.transform.position.z - 5f;
            transZMax = transZMax / (3000f / terrain.terrainData.heightmapResolution);

            int aabbZMax = (int)(transZMax + 0f);


            for (int x = aabbXMin; x <= aabbXMax; x++)
            {
                for (int z = aabbZMin; z <= aabbZMax; z++)
                {
                    newHeights[z, x] = savedHeights[z, x] - 0.006f;
                }
            }

        }


        terrain.terrainData.SetHeights(xStart, yStart, newHeights);
    }

    public void AdjustTerrainToTrack()
    {
        Debug.Log("Adjusting Terrain");
        if (savedHeights == null)
        {
            savedHeights = terrain.terrainData.GetHeights(xStart, yStart, arrayLength, arrayLength);
        }

        float[,] newHeights = new float[arrayLength, arrayLength];
        for (int x = 0; x < arrayLength; x++)
        {
            for (int z = 0; z < arrayLength; z++)
            {
                newHeights[z, x] = savedHeights[z, x];
                Vector3 posSky = new Vector3(x * (3000f / terrain.terrainData.heightmapResolution) + terrain.transform.position.x + 5f, 1000f, z * (3000f / terrain.terrainData.heightmapResolution) + terrain.transform.position.z + 5f);
                RaycastHit[] hits = Physics.RaycastAll(new Ray(posSky, new Vector3(0f, -1f, 0f)));
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.tag == "RenderedTrack")
                    {
                        newHeights[z, x] = savedHeights[z, x] - 0.006f;
                        break;
                    }
                }
            }
        }

        terrain.terrainData.SetHeights(xStart, yStart, newHeights);
    }

    public void ResetTerrainToSaved()
    {
        if (savedHeights != null)
        {
            terrain.terrainData.SetHeights(xStart, yStart, savedHeights);
        }
    }
}
