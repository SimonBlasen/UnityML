using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartBuilding : MonoBehaviour
{
    public MeshFilter meshFilter;
    public MeshCollider col;

    private PartDirection partDirection = PartDirection.East;
    private PartRotation partRotation = PartRotation.Down;
    private Vector3Int partPosition = new Vector3Int();

    //private List<BoxCollider> createdBoxColliders = new List<BoxCollider>();
    private Connectionpoint[] connectionPoints = null;
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

    public Connectionpoint[] ConnectionPoints
    {
        get
        {
            return connectionPoints;
        }
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

    public bool BoundingBoxContains(Vector3Int absolutePosition)
    {
        Vector3Int relativePosition = absolutePosition.Sub(partPosition);
        return boundingBox.IsInside(relativePosition);
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
        destroyOldData();

        currentType = type;

        Part part = Part.MakePart(type);
        meshFilter.mesh.Clear();
        meshFilter.mesh.vertices = part.GetVertices(partDirection, partRotation, 0, 0, 0);
        meshFilter.mesh.uv = part.GetUVs(partDirection, partRotation);
        meshFilter.mesh.triangles = part.GetTriangles(partDirection, partRotation, 0);

        meshFilter.mesh.RecalculateNormals();

        //col.sharedMesh = null;
        //col.sharedMesh = meshFilter.mesh;


        // Create box colliders

        boundingBox = new BoundingBox();
        connectionPoints = part.GetConnectionpoints(partDirection, partRotation);
        //connectionointInfos
        //createdBoxColliders = new BoxCollider[connectionointInfos.Length];
        //connectionPoints = new Connectionpoint[connectionointInfos.Length];
        for (int i = 0; i < connectionPoints.Length; i++)
        {
            for (int j = 0; j < connectionPoints[i].BoxCollidersAmount; j++)
            {
                BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                boxCollider.tag = "Connector";
                boxCollider.isTrigger = true;
                connectionPoints[i].SetBoxCollider(boxCollider, j);
            }

            boundingBox.AddElement(connectionPoints[i].ConnectorPosition);

            //createdBoxColliders[i] = gameObject.AddComponent<BoxCollider>();
            //createdBoxColliders[i].tag = "Connector";
            //createdBoxColliders[i].isTrigger = true;
            //createdBoxColliders[i].center = connectionointInfos[i].relativePosition;
            //createdBoxColliders[i].size = connectionointInfos[i].size;
            //
            //connectionPoints[i] = Connectionpoint.MakeConnectionpoint(connectionointInfos[i].connectorType, connectionointInfos[i].direction, connectionointInfos[i].relativePosition);
            //connectionPoints[i].Boxcollider = createdBoxColliders[i];
        }

    }

    /// <summary>
    /// This method checks, if a part can be connected to a given collider
    /// </summary>
    /// <param name="hitCollider">The collider, where the part should be tryed to connect</param>
    /// <param name="type">The type of the part, that trys to connect</param>
    /// <param name="direction">The direction of the part, that trys to connect</param>
    /// <param name="connectorPosition">Here you get the relative position of the connection point</param>
    /// <returns>Whether it can be connected or not</returns>
    public bool MatchConnection(Collider hitCollider, ConnectorType type, PartDirection direction, out Vector3Int connectorPosition)
    {
        int connectorIndex = -1;
        for (int i = 0; i < connectionPoints.Length; i++)
        {
            if (connectionPoints[i].IsYourBoxCollider(hitCollider))
            {
                connectorIndex = i;
                break;
            }
        }
        if (connectorIndex == -1)
        {
            connectorPosition = new Vector3Int();
            return false;
        }
        else
        {
            bool canBe = connectionPoints[connectorIndex].CanBeConnected(type, direction, connectionPoints[connectorIndex].GetBoxColliderIndex(hitCollider));
            connectorPosition = connectionPoints[connectorIndex].ConnectorPosition;
            return canBe;
        }


        //int boxColliderIndex = -1;
        //for (int i = 0; i < createdBoxColliders.Length; i++)
        //{
        //    if (hitCollider == createdBoxColliders[i])
        //    {
        //        boxColliderIndex = i;
        //        break;
        //    }
        //}
        //
        //if (boxColliderIndex == -1)
        //{
        //    connectorPosition = Vector3.zero;
        //    return false;
        //}
        //else
        //{
        //    bool canBe = connectionPoints[boxColliderIndex].CanBeConnected(type, direction);
        //    connectorPosition = connectionPoints[boxColliderIndex].ConnectingPosition;
        //    return canBe;
        //}

    }

    public bool IsOneOfYourColliders(Collider hitCollider)
    {
        int connectorIndex = -1;
        for (int i = 0; i < connectionPoints.Length; i++)
        {
            if (connectionPoints[i].IsYourBoxCollider(hitCollider))
            {
                connectorIndex = i;
                break;
            }
        }
        if (connectorIndex == -1)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void destroyOldData()
    {
        if (connectionPoints != null)
        {
            connectionPoints = null;
        }
    }
}
