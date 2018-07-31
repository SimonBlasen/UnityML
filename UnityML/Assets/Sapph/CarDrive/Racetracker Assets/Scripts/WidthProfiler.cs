using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WidthProfiler : RTProfiler {

    public float widthMin = 0.3f;
    public float widthMax = 2f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override Racetrack ApplyRacetrack(Racetrack racetrack)
    {
        float[] cps = new float[racetrack.track.controlPoints.Length];
        for (int i = 0; i < cps.Length; i++)
        {
            cps[i] = Random.Range(widthMin, widthMax);
        }

        ClosedSpline<float> widthSpline = new ClosedSpline<float>(cps);

        racetrack.width = widthSpline;

        return racetrack;
    }
}
