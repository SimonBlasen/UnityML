using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainAnalyzer
{
    private Terrain terrain;

    public TerrainAnalyzer(Terrain terrain)
    {
        this.terrain = terrain;
    }

    public void Analyze(int xAmount, int yAmount)
    {
        float[,] jakobiX = new float[xAmount, yAmount];
        float[,] jakobiY = new float[xAmount, yAmount];

        for (int x = 0; x < xAmount; x++)
        {
            for (int y = 0; y < yAmount; y++)
            {
                if (x == 0)
                {
                    jakobiX[x, y] = terrain.terrainData.GetHeight(x + 1, y) - terrain.terrainData.GetHeight(x, y);
                }
                else if (x == xAmount - 1)
                {
                    jakobiX[x, y] = terrain.terrainData.GetHeight(x, y) - terrain.terrainData.GetHeight(x - 1, y);
                }
                else
                {
                    jakobiX[x, y] = (terrain.terrainData.GetHeight(x + 1, y) - terrain.terrainData.GetHeight(x - 1, y)) * 0.5f;
                }


                if (y == 0)
                {
                    jakobiY[x, y] = terrain.terrainData.GetHeight(x, y + 1) - terrain.terrainData.GetHeight(x, y);
                }
                else if (y == yAmount - 1)
                {
                    jakobiY[x, y] = terrain.terrainData.GetHeight(x, y) - terrain.terrainData.GetHeight(x, y - 1);
                }
                else
                {
                    jakobiY[x, y] = (terrain.terrainData.GetHeight(x, y + 1) - terrain.terrainData.GetHeight(x, y - 1)) * 0.5f;
                }
            }
        }


    }
}
