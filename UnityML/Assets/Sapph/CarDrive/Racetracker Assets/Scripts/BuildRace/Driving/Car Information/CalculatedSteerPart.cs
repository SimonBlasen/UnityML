using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatedSteerPart : CalculatedPart
{
    protected bool invertedDirection;

    public CalculatedSteerPart()
    {

    }


    public bool InvertedDirection
    {
        get
        {
            return invertedDirection;
        }
        set
        {
            invertedDirection = value;
        }
    }
}
