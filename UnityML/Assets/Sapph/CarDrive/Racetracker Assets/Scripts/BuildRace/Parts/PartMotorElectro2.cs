using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartMotorElectro2 : Part
{
    public PartMotorElectro2()
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
            return true;
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
            return 60f;
        }
    }

    public override float MotorTopspeed
    {
        get
        {
            return 200f;
        }
    }

    public override float Mass
    {
        get
        {
            return 100f;
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
            return 30f;
        }
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

    private Vector2 grey = new Vector2(10, 4);
    private Vector2 darkGrey = new Vector2(4, 7);
    private Vector2 orange = new Vector2(1, 7);

    protected override List<Vector2> GenerateUVs(PartDirection direction, PartRotation rotation)
    {
        List<Vector2> uvs = new List<Vector2>();
        float textureUnit = 0.0625f;

        for (int i = 0; i < GenerateVertices(direction, rotation, 0, 0, 0).Count / 4; i++)
        {
            if (i >= 32 && i < 40)
            {
                uvs.Add(new Vector2(textureUnit * (orange.x), textureUnit * (orange.y)));
                uvs.Add(new Vector2(textureUnit * (orange.x), textureUnit * (orange.y + 1)));
                uvs.Add(new Vector2(textureUnit * (orange.x + 1), textureUnit * (orange.y)));
                uvs.Add(new Vector2(textureUnit * (orange.x + 1), textureUnit * (orange.y + 1)));
            }
            else if (i < 56)
            {
                uvs.Add(new Vector2(textureUnit * (darkGrey.x), textureUnit * (darkGrey.y)));
                uvs.Add(new Vector2(textureUnit * (darkGrey.x), textureUnit * (darkGrey.y + 1)));
                uvs.Add(new Vector2(textureUnit * (darkGrey.x + 1), textureUnit * (darkGrey.y)));
                uvs.Add(new Vector2(textureUnit * (darkGrey.x + 1), textureUnit * (darkGrey.y + 1)));
            }
            else
            {
                uvs.Add(new Vector2(textureUnit * (grey.x), textureUnit * (grey.y)));
                uvs.Add(new Vector2(textureUnit * (grey.x), textureUnit * (grey.y + 1)));
                uvs.Add(new Vector2(textureUnit * (grey.x + 1), textureUnit * (grey.y)));
                uvs.Add(new Vector2(textureUnit * (grey.x + 1), textureUnit * (grey.y + 1)));
            }
        }

        //uvs.Add(new Vector2(textureUnit * 1, textureUnit * 1));
        //uvs.Add(new Vector2(textureUnit * 1, textureUnit * 2));
        //uvs.Add(new Vector2(textureUnit * 2, textureUnit * 1));
        //uvs.Add(new Vector2(textureUnit * 2, textureUnit * 2));
        //uvs.Add(new Vector2(textureUnit * 1, textureUnit * 1));
        //uvs.Add(new Vector2(textureUnit * 1, textureUnit * 2));
        //uvs.Add(new Vector2(textureUnit * 2, textureUnit * 1));
        //uvs.Add(new Vector2(textureUnit * 2, textureUnit * 2));

        return uvs;
    }

    public override Connectionpoint[] GetConnectionpoints(PartDirection direction, PartRotation rotation)
    {
        int lengthToBack = 5;

        Connectionpoint[] connectionPoints = new Connectionpoint[9 + (lengthToBack - 1) * 9];

        Vector3 directionVector = direction.ToVector3();
        Vector3 sideVector = Vector3.zero;
        PartDirection firstHoleDirection = PartDirection.Down;
        PartDirection secondHoleDirection = PartDirection.Down;
        switch (direction)
        {
            case PartDirection.Down:
                switch (rotation)
                {
                    case PartRotation.Down:
                        firstHoleDirection = PartDirection.West;
                        secondHoleDirection = PartDirection.North;
                        break;
                    case PartRotation.Left:
                        firstHoleDirection = PartDirection.North;
                        secondHoleDirection = PartDirection.East;
                        break;
                    case PartRotation.Up:
                        firstHoleDirection = PartDirection.East;
                        secondHoleDirection = PartDirection.South;
                        break;
                    case PartRotation.Right:
                        firstHoleDirection = PartDirection.South;
                        secondHoleDirection = PartDirection.West;
                        break;
                }
                break;
            case PartDirection.Up:
                switch (rotation)
                {
                    case PartRotation.Down:
                        firstHoleDirection = PartDirection.West;
                        secondHoleDirection = PartDirection.North;
                        break;
                    case PartRotation.Left:
                        firstHoleDirection = PartDirection.South;
                        secondHoleDirection = PartDirection.West;
                        break;
                    case PartRotation.Up:
                        firstHoleDirection = PartDirection.East;
                        secondHoleDirection = PartDirection.South;
                        break;
                    case PartRotation.Right:
                        firstHoleDirection = PartDirection.North;
                        secondHoleDirection = PartDirection.East;
                        break;
                }
                break;
            case PartDirection.East:
                switch (rotation)
                {
                    case PartRotation.Down:
                        firstHoleDirection = PartDirection.North;
                        secondHoleDirection = PartDirection.Down;
                        break;
                    case PartRotation.Left:
                        firstHoleDirection = PartDirection.Up;
                        secondHoleDirection = PartDirection.North;
                        break;
                    case PartRotation.Up:
                        firstHoleDirection = PartDirection.South;
                        secondHoleDirection = PartDirection.Up;
                        break;
                    case PartRotation.Right:
                        firstHoleDirection = PartDirection.Down;
                        secondHoleDirection = PartDirection.South;
                        break;
                }
                break;
            case PartDirection.South:
                switch (rotation)
                {
                    case PartRotation.Down:
                        firstHoleDirection = PartDirection.East;
                        secondHoleDirection = PartDirection.Down;
                        break;
                    case PartRotation.Left:
                        firstHoleDirection = PartDirection.Up;
                        secondHoleDirection = PartDirection.East;
                        break;
                    case PartRotation.Up:
                        firstHoleDirection = PartDirection.West;
                        secondHoleDirection = PartDirection.Up;
                        break;
                    case PartRotation.Right:
                        firstHoleDirection = PartDirection.Down;
                        secondHoleDirection = PartDirection.West;
                        break;
                }
                break;
            case PartDirection.West:
                switch (rotation)
                {
                    case PartRotation.Down:
                        firstHoleDirection = PartDirection.South;
                        secondHoleDirection = PartDirection.Down;
                        break;
                    case PartRotation.Left:
                        firstHoleDirection = PartDirection.Up;
                        secondHoleDirection = PartDirection.South;
                        break;
                    case PartRotation.Up:
                        firstHoleDirection = PartDirection.North;
                        secondHoleDirection = PartDirection.Up;
                        break;
                    case PartRotation.Right:
                        firstHoleDirection = PartDirection.Down;
                        secondHoleDirection = PartDirection.North;
                        break;
                }
                break;
            case PartDirection.North:
                switch (rotation)
                {
                    case PartRotation.Down:
                        firstHoleDirection = PartDirection.West;
                        secondHoleDirection = PartDirection.Up;
                        break;
                    case PartRotation.Left:
                        firstHoleDirection = PartDirection.Up;
                        secondHoleDirection = PartDirection.East;
                        break;
                    case PartRotation.Up:
                        firstHoleDirection = PartDirection.East;
                        secondHoleDirection = PartDirection.Down;
                        break;
                    case PartRotation.Right:
                        firstHoleDirection = PartDirection.Down;
                        secondHoleDirection = PartDirection.West;
                        break;
                }
                break;
        }

        sideVector = firstHoleDirection.ToVector3();
        Vector3 upVector = secondHoleDirection.ToVector3();

        connectionPoints[0] = new ConnectorCrossPower(direction.Opposite(), Vector3.zero);
        connectionPoints[1] = new ConnectorRoundHole(direction.Opposite(), sideVector);
        connectionPoints[2] = new ConnectorRoundHole(direction.Opposite(), -sideVector);
        connectionPoints[3] = new ConnectorRoundHole(direction.Opposite(), upVector);
        connectionPoints[4] = new ConnectorRoundHole(direction.Opposite(), -upVector);
        connectionPoints[5] = new ConnectorSolid(direction, upVector + sideVector);
        connectionPoints[6] = new ConnectorSolid(direction, upVector - sideVector);
        connectionPoints[7] = new ConnectorSolid(direction, -upVector + sideVector);
        connectionPoints[8] = new ConnectorSolid(direction, -upVector - sideVector);

        for (int i = 1; i < lengthToBack; i++)
        {
            connectionPoints[i * 9] = new ConnectorSolid(direction, upVector + sideVector + directionVector * i);
            connectionPoints[i * 9 + 1] = new ConnectorSolid(direction, upVector - sideVector + directionVector * i);
            connectionPoints[i * 9 + 2] = new ConnectorSolid(direction, -upVector + sideVector + directionVector * i);
            connectionPoints[i * 9 + 3] = new ConnectorSolid(direction, -upVector - sideVector + directionVector * i);
            connectionPoints[i * 9 + 4] = new ConnectorSolid(direction, upVector + directionVector * i);
            connectionPoints[i * 9 + 5] = new ConnectorSolid(direction, -upVector + directionVector * i);
            connectionPoints[i * 9 + 6] = new ConnectorSolid(direction, sideVector + directionVector * i);
            connectionPoints[i * 9 + 7] = new ConnectorSolid(direction, -sideVector + directionVector * i);
            connectionPoints[i * 9 + 8] = new ConnectorSolid(direction, directionVector * i);
        }


        return connectionPoints;

        //ConnectionpointInfos[] infos = new ConnectionpointInfos[10];
        //for (int i = 0; i < infos.Length; i++)
        //{
        //    infos[i] = new ConnectionpointInfos();
        //    infos[i].connectorType = ConnectorType.ROUND_HOLE;
        //    infos[i].size = new Vector3(1f, 1f, 1f);
        //    if (i < 5)
        //    {
        //        infos[i].direction = PartDirection.East;
        //        infos[i].relativePosition = new Vector3(-1f, 0.5f, 0.5f + i - 2.5f);
        //    }
        //    else
        //    {
        //        infos[i].direction = PartDirection.West;
        //        infos[i].relativePosition = new Vector3(1f, 0.5f, 0.5f + i - 7.5f);
        //    }
        //}
        //
        //return infos;
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
                rotationAngle = -90f;
                break;
            case PartRotation.Down:
                rotationAngle = 180;
                break;
            case PartRotation.Left:
                rotationAngle = 90f;
                break;
        }


        Quaternion rotAxis = Quaternion.AngleAxis(rotationAngle, axisForward);


        float fac = 0.125f;
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -12f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -12f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -12f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -9f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -12f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -9f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -9f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(9f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, -6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -9f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-9f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-9f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, 6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-9f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-9f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 9f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, 6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 9f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 12f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 12f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 12f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 9f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 12f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 9f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(9f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(9f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(9f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, -6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-9f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -9f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-7f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -7f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-7f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -7f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-9f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-7f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 9f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 7f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-7f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 7f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 7f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(7f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 9f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(9f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 7f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(7f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -7f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -9f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(7f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(9f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -7f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(7f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1.5f, -1.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -0.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-0.5f, -0.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-0.5f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1.5f, -1.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-0.5f, -0.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(0.5f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(0.5f, -0.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1.5f, -1.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1.5f, -1.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(0.5f, -0.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -0.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(0.5f, 0.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 0.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1.5f, 1.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(0.5f, 0.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1.5f, 1.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(0.5f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(1f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 0.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-0.5f, 0.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1.5f, 1.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1.5f, 1.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-0.5f, 0.5f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-1f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-0.5f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -2f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 2f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 2f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 6f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 6f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 10f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 10f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 12f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 12f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 12f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 12f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 12f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 12f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, -6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, -6f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -2f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -10f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, -6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, -6f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -12f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -12f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -10f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -12f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -12f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -12f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -12f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -12f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -12f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -10f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -10f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -6f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -6f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -2f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -2f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 2f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 2f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, 6f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, 6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, 6f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, 6f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 10f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 10f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 10f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 12f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 12f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 12f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 12f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 12f, 34f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 12f, 34f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 12f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 10f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 12f, 34f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 6f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 6f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 10f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 10f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 12f, 34f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 12f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, 6f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, 6f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 10f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 12f, 34f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 12f, 34f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -12f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -12f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -12f, 34f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -12f, 34f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -12f, 34f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -12f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -10f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -10f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -6f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -6f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -12f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -12f, 34f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -10f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -10f, 12f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, -6f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, -6f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -12f, 34f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -12f, 34f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, -6f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 6f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -6f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 10f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, 6f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 2f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 2f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 6f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 6f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -2f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -2f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 2f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 2f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, -6f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, -6f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -2f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -2f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -2f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, -6f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 2f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 6f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -6f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -6f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -2f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -2f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -2f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -2f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 2f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 2f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 2f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 2f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, 6f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, 6f, 20f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -6f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -2f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, 6f, 36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 2f, 36f)) * fac));






        #region The Faces on the negative Z-Side
        #endregion

        return verts;
    }

}
