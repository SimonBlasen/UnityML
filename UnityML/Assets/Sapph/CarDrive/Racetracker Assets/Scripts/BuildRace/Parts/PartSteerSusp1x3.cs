using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSteerSusp1x3 : Part
{
    int amountOfHoles = 3;

    public PartSteerSusp1x3()
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
            return 3;
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
            return 3f;
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
            return true;
        }
    }

    public override float Health
    {
        get
        {
            return 3f;
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

        float fac = 0.125f * 0.5f;
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -8f, -40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 8f, -40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -8f, -40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -8f, -40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 8f, -40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 8f, -40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -8f, -40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 8f, -40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -4f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 4f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -4f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 4f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -4f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -4f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -4f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -4f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -2f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -2f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -4f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 2f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 3f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 4f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 2f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 3f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 4f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 4f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -4f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -4f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -4f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -2f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -4f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -4f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -2f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 3f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 2f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 3f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 2f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 4f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 4f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 4f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 3f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 3f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -4f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -4f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 3f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 3f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -4f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -4f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -8f, -40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -8f, -40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -4f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -4f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -3f, 3f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -1f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -2f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -3f, 3f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -1f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -1f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 1f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 1f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 2f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 3f, 3f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 3f, 3f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 1f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 4f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 4f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 1f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 3f, -3f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 4f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 4f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 1f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 3f, -3f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -1f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -3f, -3f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -4f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -4f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -1f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -3f, -3f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -4f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -4f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -3f, -3f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -1f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -3f, -3f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -1f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 1f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 2f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 3f, -3f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 3f, -3f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 1f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 4f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 4f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 1f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 3f, 3f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 4f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 4f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 1f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 1f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 3f, 3f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 2f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -1f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -3f, 3f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -1f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -2f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -4f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -4f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -1f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -3f, 3f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -4f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -4f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -4f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -4f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -4f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -1f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -4f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -1f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -1f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -1f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -1f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -1f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -1f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 1f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -1f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 1f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 1f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 1f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 1f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 1f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 1f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 4f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 1f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 4f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 4f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 4f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 4f, 1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 4f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 4f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 1f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 4f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 1f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 1f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 1f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -1f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -1f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -1f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -1f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, -4f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -1f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, -4f, -1f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 8f, -40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 8f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 8f, -40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 8f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 8f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 8f, 0f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 8f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 8f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 8f, 0f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 8f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 8f, -40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 8f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 8f, 0f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 8f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 8f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 8f, 0f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(8f, 8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 8f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 8f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 8f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 24f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 24f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 8f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 8f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 24f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 24f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 8f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 8f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 24f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 24f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 8f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 8f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 24f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 24f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 8f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 8f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 24f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 24f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 8f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 24f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 24f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 24f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 24f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 8f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 8f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 24f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 24f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 24f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 24f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 24f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 24f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 24f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 24f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 24f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 24f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 24f, 4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 24f, 2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 24f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 24f, -2f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 8f, -40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 8f, -4f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-8f, 8f, -36f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 8f, -2f)) * fac));


        return verts;
    }

    private Vector2 uvPos = new Vector2(1, 4);
    private Vector2 orange = new Vector2(1, 7);

    protected override List<Vector2> GenerateUVs(PartDirection direction, PartRotation rotation)
    {
        List<Vector2> uvs = new List<Vector2>();
        float textureUnit = 0.0625f;

        for (int i = 0; i < GenerateVertices(direction, rotation, 0, 0, 0).Count / 4; i++)
        {
            uvs.Add(new Vector2(textureUnit * (orange.x), textureUnit * (orange.y)));
            uvs.Add(new Vector2(textureUnit * (orange.x), textureUnit * (orange.y + 1)));
            uvs.Add(new Vector2(textureUnit * (orange.x + 1), textureUnit * (orange.y)));
            uvs.Add(new Vector2(textureUnit * (orange.x + 1), textureUnit * (orange.y + 1)));
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
        Connectionpoint[] connectionPoints = new Connectionpoint[4];

        Vector3 directionToGoIn = direction.ToVector3();
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

        connectionPoints[0] = new ConnectorCrossHole(firstHoleDirection, Vector3.zero);
        connectionPoints[1] = new ConnectorCrossHole(secondHoleDirection.Opposite(), -2f * directionToGoIn);
        connectionPoints[2] = new ConnectorCrossPin(secondHoleDirection.Opposite(), secondHoleDirection.Opposite().ToVector3());
        connectionPoints[3] = new ConnectorCrossPin(secondHoleDirection, secondHoleDirection.ToVector3());

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
