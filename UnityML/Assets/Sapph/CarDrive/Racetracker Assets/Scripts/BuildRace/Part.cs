using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PartDirection
{
    North = 0, East = 1, South = 2, West = 3, Up = 4, Down = 5
}

public enum PartRotation
{
    Up = 0, Right = 1, Down = 2, Left = 3
}

public enum PartType
{
    NONE = 0, Part1x1x5 = 1, PartPinRound2x1 = 2, Part1x1x3 = 3, Part1x1x7 = 4, Part1x1x9 = 5, Part1x1x11 = 6, Part1x1x13 = 7, Part1x1x15 = 8, PartTurn1x1x2 = 9, PartPinRound3x1 = 10, PartCorner2x4 = 11, PartCorner3x5 = 12, PartTurn1x3x3 = 13, 
    PartAxis1x2 = 14, PartAxis1x3 = 15, PartAxis1x4 = 16, PartAxis1x5 = 17, PartAxis1x6 = 18, PartAxis1x7 = 19, PartAxis1x8 = 20, PartGear8 = 21, PartGear24 = 22, PartMotorElectro1 = 23, PartCrossExtend = 24,
    PartSteer1Left = 25, PartWheelStreet1 = 26, PartSeat = 27, PartGearCorner4 = 28, PartMotorElectro2 = 29, PartMotorElectro3 = 30, PartWheelStreet2 = 31, PartWheelStreet3 = 32, PartWheelStreet4 = 33,
    PartSteerWheel = 34, Part1x1x2 = 35, PartToothBar5 = 36, PartSteerSusp1x3 = 37, PartWeaponMinigun = 38, PartWeaponMinigun_0 = 2048, PartWeaponMinigun_1 = 2049
}

public abstract class Part
{
    protected PartDrawData drawE_U = new PartDrawData();
    protected PartDrawData drawE_R = new PartDrawData();
    protected PartDrawData drawE_D = new PartDrawData();
    protected PartDrawData drawE_L = new PartDrawData();
    protected PartDrawData drawS_U = new PartDrawData();
    protected PartDrawData drawS_R = new PartDrawData();
    protected PartDrawData drawS_D = new PartDrawData();
    protected PartDrawData drawS_L = new PartDrawData();
    protected PartDrawData drawW_U = new PartDrawData();
    protected PartDrawData drawW_R = new PartDrawData();
    protected PartDrawData drawW_D = new PartDrawData();
    protected PartDrawData drawW_L = new PartDrawData();
    protected PartDrawData drawN_U = new PartDrawData();
    protected PartDrawData drawN_R = new PartDrawData();
    protected PartDrawData drawN_D = new PartDrawData();
    protected PartDrawData drawN_L = new PartDrawData();
    protected PartDrawData drawU_U = new PartDrawData();
    protected PartDrawData drawU_R = new PartDrawData();
    protected PartDrawData drawU_D = new PartDrawData();
    protected PartDrawData drawU_L = new PartDrawData();
    protected PartDrawData drawD_U = new PartDrawData();
    protected PartDrawData drawD_R = new PartDrawData();
    protected PartDrawData drawD_D = new PartDrawData();
    protected PartDrawData drawD_L = new PartDrawData();
    protected PartDrawData[,] drawData;

    protected void FillData()
    {
        drawData = new PartDrawData[6, 4];
        for (int dir = 0; dir < 6; dir++)
        {
            for (int rot = 0; rot < 4; rot++)
            {
                drawData[dir, rot] = new PartDrawData();
                drawData[dir, rot].vertices = GenerateVertices((PartDirection)dir, (PartRotation)rot, 0, 0, 0).ToArray();
                drawData[dir, rot].triangles = GenerateTriangles((PartDirection)dir, (PartRotation)rot, 0).ToArray();
                drawData[dir, rot].uvs = GenerateUVs((PartDirection)dir, (PartRotation)rot).ToArray();
            }
        }
    }

    public Vector3[] GetVertices(PartDirection direction, PartRotation rotation, int posX, int posY, int posZ)
    {
        return drawData[(int)direction, (int)rotation].vertices;
    }

    public int[] GetTriangles(PartDirection direction, PartRotation rotation, int faceCount)
    {
        return drawData[(int)direction, (int)rotation].triangles;
    }

    public Vector2[] GetUVs(PartDirection direction, PartRotation rotation)
    {
        return drawData[(int)direction, (int)rotation].uvs;
    }

    protected abstract List<Vector3> GenerateVertices(PartDirection direction, PartRotation rotation, int posX, int posY, int posZ);
    protected abstract List<int> GenerateTriangles(PartDirection direction, PartRotation rotation, int faceCount);
    protected abstract List<Vector2> GenerateUVs(PartDirection direction, PartRotation rotation);
    public abstract Connectionpoint[] GetConnectionpoints(PartDirection direction, PartRotation rotation);

    public abstract bool IsWheel { get; }
    public abstract bool IsMotor { get; }
    public abstract bool IsGear { get; }
    public abstract int AxeLength { get; }
    public abstract bool IsAxe { get; }
    public abstract float GearTooths { get; }
    public abstract float MotorTorque { get; }
    public abstract float MotorTopspeed { get; }
    public abstract float Mass { get; }
    public abstract float Grip { get; }
    public abstract float WheelRadius { get; }
    public abstract bool IsSteerSusp { get; }
    public abstract float Health { get; }


    private static PartPinRound2x1 partPinRound2x1 = new PartPinRound2x1();
    private static PartPinRound3x1 partPinRound3x1 = new PartPinRound3x1();
    private static Part1x1x3 part1x1x3 = new Part1x1x3();
    private static Part1x1x5 part1x1x5 = new Part1x1x5();
    private static Part1x1x7 part1x1x7 = new Part1x1x7();
    private static Part1x1x9 part1x1x9 = new Part1x1x9();
    private static Part1x1x11 part1x1x11 = new Part1x1x11();
    private static Part1x1x13 part1x1x13 = new Part1x1x13();
    private static Part1x1x15 part1x1x15 = new Part1x1x15();
    private static PartCorner2x4 partCorner2x4 = new PartCorner2x4();
    private static PartCorner3x5 partCorner3x5 = new PartCorner3x5();
    private static PartTurn1x1x2 partTurn1x1x2 = new PartTurn1x1x2();
    private static PartTurn1x3x3 partTurn1x3x3 = new PartTurn1x3x3();
    private static PartAxis1x2 partAxis1x2 = new PartAxis1x2();
    private static PartAxis1x3 partAxis1x3 = new PartAxis1x3();
    private static PartAxis1x4 partAxis1x4 = new PartAxis1x4();
    private static PartAxis1x5 partAxis1x5 = new PartAxis1x5();
    private static PartAxis1x6 partAxis1x6 = new PartAxis1x6();
    private static PartAxis1x7 partAxis1x7 = new PartAxis1x7();
    private static PartAxis1x8 partAxis1x8 = new PartAxis1x8();
    private static PartGear8 partGear8 = new PartGear8();
    private static PartGear24 partGear24 = new PartGear24();
    private static PartMotorElectro1 partMotorElectro1 = new PartMotorElectro1();
    private static PartCrossExtend partCrossExtend = new PartCrossExtend();
    private static PartSteer1Left partSteer1Left = new PartSteer1Left();
    private static PartWheelStreet1 partWheelStreet1 = new PartWheelStreet1();
    private static PartSeat partSeat = new PartSeat();
    private static PartWheelStreet2 partWheelStreet2 = new PartWheelStreet2();
    private static PartWheelStreet3 partWheelStreet3 = new PartWheelStreet3();
    private static PartWheelStreet4 partWheelStreet4 = new PartWheelStreet4();
    private static PartMotorElectro2 partMotorElectro2 = new PartMotorElectro2();
    private static PartMotorElectro3 partMotorElectro3 = new PartMotorElectro3();
    private static PartGearCorner4 partGearCorner4 = new PartGearCorner4();
    private static PartSteerWheel partSteerWheel = new PartSteerWheel();
    private static PartToothBar5 partToothBar5 = new PartToothBar5();
    private static Part1x1x2 part1x1x2 = new Part1x1x2();
    private static PartSteerSusp1x3 partSteerSusp1x3 = new PartSteerSusp1x3();
    private static PartWeaponMinigun partWeaponMinigun = new PartWeaponMinigun();
    private static PartWeaponMinigun_0 partWeaponMinigun_0 = new PartWeaponMinigun_0();
    private static PartWeaponMinigun_1 partWeaponMinigun_1 = new PartWeaponMinigun_1();

    //private static PartPinRound2x1 partPinRound2x1;// = new PartPinRound2x1();
    //private static PartPinRound3x1 partPinRound3x1;// = new PartPinRound3x1();
    //private static Part1x1x3 part1x1x3;// = new Part1x1x3();
    //private static Part1x1x5 part1x1x5 = new Part1x1x5();
    //private static Part1x1x7 part1x1x7;// = new Part1x1x7();
    //private static Part1x1x9 part1x1x9;// = new Part1x1x9();
    //private static Part1x1x11 part1x1x11;// = new Part1x1x11();
    //private static Part1x1x13 part1x1x13;// = new Part1x1x13();
    //private static Part1x1x15 part1x1x15;// = new Part1x1x15();
    //private static PartCorner2x4 partCorner2x4;// = new PartCorner2x4();
    //private static PartCorner3x5 partCorner3x5;// = new PartCorner3x5();
    //private static PartTurn1x1x2 partTurn1x1x2;// = new PartTurn1x1x2();

    public static Part MakePart(PartType type)
    {
        switch (type)
        {
            case PartType.Part1x1x5:
                return part1x1x5;
            case PartType.PartPinRound2x1:
                return partPinRound2x1;
            case PartType.Part1x1x3:
                return part1x1x3;
            case PartType.Part1x1x7:
                return part1x1x7;
            case PartType.Part1x1x9:
                return part1x1x9;
            case PartType.Part1x1x11:
                return part1x1x11;
            case PartType.Part1x1x13:
                return part1x1x13;
            case PartType.Part1x1x15:
                return part1x1x15;
            case PartType.PartTurn1x1x2:
                return partTurn1x1x2;
            case PartType.PartPinRound3x1:
                return partPinRound3x1;
            case PartType.PartCorner2x4:
                return partCorner2x4;
            case PartType.PartCorner3x5:
                return partCorner3x5;
            case PartType.PartTurn1x3x3:
                return partTurn1x3x3;
            case PartType.PartAxis1x2:
                return partAxis1x2;
            case PartType.PartAxis1x3:
                return partAxis1x3;
            case PartType.PartAxis1x4:
                return partAxis1x4;
            case PartType.PartAxis1x5:
                return partAxis1x5;
            case PartType.PartAxis1x6:
                return partAxis1x6;
            case PartType.PartAxis1x7:
                return partAxis1x7;
            case PartType.PartAxis1x8:
                return partAxis1x8;
            case PartType.PartGear8:
                return partGear8;
            case PartType.PartGear24:
                return partGear24;
            case PartType.PartMotorElectro1:
                return partMotorElectro1;
            case PartType.PartCrossExtend:
                return partCrossExtend;
            case PartType.PartSteer1Left:
                return partSteer1Left;
            case PartType.PartWheelStreet1:
                return partWheelStreet1;
            case PartType.PartSeat:
                return partSeat;
            case PartType.PartWheelStreet2:
                return partWheelStreet2;
            case PartType.PartWheelStreet3:
                return partWheelStreet3;
            case PartType.PartWheelStreet4:
                return partWheelStreet4;
            case PartType.PartMotorElectro2:
                return partMotorElectro2;
            case PartType.PartMotorElectro3:
                return partMotorElectro3;
            case PartType.PartGearCorner4:
                return partGearCorner4;
            case PartType.PartSteerWheel:
                return partSteerWheel;
            case PartType.Part1x1x2:
                return part1x1x2;
            case PartType.PartToothBar5:
                return partToothBar5;
            case PartType.PartSteerSusp1x3:
                return partSteerSusp1x3;
            case PartType.PartWeaponMinigun:
                return partWeaponMinigun;
            case PartType.PartWeaponMinigun_0:
                return partWeaponMinigun_0;
            case PartType.PartWeaponMinigun_1:
                return partWeaponMinigun_1;
            default:
                return part1x1x5;
        }
    }
}

public class PartDrawData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;
}

public static class PartDirectionMethods
{
    /// <summary>
    /// Returns the direction that points in the opposite direction
    /// </summary>
    /// <param name="pD">The direction the oposite is requested</param>
    /// <returns>The oposite direction</returns>
    public static PartDirection Opposite(this PartDirection pD)
    {
        switch (pD)
        {
            case PartDirection.Down:
                return PartDirection.Up;
            case PartDirection.Up:
                return PartDirection.Down;
            case PartDirection.East:
                return PartDirection.West;
            case PartDirection.West:
                return PartDirection.East;
            case PartDirection.North:
                return PartDirection.South;
            case PartDirection.South:
                return PartDirection.North;
            default:
                return PartDirection.East;
        }
    }

    // Right hand rule
    public static PartDirection Cross(this PartDirection pD, PartDirection pD2)
    {
        switch (pD)
        {
            case PartDirection.Down:
                switch (pD2)
                {
                    case PartDirection.Down:
                        return PartDirection.Down;
                    case PartDirection.Up:
                        return PartDirection.Down;
                    case PartDirection.East:
                        return PartDirection.South;
                    case PartDirection.West:
                        return PartDirection.North;
                    case PartDirection.North:
                        return PartDirection.East;
                    case PartDirection.South:
                        return PartDirection.West;
                    default:
                        return PartDirection.East;
                }
            case PartDirection.Up:
                switch (pD2)
                {
                    case PartDirection.Down:
                        return PartDirection.Up;
                    case PartDirection.Up:
                        return PartDirection.Up;
                    case PartDirection.East:
                        return PartDirection.North;
                    case PartDirection.West:
                        return PartDirection.South;
                    case PartDirection.North:
                        return PartDirection.West;
                    case PartDirection.South:
                        return PartDirection.East;
                    default:
                        return PartDirection.East;
                }
            case PartDirection.East:
                switch (pD2)
                {
                    case PartDirection.Down:
                        return PartDirection.North;
                    case PartDirection.Up:
                        return PartDirection.South;
                    case PartDirection.East:
                        return PartDirection.East;
                    case PartDirection.West:
                        return PartDirection.East;
                    case PartDirection.North:
                        return PartDirection.Up;
                    case PartDirection.South:
                        return PartDirection.Down;
                    default:
                        return PartDirection.East;
                }
            case PartDirection.West:
                switch (pD2)
                {
                    case PartDirection.Down:
                        return PartDirection.South;
                    case PartDirection.Up:
                        return PartDirection.North;
                    case PartDirection.East:
                        return PartDirection.West;
                    case PartDirection.West:
                        return PartDirection.West;
                    case PartDirection.North:
                        return PartDirection.Down;
                    case PartDirection.South:
                        return PartDirection.Up;
                    default:
                        return PartDirection.East;
                }
            case PartDirection.North:
                switch (pD2)
                {
                    case PartDirection.Down:
                        return PartDirection.West;
                    case PartDirection.Up:
                        return PartDirection.East;
                    case PartDirection.East:
                        return PartDirection.Down;
                    case PartDirection.West:
                        return PartDirection.Up;
                    case PartDirection.North:
                        return PartDirection.North;
                    case PartDirection.South:
                        return PartDirection.North;
                    default:
                        return PartDirection.East;
                }
            case PartDirection.South:
                switch (pD2)
                {
                    case PartDirection.Down:
                        return PartDirection.East;
                    case PartDirection.Up:
                        return PartDirection.West;
                    case PartDirection.East:
                        return PartDirection.Up;
                    case PartDirection.West:
                        return PartDirection.Down;
                    case PartDirection.North:
                        return PartDirection.South;
                    case PartDirection.South:
                        return PartDirection.South;
                    default:
                        return PartDirection.East;
                }
            default:
                return PartDirection.East;
        }
    }

    public static Vector3 ToVector3(this PartDirection pD)
    {
        switch (pD)
        {
            case PartDirection.Down:
                return new Vector3(0f, -1f, 0f);
            case PartDirection.Up:
                return new Vector3(0f, 1f, 0f);
            case PartDirection.East:
                return new Vector3(1f, 0f, 0f);
            case PartDirection.West:
                return new Vector3(-1f, 0f, 0f);
            case PartDirection.North:
                return new Vector3(0f, 0f, 1f);
            case PartDirection.South:
                return new Vector3(0f, 0f, -1f);
            default:
                return new Vector3(0f, 0f, 1f);
        }
    }

    public static Vector3Int ToVector3Int(this PartDirection pD)
    {
        switch (pD)
        {
            case PartDirection.Down:
                return new Vector3Int(0, -1, 0);
            case PartDirection.Up:
                return new Vector3Int(0, 1, 0);
            case PartDirection.East:
                return new Vector3Int(1, 0, 0);
            case PartDirection.West:
                return new Vector3Int(-1, 0, 0);
            case PartDirection.North:
                return new Vector3Int(0, 0, 1);
            case PartDirection.South:
                return new Vector3Int(0, 0, -1);
            default:
                return new Vector3Int(0, 0, 1);
        }
    }
}



public static class PartVerticesHelpMethods
{
    public static void InnerHoles(List<Vector3> verts, Quaternion rotAxis, Quaternion rotQuat, float fac, Vector2 holePos)
    {
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y - 2, holePos.x - 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y - 2, holePos.x - 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y - 1, holePos.x - 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y - 1, holePos.x - 2)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y - 1, holePos.x - 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y - 1, holePos.x - 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y + 1, holePos.x - 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y + 1, holePos.x - 2)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y + 1, holePos.x - 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y + 1, holePos.x - 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y + 2, holePos.x - 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y + 2, holePos.x - 1)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y + 2, holePos.x - 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y + 2, holePos.x - 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y + 2, holePos.x + 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y + 2, holePos.x + 1)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y + 2, holePos.x + 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y + 2, holePos.x + 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y + 1, holePos.x + 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y + 1, holePos.x + 2)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y + 1, holePos.x + 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y + 1, holePos.x + 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y - 1, holePos.x + 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y - 1, holePos.x + 2)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y - 1, holePos.x + 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y - 1, holePos.x + 2)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y - 2, holePos.x + 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y - 2, holePos.x + 1)) * fac));

        verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y - 2, holePos.x + 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y - 2, holePos.x + 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(4, holePos.y - 2, holePos.x - 1)) * fac));
        verts.Add(rotAxis * rotQuat * ((new Vector3(-4, holePos.y - 2, holePos.x - 1)) * fac));
    }

    public static void Wheel(List<Vector3> verts, Quaternion rotAxis, Quaternion rotQuat, int amountOfEdges, float fullDiameter, float innterDiameter, float width)
    {
        Wheel(verts, rotAxis, rotQuat, amountOfEdges, fullDiameter, innterDiameter, width, 0.2f);
    }

    public static void Wheel(List<Vector3> verts, Quaternion rotAxis, Quaternion rotQuat, int amountOfEdges, float fullDiameter, float innterDiameter, float width, float roundingA)
    {
        float littleOffset = 0.01f;

        width = width + littleOffset;

        if (amountOfEdges % 2 == 0)
        {
            float fac = 1f;// 0.125f * 0.5f;

            float circleSteps = 1f / ((float)amountOfEdges);
            float fullRadius = fullDiameter * 0.5f;

            for (int i = 0; i < amountOfEdges; i++)
            {
                Vector2 cA0 = Circle(fullRadius - roundingA, circleSteps * i);
                Vector2 cA1 = Circle(fullRadius - roundingA, circleSteps * (i + 1));
                Vector2 cTop0 = Circle(fullRadius, circleSteps * i);
                Vector2 cTop1 = Circle(fullRadius, circleSteps * (i + 1));
                Vector2 cBorder0 = Circle(innterDiameter * 0.5f, circleSteps * i);
                Vector2 cBorder1 = Circle(innterDiameter * 0.5f, circleSteps * (i + 1));

                verts.Add(rotAxis * rotQuat * ((new Vector3(cBorder1.x, cBorder1.y, width * -0.5f)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cA1.x, cA1.y, width * -0.5f)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cBorder0.x, cBorder0.y, width * -0.5f)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cA0.x, cA0.y, width * -0.5f)) * fac));

                verts.Add(rotAxis * rotQuat * ((new Vector3(cBorder0.x, cBorder0.y, width * 0.5f)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cA0.x, cA0.y, width * 0.5f)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cBorder1.x, cBorder1.y, width * 0.5f)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cA1.x, cA1.y, width * 0.5f)) * fac));




                verts.Add(rotAxis * rotQuat * ((new Vector3(cA1.x, cA1.y, width * -0.5f)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cTop1.x, cTop1.y, width * -0.5f + roundingA)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cA0.x, cA0.y, width * -0.5f)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cTop0.x, cTop0.y, width * -0.5f + roundingA)) * fac));

                verts.Add(rotAxis * rotQuat * ((new Vector3(cA0.x, cA0.y, width * 0.5f)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cTop0.x, cTop0.y, width * 0.5f - roundingA)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cA1.x, cA1.y, width * 0.5f)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cTop1.x, cTop1.y, width * 0.5f - roundingA)) * fac));




                verts.Add(rotAxis * rotQuat * ((new Vector3(cTop0.x, cTop0.y, width * -0.5f + roundingA)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cTop1.x, cTop1.y, width * -0.5f + roundingA)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cTop0.x, cTop0.y, width * 0.5f - roundingA)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cTop1.x, cTop1.y, width * 0.5f - roundingA)) * fac));
            }

            for (int i = 0; i < amountOfEdges / 2; i++)
            {
                Vector2 cBorder0 = Circle(innterDiameter * 0.5f, circleSteps * i);
                Vector2 cBorder1 = Circle(innterDiameter * 0.5f, circleSteps * (amountOfEdges - i - 1));
                Vector2 cBorder2 = Circle(innterDiameter * 0.5f, circleSteps * (i + 1));
                Vector2 cBorder3 = Circle(innterDiameter * 0.5f, circleSteps * (amountOfEdges - i - 2));


                verts.Add(rotAxis * rotQuat * ((new Vector3(cBorder0.x, cBorder0.y, width * 0.5f)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cBorder2.x, cBorder2.y, width * 0.5f)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cBorder1.x, cBorder1.y, width * 0.5f)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cBorder3.x, cBorder3.y, width * 0.5f)) * fac));

                verts.Add(rotAxis * rotQuat * ((new Vector3(cBorder1.x, cBorder1.y, width * -0.5f)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cBorder3.x, cBorder3.y, width * -0.5f)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cBorder0.x, cBorder0.y, width * -0.5f)) * fac));
                verts.Add(rotAxis * rotQuat * ((new Vector3(cBorder2.x, cBorder2.y, width * -0.5f)) * fac));
            }
        }
    }

    public static Vector2 Circle(float radius, float posClockwise)
    {
        return new Vector2(radius * Mathf.Sin(posClockwise * (2.0f * Mathf.PI)), radius * Mathf.Cos(posClockwise * (2.0f * Mathf.PI)));
    }
}