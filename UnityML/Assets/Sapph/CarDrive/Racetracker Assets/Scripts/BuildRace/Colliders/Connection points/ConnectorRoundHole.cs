using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorRoundHole : Connectionpoint
{

	public ConnectorRoundHole(PartDirection facedDirection, Vector3 relativePosition)
    {
        type = ConnectorType.ROUND_HOLE;

        this.facedDirection = facedDirection;
        connectorPosition = new Vector3Int(relativePosition.x, relativePosition.y, relativePosition.z);

        //Vector3 addVector = Vector3.zero;
        //if (facedDirection == PartDirection.East)
        //    addVector = new Vector3(1f, 0, 0);
        //if (facedDirection == PartDirection.West)
        //    addVector = new Vector3(-1f, 0, 0);
        //
        //this.connectingPosition = relativePosition + addVector;
        boxColliderDirections = new PartDirection[2];
        boxColliderDirections[0] = facedDirection.Opposite();
        boxColliderDirections[1] = facedDirection;
        acceptedConnectortypes = new ConnectorType[2];
        acceptedConnectortypes[0] = ConnectorType.ROUND_PIN;
        acceptedConnectortypes[1] = ConnectorType.CROSS_PIN;

        boxColliders = new BoxCollider[2];
    }

    public override int BoxCollidersAmount
    {
        get
        {
            return 2;
        }
    }

    public override void SetBoxCollider(BoxCollider boxCollider, int index)
    {
        boxColliders[index] = boxCollider;
        //TODO Eigenschaften setzen

        Vector3 boxCollider1Direction = facedDirection.ToVector3();

        if (index == 0)
        {
            boxCollider.center = connectorPosition.ToVector3() + 0.25f * boxCollider1Direction;
        }
        else if (index == 1)
        {
            boxCollider.center = connectorPosition.ToVector3() + -0.25f * boxCollider1Direction;
        }
        boxCollider.size = (new Vector3(1f - 0.5f * boxCollider1Direction.x * boxCollider1Direction.x,
                                        1f - 0.5f * boxCollider1Direction.y * boxCollider1Direction.y,
                                        1f - 0.5f * boxCollider1Direction.z * boxCollider1Direction.z));
    }
}
