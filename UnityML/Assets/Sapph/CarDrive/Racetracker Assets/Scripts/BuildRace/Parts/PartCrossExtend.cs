using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartCrossExtend : Part
{
    public PartCrossExtend()
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
            return 1f;
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
            return 2f;
        }
    }

    public override Connectionpoint[] GetConnectionpoints(PartDirection direction, PartRotation rotation)
    {
        Connectionpoint[] connectionPoints = new Connectionpoint[2];

        Vector3 directionVector = direction.ToVector3();

        connectionPoints[0] = new ConnectorCrossHole(direction.Opposite(), directionVector);
        connectionPoints[1] = new ConnectorCrossHole(direction, new Vector3(0f, 0f, 0f));

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

    private Vector2 uvPos = new Vector2(10, 4);

    protected override List<Vector2> GenerateUVs(PartDirection direction, PartRotation rotation)
    {
        List<Vector2> uvs = new List<Vector2>();
        float textureUnit = 0.0625f;

        for (int i = 0; i < GenerateVertices(direction, rotation, 0, 0, 0).Count / 4; i++)
        {
            uvs.Add(new Vector2(textureUnit * (uvPos.x), textureUnit * (uvPos.y)));
            uvs.Add(new Vector2(textureUnit * (uvPos.x), textureUnit * (uvPos.y + 1)));
            uvs.Add(new Vector2(textureUnit * (uvPos.x + 1), textureUnit * (uvPos.y)));
            uvs.Add(new Vector2(textureUnit * (uvPos.x + 1), textureUnit * (uvPos.y + 1)));
        }

        return uvs;
    }

    protected override List<Vector3> GenerateVertices(PartDirection direction, PartRotation rotation, int posX, int posY, int posZ)
    {
        List<Vector3> verts = new List<Vector3>();

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


        float fac = 0.125f * 0.5f;

        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 6f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 6f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 6f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 2f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -2f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 2f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -2f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -6f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -6f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -6f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -6f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -2f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -2f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 2f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 2f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 1f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 6f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 6f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 6f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 6f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 2f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -2f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 2f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -2f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -6f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -6f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -6f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -6f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -2f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -2f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 2f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 2f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 1f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 6f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 4f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -2f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -2f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 2f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 2f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 2f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 2f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 6f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 6f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 6f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 6f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 6f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 6f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 6f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 6f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 2f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 2f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 2f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 2f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -2f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -2f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -6f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -6f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -2f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -2f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -6f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -6f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -6f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -6f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -6f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -6f, 24f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -2f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -2f, 24f)) * fac));



        return verts;
    }
}
