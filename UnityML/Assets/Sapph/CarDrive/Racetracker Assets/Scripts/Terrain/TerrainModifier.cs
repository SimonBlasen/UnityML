using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainModifier
{
    private ClosedSpline<float> heightsLeft;
    private ClosedSpline<float> heightsRight;

    private Terrain terrain;

    private TensorProductPlane tensorProductPlane;

    private List<TensorProductPlane> sectors;

    private float[,] tempGeneratedCPs = null;

    public TerrainModifier(Terrain terrain, List<GeneratedElement> elements)
    {
        this.terrain = terrain;

        float[] cpsLeft = new float[elements.Count];
        float[] cpsRight = new float[elements.Count];
        
        for (int i = 0; i < elements.Count; i++)
        {
            Vector3 toRight = (Quaternion.Euler(0f, (elements[i].Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(elements[i].WidthEnd, 0f, 0f));

            Vector3 samplePosLeft = elements[i].Position - toRight;
            Vector3 samplePosRight = elements[i].Position + toRight;

            cpsLeft[i] = terrain.SampleHeight(samplePosLeft);
            cpsRight[i] = terrain.SampleHeight(samplePosRight);
        }

        heightsLeft = new ClosedSpline<float>(cpsLeft);
        heightsRight = new ClosedSpline<float>(cpsRight);

        int cpsAmount = 2000;
        float scale = 10f;

        float[,] cps = new float[cpsAmount + 1, cpsAmount + 1];

        for (int x = cpsAmount / -2; x <= cpsAmount / 2; x++)
        {
            for (int y = cpsAmount / -2; y <= cpsAmount / 2; y++)
            {
                cps[x + cpsAmount / 2, y + cpsAmount / 2] = terrain.SampleHeight(new Vector3(x * scale, 0f, y * scale));
            }
        }

        tempGeneratedCPs = cps;

        tensorProductPlane = new TensorProductPlane(new Vector3((cpsAmount / -2) * scale, 0f, (cpsAmount / -2) * scale), scale, cps);
    }

    public TerrainModifier(Terrain terrain)
    {
        sectors = new List<TensorProductPlane>();

        this.terrain = terrain;

        int cpsAmount = 2000;
        float scale = 10f;

        float[,] cps = new float[cpsAmount + 1, cpsAmount + 1];

        for (int x = cpsAmount / -2; x <= cpsAmount / 2; x++)
        {
            for (int y = cpsAmount / -2; y <= cpsAmount / 2; y++)
            {
                cps[x + cpsAmount / 2, y + cpsAmount / 2] = terrain.SampleHeight(new Vector3(x * scale, 0f, y * scale));
            }
        }

        tempGeneratedCPs = cps;

        tensorProductPlane = new TensorProductPlane(new Vector3((cpsAmount / -2) * scale, 0f, (cpsAmount / -2) * scale), scale, cps);
    }

    public void ApplyGrass()
    {
        int grassResolution = 3004;

        terrain.terrainData.SetDetailResolution(grassResolution, 16);
        int[,] newMap = new int[grassResolution, grassResolution];

        for (int x = 0; x < grassResolution; x++)
        {
            for (int z = 0; z < grassResolution; z++)
            {
                newMap[z, x] = 1;
                Vector3 posSky = new Vector3(x * (terrain.terrainData.detailWidth / terrain.terrainData.detailResolution) + terrain.transform.position.x + 0f, 1000f, z * (terrain.terrainData.detailHeight / terrain.terrainData.detailResolution) + terrain.transform.position.z + 0f);
                RaycastHit[] hits = Physics.RaycastAll(new Ray(posSky, new Vector3(0f, -1f, 0f)));
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.tag == "RenderedTrack")
                    {
                        newMap[z, x] = 0;
                        break;
                    }
                }
            }
        }

        terrain.detailObjectDistance = 100f;
        terrain.terrainData.SetDetailLayer(0, 0, 0, newMap);
    }

    public void ApplyGrass(Vector3[] trackVertices)
    {
        int grassResolution = 3004;

        terrain.terrainData.SetDetailResolution(grassResolution, 16);
        int[,] newMap = new int[grassResolution, grassResolution];

        for (int x = 0; x < grassResolution; x++)
        {
            for (int z = 0; z < grassResolution; z++)
            {
                newMap[z, x] = 1;
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

            float transXMin = min.x - terrain.transform.position.x;
            transXMin = transXMin / (terrain.terrainData.detailWidth / terrain.terrainData.detailResolution);

            int aabbXMin = (int)transXMin;


            float transXMax = max.x - terrain.transform.position.x;
            transXMax = transXMax / (terrain.terrainData.detailWidth / terrain.terrainData.detailResolution);

            int aabbXMax = (int)(transXMax + 1f);




            float transZMin = min.y - terrain.transform.position.z;
            transZMin = transZMin / (terrain.terrainData.detailWidth / terrain.terrainData.detailResolution);

            int aabbZMin = (int)transZMin;


            float transZMax = max.y - terrain.transform.position.z;
            transZMax = transZMax / (terrain.terrainData.detailWidth / terrain.terrainData.detailResolution);

            int aabbZMax = (int)(transZMax + 1f);


            for (int x = aabbXMin; x <= aabbXMax; x++)
            {
                for (int z = aabbZMin; z <= aabbZMax; z++)
                {
                    newMap[z, x] = 0;
                }
            }



            //Vector3 posSky = new Vector3(x * (terrain.terrainData.detailWidth / terrain.terrainData.detailResolution) + terrain.transform.position.x + 0f, 1000f, z * (terrain.terrainData.detailHeight / terrain.terrainData.detailResolution) + terrain.transform.position.z + 0f);
        }


        terrain.detailObjectDistance = 100f;
        terrain.terrainData.SetDetailLayer(0, 0, 0, newMap);
    }

    private float maxSlope = 100f / 10f;

    public void FillSectorsByElements(List<Vector3> verticesAbs, int sectorIndex, float[,] predefinedHeights)
    {
        int cpsAmount = 2000;
        float scale = 10f;

        float[,] cps = predefinedHeights;
        float[,] trackDistance = new float[cpsAmount + 1, cpsAmount + 1];
        for (int x = cpsAmount / -2; x <= cpsAmount / 2; x++)
        {
            for (int y = cpsAmount / -2; y <= cpsAmount / 2; y++)
            {
                trackDistance[x + cpsAmount / 2, y + cpsAmount / 2] = -1f;
            }
        }

        /*float[,] cps = new float[cpsAmount + 1, cpsAmount + 1];

        for (int x = cpsAmount / -2; x <= cpsAmount / 2; x++)
        {
            for (int y = cpsAmount / -2; y <= cpsAmount / 2; y++)
            {
                cps[x + cpsAmount / 2, y + cpsAmount / 2] = terrain.SampleHeight(new Vector3(x * scale, 0f, y * scale));
            }
        }*/

        Vector3[] vertices = new Vector3[verticesAbs.Count];
        for (int i = 0; i < verticesAbs.Count; i++)
        {
            vertices[i] = verticesAbs[i] - (new Vector3((cpsAmount / -2) * scale, 0f, (cpsAmount / -2) * scale));

            int castX = (int)(vertices[i].x / 10f);
            int castZ = (int)(vertices[i].z / 10f);

            trackDistance[castX -1, castZ -1] = (float)(((int)i / (int)2));
            trackDistance[castX - 1, castZ] = (float)(((int)i / (int)2));
            trackDistance[castX - 1, castZ + 1] = (float)(((int)i / (int)2));
            trackDistance[castX - 1, castZ + 2] = (float)(((int)i / (int)2));
            trackDistance[castX, castZ + 2] = (float)(((int)i / (int)2));
            trackDistance[castX + 1, castZ + 2] = (float)(((int)i / (int)2));
            trackDistance[castX + 2, castZ + 2] = (float)(((int)i / (int)2));
            trackDistance[castX + 2, castZ + 1] = (float)(((int)i / (int)2));
            trackDistance[castX + 2, castZ] = (float)(((int)i / (int)2));
            trackDistance[castX + 2, castZ - 1] = (float)(((int)i / (int)2));
            trackDistance[castX + 1, castZ - 1] = (float)(((int)i / (int)2));
            trackDistance[castX, castZ - 1] = (float)(((int)i / (int)2));
            trackDistance[castX, castZ] = (float)(((int)i / (int)2));
            trackDistance[castX, castZ + 1] = (float)(((int)i / (int)2));
            trackDistance[castX + 1, castZ] = (float)(((int)i / (int)2));
            trackDistance[castX + 1, castZ + 1] = (float)(((int)i / (int)2));


            //vertices[i] = vertices[i] / scale;
        }


        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minZ = float.MaxValue;
        float maxZ = float.MinValue;

        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i].x < minX)
            {
                minX = vertices[i].x;
            }
            if (vertices[i].x > maxX)
            {
                maxX = vertices[i].x;
            }
            if (vertices[i].z < minZ)
            {
                minZ = vertices[i].z;
            }
            if (vertices[i].z > maxZ)
            {
                maxZ = vertices[i].z;
            }
        }

        minX = minX / 10f;
        maxX = maxX / 10f;
        minZ = minZ / 10f;
        maxZ = maxZ / 10f;

        int intMaxX = ((int)maxX) + 1;
        int intMinX = ((int)minX) - 1;
        int intMaxZ = ((int)maxZ) + 1;
        int intMinZ = ((int)minZ) - 1;

        for (int x = intMinX; x <= intMaxX; x++)
        {
            for (int z = intMinZ; z <= intMaxZ; z++)
            {
                recRaiseCps(cps, x, z, scale, trackDistance);
            }
        }


        if (sectorIndex >= sectors.Count)
        {
            sectors.Add(new TensorProductPlane(new Vector3((cpsAmount / -2) * scale, 0f, (cpsAmount / -2) * scale), scale, cps));
        }
        else
        {
            sectors[sectorIndex] = new TensorProductPlane(new Vector3((cpsAmount / -2) * scale, 0f, (cpsAmount / -2) * scale), scale, cps);
        }





    }

    public void FillSectorsByElements(List<Vector3> verticesAbs, int sectorIndex)
    {
        int cpsAmount = 2000;
        float scale = 10f;
        
        float[,] cps = new float[cpsAmount + 1, cpsAmount + 1];

        for (int x = cpsAmount / -2; x <= cpsAmount / 2; x++)
        {
            for (int y = cpsAmount / -2; y <= cpsAmount / 2; y++)
            {
                cps[x + cpsAmount / 2, y + cpsAmount / 2] = terrain.SampleHeight(new Vector3(x * scale, 0f, y * scale));
            }
        }


        FillSectorsByElements(verticesAbs, sectorIndex, cps);

        return;
        /*
        Vector3[] vertices = new Vector3[verticesAbs.Count];
        for (int i = 0; i < verticesAbs.Count; i++)
        {
            vertices[i] = verticesAbs[i] - (new Vector3((cpsAmount / -2) * scale, 0f, (cpsAmount / -2) * scale));
        }


        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minZ = float.MaxValue;
        float maxZ = float.MinValue;

        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i].x < minX)
            {
                minX = vertices[i].x;
            }
            if (vertices[i].x > maxX)
            {
                maxX = vertices[i].x;
            }
            if (vertices[i].z < minZ)
            {
                minZ = vertices[i].z;
            }
            if (vertices[i].z > maxZ)
            {
                maxZ = vertices[i].z;
            }
        }

        minX = minX / 10f;
        maxX = maxX / 10f;
        minZ = minZ / 10f;
        maxZ = maxZ / 10f;

        int intMaxX = ((int)maxX) + 1;
        int intMinX = ((int)minX) - 1;
        int intMaxZ = ((int)maxZ) + 1;
        int intMinZ = ((int)minZ) - 1;

        for (int x = intMinX; x <= intMaxX; x++)
        {
            for (int z = intMinZ; z <= intMaxZ; z++)
            {
                recRaiseCps(cps, x, z, scale);
            }
        }


        sectors.Add(new TensorProductPlane(new Vector3((cpsAmount / -2) * scale, 0f, (cpsAmount / -2) * scale), scale, cps));

    */
    

    }

    private void recRaiseCps(float[,] cps, int x, int z, float scale, float[,] trackDistance)
    {
        if (trackDistance[x, z] >= 0f && x - 1 >= 0 && z - 1 >= 0 && x + 1 < cps.GetLength(0) && z + 1 < cps.GetLength(1))
        {
            if (((cps[x, z] - cps[x - 1, z]) / scale) > maxSlope && Mathf.Abs(trackDistance[x, z] - trackDistance[x - 1, z]) < 10f)
            {
                cps[x - 1, z] = cps[x, z] - (maxSlope * scale);
                recRaiseCps(cps, x - 1, z, scale, trackDistance);
            }
            if (((cps[x, z] - cps[x + 1, z]) / scale) > maxSlope && Mathf.Abs(trackDistance[x, z] - trackDistance[x + 1, z]) < 10f)
            {
                cps[x + 1, z] = cps[x, z] - (maxSlope * scale);
                recRaiseCps(cps, x + 1, z, scale, trackDistance);
            }
            if (((cps[x, z] - cps[x, z - 1]) / scale) > maxSlope && Mathf.Abs(trackDistance[x, z] - trackDistance[x, z - 1]) < 10f)
            {
                cps[x, z - 1] = cps[x, z] - (maxSlope * scale);
                recRaiseCps(cps, x, z - 1, scale, trackDistance);
            }
            if (((cps[x, z] - cps[x, z + 1]) / scale) > maxSlope && Mathf.Abs(trackDistance[x, z] - trackDistance[x, z + 1]) < 10f)
            {
                cps[x, z + 1] = cps[x, z] - (maxSlope * scale);
                recRaiseCps(cps, x, z + 1, scale, trackDistance);
            }
        }
    }

    public void ConnectSectors(int sector1, int sector2, Vector3 point)
    {
        int cpsAmount = 2000;
        float scale = 10f;
        float[,] sec1 = sectors[sector1].Controlpoints;
        float[,] sec2 = sectors[sector2].Controlpoints;

        Vector3 transPos = point - (new Vector3((cpsAmount / -2) * scale, 0f, (cpsAmount / -2) * scale));
        transPos = transPos / scale;

        int minX = ((int)(transPos.x)) - 1;
        int maxX = ((int)(transPos.x)) + 2;
        int minZ = ((int)(transPos.z)) - 1;
        int maxZ = ((int)(transPos.z)) + 2;

        bool raised1 = false;
        bool raised2 = false;

        for (int x = minX; x <= maxX; x++)
        {
            for (int z = minZ; z < maxZ; z++)
            {
                if (sec1[x, z] - sec2[x, z] > 0.05f)
                {
                    sec2[x, z] = sec1[x, z];
                    raised2 = true;
                }
                else if (sec2[x, z] - sec1[x, z] > 0.05f)
                {
                    sec1[x, z] = sec2[x, z];
                    raised1 = true;
                }
            }
        }


        float achja = 2.2f;

        if (raised1)
        {
            List<Vector3> points = new List<Vector3>();
            points.Add(point + new Vector3(scale * -achja, 0f, scale * -achja));
            points.Add(point + new Vector3(scale * achja, 0f, scale * -achja));
            points.Add(point + new Vector3(scale * -achja, 0f, scale * achja));
            points.Add(point + new Vector3(scale * achja, 0f, scale * achja));
            FillSectorsByElements(points, sector1, sec1);
        }
        if (raised2)
        {
            List<Vector3> points = new List<Vector3>();
            points.Add(point + new Vector3(scale * -achja, 0f, scale * -achja));
            points.Add(point + new Vector3(scale * achja, 0f, scale * -achja));
            points.Add(point + new Vector3(scale * -achja, 0f, scale * achja));
            points.Add(point + new Vector3(scale * achja, 0f, scale * achja));
            FillSectorsByElements(points, sector2, sec2);
        }
    }


    public float GetHeight(int elementIndex, float s, float leftToRight)
    {
        float absS = ((float)elementIndex) / heightsLeft.ControlPointsAmount;
        float partS = s * (1f / heightsLeft.ControlPointsAmount);

        float left = heightsLeft.SplineAt(absS + partS);
        float right = heightsRight.SplineAt(absS + partS);
        //float left = heightsLeft.SplineAt(absS);
        //float right = heightsRight.SplineAt(absS);

        return leftToRight * (right - left) + left;
    }

    public float[,] Checkpoints
    {
        get
        {
            return tempGeneratedCPs;
        }
    }

    public float GetHeight(float x, float z)
    {
        return terrain.SampleHeight(new Vector3(x, 0f, z));
    }

    public float GetTensorHeight(float x, float z)
    {
        return tensorProductPlane.At(x, z);
    }

    public float GetTensorHeightSector(float x, float z, int sector)
    {
        if (sector >= 0 && sector < sectors.Count)
        {
            return sectors[sector].At(x, z);
        }

        return 0f;
    }
}
