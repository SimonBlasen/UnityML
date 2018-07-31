using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;


public class SpringakeAgent : Agent
{
    private ExecMode execMode = ExecMode.Current;


    [SerializeField]
    private HingeJoint[] hingeJoints;
    [SerializeField]
    private Transform[] blocks;
    [SerializeField]
    private GameObject[] parcours;
    [SerializeField]
    private float motorVelocity = 10f;
    [SerializeField]
    private float motorForce = 50f;
    [SerializeField]
    private Camera connectedCam;


    private Vector3[] startPoss;
    private Quaternion[] startRots;

    [SerializeField]
    private Transform[] children = null;

    private float maxTime = 300f;

    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < hingeJoints.Length; i++)
        {
            JointMotor mot = hingeJoints[i].motor;
            mot.force = motorForce;

            hingeJoints[i].motor = mot;
        }

        if (execMode == ExecMode.Snake003)
        {
            maxTime = 50f;
        }

       // children = GetComponentsInChildren<Transform>();
        
        startPoss = new Vector3[children.Length + 1];
        startPoss[0] = transform.position;
        for (int i = 0; i < children.Length; i++)
        {
            Debug.Log("Name[" + i + "]: " + children[i].name);

            startPoss[i + 1] = children[i].localPosition;
        }
        startRots = new Quaternion[children.Length + 1];
        startRots[0] = transform.rotation;
        for (int i = 0; i < children.Length; i++)
        {
            startRots[i + 1] = children[i].localRotation;
        }



        float mid = 0f;
        for (int i = 0; i < children.Length; i++)
        {
            mid += children[i].position.z;
        }

        mid /= children.Length;

        lastZMid = mid;
    }

    private float timeP = 0f;

    private int parcoursHeightIndex = 0;

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Done();
        }
        /*if (Input.GetKeyDown(KeyCode.Q))
        {
            parcoursHeightIndex++;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            parcoursHeightIndex--;
        }*/

        timeP += Time.deltaTime;
	    if (connectedCam != null)
        {
            connectedCam.transform.position = new Vector3(connectedCam.transform.position.x, connectedCam.transform.position.y, lastZMid);
        }
	}

    private float[] hingeForces = new float[0];

    private float lastZMid = 0f;

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (hingeForces.Length != vectorAction.Length)
        {
            hingeForces = new float[vectorAction.Length];
        }

        for (int i = 0; i < hingeForces.Length; i++)
        {
            hingeForces[i] = Mathf.Clamp(vectorAction[i], -1f, 1f);

            JointMotor mot = hingeJoints[i].motor;
            mot.targetVelocity = motorVelocity * hingeForces[i];

            hingeJoints[i].motor = mot;
        }
        /*
        float mid = 0f;
        for (int i = 0; i < children.Length; i++)
        {
            mid += children[i].position.z;
        }

        mid /= children.Length;
        */

        float mid = children[children.Length - 1].position.z;
        SetReward((mid - lastZMid) * 10f);

        //Debug.Log("Reward given: " + ((mid - lastZMid) * 100f).ToString());
        //Debug.Log("Reward acc: " + GetCumulativeReward().ToString());

        lastZMid = mid;


        if (timeP >= maxTime)
        {
            SetReward(-1f);
            Done();
        }
        else if (children[1].position.z >= 20f)
        {
            if (execMode != ExecMode.Snake003)
            {
                SetReward(0.2f);
                Done();
            }
        }
        else if (children[children.Length - 9].position.z > children[children.Length - 1].position.z + 0.4f)
        {
            //Aufm Kopf
            //SetReward(-1f);
            //Done();
        }
        else
        {
            //AddReward(-0.05f);
        }
    }

    public override void CollectObservations()
    {
        for (int i = 0; i < hingeJoints.Length; i++)
        {
            AddVectorObs(hingeJoints[i].angle / 180f);
            //Debug.Log("Angle[" + i + "]: " + hingeJoints[i].angle);
        }

        for (int i = 0; i < blocks.Length; i++)
        {
            float transAngle = blocks[i].rotation.eulerAngles.x;
            if (transAngle >= 180)
            {
                transAngle = transAngle - 360f;
            }


            if (execMode == ExecMode.Snake003)
            {
                transAngle /= 180f;
                AddVectorObs(transAngle);
            }
            else if (execMode == ExecMode.Current)
            {
                transAngle = (transAngle * Mathf.PI) / 180f;

                float sin = Mathf.Sin(transAngle);
                float cos = Mathf.Cos(transAngle);


                AddVectorObs(sin);
                AddVectorObs(cos);



                if (i == 0)
                {
                    Debug.Log("Angle[" + i + "]: " + sin + "," + cos);
                }
            }
            
        }







        Vector3 addition = new Vector3(0f, 0.1f, 0f);

        for (int i = 0; i < 1; i++)
        {

            float raycastMaxDistance = 0.7f;

            Vector3 dir = new Vector3(0f, -0.2f, 1f);


            if (execMode == ExecMode.Snake003)
            {
                dir = new Vector3(0f, -0f, 1f);
                raycastMaxDistance = 3f;
            }
            else if (execMode == ExecMode.Current)
            {

            }




            RaycastHit hitMid;

            float distance = raycastMaxDistance;

            if (Physics.Raycast(new Ray(children[children.Length - 1 - i * 2].position + addition * (i + 1), dir), out hitMid, raycastMaxDistance, LayerMask.GetMask("mlsidewalls")))
            {
                distance = hitMid.distance;
            }

            distance /= raycastMaxDistance;

            //distance = 1f;

            if (execMode == ExecMode.Snake003)
            {
                //AddVectorObs(1f);
            }
            else if (execMode == ExecMode.Current)
            {
                AddVectorObs(1f - distance);
            }

            Debug.DrawRay(children[children.Length - 1 - i * 2].position + addition * (i + 1), dir.normalized * distance * raycastMaxDistance, Color.green);
        }



    }

    public override void AgentReset()
    {
        int parc = Random.Range(0, parcours.Length);
        //parc = parcoursHeightIndex + 13;

        if (execMode == ExecMode.Snake003)
        {
            parc = 0;
        }

        for (int i = 0; i < parcours.Length; i++)
        {
            if (parc == i)
            {
                parcours[i].SetActive(true);
            }
            else
            {
                parcours[i].SetActive(false);
            }
        }


        transform.position = startPoss[0];
        for (int i = 0; i < children.Length; i++)
        {
            children[i].localPosition = startPoss[i + 1];
        }

        transform.rotation = startRots[0];
        for (int i = 0; i < children.Length; i++)
        {
            children[i].localRotation = startRots[i + 1];
        }

        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].GetComponent<Rigidbody>() != null)
            {
                children[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                children[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
        }

        timeP = 0f;
    }
}
