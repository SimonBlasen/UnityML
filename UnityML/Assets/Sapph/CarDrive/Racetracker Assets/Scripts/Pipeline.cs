using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipeline : MonoBehaviour {

    public WidthProfiler width;
    public HeightProfiler height;
    public BendProfiler bend;
    public TrackGenerator track;
    public RTProfiler rendererProfiler;

    public bool refresh = false;

    private List<RTRule> rules;

    [HideInInspector]
    public Racetrack racetrack;

	// Use this for initialization
	void Start ()
    {
        rules = new List<RTRule>();
        rules.Add(new CurveRule());
        racetrack = new Racetrack();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (refresh)
        {
            refresh = false;
            Refresh();
        }
    }

    public void Refresh()
    {
        racetrack = new Racetrack();
        racetrack = track.ApplyRacetrack(racetrack);
        racetrack = width.ApplyRacetrack(racetrack);
        racetrack = height.ApplyRacetrack(racetrack);
        racetrack = bend.ApplyRacetrack(racetrack);

        int tries = 0;

        while (racetrackOkay(racetrack) == false)
        {
            tries++;
            racetrack = new Racetrack();
            racetrack = track.ApplyRacetrack(racetrack);
            racetrack = width.ApplyRacetrack(racetrack);
            racetrack = height.ApplyRacetrack(racetrack);
            racetrack = bend.ApplyRacetrack(racetrack);

            if (tries >= 2000)
            {
                break;
            }
        }

        Debug.Log("Tries: " + tries);

        racetrack = rendererProfiler.ApplyRacetrack(racetrack);
    }

    private bool racetrackOkay(Racetrack track)
    {
        for (int i = 0; i < rules.Count; i++)
        {
            if (rules[i].Check(track) == false)
            {
                return false;
            }
        }

        return true;
    }
}


public class RTRule
{
    public virtual bool Check(Racetrack racetrack)
    {
        return true;
    }
}

public class CurveRule : RTRule
{
    public override bool Check(Racetrack racetrack)
    {
        float tolerance = 0.1f;
        int resolution = 400;

        List<Line> leftLines = new List<Line>();
        List<Line> rightLines = new List<Line>();

        Vector3 middlePosFirst = racetrack.track.SplineAt(0f);
        Vector3[] tangentFirst = racetrack.track.TangentAt(0f);
        Vector3 forwardFirst = tangentFirst[1] - tangentFirst[0];
        forwardFirst.Normalize();
        Vector3 sidewardsFirst = Quaternion.Euler(0f, 90f, 0f) * forwardFirst;
        sidewardsFirst.Normalize();

        Vector2 leftFirst = new Vector2((middlePosFirst - sidewardsFirst * racetrack.width.SplineAt(0f) * 0.5f).x, (middlePosFirst - sidewardsFirst * racetrack.width.SplineAt(0f) * 0.5f).z);
        Vector2 rightFirst = new Vector2((middlePosFirst + sidewardsFirst * racetrack.width.SplineAt(0f) * 0.5f).x, (middlePosFirst + sidewardsFirst * racetrack.width.SplineAt(0f) * 0.5f).z);

        for (int i = 1; i < resolution; i++)
        {
            float xPos = ((float)i) / ((float)resolution);

            Vector3 middlePos = racetrack.track.SplineAt(xPos);
            Vector3[] tangent = racetrack.track.TangentAt(xPos);
            Vector3 forward = tangent[1] - tangent[0];
            forward.Normalize();
            Vector3 sidewards = Quaternion.Euler(0f, 90f, 0f) * forward;
            sidewards.Normalize();


            Vector2 left = new Vector2((middlePos - sidewards * racetrack.width.SplineAt(xPos) * 0.5f).x, (middlePos - sidewards * racetrack.width.SplineAt(xPos) * 0.5f).z);
            Vector2 right = new Vector2((middlePos + sidewards * racetrack.width.SplineAt(xPos) * 0.5f).x, (middlePos + sidewards * racetrack.width.SplineAt(xPos) * 0.5f).z);

            leftLines.Add(new Line(leftFirst, left));
            rightLines.Add(new Line(rightFirst, right));

            leftFirst = left;
            rightFirst = right;

            for (int j = leftLines.Count - 2; j >= 0; j--)
            {
                Vector2 point;
                if (leftLines[j].IntersectsOffset(leftLines[leftLines.Count - 1], tolerance, out point))
                {
                    return false;
                }
            }
            for (int j = rightLines.Count - 2; j >= 0; j--)
            {
                Vector2 point;
                if (rightLines[j].IntersectsOffset(rightLines[rightLines.Count - 1], tolerance, out point))
                {
                    return false;
                }
            }
        }

        return true;
    }
}