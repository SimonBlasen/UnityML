using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatedSteerBar : CalculatedPart
{
    protected float factorToSteerWheel = 0f;
    protected Vector3 axeMoveAlong;
    protected float halfwayDistance;

    public CalculatedSteerBar()
    {

    }


    public float FactorToSteerWheel
    {
        get
        {
            return factorToSteerWheel;
        }
        set
        {
            factorToSteerWheel = value;
        }
    }

    public Vector3 AxeMoveAlong
    {
        get
        {
            return axeMoveAlong;
        }
        set
        {
            axeMoveAlong = value;
        }
    }

    public float HalfwayDistance
    {
        get
        {
            return halfwayDistance;
        }
        set
        {
            halfwayDistance = value;
        }
    }
}
