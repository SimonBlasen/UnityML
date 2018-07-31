using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatedWheel : CalculatedPart
{
    protected bool steering;
    protected bool powering;
    protected float radius;
    protected float grip;
    protected float steerAngle;
    protected bool steerCorrectDir = true;
    protected float springStrength;
    protected float damperStrength;
    //TODO sonstige Eigenschaften

    public CalculatedWheel()
    {

    }

    public bool Steering
    {
        get
        {
            return steering;
        }
        set
        {
            steering = value;
        }
    }

    public bool Powering
    {
        get
        {
            return powering;
        }
        set
        {
            powering = value;
        }
    }

    public float Grip
    {
        get
        {
            return grip;
        }
        set
        {
            grip = value;
        }
    }

    public float Radius
    {
        get
        {
            return radius;
        }
        set
        {
            radius = value;
        }
    }

    public float SteerAngle
    {
        get
        {
            return steerAngle;
        }
        set
        {
            steerAngle = value;
        }
    }

    public bool SteerCorrectDirection
    {
        get
        {
            return steerCorrectDir;
        }
        set
        {
            steerCorrectDir = value;
        }
    }

    public float SpringStrength
    {
        get
        {
            return springStrength;
        }
        set
        {
            springStrength = value;
        }
    }

    public float DamperStrength
    {
        get
        {
            return damperStrength;
        }
        set
        {
            damperStrength = value;
        }
    }
}
