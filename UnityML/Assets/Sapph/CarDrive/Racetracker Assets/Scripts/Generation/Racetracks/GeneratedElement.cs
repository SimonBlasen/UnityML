using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneratedElement
{
    //
    // The position as 3D-coordinate
    //
    protected Vector3 position;

    //
    // The direction of the element, given as radian angle
    //
    protected float direction;

    protected float widthStart;
    protected float widthEnd;

    protected List<GeneratedCurb> curbs = new List<GeneratedCurb>();

    protected abstract Vector3 getEndPosition();
    protected abstract float getEndDirection();
    protected abstract float getLength();

    public abstract bool ForceEndDirection(float newEndDirection);

    public abstract GeneratedElement Copy();

    public void AddCurb(GeneratedCurb curb)
    {
        curbs.Add(curb);
    }

    public GeneratedCurb[] Curbs
    {
        get
        {
            return curbs.ToArray();
        }
    }

    public Vector3 Position
    {
        get
        {
            return position;
        }
    }

    public float Direction
    {
        get
        {
            return direction;
        }
    }

    public float WidthStart
    {
        get
        {
            return widthStart;
        }
    }

    public float WidthEnd
    {
        get
        {
            return widthEnd;
        }
    }

    public Vector3 EndPosition
    {
        get
        {
            return getEndPosition();
        }
    }

    public float EndDirection
    {
        get
        {
            return getEndDirection();
        }
    }

    public float Length
    {
        get
        {
            return getLength();
        }
    }
}
