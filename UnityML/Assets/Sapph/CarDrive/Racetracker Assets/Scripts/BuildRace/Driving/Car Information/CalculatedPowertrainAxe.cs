using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatedPowertrainAxe : CalculatedPart
{
    protected float factorToWheel;
    protected int wheelID;
    protected Vector3 axeRotateAround;

    public CalculatedPowertrainAxe()
    {

    }

    public float FactorToWheel
    {
        get
        {
            return factorToWheel;
        }
        set
        {
            factorToWheel = value;
        }
    }

    public int AccordingWheelID
    {
        get
        {
            return wheelID;
        }
        set
        {
            wheelID = value;
        }
    }

    public Vector3 RotateAroundAxe
    {
        get
        {
            return axeRotateAround;
        }
        set
        {
            axeRotateAround = value;
        }
    }
}
