using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartVisible : MonoBehaviour {

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

    
}
