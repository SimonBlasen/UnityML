using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarVisualize : MonoBehaviour
{
    private DiscreteTrack track = null;

    private float deltaT = 1f / 60f;

    private Vector3[] path = null;
    private Vector3[] forces = null;

    private int pathC = 0;

    private GameObject lineX;
    private GameObject lineZ;

    // Use this for initialization
    void Start ()
    {
        GameObject line = new GameObject();
        //line.layer = 9;
        line.transform.SetParent(transform);
        line.transform.position = transform.position;
        line.AddComponent<LineRenderer>();
        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr.SetColors(Color.red, Color.red);
        lr.SetWidth(0.5f, 0.5f);
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, transform.position);
        lineX = line;


        GameObject line2 = new GameObject();
        //line2.layer = 9;
        line2.transform.SetParent(transform);
        line2.transform.position = transform.position;
        line2.AddComponent<LineRenderer>();
        LineRenderer lr2 = line2.GetComponent<LineRenderer>();
        lr2.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
        lr2.SetColors(Color.blue, Color.blue);
        lr2.SetWidth(0.5f, 0.5f);
        lr2.SetPosition(0, transform.position);
        lr2.SetPosition(1, transform.position);
        lineZ = line2;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (job != null && job.IsDone)
        {
            path = job.path;
            for (int i = 0; i < path.Length; i++)
            {
                path[i] = path[i] + new Vector3(0f, 0.5f, 0f);
            }
            forces = job.forces;
            job = null;
        }
	}

    private void FixedUpdate()
    {
        if (path != null)
        {
            transform.position = path[pathC];
            transform.LookAt(new Vector3(path[(pathC + 1) % path.Length].x, path[(pathC + 1) % path.Length].y, path[(pathC + 1) % path.Length].z));

            lineX.GetComponent<LineRenderer>().SetPosition(0, transform.position + new Vector3(0f, 3f, 0f));
            lineZ.GetComponent<LineRenderer>().SetPosition(0, transform.position + new Vector3(0f, 3f, 0f));
            lineX.GetComponent<LineRenderer>().SetPosition(1, transform.position + transform.right.normalized * forces[(pathC + 1) % path.Length].x * 0.38f + new Vector3(0f, 3f, 0f));
            lineZ.GetComponent<LineRenderer>().SetPosition(1, transform.position + transform.forward.normalized * forces[(pathC + 1) % path.Length].z * 40f + new Vector3(0f, 3f, 0f));

            //Debug.Log("Force: " + forces[(pathC + 1) % path.Length].ToString());

            pathC++;
            if (pathC >= path.Length)
            {
                pathC = 0;
            }
        }
    }

    public void SetTrack(DiscreteTrack track)
    {
        SetTrack(track, null);
    }

    private JobCarvisualize job = null;

    public void SetTrack(DiscreteTrack track, TerrainModifier terrainModifier)
    {
        if (job != null)
        {
            job.Abort();
            job = null;
        }

        pathC = 0;

        job = new JobCarvisualize();
        job.deltaT = deltaT;
        job.DiscreteTrack = track;
        job.TerrainModifier = terrainModifier;
        job.Start();
    }
}
