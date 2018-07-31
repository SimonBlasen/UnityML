using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartCorner3x5 : Part
{
    int amountOfHoles = 5;

    private Vector2 uvPos = new Vector2(1, 4);

    public PartCorner3x5()
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
            return 7f;
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
            return 7f;
        }
    }

    protected override List<int> GenerateTriangles(PartDirection direction, PartRotation rotation, int faceCount)
    {
        List<int> trias = new List<int>();
        //Front side
        //trias.Add(faceCount);
        //trias.Add(faceCount + 2);
        //trias.Add(faceCount + 3);
        //
        //trias.Add(faceCount);
        //trias.Add(faceCount + 3);
        //trias.Add(faceCount + 1);
        //
        ////Left side
        //trias.Add(faceCount + 4);
        //trias.Add(faceCount + 6);
        //trias.Add(faceCount + 2);
        //
        //trias.Add(faceCount + 4);
        //trias.Add(faceCount + 2);
        //trias.Add(faceCount + 0);
        //
        ////Up side
        //trias.Add(faceCount + 6);
        //trias.Add(faceCount + 7);
        //trias.Add(faceCount + 3);
        //
        //trias.Add(faceCount + 6);
        //trias.Add(faceCount + 3);
        //trias.Add(faceCount + 2);
        //
        ////Down side
        //trias.Add(faceCount + 5);
        //trias.Add(faceCount + 4);
        //trias.Add(faceCount + 1);
        //
        //trias.Add(faceCount + 4);
        //trias.Add(faceCount + 0);
        //trias.Add(faceCount + 1);
        //
        ////Right side
        //trias.Add(faceCount + 1);
        //trias.Add(faceCount + 3);
        //trias.Add(faceCount + 7);
        //
        //trias.Add(faceCount + 1);
        //trias.Add(faceCount + 7);
        //trias.Add(faceCount + 5);
        //
        ////Back side
        //trias.Add(faceCount + 5);
        //trias.Add(faceCount + 7);
        //trias.Add(faceCount + 6);
        //
        //trias.Add(faceCount + 5);
        //trias.Add(faceCount + 6);
        //trias.Add(faceCount + 4);

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

        #region The front side

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -4, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, -4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, -2)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, -4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 18, -4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 17, -2)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 18, -4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 17, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 20, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 18, -1)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 18, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 18, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 20, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 20, 2)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 18, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 17, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 20, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 18, 4)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 4, 4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 17, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 18, 4)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 2, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 7, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 6, -1)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 2, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 2, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 6, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 6, 1)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 2, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 6, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 7, 2)) * fac));


        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 9, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 10, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 15, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 14, -1)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 10, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 10, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 14, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 14, 1)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 10, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 9, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 14, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 15, 2)) * fac));


        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -4, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -4, 34)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, 33)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, 7)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, 6)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, 6)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 6)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 6)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 4, 4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 2, 7)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 2, 7)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 2, 33)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 4, 4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 4, 34)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, 9)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, 15)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, 10)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, 14)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, 10)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, 14)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 10)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 14)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 10)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 14)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 2, 9)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 2, 15)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, 17)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, 23)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, 18)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, 22)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, 18)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, 22)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 18)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 22)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 18)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 22)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 2, 17)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 2, 23)) * fac));


        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, 25)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, 31)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, 26)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, 30)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, 26)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, 30)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 26)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 30)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 26)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 30)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 2, 25)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 2, 31)) * fac));


        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -4, 34)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, 36)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, 33)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, 34)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -1, 34)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, 36)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 34)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 2, 36)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 2, 33)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 1, 34)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 4, 34)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 2, 36)) * fac));

        #endregion



        #region The back side

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -4, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, -4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, -2)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, -4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 18, -4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 17, -2)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 18, -4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 20, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 17, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 18, -1)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 18, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 20, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 18, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 20, 2)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 18, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 20, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 17, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 18, 4)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 17, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 4, 4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 18, 4)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 7, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 6, -1)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 6, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 6, 1)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 6, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 7, 2)) * fac));


        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 9, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 15, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 10, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 14, -1)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 10, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 14, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 10, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 14, 1)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 10, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 14, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 9, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 15, 2)) * fac));


        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -4, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -4, 34)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 33)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 7)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 6)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 6)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 6)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 4, 4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 6)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 7)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 7)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 4, 4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 33)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 4, 34)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 9)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 10)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 15)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 14)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 10)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 10)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 14)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 14)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 10)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 9)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 14)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 15)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 17)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 18)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 23)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 22)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 18)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 18)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 22)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 22)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 18)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 17)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 22)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 23)) * fac));


        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 25)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 26)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 31)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 30)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 26)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 26)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 30)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 30)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 26)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 25)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 30)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 31)) * fac));


        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -4, 34)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 33)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 36)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 34)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 34)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 34)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 36)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 36)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 33)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 4, 34)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 34)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 36)) * fac));
        /*verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -4, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, -4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, -2)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, -4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 10, -4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 9, -2)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 10, -4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 12, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 9, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 10, -1)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 10, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 12, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 10, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 12, 2)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 10, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 12, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 9, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 10, 4)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 9, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 4, 4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 10, 4)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 7, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 6, -1)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 6, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 6, 1)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 6, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 7, 2)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -4, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, -1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -4, 26)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 25)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 7)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 6)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 6)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 6)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 4, 4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 6)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 7)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 7)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 4, 4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 25)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 4, 26)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 9)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 10)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 15)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 14)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 10)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 10)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 14)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 14)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 10)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 9)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 14)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 15)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 17)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 18)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 23)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 22)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 18)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 18)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 22)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 22)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 18)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 17)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 22)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 23)) * fac));


        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -4, 26)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 25)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 28)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 26)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -1, 26)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 26)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 28)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 28)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 25)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 4, 26)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 1, 26)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 28)) * fac));*/

        #endregion



        #region Inner holes

        //Vector2 holePos = new Vector2(0, 0);
        //verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y - 2, holePos.x - 1)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y - 2, holePos.x - 1)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y - 1, holePos.x - 2)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y - 1, holePos.x - 2)) * fac));
        //
        //verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y - 1, holePos.x - 2)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y - 1, holePos.x - 2)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y + 1, holePos.x - 2)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y + 1, holePos.x - 2)) * fac));
        //
        //verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y + 1, holePos.x - 2)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y + 1, holePos.x - 2)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y + 2, holePos.x - 1)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y + 2, holePos.x - 1)) * fac));
        //
        //verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y + 2, holePos.x - 1)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y + 2, holePos.x - 1)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y + 2, holePos.x + 1)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y + 2, holePos.x + 1)) * fac));
        //
        //verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y + 2, holePos.x + 1)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y + 2, holePos.x + 1)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y + 1, holePos.x + 2)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y + 1, holePos.x + 2)) * fac));
        //
        //verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y + 1, holePos.x + 2)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y + 1, holePos.x + 2)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y - 1, holePos.x + 2)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y - 1, holePos.x + 2)) * fac));
        //
        //verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y - 1, holePos.x + 2)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y - 1, holePos.x + 2)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y - 2, holePos.x + 1)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y - 2, holePos.x + 1)) * fac));
        //
        //verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y - 2, holePos.x + 1)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y - 2, holePos.x + 1)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y - 2, holePos.x - 1)) * fac));
        //verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y - 2, holePos.x - 1)) * fac));

        PartVerticesHelpMethods.InnerHoles(verts, rotAxis, rotQuat, fac, new Vector2(0, 0));
        PartVerticesHelpMethods.InnerHoles(verts, rotAxis, rotQuat, fac, new Vector2(0, 8));
        PartVerticesHelpMethods.InnerHoles(verts, rotAxis, rotQuat, fac, new Vector2(0, 16));
        PartVerticesHelpMethods.InnerHoles(verts, rotAxis, rotQuat, fac, new Vector2(8, 0));
        PartVerticesHelpMethods.InnerHoles(verts, rotAxis, rotQuat, fac, new Vector2(16, 0));
        PartVerticesHelpMethods.InnerHoles(verts, rotAxis, rotQuat, fac, new Vector2(24, 0));
        PartVerticesHelpMethods.InnerHoles(verts, rotAxis, rotQuat, fac, new Vector2(32, 0));

        #endregion



        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -4, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -4, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, -4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, -4)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, -4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, -4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 18, -4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 18, -4)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 18, -4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 18, -4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 20, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 20, -2)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 20, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 20, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 20, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 20, 2)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 20, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 20, 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 18, 4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 18, 4)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 18, 4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 18, 4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 4, 4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 4, 4)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 4, 4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 4, 4)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 4, 34)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 4, 34)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 4, 34)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 4, 34)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 36)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 2, 36)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, 2, 36)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, 2, 36)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 36)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, 36)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -2, 36)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -2, 36)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -4, 34)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -4, 34)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -4, 34)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -4, 34)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, -4, -2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, -4, -2)) * fac));



        return verts;
    }

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
        Connectionpoint[] connectionPoints = new Connectionpoint[7];

        Vector3 directionToGoIn = direction.ToVector3();
        Vector3 directionUp = direction.ToVector3();
        PartDirection holesLookDirection = PartDirection.Down;
        switch (direction)
        {
            case PartDirection.Down:
                switch (rotation)
                {
                    case PartRotation.Down:
                        holesLookDirection = PartDirection.West;
                        directionUp = new Vector3(0, 0, -1);
                        break;
                    case PartRotation.Left:
                        holesLookDirection = PartDirection.North;
                        directionUp = new Vector3(-1, 0, 0);
                        break;
                    case PartRotation.Up:
                        holesLookDirection = PartDirection.East;
                        directionUp = new Vector3(0, 0, 1);
                        break;
                    case PartRotation.Right:
                        holesLookDirection = PartDirection.South;
                        directionUp = new Vector3(1, 0, 0);
                        break;
                }
                break;
            case PartDirection.Up:
                switch (rotation)
                {
                    case PartRotation.Down:
                        holesLookDirection = PartDirection.West;
                        directionUp = new Vector3(0, 0, 1);
                        break;
                    case PartRotation.Left:
                        holesLookDirection = PartDirection.South;
                        directionUp = new Vector3(-1, 0, 0);
                        break;
                    case PartRotation.Up:
                        holesLookDirection = PartDirection.East;
                        directionUp = new Vector3(0, 0, -1);
                        break;
                    case PartRotation.Right:
                        holesLookDirection = PartDirection.North;
                        directionUp = new Vector3(1, 0, 0);
                        break;
                }
                break;
            case PartDirection.East:
                switch (rotation)
                {
                    case PartRotation.Down:
                        holesLookDirection = PartDirection.North;
                        directionUp = new Vector3(0, -1, 0);
                        break;
                    case PartRotation.Left:
                        holesLookDirection = PartDirection.Up;
                        directionUp = new Vector3(0, 0, 1);
                        break;
                    case PartRotation.Up:
                        holesLookDirection = PartDirection.South;
                        directionUp = new Vector3(0, 1, 0);
                        break;
                    case PartRotation.Right:
                        holesLookDirection = PartDirection.Down;
                        directionUp = new Vector3(0, 0, -1);
                        break;
                }
                break;
            case PartDirection.South:
                switch (rotation)
                {
                    case PartRotation.Down:
                        holesLookDirection = PartDirection.East;
                        directionUp = new Vector3(0, -1, 0);
                        break;
                    case PartRotation.Left:
                        holesLookDirection = PartDirection.Up;
                        directionUp = new Vector3(1, 0, 0);
                        break;
                    case PartRotation.Up:
                        holesLookDirection = PartDirection.West;
                        directionUp = new Vector3(0, 1, 0);
                        break;
                    case PartRotation.Right:
                        holesLookDirection = PartDirection.Down;
                        directionUp = new Vector3(-1, 0, 0);
                        break;
                }
                break;
            case PartDirection.West:
                switch (rotation)
                {
                    case PartRotation.Down:
                        holesLookDirection = PartDirection.South;
                        directionUp = new Vector3(0, -1, 0);
                        break;
                    case PartRotation.Left:
                        holesLookDirection = PartDirection.Up;
                        directionUp = new Vector3(0, 0, -1);
                        break;
                    case PartRotation.Up:
                        holesLookDirection = PartDirection.North;
                        directionUp = new Vector3(0, 1, 0);
                        break;
                    case PartRotation.Right:
                        holesLookDirection = PartDirection.Down;
                        directionUp = new Vector3(0, 0, 1);
                        break;
                }
                break;
            case PartDirection.North:
                switch (rotation)
                {
                    case PartRotation.Down:
                        holesLookDirection = PartDirection.West;
                        directionUp = new Vector3(0, -1, 0);
                        break;
                    case PartRotation.Left:
                        holesLookDirection = PartDirection.Up;
                        directionUp = new Vector3(-1, 0, 0);
                        break;
                    case PartRotation.Up:
                        holesLookDirection = PartDirection.East;
                        directionUp = new Vector3(0, 1, 0);
                        break;
                    case PartRotation.Right:
                        holesLookDirection = PartDirection.Down;
                        directionUp = new Vector3(1, 0, 0);
                        break;
                }
                break;
        }

        connectionPoints[0] = new ConnectorRoundHole(holesLookDirection, directionUp * 2);
        connectionPoints[1] = new ConnectorRoundHole(holesLookDirection, directionUp);
        connectionPoints[2] = new ConnectorRoundHole(holesLookDirection, Vector3.zero);
        connectionPoints[3] = new ConnectorRoundHole(holesLookDirection, directionToGoIn);
        connectionPoints[4] = new ConnectorRoundHole(holesLookDirection, directionToGoIn * 2);
        connectionPoints[5] = new ConnectorRoundHole(holesLookDirection, directionToGoIn * 3);
        connectionPoints[6] = new ConnectorRoundHole(holesLookDirection, directionToGoIn * 4);


        return connectionPoints;
    }
}
