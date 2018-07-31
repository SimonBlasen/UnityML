using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartVisibleMulti : MonoBehaviour
{

    public MeshFilter meshFilter;

    private PartDirection partDirection = PartDirection.East;
    private PartRotation partRotation = PartRotation.Down;
    private Vector3Int partPosition = new Vector3Int();

    private BoundingBox boundingBox = new BoundingBox();
    private PartType currentType = PartType.NONE;

    // Use this for initialization
    void Start()
    {
        //mesh = GetComponent<MeshFilter>().mesh;
        //col = GetComponent<MeshCollider>();

        //SetPart(PartType.Part1x1x5);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector3Int Position
    {
        get
        {
            return partPosition;
        }
    }

    public PartRotation Rotation
    {
        get
        {
            return partRotation;
        }
    }

    public PartDirection Direction
    {
        get
        {
            return partDirection;
        }
    }

    public PartType Type
    {
        get
        {
            return currentType;
        }
    }

    public BoundingBox BoundingBox
    {
        get
        {
            return boundingBox;
        }
    }

    public void SetPosition(Vector3Int position)
    {
        partPosition = position;
        transform.position = position.ToVector3();
    }

    public void SetDirection(PartDirection direction)
    {
        partDirection = direction;
        //TODO neu zeichnen
    }

    public void SetRotation(PartRotation rotation)
    {
        partRotation = rotation;
        //TODO neu zeichnen
    }

    public void AddPart(PartType type, Vector3Int position, PartDirection direciton, PartRotation rotation)
    {
        AddPart(type, position, direciton, rotation, true);
    }

    public void AddPart(PartType type, Vector3Int position, PartDirection direciton, PartRotation rotation, bool recalculate)
    {
        Part part = Part.MakePart(type);
        Vector3[] addVerts = part.GetVertices(direciton, rotation, 0, 0, 0);
        Vector2[] addUVs = part.GetUVs(direciton, rotation);
        int[] addTrias = part.GetTriangles(direciton, rotation, 0);


        Vector3[] oldVerts = meshFilter.mesh.vertices;
        Vector2[] oldUVs = meshFilter.mesh.uv;
        int[] oldTrias = meshFilter.mesh.triangles;

        Vector3[] newVerts = new Vector3[oldVerts.Length + addVerts.Length];
        Vector2[] newUVs = new Vector2[oldUVs.Length + addUVs.Length];
        int[] newTrias = new int[oldTrias.Length + addTrias.Length];

        for (int i = 0; i < oldVerts.Length; i++)
        {
            newVerts[i] = oldVerts[i];
        }
        for (int i = 0; i < addVerts.Length; i++)
        {
            newVerts[oldVerts.Length + i] = addVerts[i] + new Vector3(position.x, position.y, position.z);
        }

        for (int i = 0; i < oldUVs.Length; i++)
        {
            newUVs[i] = oldUVs[i];
        }
        for (int i = 0; i < addUVs.Length; i++)
        {
            newUVs[oldUVs.Length + i] = addUVs[i];
        }

        for (int i = 0; i < oldTrias.Length; i++)
        {
            newTrias[i] = oldTrias[i];
        }
        for (int i = 0; i < addTrias.Length; i++)
        {
            newTrias[oldTrias.Length + i] = addTrias[i] + oldVerts.Length;
        }

        meshFilter.mesh.Clear();
        meshFilter.mesh.vertices = newVerts;
        meshFilter.mesh.uv = newUVs;
        meshFilter.mesh.triangles = newTrias;

        if (recalculate)
        {
            meshFilter.mesh.RecalculateNormals();
        }

        BoxCollider attachedBC = GetComponentInParent<BoxCollider>();
        //attachedBC.enabled = false;

        Vector3[] addedVerts = new Vector3[addVerts.Length];
        for (int i = 0; i < addedVerts.Length; i++)
        {
            addedVerts[i] = meshFilter.mesh.vertices[meshFilter.mesh.vertices.Length - addVerts.Length + i];
        }
        addBoxCollider(addedVerts);
    }

    public void RecalculateNormals()
    {
        meshFilter.mesh.RecalculateNormals();
    }

    public int VerticesCount
    {
        get
        {
            return meshFilter.mesh.vertices.Length;
        }
    }

    private void addBoxCollider(Vector3[] verts)
    {
        BoxCollider newBC = gameObject.AddComponent<BoxCollider>();
        boundingBox = new BoundingBox();

        //Connectionpoint[] connectionPoints = part.GetConnectionpoints(partDirection, partRotation);
        for (int i = 0; i < verts.Length; i++)
        {
            boundingBox.AddElement(new Vector3Int(verts[i]).Add(partPosition));
        }

        Vector3 middle = new Vector3((boundingBox.MaxValues.x - boundingBox.MinValues.x) * 0.5f + boundingBox.MinValues.x
                                        , (boundingBox.MaxValues.y - boundingBox.MinValues.y) * 0.5f + boundingBox.MinValues.y
                                        , (boundingBox.MaxValues.z - boundingBox.MinValues.z) * 0.5f + boundingBox.MinValues.z);
        //Vector3 middle = partPosition.ToVector3();

        newBC.center = middle;
        newBC.size = new Vector3(boundingBox.MaxValues.x + 1f - boundingBox.MinValues.x
                                        , boundingBox.MaxValues.y + 1f - boundingBox.MinValues.y
                                        , boundingBox.MaxValues.z + 1f - boundingBox.MinValues.z);
    }

    public void SetPart(PartType type)
    {
        currentType = type;

        Part part = Part.MakePart(type);
        meshFilter.mesh.Clear();
        meshFilter.mesh.vertices = part.GetVertices(partDirection, partRotation, 0, 0, 0);
        meshFilter.mesh.uv = part.GetUVs(partDirection, partRotation);
        meshFilter.mesh.triangles = part.GetTriangles(partDirection, partRotation, 0);

        meshFilter.mesh.RecalculateNormals();

        BoxCollider attachedBC = GetComponentInParent<BoxCollider>();
        if (attachedBC != null)
        {
            boundingBox = new BoundingBox();

            //Connectionpoint[] connectionPoints = part.GetConnectionpoints(partDirection, partRotation);
            for (int i = 0; i < meshFilter.mesh.vertices.Length; i++)
            {
                boundingBox.AddElement(new Vector3Int(meshFilter.mesh.vertices[i]).Add(partPosition));
            }

            Vector3 middle = new Vector3((boundingBox.MaxValues.x - boundingBox.MinValues.x) * 0.5f + boundingBox.MinValues.x
                                            , (boundingBox.MaxValues.y - boundingBox.MinValues.y) * 0.5f + boundingBox.MinValues.y
                                            , (boundingBox.MaxValues.z - boundingBox.MinValues.z) * 0.5f + boundingBox.MinValues.z);
            //Vector3 middle = partPosition.ToVector3();

            attachedBC.center = middle;
            attachedBC.size = new Vector3(boundingBox.MaxValues.x + 1f - boundingBox.MinValues.x
                                            , boundingBox.MaxValues.y + 1f - boundingBox.MinValues.y
                                            , boundingBox.MaxValues.z + 1f - boundingBox.MinValues.z);
        }
    }

    public void ClearComplete()
    {
        BoxCollider[] boxColls = GetComponents<BoxCollider>();

        meshFilter.mesh.Clear();
        meshFilter.mesh.vertices = null;
        meshFilter.mesh.uv = null;
        meshFilter.mesh.triangles = null;

        meshFilter.mesh.RecalculateNormals();
    }
}
