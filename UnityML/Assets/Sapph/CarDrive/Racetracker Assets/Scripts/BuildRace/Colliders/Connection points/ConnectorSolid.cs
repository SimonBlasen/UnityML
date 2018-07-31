using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectorSolid : Connectionpoint
{

    public ConnectorSolid(PartDirection facedDirection, Vector3 relativePosition)
    {
        type = ConnectorType.SOLID;

        this.facedDirection = facedDirection;
        connectorPosition = new Vector3Int(relativePosition.x, relativePosition.y, relativePosition.z);

        //Vector3 addVector = Vector3.zero;
        //if (facedDirection == PartDirection.East)
        //    addVector = new Vector3(1f, 0, 0);
        //if (facedDirection == PartDirection.West)
        //    addVector = new Vector3(-1f, 0, 0);
        //
        //this.connectingPosition = relativePosition + addVector;
        boxColliderDirections = new PartDirection[1];
        boxColliderDirections[0] = facedDirection;
        acceptedConnectortypes = new ConnectorType[0];

        boxColliders = new BoxCollider[2];
    }

    public override int BoxCollidersAmount
    {
        get
        {
            return 1;
        }
    }

    public override void SetBoxCollider(BoxCollider boxCollider, int index)
    {
        boxColliders[index] = boxCollider;

        Vector3 boxCollider1Direction = facedDirection.ToVector3();

        
        boxCollider.center = connectorPosition.ToVector3();
        
        boxCollider.size = (new Vector3(1f, 1f, 1f));
    }
}
