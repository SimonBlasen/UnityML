using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurbRenderer : MonoBehaviour
{
    private MeshFilter meshFilter = null;
    private MeshCollider meshCollider = null;

    // Use this for initialization
    void Start ()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ApplyCurb(GeneratedCurb curb, TerrainModifier terrainModifier)
    {
        // Todo height of TPP apply

        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
            meshCollider = GetComponent<MeshCollider>();
        }

        Vector3[] trVerts = new Vector3[curb.RenderVertices.Length];
        for (int i = 0; i < trVerts.Length; i++)
        {
            trVerts[i] = curb.RenderVertices[i] + new Vector3(0f, terrainModifier.GetTensorHeight(curb.RenderVertices[i].x, curb.RenderVertices[i].z), 0f);
        }




        meshFilter.mesh.Clear();

        meshFilter.mesh.vertices = trVerts;
        meshFilter.mesh.uv = curb.RenderUVs;

        meshFilter.mesh.subMeshCount = 1;

        meshFilter.mesh.SetTriangles(curb.RenderTriangles, 0);

        meshFilter.mesh.RecalculateNormals();

        meshCollider.sharedMesh = meshFilter.mesh;
    }
}
