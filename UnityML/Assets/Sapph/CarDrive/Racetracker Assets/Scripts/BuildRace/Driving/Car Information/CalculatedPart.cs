using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatedPart
{
    protected int id;
    protected Vector3 position;
    protected PartDirection direction;
    protected PartRotation rotation;
    protected PartType type;
    protected float health = 1f;

    public CalculatedPart()
    {

    }

    public Vector3 Position
    {
        get
        {
            return position;
        }
        set
        {
            position = value;
        }
    }

    public int ID
    {
        get
        {
            return id;
        }
        set
        {
            id = value;
        }
    }

    public PartType Type
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
        }
    }

    public PartDirection Direction
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value;
        }
    }

    public PartRotation Rotation
    {
        get
        {
            return rotation;
        }
        set
        {
            rotation = value;
        }
    }

    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }
}
