using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorGearCorner : Connectionpoint
{
    public ConnectorGearCorner(PartDirection facedDirection, Vector3 relativePosition)
    {
        type = ConnectorType.GEAR_CORNER;

        this.facedDirection = facedDirection;
        connectorPosition = new Vector3Int(relativePosition.x, relativePosition.y, relativePosition.z);

        //Vector3 addVector = Vector3.zero;
        //if (facedDirection == PartDirection.East)
        //    addVector = new Vector3(1f, 0, 0);
        //if (facedDirection == PartDirection.West)
        //    addVector = new Vector3(-1f, 0, 0);
        //
        //this.connectingPosition = relativePosition + addVector;
        boxColliderDirections = new PartDirection[6];
        boxColliderDirections[0] = PartDirection.Down;
        boxColliderDirections[1] = PartDirection.East;
        boxColliderDirections[2] = PartDirection.North;
        boxColliderDirections[3] = PartDirection.South;
        boxColliderDirections[4] = PartDirection.Up;
        boxColliderDirections[5] = PartDirection.West;
        acceptedConnectortypes = new ConnectorType[1];
        acceptedConnectortypes[0] = ConnectorType.GEAR_CORNER;

        boxColliders = new BoxCollider[6];
    }

    public override int BoxCollidersAmount
    {
        get
        {
            return 6;
        }
    }

    public override void SetBoxCollider(BoxCollider boxCollider, int index)
    {
        if (index == 0)
        {
            boxColliders[index] = boxCollider;
            //TODO Eigenschaften setzen

            boxCollider.center = connectorPosition.ToVector3();
            boxCollider.size = new Vector3(1f, 1f, 1f);
        }
        else
        {
            boxColliders[index] = boxCollider;
            //TODO Eigenschaften setzen

            boxCollider.center = connectorPosition.ToVector3();
            boxCollider.size = new Vector3(0f, 0f, 0f);
        }
    }
}
