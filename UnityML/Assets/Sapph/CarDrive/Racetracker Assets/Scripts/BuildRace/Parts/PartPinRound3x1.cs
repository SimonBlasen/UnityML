using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartPinRound3x1 : Part
{
    public PartPinRound3x1()
    {
        FillData();
    }

    public override bool IsWheel
    {
        get
        {
            return false;
        }
    }

    public override bool IsMotor
    {
        get
        {
            return false;
        }
    }

    public override bool IsGear
    {
        get
        {
            return false;
        }
    }

    public override int AxeLength
    {
        get
        {
            return 0;
        }
    }

    public override bool IsAxe
    {
        get
        {
            return false;
        }
    }

    public override float GearTooths
    {
        get
        {
            return 1f;
        }
    }

    public override float MotorTorque
    {
        get
        {
            return 0f;
        }
    }

    public override float MotorTopspeed
    {
        get
        {
            return 0f;
        }
    }

    public override float Mass
    {
        get
        {
            return 1.5f;
        }
    }

    public override float Grip
    {
        get
        {
            return 0.1f;
        }
    }

    public override float WheelRadius
    {
        get
        {
            return 0f;
        }
    }

    public override bool IsSteerSusp
    {
        get
        {
            return false;
        }
    }

    public override float Health
    {
        get
        {
            return 3f;
        }
    }

    public override Connectionpoint[] GetConnectionpoints(PartDirection direction, PartRotation rotation)
    {
        Connectionpoint[] connectionPoints = new Connectionpoint[3];

        Vector3 directionVector = direction.ToVector3();

        connectionPoints[0] = new ConnectorRoundPin(direction, directionVector);
        connectionPoints[1] = new ConnectorRoundPin(direction, directionVector + directionVector);
        connectionPoints[2] = new ConnectorRoundPin(direction.Opposite(), new Vector3(0f, 0f, 0f));

        return connectionPoints;
    }

    protected override List<int> GenerateTriangles(PartDirection direction, PartRotation rotation, int faceCount)
    {
        List<int> trias = new List<int>();

        for (int i = 0; i < GenerateVertices(direction, rotation, 0, 0, 0).Count / 4; i++)
        {
            trias.Add(faceCount + i * 4);
            trias.Add(faceCount + i * 4 + 2);
            trias.Add(faceCount + i * 4 + 1);
            trias.Add(faceCount + i * 4 + 2);
            trias.Add(faceCount + i * 4 + 3);
            trias.Add(faceCount + i * 4 + 1);
        }

        return trias;
    }

    protected override List<Vector2> GenerateUVs(PartDirection direction, PartRotation rotation)
    {
        List<Vector2> uvs = new List<Vector2>();
        float textureUnit = 0.0625f;

        for (int i = 0; i < GenerateVertices(direction, rotation, 0, 0, 0).Count / 4; i++)
        {
            uvs.Add(new Vector2(textureUnit * 4, textureUnit * 4));
            uvs.Add(new Vector2(textureUnit * 4, textureUnit * 5));
            uvs.Add(new Vector2(textureUnit * 5, textureUnit * 4));
            uvs.Add(new Vector2(textureUnit * 5, textureUnit * 5));
        }

        return uvs;
    }

    protected override List<Vector3> GenerateVertices(PartDirection direction, PartRotation rotation, int posX, int posY, int posZ)
    {
        List<Vector3> verts = new List<Vector3>();


        //verts.Add(new Vector3(posX, posY, posZ));               // 0
        //verts.Add(new Vector3(posX + 1, posY, posZ));           // 1
        //verts.Add(new Vector3(posX, posY + 1, posZ));           // 2
        //verts.Add(new Vector3(posX + 1, posY + 1, posZ));       // 3
        //
        //verts.Add(new Vector3(posX, posY, posZ + 5));           // 4
        //verts.Add(new Vector3(posX + 1, posY, posZ + 5));       // 5
        //verts.Add(new Vector3(posX, posY + 1, posZ + 5));       // 6
        //verts.Add(new Vector3(posX + 1, posY + 1, posZ + 5));   // 7



        Quaternion rotQuat = Quaternion.Euler(0, 0, 0);
        Vector3 axisForward = direction.ToVector3();

        switch (direction)
        {
            case PartDirection.North:
                rotQuat = Quaternion.Euler(0, 0, 0);
                break;
            case PartDirection.East:
                rotQuat = Quaternion.Euler(0, 90, 0);
                break;
            case PartDirection.South:
                rotQuat = Quaternion.Euler(0, 180, 0);
                break;
            case PartDirection.West:
                rotQuat = Quaternion.Euler(0, -90, 0);
                break;
            case PartDirection.Up:
                rotQuat = Quaternion.Euler(-90, 0, 0);
                break;
            case PartDirection.Down:
                rotQuat = Quaternion.Euler(90, 0, 0);
                break;
        }

        float rotationAngle = 0f;

        switch (rotation)
        {
            case PartRotation.Up:
                rotationAngle = 0f;
                break;
            case PartRotation.Right:
                rotationAngle = 90f;
                break;
            case PartRotation.Down:
                rotationAngle = 0;
                break;
            case PartRotation.Left:
                rotationAngle = 90f;
                break;
        }


        Quaternion rotAxis = Quaternion.AngleAxis(rotationAngle, axisForward);


        #region The both ends

        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.125f, posY - 0.25f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.125f, posY - 0.25f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.25f, posY - 0.125f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.25f, posY - 0.125f, posZ - 0.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.25f, posY - 0.125f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.25f, posY - 0.125f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.25f, posY + 0.125f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.25f, posY + 0.125f, posZ - 0.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.25f, posY + 0.125f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.25f, posY + 0.125f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.125f, posY + 0.25f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.125f, posY + 0.25f, posZ - 0.5f)));                       // 0



        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.125f, posY - 0.25f, posZ + 2.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.25f, posY - 0.125f, posZ + 2.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.125f, posY - 0.25f, posZ + 2.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.25f, posY - 0.125f, posZ + 2.5f)));                       // 0
                                                                                       
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.25f, posY - 0.125f, posZ + 2.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.25f, posY + 0.125f, posZ + 2.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.25f, posY - 0.125f, posZ + 2.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.25f, posY + 0.125f, posZ + 2.5f)));                       // 0
                                                                                       
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.25f, posY + 0.125f, posZ + 2.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.125f, posY + 0.25f, posZ + 2.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.25f, posY + 0.125f, posZ + 2.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.125f, posY + 0.25f, posZ + 2.5f)));                       // 0

        #endregion


        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.125f, posY - 0.25f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.125f, posY - 0.25f, posZ + 2.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.25f, posY - 0.125f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.25f, posY - 0.125f, posZ + 2.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.25f, posY - 0.125f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.25f, posY - 0.125f, posZ + 2.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.25f, posY + 0.125f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.25f, posY + 0.125f, posZ + 2.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.25f, posY + 0.125f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.25f, posY + 0.125f, posZ + 2.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.125f, posY + 0.25f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.125f, posY + 0.25f, posZ + 2.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.125f, posY + 0.25f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.125f, posY + 0.25f, posZ + 2.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.125f, posY + 0.25f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.125f, posY + 0.25f, posZ + 2.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.125f, posY + 0.25f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.125f, posY + 0.25f, posZ + 2.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.25f, posY + 0.125f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.25f, posY + 0.125f, posZ + 2.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.25f, posY + 0.125f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.25f, posY + 0.125f, posZ + 2.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.25f, posY - 0.125f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.25f, posY - 0.125f, posZ + 2.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.25f, posY - 0.125f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.25f, posY - 0.125f, posZ + 2.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.125f, posY - 0.25f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.125f, posY - 0.25f, posZ + 2.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.125f, posY - 0.25f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.125f, posY - 0.25f, posZ + 2.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.125f, posY - 0.25f, posZ - 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.125f, posY - 0.25f, posZ + 2.5f)));                       // 0


        return verts;
    }
}
