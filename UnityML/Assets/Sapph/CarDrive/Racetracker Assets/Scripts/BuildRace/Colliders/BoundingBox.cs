using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox
{
    private Vector3Int minVals;
    private Vector3Int maxVals;

    public BoundingBox()
    {
        minVals = new Vector3Int(int.MaxValue, int.MaxValue, int.MaxValue);
        maxVals = new Vector3Int(int.MinValue, int.MinValue, int.MinValue);
    }

    public void AddElement(Vector3Int element)
    {
        AddElement(element.x, element.y, element.z);
    }

    public Vector3Int MinValues
    {
        get
        {
            return minVals;
        }
    }

    public Vector3Int MaxValues
    {
        get
        {
            return maxVals;
        }
    }

    public void AddElement(int x, int y, int z)
    {
        if (x < minVals.x)
            minVals.x = x;
        if (y < minVals.y)
            minVals.y = y;
        if (z < minVals.z)
            minVals.z = z;

        if (x > maxVals.x)
            maxVals.x = x;
        if (y > maxVals.y)
            maxVals.y = y;
        if (z > maxVals.z)
            maxVals.z = z;
    }

    public bool IsInside(Vector3Int point)
    {
        return IsInside(point.x, point.y, point.z);
    }

    public bool IsInside(int x, int y, int z)
    {
        return (x >= minVals.x && x <= maxVals.x &&
                y >= minVals.y && y <= maxVals.y &&
                z >= minVals.z && z <= maxVals.z);
    }
}
