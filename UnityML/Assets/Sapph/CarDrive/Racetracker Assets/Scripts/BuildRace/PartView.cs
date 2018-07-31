using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartView : MonoBehaviour
{
    public MeshFilter meshFilter;
    public MeshCollider col;

    private PartType partType = PartType.Part1x1x5;
    private PartDirection partDirection = PartDirection.East;
    private PartRotation partRotation = PartRotation.Down;
    private Vector3Int partPosition = new Vector3Int();

    private bool isVisible = true;

    //private List<BoxCollider> createdBoxColliders = new List<BoxCollider>();
    private Connectionpoint[] connectionPoints = null;

    private Connectionpoint[] connectionPointsNotSolid = null;

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

    public PartType Type
    {
        get
        {
            return partType;
        }
    }

    public bool Visible
    {
        get
        {
            return isVisible;
        }
        set
        {
            //Debug.Log("Visibility set to: " + value.ToString());
            isVisible = value;
            if (value)
            {
                SetPart(partType);
            }
            else
            {
                meshFilter.mesh.Clear();
            }
        }
    }

    public PartDirection Direction
    {
        get
        {
            return partDirection;
        }
    }

    public PartRotation Rotation
    {
        get
        {
            return partRotation;
        }
    }

    public Connectionpoint[] ConnectionPoints
    {
        get
        {
            return connectionPoints;
        }
    }

    public Connectionpoint[] ConnectionPointsNotSolid
    {
        get
        {
            return connectionPointsNotSolid;
        }
    }

    public Vector3Int Position
    {
        get
        {
            return partPosition;
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
        
            SetPart(partType);
    }

    public void SetRotation(PartRotation rotation)
    {
        partRotation = rotation;
        
            SetPart(partType);
    }

    public void SetPart(PartType type)
    {
        destroyOldData();

        partType = type;

        Part part = Part.MakePart(type);
        if (isVisible)
        {
            //Debug.Log("IsVisible");
            meshFilter.mesh.Clear();
            meshFilter.mesh.vertices = part.GetVertices(partDirection, partRotation, 0, 0, 0);
            meshFilter.mesh.uv = part.GetUVs(partDirection, partRotation);
            meshFilter.mesh.triangles = part.GetTriangles(partDirection, partRotation, 0);

            meshFilter.mesh.RecalculateNormals();
        }
        else
        {
            //Debug.Log("NotVisible");
        }

        //col.sharedMesh = null;
        //col.sharedMesh = meshFilter.mesh;


        // Create box colliders

        connectionPoints = part.GetConnectionpoints(partDirection, partRotation);
        //connectionointInfos
        //createdBoxColliders = new BoxCollider[connectionointInfos.Length];
        //connectionPoints = new Connectionpoint[connectionointInfos.Length];
        //for (int i = 0; i < connectionPoints.Length; i++)
        //{
        //    for (int j = 0; j < connectionPoints[i].BoxCollidersAmount; j++)
        //    {
        //        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        //        boxCollider.tag = "Connector";
        //        boxCollider.isTrigger = true;
        //        connectionPoints[i].SetBoxCollider(boxCollider, j);
        //    }
        //
        //
        //    //createdBoxColliders[i] = gameObject.AddComponent<BoxCollider>();
        //    //createdBoxColliders[i].tag = "Connector";
        //    //createdBoxColliders[i].isTrigger = true;
        //    //createdBoxColliders[i].center = connectionointInfos[i].relativePosition;
        //    //createdBoxColliders[i].size = connectionointInfos[i].size;
        //    //
        //    //connectionPoints[i] = Connectionpoint.MakeConnectionpoint(connectionointInfos[i].connectorType, connectionointInfos[i].direction, connectionointInfos[i].relativePosition);
        //    //connectionPoints[i].Boxcollider = createdBoxColliders[i];
        //}

        int conPointsNotSolid = 0;
        for (int i = 0; i < connectionPoints.Length; i++)
        {
            if (connectionPoints[i].Connectortype != ConnectorType.SOLID)
            {
                conPointsNotSolid++;
            }
        }

        connectionPointsNotSolid = new Connectionpoint[conPointsNotSolid];

        int conPointsNotSolidCounter = 0;

        for (int i = 0; i < connectionPoints.Length; i++)
        {
            if (connectionPoints[i].Connectortype != ConnectorType.SOLID)
            {
                connectionPointsNotSolid[conPointsNotSolidCounter] = connectionPoints[i];
                conPointsNotSolidCounter++;
            }
        }
    }

    public void SetEmpty()
    {
        meshFilter.mesh.Clear();
        destroyOldData();
    }

    public bool IsNotEmpty
    {
        get
        {
            return connectionPoints != null;
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
    //public bool MatchConnection(Collider hitCollider, ConnectorType type, PartDirection direction, out Vector3Int connectorPosition)
    //{
    //    int connectorIndex = -1;
    //    for (int i = 0; i < connectionPoints.Length; i++)
    //    {
    //        if (connectionPoints[i].IsYourBoxCollider(hitCollider))
    //        {
    //            connectorIndex = i;
    //            break;
    //        }
    //    }
    //    if (connectorIndex == -1)
    //    {
    //        connectorPosition = new Vector3Int();
    //        return false;
    //    }
    //    else
    //    {
    //        bool canBe = connectionPoints[connectorIndex].CanBeConnected(type, direction, connectionPoints[connectorIndex].GetBoxColliderIndex(hitCollider));
    //        connectorPosition = connectionPoints[connectorIndex].ConnectorPosition;
    //        return canBe;
    //    }
    //
    //
    //    //int boxColliderIndex = -1;
    //    //for (int i = 0; i < createdBoxColliders.Length; i++)
    //    //{
    //    //    if (hitCollider == createdBoxColliders[i])
    //    //    {
    //    //        boxColliderIndex = i;
    //    //        break;
    //    //    }
    //    //}
    //    //
    //    //if (boxColliderIndex == -1)
    //    //{
    //    //    connectorPosition = Vector3.zero;
    //    //    return false;
    //    //}
    //    //else
    //    //{
    //    //    bool canBe = connectionPoints[boxColliderIndex].CanBeConnected(type, direction);
    //    //    connectorPosition = connectionPoints[boxColliderIndex].ConnectingPosition;
    //    //    return canBe;
    //    //}
    //
    //}

    private void destroyOldData()
    {
        if (connectionPoints != null)
        {
            connectionPoints = null;
        }
    }
}
