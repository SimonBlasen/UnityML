using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Vehicles.Car;

public class CarDriveAgent : Agent
{
    public float raycastDistance = 30f;
    public CarAllCheckpoints allCheckpoints;

    private Vector3 startPos;
    private Quaternion startRot;

    private float timeCP = 0f;


    private float episodeTime = 200f;
    private float episodeCounter = 0f;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
        startRot = transform.rotation;
	}

    private float noRewFor = 0f;
	// Update is called once per frame
	void Update () {
        timeCP += Time.deltaTime;
        noRewFor += Time.deltaTime;
        episodeCounter += Time.deltaTime;
    }

    private int oldCheckpoints = 0;
    private int oldNegCheckpoints = 0;

    public int PassedCheckpoints { get; set; }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float actionZ = 1f * Mathf.Clamp(vectorAction[0], -1f, 1f);
        //float actionX = 1f * Mathf.Clamp(vectorAction[1], -1f, 1f);
        //float actionX = 50f * (Mathf.Clamp(vectorAction[1], -1f, 1f) + 1f); // between 0f and 100f
        GetComponent<CarUserControl>().Steering = actionZ;
        GetComponent<CarUserControl>().Throttle = 1f;//actionX;


        //Debug.Log("Goal speed: " + GetComponent<CarUserControl>().GoalSpeed);


        if (false && noRewFor >= 10f)
        {
            Done();
            SetReward(-30f);
        }
        else if (episodeCounter >= episodeTime)
        {
            Done();
            SetReward(-0.1f);
            Debug.Log("++ episodeTime: " + GetCumulativeReward().ToString());
        }
        else
        {
            if (collided)
            {
                Done();
                SetReward(-0.2f);
                Debug.Log("++ collided: " + GetCumulativeReward().ToString());
            }
            /*else if (allCheckpoints.AreAll)
            {
                Done();
                SetReward(1f);
                noRewFor = 0f;
            }*/
            else if (IsDone() == false)
            {
                if (allCheckpoints.AmountActivated > oldCheckpoints)
                {
                    int amount = allCheckpoints.AmountActivated - oldCheckpoints;

                    oldCheckpoints = allCheckpoints.AmountActivated;
                    SetReward(0.1f * amount/* / timeCP*/);

                    Debug.Log("++ Reward: " + GetCumulativeReward().ToString());

                    timeCP = 0f;
                    noRewFor = 0f;
                }
                else if (allCheckpoints.AmountNegActivated > oldNegCheckpoints)
                {
                    int amount = allCheckpoints.AmountNegActivated - oldNegCheckpoints;

                    oldNegCheckpoints = allCheckpoints.AmountNegActivated;
                    SetReward(-0.1f * amount/* / timeCP*/);

                    Debug.Log("-- Reward: " + GetCumulativeReward().ToString());
                    
                }
                else
                {
                    SetReward(-0.00f);
                }
            }
        }



    }

    private bool collided = false;

    public void YouCollided()
    {
        collided = true;
    }

    public override void CollectObservations()
    {
        AddVectorObs(GetComponent<Rigidbody>().velocity.magnitude * 0.01f);



        float[] vecs = new float[5];

        for (int i = 0; i < vecs.Length; i++)
        {
            RaycastHit hitMid;
            float distMid = raycastDistance;

            Vector3 dir = Quaternion.Euler(0f, (i - (vecs.Length - 1) * 0.5f) * 90f * 0.5f, 0f) * transform.forward;

            dir.y = 0f;

            Vector3 forw = transform.forward;
            forw.y = 0f;
            forw.Normalize();


            if (Physics.Raycast(new Ray(transform.position + forw * 2.5f + (new Vector3(0f, 0.5f, 0f)), dir), out hitMid, raycastDistance, LayerMask.GetMask("mlsidewalls")))
            {
                if (hitMid.distance < distMid)
                {
                    distMid = hitMid.distance;
                }
            }

            Debug.DrawRay(transform.position + forw * 2.5f + new Vector3(0f, 0.5f, 0f), dir.normalized * distMid, Color.green);

            vecs[i] = distMid;
            AddVectorObs(distMid / raycastDistance);
        }
    }

    private bool firstTime = true;

    public override void AgentReset()
    {
        PassedCheckpoints = 0;

        episodeCounter = 0f;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().velocity = Vector3.zero;

        if (false && firstTime)
        {
            transform.rotation = startRot;
            transform.position = startPos;
        }
        else
        {
            Transform[] destTrans = allCheckpoints.RandomCPPos;
            transform.forward = destTrans[1].GetComponent<MeshCollider>().bounds.center - destTrans[0].GetComponent<MeshCollider>().bounds.center;
            transform.position = destTrans[0].GetComponent<MeshCollider>().bounds.center + new Vector3(0f, 2f, 0f);
        }



        allCheckpoints.ResetAll();

        collided = false;
        oldCheckpoints = 0;
        oldNegCheckpoints = 0;

        noRewFor = 0f;

    }
}
