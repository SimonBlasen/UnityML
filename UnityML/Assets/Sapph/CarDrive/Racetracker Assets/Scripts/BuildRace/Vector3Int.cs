using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vector3Int
{
    public int x;
    public int y;
    public int z;

    public Vector3Int()
    {
        x = 0;
        y = 0;
        z = 0;
    }

    public Vector3Int(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3Int(float x, float y, float z)
    {
        this.x = (int)x;
        this.y = (int)y;
        this.z = (int)z;
    }

    public Vector3Int(Vector3 vector3)
    {
        x = (int)vector3.x;
        y = (int)vector3.y;
        z = (int)vector3.z;
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType() == typeof(Vector3Int))
        {
            return ((Vector3Int)obj).x == x && ((Vector3Int)obj).y == y && ((Vector3Int)obj).z == z;
        }
        else
        {
            return false;
        }
    }

    public static bool operator ==(Vector3Int a, Vector3Int b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Vector3Int a, Vector3Int b)
    {
        return !a.Equals(b);
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public Vector3Int Add(Vector3Int additionVector)
    {
        return new Vector3Int(x + additionVector.x, y + additionVector.y, z + additionVector.z);
    }

    public Vector3Int Multiply(int factor)
    {
        return new Vector3Int(x * factor, y * factor, z * factor);
    }

    public Vector3Int Sub(Vector3Int subtractionalVector)
    {
        return new Vector3Int(x - subtractionalVector.x, y - subtractionalVector.y, z - subtractionalVector.z);
    }

    public override string ToString()
    {
        return "(" + x + "," + y + "," + z + ")";
    }
}
