using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedTurn : GeneratedElement
{
    private float radius;
    private float degree;

    public GeneratedTurn(Vector3 position, float direction, float radius, float degree, float width) : this(position, direction, radius, degree, width, width)
    {

    }

    public GeneratedTurn(Vector3 position, float direction, float radius, float degree, float widthStart, float widthEnd)
    {
        this.position = position;
        this.direction = direction;
        this.radius = radius;
        this.degree = degree;
        this.widthStart = widthStart;
        this.widthEnd = widthEnd;
    }

    public float Radius
    {
        get
        {
            return radius;
        }
    }

    public float Degree
    {
        get
        {
            return degree;
        }
    }

    protected override float getEndDirection()
    {
        return Direction + ((Degree * Mathf.PI) / 180f);
    }

    protected override Vector3 getEndPosition()
    {
        Vector3 toRight = (Quaternion.Euler(0f, (Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(1f, 0f, 0f));
        Vector3 middlePoint = Position + toRight * Radius * ((Degree >= 0f) ? 1f : -1f);
        Vector3 toRightTurnedEnd = (Quaternion.Euler(0f, Degree, 0f)) * toRight;
        return middlePoint + toRightTurnedEnd * Radius * ((Degree >= 0f) ? -1f : 1f);
    }

    public override bool ForceEndDirection(float newEndDirection)
    {
        return false;
    }

    protected override float getLength()
    {
        return 2f * Mathf.PI * radius * (Mathf.Abs(degree) / 360f);
    }

    public override GeneratedElement Copy()
    {
        GeneratedTurn copy = new GeneratedTurn(position, direction, radius, degree, widthStart, widthEnd);

        return copy;
    }
}
