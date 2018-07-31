using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Partcontainer : MonoBehaviour
{
    private Mesh mesh;
    private MeshCollider col;

    private PartDirection partDirection = PartDirection.East;
    private PartRotation partRotation = PartRotation.Down;

    // Use this for initialization
    void Start ()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        col = GetComponent<MeshCollider>();

        SetPart(PartType.Part1x1x5);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetPart(PartType type)
    {
        Part part = Part.MakePart(type);
        mesh.Clear();
        mesh.vertices = part.GetVertices(partDirection, partRotation, 0, 0, 0);
        mesh.uv = part.GetUVs(partDirection, partRotation);
        mesh.triangles = part.GetTriangles(partDirection, partRotation, 0);

        mesh.RecalculateNormals();

        col.sharedMesh = null;
        col.sharedMesh = mesh;
    }

    public void SetDirection(PartDirection direction)
    {
        partDirection = direction;
    }

    public void SetRotation(PartRotation rotation)
    {
        partRotation = rotation;
    }
}
