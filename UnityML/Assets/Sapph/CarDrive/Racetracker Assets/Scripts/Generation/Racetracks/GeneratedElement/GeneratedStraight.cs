using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedStraight : GeneratedElement
{
    private float length;

    public GeneratedStraight(Vector3 position, float direction, float length, float width) : this(position, direction, length, width, width)
    {

    }

    public GeneratedStraight(Vector3 position, float direction, float length, float widthStart, float widthEnd)
    {
        this.position = position;
        this.direction = direction;
        this.length = length;
        this.widthStart = widthStart;
        this.widthEnd = widthEnd;
    }

    public float Length
    {
        get
        {
            return length;
        }
    }

    protected override float getEndDirection()
    {
        return Direction;
    }

    protected override Vector3 getEndPosition()
    {
        Vector3 toFront = (Quaternion.Euler(0f, (Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(0f, 0f, Length));
        return Position + toFront;
    }

    public override bool ForceEndDirection(float newEndDirection)
    {
        return false;
    }

    protected override float getLength()
    {
        return length;
    }

    public override GeneratedElement Copy()
    {
        GeneratedStraight copy = new GeneratedStraight(position, direction, length, widthStart, widthEnd);

        return copy;
    }
}
