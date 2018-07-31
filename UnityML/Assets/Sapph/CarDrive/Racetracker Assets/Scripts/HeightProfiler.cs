using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightProfiler : RTProfiler {

    public Terrain terrain;
    public int checkpointsAmount = 50;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    public override Racetrack ApplyRacetrack(Racetrack racetrack)
    {

        float[] cps = new float[checkpointsAmount];
        for (int i = 0; i < cps.Length; i++)
        {
            float heightThere = terrain.SampleHeight(racetrack.track.SplineAt(((float)i) / ((float)cps.Length)) * 20f);
            //Debug.Log("Height (" + (int)(racetrack.track.controlPoints[i].x * 20f + transform.position.x) + "," + (int)(racetrack.track.controlPoints[i].z * 20f + transform.position.z) + "): " + heightThere);
            cps[i] = heightThere;
        }

        ClosedSpline<float> heightSpline = new ClosedSpline<float>(cps);

        racetrack.height = heightSpline;

        return racetrack;
    }
}
