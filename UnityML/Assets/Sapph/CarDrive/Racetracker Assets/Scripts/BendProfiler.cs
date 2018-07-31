using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BendProfiler : RTProfiler {

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
            //Debug.Log("Height (" + (int)(racetrack.track.controlPoints[i].x * 20f + transform.position.x) + "," + (int)(racetrack.track.controlPoints[i].z * 20f + transform.position.z) + "): " + heightThere);


            Vector3 middlePosFirst = racetrack.track.SplineAt(((float)i) / ((float)cps.Length));
            Vector3[] tangentFirst = racetrack.track.TangentAt(((float)i) / ((float)cps.Length));
            Vector3 forwardFirst = tangentFirst[1] - tangentFirst[0];
            forwardFirst.Normalize();
            Vector3 sidewardsFirst = Quaternion.Euler(0f, 90f, 0f) * forwardFirst;
            sidewardsFirst.Normalize();

            Vector3 leftFirst = (middlePosFirst - sidewardsFirst * racetrack.width.SplineAt(0f) * 0.5f);
            Vector3 rightFirst = (middlePosFirst + sidewardsFirst * racetrack.width.SplineAt(0f) * 0.5f);

            leftFirst *= 20f;
            rightFirst *= 20f;

            float heightLeft = terrain.SampleHeight(leftFirst);
            float heightRight = terrain.SampleHeight(rightFirst);

            Vector3 leftFirstH = leftFirst;
            Vector3 rightFirstH = rightFirst;
            leftFirstH.y = heightLeft;
            rightFirstH.y = heightRight;



            cps[i] = Vector3.Angle(leftFirst - rightFirst, leftFirstH - rightFirstH) * Mathf.Sign(heightRight - heightLeft);
        }

        ClosedSpline<float> heightSpline = new ClosedSpline<float>(cps);

        racetrack.bend = heightSpline;

        return racetrack;
    }
}
