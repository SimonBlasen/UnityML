using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSteer1Right : Part
{
    public PartSteer1Right()
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
            return 2f;
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
        Connectionpoint[] connectionPoints = new Connectionpoint[2];

        Vector3 directionVector = direction.ToVector3();
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

        connectionPoints[0] = new ConnectorRoundHole(secondHoleDirection, directionVector);
        connectionPoints[1] = new ConnectorRoundHole(firstHoleDirection, new Vector3(0f, 0f, 0f));

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


        #region The one side

        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.25f, -0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.125f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.25f, -0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.125f, -0.25f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.25f, -0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.125f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.5f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.25f, -0.125f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.25f, -0.125f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.25f, 0.125f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.5f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.5f, 0.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.125f, 0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.25f, 0.125f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.5f, 0.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.125f, 0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.125f, 0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.5f, 1.25f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f, 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.25f, 0.125f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.125f, 0.25f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f, 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.25f, -0.125f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.25f, 0.125f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.25f, -0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.125f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.25f, -0.125f)));                       // 0

        #endregion

        #region The opposite of the one side

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.25f, -0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.25f, -0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.125f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.125f, -0.25f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.25f, -0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.5f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.125f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.25f, -0.125f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.25f, -0.125f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.5f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.25f, 0.125f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.5f, 0.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.125f, 0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.25f, 0.125f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.5f, 0.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.125f, 0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.125f, 0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.5f, 1.25f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f, 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.25f, 0.125f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.125f, 0.25f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.25f, -0.125f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f, 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.25f, 0.125f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.25f, -0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.125f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.25f, -0.125f)));                       // 0

        #endregion




        #region The top side

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.5f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.5f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.125f, 0.5f, 0.75f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.125f, 0.5f, 0.75f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.5f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.5f, 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.125f, 0.5f, 0.75f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.25f, 0.5f, 0.875f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(0.25f, 0.5f, 0.875f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.5f, 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.25f, 0.5f, 1.125f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.5f, 1.25f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(0.25f, 0.5f, 1.125f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.125f, 0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.25f, 0.5f, 1.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.125f, 0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.125f, 0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.25f, 0.5f, 1.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.25f, 0.5f, 1.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.25f, 0.5f, 1.125f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.125f, 0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.25f, 0.5f, 1.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.5f, 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.25f, 0.5f, 0.875f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.25f, 0.5f, 1.125f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.5f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.125f, 0.5f, 0.75f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.5f, 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.25f, 0.5f, 0.875f)));                       // 0

        #endregion

        #region The down side

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.125f, -0.5f, 0.75f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.125f, -0.5f, 0.75f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.125f, -0.5f, 0.75f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f, 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.25f, -0.5f, 0.875f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(0.25f, -0.5f, 0.875f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.25f, -0.5f, 1.125f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f, 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f, 1.25f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(0.25f, -0.5f, 1.125f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.125f, -0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.25f, -0.5f, 1.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.125f, -0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.25f, -0.5f, 1.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.125f, -0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.25f, -0.5f, 1.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.25f, -0.5f, 1.125f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.125f, -0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.25f, -0.5f, 1.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f, 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.25f, -0.5f, 0.875f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.25f, -0.5f, 1.125f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f, 0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.125f, -0.5f, 0.75f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.25f, -0.5f, 0.875f)));                       // 0

        #endregion



        #region The End sides

        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.25f, -0.5f, 1.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.25f, 0.5f, 1.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(0.25f, -0.5f, 1.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.25f, -0.5f, 1.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.25f, 0.5f, 1.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.25f, 0.5f, 1.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.25f, -0.5f, 1.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f, 1.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.25f, 0.5f, 1.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.5f, 1.25f)));                       // 0



        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.25f, -0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.25f, -0.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.25f, -0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.25f, -0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.25f, -0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.25f, -0.5f)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.25f, -0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.25f, -0.5f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, 0.5f, -0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, 0.5f, -0.25f)));                       // 0

        #endregion


        #region Inner holes

        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f + 0.375f, -0.5f + 0.25f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f + 0.375f, -0.5f + 0.25f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f + 0.625f, -0.5f + 0.25f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f + 0.625f, -0.5f + 0.25f)));

        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f + 0.625f, -0.5f + 0.25f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f + 0.625f, -0.5f + 0.25f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f + 0.75f, -0.5f + 0.375f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f + 0.75f, -0.5f + 0.375f)));

        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f + 0.75f, -0.5f + 0.375f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f + 0.75f, -0.5f + 0.375f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f + 0.75f, -0.5f + 0.625f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f + 0.75f, -0.5f + 0.625f)));

        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f + 0.75f, -0.5f + 0.625f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f + 0.75f, -0.5f + 0.625f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f + 0.625f, -0.5f + 0.75f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f + 0.625f, -0.5f + 0.75f)));

        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f + 0.625f, -0.5f + 0.75f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f + 0.625f, -0.5f + 0.75f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f + 0.375f, -0.5f + 0.75f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f + 0.375f, -0.5f + 0.75f)));

        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f + 0.375f, -0.5f + 0.75f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f + 0.375f, -0.5f + 0.75f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f + 0.25f, -0.5f + 0.625f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f + 0.25f, -0.5f + 0.625f)));

        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f + 0.25f, -0.5f + 0.625f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f + 0.25f, -0.5f + 0.625f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f + 0.25f, -0.5f + 0.375f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f + 0.25f, -0.5f + 0.375f)));

        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f + 0.25f, -0.5f + 0.375f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f + 0.25f, -0.5f + 0.375f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(0.5f, -0.5f + 0.375f, -0.5f + 0.25f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f, -0.5f + 0.375f, -0.5f + 0.25f)));





        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.375f, 0.5f, -0.5f + 0.25f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.625f, 0.5f, -0.5f + 0.25f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.375f, -0.5f, -0.5f + 0.25f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.625f, -0.5f, -0.5f + 0.25f + 1f)));

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.625f, 0.5f, -0.5f + 0.25f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.75f, 0.5f, -0.5f + 0.375f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.625f, -0.5f, -0.5f + 0.25f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.75f, -0.5f, -0.5f + 0.375f + 1f)));

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.75f, 0.5f, -0.5f + 0.375f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.75f, 0.5f, -0.5f + 0.625f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.75f, -0.5f, -0.5f + 0.375f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.75f, -0.5f, -0.5f + 0.625f + 1f)));

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.75f, 0.5f, -0.5f + 0.625f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.625f, 0.5f, -0.5f + 0.75f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.75f, -0.5f, -0.5f + 0.625f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.625f, -0.5f, -0.5f + 0.75f + 1f)));

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.625f, 0.5f, -0.5f + 0.75f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.375f, 0.5f, -0.5f + 0.75f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.625f, -0.5f, -0.5f + 0.75f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.375f, -0.5f, -0.5f + 0.75f + 1f)));

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.375f, 0.5f, -0.5f + 0.75f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.25f, 0.5f, -0.5f + 0.625f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.375f, -0.5f, -0.5f + 0.75f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.25f, -0.5f, -0.5f + 0.625f + 1f)));

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.25f, 0.5f, -0.5f + 0.625f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.25f, 0.5f, -0.5f + 0.375f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.25f, -0.5f, -0.5f + 0.625f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.25f, -0.5f, -0.5f + 0.375f + 1f)));

        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.25f, 0.5f, -0.5f + 0.375f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.375f, 0.5f, -0.5f + 0.25f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.25f, -0.5f, -0.5f + 0.375f + 1f)));
        verts.Add(rotAxis * rotQuat * (new Vector3(-0.5f + 0.375f, -0.5f, -0.5f + 0.25f + 1f)));

        #endregion



        return verts;
    }
}
