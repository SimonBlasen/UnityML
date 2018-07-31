using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSeat : Part
{
    int amountOfHoles = 3;

    public PartSeat()
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
            return 100f;
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


        #region The both ends

        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f))));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f))));                   // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f))));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f))));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f))));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f))));                   // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f))));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f))));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 1, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 1, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));               // 0


        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f, posZ - (((float)amountOfHoles) * 0.5f) + (((float)amountOfHoles) - 0.25f))));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f, posZ - (((float)amountOfHoles) * 0.5f) + (((float)amountOfHoles) - 0.25f))));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + (((float)amountOfHoles)))));               // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + (((float)amountOfHoles)))));                   // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + (((float)amountOfHoles)))));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + (((float)amountOfHoles)))));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + (((float)amountOfHoles)))));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + (((float)amountOfHoles)))));                   // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + (((float)amountOfHoles)))));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + (((float)amountOfHoles)))));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 1, posZ - (((float)amountOfHoles) * 0.5f) + (((float)amountOfHoles) - 0.25f))));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 1, posZ - (((float)amountOfHoles) * 0.5f) + (((float)amountOfHoles) - 0.25f))));               // 0


        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 1, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 1, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 1, posZ - (((float)amountOfHoles) * 0.5f) + (((float)amountOfHoles) - 0.25f))));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 1, posZ - (((float)amountOfHoles) * 0.5f) + (((float)amountOfHoles) - 0.25f))));               // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f, posZ - (((float)amountOfHoles) * 0.5f) + (((float)amountOfHoles) - 0.25f))));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f, posZ - (((float)amountOfHoles) * 0.5f) + (((float)amountOfHoles) - 0.25f))));               // 0

        #endregion





        #region On the X + 1 Side

        // The left 3

        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f))));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + 0.375f)));               // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f))));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f))));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));               // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f))));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 1, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + 0.375f)));               // 0


        // The right 3

        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.375f)));               // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.25f)));               // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles)));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.375f)));               // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 1, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles)));                       // 0



        // The top bar
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + 0.375f)));               // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.375f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 1, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 1, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.25f)));                       // 0

        // The down bar
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));               // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + 0.375f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.375f)));                       // 0


        // The mid connector parts
        for (int i = 0; i < amountOfHoles - 1; i++)
        {
            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.625f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + i + 1.375f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.75f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + i + 1.25f)));

            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.75f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + i + 1.25f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.75f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + i + 1.25f)));

            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.75f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + i + 1.25f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.625f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + i + 1.375f)));
        }

        #endregion

        #region On the X Side

        // The left 3

        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + 0.375f)));               // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f))));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));               // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f))));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f))));                       // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + 0.375f)));               // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 1, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f))));                       // 0


        // The right 3

        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.375f)));               // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.25f)));               // 0

        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 1, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.375f)));               // 0



        // The top bar
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 1, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.25f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.375f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 1, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + 0.375f)));               // 0

        // The down bar
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.375f)));                       // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f, posZ - (((float)amountOfHoles) * 0.5f) + amountOfHoles - 0.25f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + 0.375f)));                   // 0
        verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f, posZ - (((float)amountOfHoles) * 0.5f) + 0.25f)));               // 0


        // The mid connector parts
        for (int i = 0; i < amountOfHoles - 1; i++)
        {
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + i + 1.25f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + i + 1.375f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.75f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.625f)));

            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + i + 1.25f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + i + 1.25f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.75f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.75f)));

            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + i + 1.375f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + i + 1.25f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.625f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.75f)));
        }

        #endregion

        // The mid parts

        for (int i = 0; i < amountOfHoles; i++)
        {
            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.25f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.25f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.25f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.25f)));

            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.25f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.25f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.375f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.375f)));

            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.375f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.375f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.625f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.625f)));

            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.625f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.75f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.625f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.75f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.75f)));

            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.75f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.625f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.75f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.75f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.75f)));

            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.75f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.75f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.625f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.625f)));

            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.625f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.625f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.375f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.375f)));

            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.375f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.25f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.375f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX + 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.25f)));
            verts.Add(rotAxis * rotQuat * (new Vector3(posX - 0.5f, posY - 0.5f + 0.375f, posZ - (((float)amountOfHoles) * 0.5f) + i + 0.25f)));
        }

        return verts;
    }

    private Vector2 uvPos = new Vector2(10, 1);

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
        Connectionpoint[] connectionPoints = new Connectionpoint[3];

        Vector3 directionToGoIn = direction.ToVector3();
        PartDirection holesLookDirection = PartDirection.Down;
        switch (direction)
        {
            case PartDirection.Down:
                switch (rotation)
                {
                    case PartRotation.Down:
                        holesLookDirection = PartDirection.West;
                        break;
                    case PartRotation.Left:
                        holesLookDirection = PartDirection.North;
                        break;
                    case PartRotation.Up:
                        holesLookDirection = PartDirection.East;
                        break;
                    case PartRotation.Right:
                        holesLookDirection = PartDirection.South;
                        break;
                }
                break;
            case PartDirection.Up:
                switch (rotation)
                {
                    case PartRotation.Down:
                        holesLookDirection = PartDirection.West;
                        break;
                    case PartRotation.Left:
                        holesLookDirection = PartDirection.South;
                        break;
                    case PartRotation.Up:
                        holesLookDirection = PartDirection.East;
                        break;
                    case PartRotation.Right:
                        holesLookDirection = PartDirection.North;
                        break;
                }
                break;
            case PartDirection.East:
                switch (rotation)
                {
                    case PartRotation.Down:
                        holesLookDirection = PartDirection.North;
                        break;
                    case PartRotation.Left:
                        holesLookDirection = PartDirection.Up;
                        break;
                    case PartRotation.Up:
                        holesLookDirection = PartDirection.South;
                        break;
                    case PartRotation.Right:
                        holesLookDirection = PartDirection.Down;
                        break;
                }
                break;
            case PartDirection.South:
                switch (rotation)
                {
                    case PartRotation.Down:
                        holesLookDirection = PartDirection.East;
                        break;
                    case PartRotation.Left:
                        holesLookDirection = PartDirection.Up;
                        break;
                    case PartRotation.Up:
                        holesLookDirection = PartDirection.West;
                        break;
                    case PartRotation.Right:
                        holesLookDirection = PartDirection.Down;
                        break;
                }
                break;
            case PartDirection.West:
                switch (rotation)
                {
                    case PartRotation.Down:
                        holesLookDirection = PartDirection.South;
                        break;
                    case PartRotation.Left:
                        holesLookDirection = PartDirection.Up;
                        break;
                    case PartRotation.Up:
                        holesLookDirection = PartDirection.North;
                        break;
                    case PartRotation.Right:
                        holesLookDirection = PartDirection.Down;
                        break;
                }
                break;
            case PartDirection.North:
                switch (rotation)
                {
                    case PartRotation.Down:
                        holesLookDirection = PartDirection.West;
                        break;
                    case PartRotation.Left:
                        holesLookDirection = PartDirection.Up;
                        break;
                    case PartRotation.Up:
                        holesLookDirection = PartDirection.East;
                        break;
                    case PartRotation.Right:
                        holesLookDirection = PartDirection.Down;
                        break;
                }
                break;
        }

        for (int i = 0; i < connectionPoints.Length; i++)
        {
            connectionPoints[i] = new ConnectorRoundHole(holesLookDirection, new Vector3(directionToGoIn.x * (i - (((float)amountOfHoles) * 0.5f) + 0.5f), directionToGoIn.y * (i - (((float)amountOfHoles) * 0.5f) + 0.5f), directionToGoIn.z * (i - (((float)amountOfHoles) * 0.5f) + 0.5f)));
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
}
