using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobCarvisualize : ThreadedJob
{
    public TerrainModifier TerrainModifier { get; set; }
    public DiscreteTrack DiscreteTrack { get; set; }
    public float deltaT = 1f / 60f;

    private int pathC = 0;
    private DiscreteTrack track;

    public Vector3[] path = null;
    public Vector3[] forces = null;

    protected override void ThreadFunction()
    {
        track = DiscreteTrack;

        pathC = 0;
        List<Vector3> posList = new List<Vector3>();
        List<Vector3> forcList = new List<Vector3>();

        float exactment = 0.1f;

        int resolution = track.speedAnalyzer.Vs.Length;

        Vector3 oldPos = track.idealLineSpline.SplineAt(0f);
        float oldV = track.speedAnalyzer.Vs[0];

        Vector3 oldForce = track.speedAnalyzer.Forces[0];

        float s = 0f;
        float steps = (1f / resolution);
        int debugWhileCounter2 = 0;
        while (s < 1f)
        {
            debugWhileCounter2++;
            if (debugWhileCounter2 > 10000)
            {
                Debug.LogError("While counter2 > 10000");
                break;
            }
            s += steps;

            Vector3 pos = track.idealLineSpline.SplineAt(s);
            float traveledDistance = oldV * deltaT;

            float adjustStep = steps * 0.5f;

            int debugWhileCounter = 0;
            while (Mathf.Abs(Vector3.Distance(pos, oldPos) - traveledDistance) > exactment)
            {
                debugWhileCounter++;
                if (debugWhileCounter > 1000)
                {
                    //Debug.LogError("While counter > 1000");
                    break;
                }
                if (TerrainModifier != null)
                {
                    pos = track.idealLineSpline.SplineAt(s);// + new Vector3(0f, terrainModifier.GetTensorHeight(track.idealLineSpline.SplineAt(s).x, track.idealLineSpline.SplineAt(s).z), 0f);
                }
                else
                {
                    pos = track.idealLineSpline.SplineAt(s);
                }

                if (Vector3.Distance(pos, oldPos) > traveledDistance)
                {
                    s -= (adjustStep);
                }
                else
                {
                    s += (adjustStep);
                }
                adjustStep *= 0.5f;
            }

            forcList.Add(oldForce);
            posList.Add(pos);



            int rounded = (int)(s * resolution);
            oldV = track.speedAnalyzer.Vs[rounded % track.speedAnalyzer.Vs.Length];
            oldForce = track.speedAnalyzer.Forces[rounded % track.speedAnalyzer.Vs.Length];

            oldPos = pos;
        }

        path = posList.ToArray();
        forces = forcList.ToArray();


    }
}
