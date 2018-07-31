using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneRenderer : MonoBehaviour
{

    public int resolution = 1000;
    public float scale = 1f;

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    // Use this for initialization
    void Start () {
        meshFilter = GetComponent<MeshFilter>();

        meshCollider = GetComponent<MeshCollider>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RefreshPlane(TerrainModifier terrainModifier)
    {
        List<Vector3> verts = new List<Vector3>();
        List<int> trias = new List<int>();

        for (int x = -resolution; x <= resolution; x++)
        {
            for (int y = -resolution; y <= resolution; y++)
            {
                verts.Add(new Vector3(x * scale, terrainModifier.GetTensorHeight(x * scale, y * scale), y * scale));
            }
        }

        for (int x = resolution * 2 + 1; x < verts.Count; x++)
        {
            if (x % (resolution * 2 + 1) != 0)
            {
                trias.Add(x - (resolution * 2 + 1) - 1);
                trias.Add(x - 1);
                trias.Add(x);
                trias.Add(x - (resolution * 2 + 1) - 1);
                trias.Add(x);
                trias.Add(x - (resolution * 2 + 1));
            }
        }

        meshFilter.mesh.Clear();
        meshFilter.mesh.vertices = verts.ToArray();

        meshFilter.mesh.subMeshCount = 1;

        meshFilter.mesh.SetTriangles(trias.ToArray(), 0);

        meshFilter.mesh.RecalculateNormals();

        meshCollider.sharedMesh = meshFilter.mesh;
    }
}
