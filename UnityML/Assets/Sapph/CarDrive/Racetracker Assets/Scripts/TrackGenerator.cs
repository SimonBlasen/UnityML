using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TrackGenerator : RTProfiler
{
    public GameObject prefabTrack;
    public GameObject prefabControlpoint;
    public float minAngle = -30f;
    public float maxAngle = 30f;
    public float minRadius = 2f;
    public float maxRadius = 2f;

    public int amountOfCPs = 10;
    public bool refresh = false;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (refresh)
        {
            refresh = false;

            angle = 0f;
            radius = 3f;

            Generate(amountOfCPs);
        }
	}

    float angle = 0f;
    float radius = 2f;

    GameObject instTrack = null;
    List<GameObject> instCPs = new List<GameObject>();

    public void Generate(int amountControlpoints)
    {
        if (instTrack != null)
        {
            Destroy(instTrack);
            instTrack = null;

            for (int i = 0; i < instCPs.Count; i++)
            {
                Destroy(instCPs[i]);
            }

            instCPs.Clear();
        }

        instTrack = (GameObject)Instantiate(prefabTrack);

        for (int i = 0; i < amountControlpoints; i++)
        {
            GameObject instCp = (GameObject)Instantiate(prefabControlpoint, instTrack.transform);
            angle += Random.Range(minAngle, maxAngle);
            radius += Random.Range(minRadius, maxRadius);

            Vector2 point = new Vector2(Mathf.Sin((angle * Mathf.PI) / 180f) * radius, Mathf.Cos((angle * Mathf.PI) / 180f) * radius);

            instCp.transform.position = new Vector3(point.x, 0f, point.y);

            instCPs.Add(instCp);
        }


        instTrack.GetComponent<SappClosedSpline>().ControlPoints = instCPs.ToArray();
        instTrack.GetComponent<SappClosedSpline>().RefreshSpline();
    }

    public SappClosedSpline Track
    {
        get
        {
            if (instTrack != null)
            {
                return instTrack.GetComponent<SappClosedSpline>();
            }

            return null;
        }
    }

    public override Racetrack ApplyRacetrack(Racetrack racetrack)
    {
        angle = 0f;
        radius = 3f;
        //Generate(amountOfCPs);

        Vector3[] checkpoints = new Vector3[amountOfCPs];

        for (int i = 0; i < amountOfCPs; i++)
        {
            //GameObject instCp = (GameObject)Instantiate(prefabControlpoint, instTrack.transform);
            angle += Random.Range(minAngle, maxAngle);
            radius += Random.Range(minRadius, maxRadius);

            Vector2 point = new Vector2(Mathf.Sin((angle * Mathf.PI) / 180f) * radius, Mathf.Cos((angle * Mathf.PI) / 180f) * radius);
            
            checkpoints[i] = new Vector3(point.x, 0f, point.y);

            //instCp.transform.position = new Vector3(point.x, 0f, point.y);

            //instCPs.Add(instCp);
        }


        ClosedSpline<Vector3> clSpline = new ClosedSpline<Vector3>(checkpoints);

        //if (instTrack != null)
        //{
        racetrack.track = clSpline;
        //}

        return racetrack;
    }
}
