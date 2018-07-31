using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartWeaponMinigun : Part
{
    public PartWeaponMinigun()
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
            return 70f;
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
            return 40f;
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

        float fac = 0.125f * 0.5f;
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -24f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -24f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -24f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -24f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, -8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, -8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, -12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, -8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, -12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, -8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, -12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, -12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, 12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, 12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, 8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, 12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, 8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 24f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, 8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, 12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, 8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 24f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 24f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 24f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, 12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, 20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, 20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, 12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, 12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, 20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, -20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, -12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, -12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, -20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, -20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, -12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, -20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, -12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, -12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, -12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, -12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, -20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, -20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, -20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, -12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, -12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, -12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, -12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, 12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, 12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, 20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, 20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, 12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(18f, 12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, 12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, 12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, 12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, 12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, 20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-18f, 20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 20f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 18f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 18f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 14f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 14f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 12f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 12f, 8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -24f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -24f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, -20f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, -20f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, -8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, -8f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, -8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, -8f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, 8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, 8f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, 8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, 8f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, 20f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, 20f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 24f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 24f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 24f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 24f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 24f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 24f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, 20f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 24f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 24f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, 8f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, 8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, 20f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, 20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, 8f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, 8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, -8f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, -8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -4f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, -20f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, -8f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, -8f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -24f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -24f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, -20f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, -20f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -24f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -24f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -24f, -8f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -24f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, 20f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, 20f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 24f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 24f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, 8f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, 8f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, 20f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, 20f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, 8f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, 8f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, -8f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, -8f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, -20f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, -20f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, -8f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, -8f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(20f, -24f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-20f, -24f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(24f, -20f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-24f, -20f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -10f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -12f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -6f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -12f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -6f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -12f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -10f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -10f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -6f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 6f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 10f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 10f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 12f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 6f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 12f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 6f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 12f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 10f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -2f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 2f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 2f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, 2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 2f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -2f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 2f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -2f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(6f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(10f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -2f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(12f, -2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 6f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 6f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 10f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 10f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 10f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 10f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 12f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 12f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 12f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 12f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 12f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 12f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 10f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 10f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 12f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 12f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 6f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 6f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 10f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 10f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 6f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 6f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 6f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 6f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -2f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 2f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 2f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 2f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, 4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, 4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -2f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, 2f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-12f, -2f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-10f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-6f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -2f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -2f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -10f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -10f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -6f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -6f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -6f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -6f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -6f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -6f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -4f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -4f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -10f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -10f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -6f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -6f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -12f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -12f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -10f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -10f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -12f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-2f, -12f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -12f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -12f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -12f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(2f, -12f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -10f, 40f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -10f, 104f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 4f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -4f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 14f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 14f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -4f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -14f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 14f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 4f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -14f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -14f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 4f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -4f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -14f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -14f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 4f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -4f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -4f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -14f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 14f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 4f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 4f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -4f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 14f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 14f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -4f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -4f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 4f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 4f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 4f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 4f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 14f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 14f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 14f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 14f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 14f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 14f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 4f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 4f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 14f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 14f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -4f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -4f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 4f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 4f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -14f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -14f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -4f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -4f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -14f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -14f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -14f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -14f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -14f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -14f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -4f, 46f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -4f, 50f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -14f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -14f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 4f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -4f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -4f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -14f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 14f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 4f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 4f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -4f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 14f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 14f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -14f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -14f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 4f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -4f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -4f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -14f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 14f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 4f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 4f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -4f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 14f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 14f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -4f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -4f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 4f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 4f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 4f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, 4f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 14f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 14f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 14f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, 14f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 14f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 14f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 4f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 4f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 14f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, 14f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -4f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -4f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 4f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, 4f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -14f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -14f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -4f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-14f, -4f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -14f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4f, -14f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -14f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -14f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -14f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4f, -14f, 98f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -4f, 94f)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(14f, -4f, 98f)) * fac));



        return verts;
    }

    private Vector2 uvPos = new Vector2(7, 1);
    private Vector2 uv_Barrols = new Vector2(10, 4);
    private Vector2 uv_Binders = new Vector2(4, 7);

    protected override List<Vector2> GenerateUVs(PartDirection direction, PartRotation rotation)
    {
        List<Vector2> uvs = new List<Vector2>();
        float textureUnit = 0.0625f;

        for (int i = 0; i < GenerateVertices(direction, rotation, 0, 0, 0).Count / 4; i++)
        {
            if (i < (360 / 4))
            {
                uvs.Add(new Vector2(textureUnit * (uvPos.x), textureUnit * (uvPos.y)));
                uvs.Add(new Vector2(textureUnit * (uvPos.x), textureUnit * (uvPos.y + 1)));
                uvs.Add(new Vector2(textureUnit * (uvPos.x + 1), textureUnit * (uvPos.y)));
                uvs.Add(new Vector2(textureUnit * (uvPos.x + 1), textureUnit * (uvPos.y + 1)));
            }
            else if (i < (536 / 4))
            {
                uvs.Add(new Vector2(textureUnit * (uv_Barrols.x), textureUnit * (uv_Barrols.y)));
                uvs.Add(new Vector2(textureUnit * (uv_Barrols.x), textureUnit * (uv_Barrols.y + 1)));
                uvs.Add(new Vector2(textureUnit * (uv_Barrols.x + 1), textureUnit * (uv_Barrols.y)));
                uvs.Add(new Vector2(textureUnit * (uv_Barrols.x + 1), textureUnit * (uv_Barrols.y + 1)));
            }
            else
            {
                uvs.Add(new Vector2(textureUnit * (uv_Binders.x), textureUnit * (uv_Binders.y)));
                uvs.Add(new Vector2(textureUnit * (uv_Binders.x), textureUnit * (uv_Binders.y + 1)));
                uvs.Add(new Vector2(textureUnit * (uv_Binders.x + 1), textureUnit * (uv_Binders.y)));
                uvs.Add(new Vector2(textureUnit * (uv_Binders.x + 1), textureUnit * (uv_Binders.y + 1)));
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
        Connectionpoint[] connectionPoints = new Connectionpoint[31];

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

        connectionPoints[0] = new ConnectorRoundHole(direction.Opposite(), firstHoleDirection.ToVector3() + secondHoleDirection.ToVector3());
        connectionPoints[1] = new ConnectorRoundHole(direction.Opposite(), firstHoleDirection.ToVector3() - secondHoleDirection.ToVector3());
        connectionPoints[2] = new ConnectorRoundHole(direction.Opposite(), -firstHoleDirection.ToVector3() + secondHoleDirection.ToVector3());
        connectionPoints[3] = new ConnectorRoundHole(direction.Opposite(), -firstHoleDirection.ToVector3() - secondHoleDirection.ToVector3());

        connectionPoints[4] = new ConnectorSolid(direction, Vector3.zero);
        connectionPoints[5] = new ConnectorSolid(direction, firstHoleDirection.ToVector3());
        connectionPoints[6] = new ConnectorSolid(direction, -firstHoleDirection.ToVector3());
        connectionPoints[7] = new ConnectorSolid(direction, secondHoleDirection.ToVector3());
        connectionPoints[8] = new ConnectorSolid(direction, -secondHoleDirection.ToVector3());

        connectionPoints[9] = new ConnectorSolid(direction, directionToGoIn + firstHoleDirection.ToVector3() + secondHoleDirection.ToVector3());
        connectionPoints[10] = new ConnectorSolid(direction, directionToGoIn + firstHoleDirection.ToVector3() - secondHoleDirection.ToVector3());
        connectionPoints[11] = new ConnectorSolid(direction, directionToGoIn - firstHoleDirection.ToVector3() + secondHoleDirection.ToVector3());
        connectionPoints[12] = new ConnectorSolid(direction, directionToGoIn - firstHoleDirection.ToVector3() - secondHoleDirection.ToVector3());
        connectionPoints[13] = new ConnectorSolid(direction, directionToGoIn + Vector3.zero);
        connectionPoints[14] = new ConnectorSolid(direction, directionToGoIn + firstHoleDirection.ToVector3());
        connectionPoints[15] = new ConnectorSolid(direction, directionToGoIn - firstHoleDirection.ToVector3());
        connectionPoints[16] = new ConnectorSolid(direction, directionToGoIn + secondHoleDirection.ToVector3());
        connectionPoints[17] = new ConnectorSolid(direction, directionToGoIn - secondHoleDirection.ToVector3());

        connectionPoints[18] = new ConnectorSolid(direction, 2*directionToGoIn + firstHoleDirection.ToVector3() + secondHoleDirection.ToVector3());
        connectionPoints[19] = new ConnectorSolid(direction, 2*directionToGoIn + firstHoleDirection.ToVector3() - secondHoleDirection.ToVector3());
        connectionPoints[20] = new ConnectorSolid(direction, 2*directionToGoIn - firstHoleDirection.ToVector3() + secondHoleDirection.ToVector3());
        connectionPoints[21] = new ConnectorSolid(direction, 2*directionToGoIn - firstHoleDirection.ToVector3() - secondHoleDirection.ToVector3());
        connectionPoints[22] = new ConnectorSolid(direction, 2*directionToGoIn + Vector3.zero);
        connectionPoints[23] = new ConnectorSolid(direction, 2*directionToGoIn + firstHoleDirection.ToVector3());
        connectionPoints[24] = new ConnectorSolid(direction, 2*directionToGoIn - firstHoleDirection.ToVector3());
        connectionPoints[25] = new ConnectorSolid(direction, 2*directionToGoIn + secondHoleDirection.ToVector3());
        connectionPoints[26] = new ConnectorSolid(direction, 2*directionToGoIn - secondHoleDirection.ToVector3());

        connectionPoints[27] = new ConnectorSolid(direction, 3 * directionToGoIn + Vector3.zero);

        connectionPoints[28] = new ConnectorSolid(direction, 4 * directionToGoIn + Vector3.zero);

        connectionPoints[29] = new ConnectorSolid(direction, 5 * directionToGoIn + Vector3.zero);

        connectionPoints[30] = new ConnectorSolid(direction, 6 * directionToGoIn + Vector3.zero);

        return connectionPoints;
    }
}
