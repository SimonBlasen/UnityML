using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPlatformBouncyAgent : Agent
{
    public GameObject ball;
    public Vector3 ballStartMin;
    public Vector3 ballStartMax;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float actionZ = 2f * Mathf.Clamp(vectorAction[0], -1f, 1f);
        float actionX = 2f * Mathf.Clamp(vectorAction[1], -1f, 1f);


        transform.Rotate(new Vector3(0f, 0f, 1f), actionZ);
        transform.Rotate(new Vector3(1f, 0f, 0f), actionX);

        if (ball.transform.position.y + 5f < transform.position.y)
        {
            Done();
            SetReward(-1f);
        }

        else if (IsDone() == false)
        {
            SetReward(0.1f);
        }

    }

    public override void CollectObservations()
    {
        AddVectorObs(transform.rotation.z);
        AddVectorObs(transform.rotation.x);
        AddVectorObs(ball.transform.position.x - transform.position.x);
        AddVectorObs(ball.transform.position.y - transform.position.y);
        AddVectorObs(ball.transform.position.z - transform.position.z);
        AddVectorObs(ball.GetComponent<Rigidbody>().velocity.x);
        AddVectorObs(ball.GetComponent<Rigidbody>().velocity.y);
        AddVectorObs(ball.GetComponent<Rigidbody>().velocity.z);
    }

    public override void AgentReset()
    {
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.rotation = Quaternion.identity;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

        Vector3 startPos = Vector3.zero;
        startPos.x = Random.Range(ballStartMin.x, ballStartMax.x);
        startPos.y = Random.Range(ballStartMin.y, ballStartMax.y);
        startPos.z = Random.Range(ballStartMin.z, ballStartMax.z);

        ball.transform.position = startPos + transform.position;
    }
}
