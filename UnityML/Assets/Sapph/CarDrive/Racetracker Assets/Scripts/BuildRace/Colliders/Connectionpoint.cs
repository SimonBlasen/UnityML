using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConnectorType
{
    SOLID, ROUND_HOLE, CROSS_HOLE, ROUND_PIN, CROSS_PIN, CROSS_POWER, GEAR_CORNER
}

/*public static class ConnectorTypeMethods
{
    /// <summary>
    /// Returns the direction that points in the opposite direction
    /// </summary>
    /// <param name="pD">The direction the oposite is requested</param>
    /// <returns>The oposite direction</returns>
    public static string ToString(this ConnectorType pD)
    {
        switch (pD)
        {
            case PartDirection.Down:
                return PartDirection.Up;
            case PartDirection.Up:
                return PartDirection.Down;
            case PartDirection.East:
                return PartDirection.West;
            case PartDirection.West:
                return PartDirection.East;
            case PartDirection.North:
                return PartDirection.South;
            case PartDirection.South:
                return PartDirection.North;
            default:
                return PartDirection.East;
        }
    }
}*/

public class ConnectionpointInfos
{
    public Vector3 relativePosition;
    public Vector3 size;
    public PartDirection direction;
    public ConnectorType connectorType;
}

public abstract class Connectionpoint
{
    protected ConnectorType type;
    protected ConnectorType[] acceptedConnectortypes;
    protected bool isConnected = false;
    protected PartDirection facedDirection = PartDirection.East;
    //protected Vector3 relativePosition;
    protected Vector3Int connectorPosition;

    protected BoxCollider[] boxColliders = null;
    protected PartDirection[] boxColliderDirections = null;

    public PartDirection FacedDirection
    {
        get
        {
            return facedDirection;
        }
    }

    public PartDirection[] AcceptedDirections
    {
        get
        {
            return boxColliderDirections;
        }
    }

    public ConnectorType Connectortype
    {
        get
        {
            return type;
        }
    }

    public bool IsConnected
    {
        get
        {
            return isConnected;
        }
        set
        {
            isConnected = value;
        }
    }

    //public BoxCollider Boxcollider
    //{
    //    get
    //    {
    //        return boxCollider;
    //    }
    //    set
    //    {
    //        boxCollider = value;
    //    }
    //}

    public Vector3Int ConnectorPosition
    {
        get
        {
            return connectorPosition;
        }
    }

    public bool IsYourBoxCollider(Collider collider)
    {
        if (boxColliders != null)
        {
            for (int i = 0; i < boxColliders.Length; i++)
            {
                if (collider == boxColliders[i])
                {
                    return true;
                }
            }
        }

        return false;
    }

    public int GetBoxColliderIndex(Collider collider)
    {
        if (boxColliders != null)
        {
            for (int i = 0; i < boxColliders.Length; i++)
            {
                if (collider == boxColliders[i])
                {
                    return i;
                }
            }
        }

        return -1;
    }

    public PartDirection GetBoxColliderDirection(int index)
    {
        return boxColliderDirections[index];
    }

    public abstract int BoxCollidersAmount { get; }

    public abstract void SetBoxCollider(BoxCollider boxCollider, int index);


    public bool CanBeConnected(ConnectorType otherType, PartDirection otherDirection, int boxColliderIndex)
    {
        if (isConnected == false && boxColliderDirections != null && boxColliders != null && boxColliderDirections.Length == boxColliders.Length && boxColliderIndex >= 0 && boxColliderIndex < boxColliderDirections.Length && acceptedConnectortypes != null && acceptedConnectortypes.Length > 0)
        {
            bool isItAcceptable = false;
            for (int i = 0; i < acceptedConnectortypes.Length; i++)
            {
                if (acceptedConnectortypes[i] == otherType)
                {
                    isItAcceptable = true;
                    break;
                }
            }

            if (isItAcceptable)
            {
                PartDirection acceptedDirection = boxColliderDirections[boxColliderIndex];

                return acceptedDirection == otherDirection;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    //public static Connectionpoint MakeConnectionpoint(ConnectorType type, PartDirection facedDirection, Vector3 relativePosition)
    //{
    //    switch (type)
    //    {
    //        case ConnectorType.CROSS_HOLE:
    //            return new ConnectorCrossHole(facedDirection, relativePosition);
    //        case ConnectorType.CROSS_PIN:
    //            return new ConnectorCrossPin(facedDirection, relativePosition);
    //        case ConnectorType.ROUND_HOLE:
    //            return new ConnectorRoundHole(facedDirection, relativePosition);
    //        case ConnectorType.ROUND_PIN:
    //            return new ConnectorRoundPin(facedDirection, relativePosition);
    //        default:
    //            return new ConnectorRoundHole(facedDirection, relativePosition);
    //    }
    //}
}
