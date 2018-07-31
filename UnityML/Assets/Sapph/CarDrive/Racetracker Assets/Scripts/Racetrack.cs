using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racetrack
{
    public ClosedSpline<Vector3> track;
    public ClosedSpline<float> height;
    public ClosedSpline<float> bend;
    public ClosedSpline<float> width;
}
