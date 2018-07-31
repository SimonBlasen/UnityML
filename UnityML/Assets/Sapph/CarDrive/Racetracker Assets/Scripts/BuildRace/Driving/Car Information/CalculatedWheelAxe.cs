using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculatedWheelAxe : CalculatedPart
{
    protected int accordingWheelID;

    public CalculatedWheelAxe()
    {

    }

    public int AccordingWheelID
    {
        get
        {
            return accordingWheelID;
        }
        set
        {
            accordingWheelID = value;
        }
    }
}
