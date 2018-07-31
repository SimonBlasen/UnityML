using MLAgents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPlatformAgent : Agent
{
    public GameObject ball;
    public Vector3 ballStartMin;
    public Vector3 ballStartMax;
    public GameObject parentObj;

    public bool platformNo1 = false;

    private Quaternion startRotation;
    private Vector3 startPosition;

    // Use this for initialization
    void Start()
    {
        startRotation = transform.rotation;
        startPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startPosition;

    }

    private bool leftBefore = true;

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float actionZ = 160f * Mathf.Clamp(vectorAction[0], -1f, 1f);

        JointMotor jm = new JointMotor();
        jm.force = 10000;
        jm.targetVelocity = actionZ;

        GetComponent<HingeJoint>().motor = jm;


        /*if (transform.eulerAngles.z + actionZ > 35f && transform.eulerAngles.z + actionZ < 90f)
        {
            transform.rotation = Quatdernion.Euler(0f, startRotation.eulerAngles.y, 35f);
        }
        else if (transform.eulerAngles.z + actionZ < 270f && transform.eulerAngles.z + actionZ >= 180f)
        {
            transform.rotation = Quaternion.Euler(0f, startRotation.eulerAngles.y, 270f);
        }
        else
        {
            transform.Rotate(new Vector3(0f, 0f, 1f), actionZ);
        }*/

        /*if (platform2.transform.eulerAngles.z + actionZ2 > 90f)
        {
            platform2.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        else if (platform2.transform.eulerAngles.z + actionZ2 < -35f)
        {
            platform2.transform.rotation = Quaternion.Euler(0f, 0f, -35f);
        }
        else
        {
            platform2.transform.Rotate(new Vector3(0f, 0f, 1f), actionZ2);
        }*/

        if (ball.transform.position.y + 5f < transform.position.y)
        {
            Done();
            SetReward(-1f);
        }

        else if (IsDone() == false)
        {
            if (platformNo1)
            {
                if (ball.transform.localPosition.x > 1f && leftBefore)
                {
                    leftBefore = false;
                    SetReward(0.1f);
                }
                else if (ball.transform.localPosition.x < -1f && leftBefore == false)
                {
                    leftBefore = true;
                }
            }
            else
            {
                if (ball.transform.localPosition.x > 1f && leftBefore)
                {
                    leftBefore = false;
                }
                else if (ball.transform.localPosition.x < -1f && leftBefore == false)
                {
                    leftBefore = true;
                    SetReward(0.1f);
                }
            }
        }

    }

    public override void CollectObservations()
    {
        AddVectorObs(transform.rotation.z);
        AddVectorObs((platformNo1 ? ball.transform.position.x : -ball.transform.position.x) - transform.position.x);
        AddVectorObs(ball.transform.position.y - transform.position.y);
        AddVectorObs((platformNo1 ? ball.GetComponent<Rigidbody>().velocity.x : -ball.GetComponent<Rigidbody>().velocity.x));
        AddVectorObs(ball.GetComponent<Rigidbody>().velocity.y);
        AddVectorObs(ball.GetComponent<Rigidbody>().angularVelocity.z);
    }

    public override void AgentReset()
    {
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        transform.rotation = startRotation;

        if (platformNo1)
        {
            ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
            ball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            Vector3 startPos = Vector3.zero;
            startPos.x = Random.Range(ballStartMin.x, ballStartMax.x);
            startPos.y = Random.Range(ballStartMin.y, ballStartMax.y);
            startPos.z = Random.Range(ballStartMin.z, ballStartMax.z);

            ball.transform.position = startPos + parentObj.transform.position;
        }
    }
}
