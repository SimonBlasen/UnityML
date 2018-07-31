using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;


public class SnakeAgentSmart : Agent
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

    public GameObject debugTurner;
    public GameObject debugTurner2;

    public Material flippedMat;
    public Material notFlippedMat;

    private float maxTime = 30f;

    // Use this for initialization
    void Start ()
    {
        if (brain.brainType == BrainType.Internal)
        {
            maxTime = 400f;
        }

        for (int i = 0; i < hingeJoints.Length; i++)
        {
            JointMotor mot = hingeJoints[i].motor;
            mot.force = motorForce;

            hingeJoints[i].motor = mot;
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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //parcoursHeightIndex++;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            //parcoursHeightIndex--;
        }

        timeP += Time.deltaTime;
	    if (connectedCam != null)
        {
            connectedCam.transform.position = new Vector3(connectedCam.transform.position.x, connectedCam.transform.position.y, lastZMid);
        }



        Vector3 sphereFrontPos = flipped ? children[children.Length - 9].position : children[children.Length - 1].position;
        Vector3 sphereBackPos = flipped ? children[children.Length - 1].position : children[children.Length - 9].position;

        if (sphereFrontPos.z < sphereBackPos.z)
        {
            flipTime += Time.deltaTime;

            if (flipTime >= 1.8f)
            {
                Debug.Log("Flipped");
                flipped = !flipped;
                flipTime = 0f;

                if (flipped)
                {
                    debugTurner.GetComponentInChildren<MeshRenderer>().sharedMaterial = flippedMat;
                }
                else
                {
                    debugTurner.GetComponentInChildren<MeshRenderer>().sharedMaterial = notFlippedMat;
                }
            }
        }
        else
        {
            flipTime = 0f;
        }

        lastZsCounter += Time.deltaTime;
        if (lastZsCounter >= 0.4f)
        {
            lastZs[lastZsIndex] = lastZMid;

            lastZsCounter = 0f;
            lastZsIndex++;
            lastZsIndex = lastZsIndex % lastZs.Length;


            float minZ = float.MaxValue;
            float maxZ = float.MinValue;
            for (int i = 0; i < lastZs.Length; i++)
            {
                if (lastZs[i] < minZ)
                {
                    minZ = lastZs[i];
                }
                if (lastZs[i] > maxZ)
                {
                    maxZ = lastZs[i];
                }
            }

            if (maxZ - minZ < 0.1f)
            {
                notMoving = true;
            }
        }
    }

    private bool notMoving = false;

    private float lastZsCounter = 0f;

    public void YouBroke()
    {
        SetReward(-5f);
        Done();
    }

    private float[] hingeForces = new float[0];

    private float lastZMid = 0f;

    private int checkpointsAcc = 0;
    private bool inGoal = false;

    public void ReachedCheckpoint()
    {
        checkpointsAcc++;
    }
    public void ReachedGoal()
    {
        inGoal = true;
    }

    private bool flipped = false;
    private float flipTime = 0f;

    private float[] lastZs = new float[20];
    private int lastZsIndex = 0;

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (hingeForces.Length != vectorAction.Length)
        {
            hingeForces = new float[vectorAction.Length];
        }

        for (int i = 0; i < hingeForces.Length; i++)
        {
            int index = flipped ? hingeForces.Length - 1 - i : i;
            float fac = flipped ? -1f : 1f;

            hingeForces[index] = Mathf.Clamp(vectorAction[i], -1f, 1f);

            JointMotor mot = hingeJoints[index].motor;
            mot.targetVelocity = motorVelocity * hingeForces[index] * fac;

            hingeJoints[index].motor = mot;
        }
        /*
        float mid = 0f;
        for (int i = 0; i < children.Length; i++)
        {
            mid += children[i].position.z;
        }

        mid /= children.Length;
        */

        Vector3 sphereFrontPos = flipped ? children[children.Length - 9].position : children[children.Length - 1].position;
        float mid = sphereFrontPos.z;

        float littleRew = (mid - lastZMid) * 1f;
        

        //Debug.Log("Reward given: " + ((mid - lastZMid) * 100f).ToString());
        //Debug.Log("Reward acc: " + GetCumulativeReward().ToString());

        lastZMid = mid;


        int amountBlocksInverse = 0;
        for (int i = 0; i < blocks.Length; i++)
        {
            float transAngle = blocks[i].rotation.eulerAngles.x;
            if (transAngle >= 180)
            {
                transAngle = transAngle - 360f;
            }
            
            if (transAngle >= 170 || transAngle <= -170)
            {
                amountBlocksInverse++;
            }
        }

        if (amountBlocksInverse >= 2)
        {
            SetReward(-1f);
            Done();
        }






        if (timeP >= maxTime)
        {
            SetReward(-1f);
            Done();
        }
        else if (inGoal)
        {
            SetReward(5f);
            Done();
        }
        else if (checkpointsAcc > 0)
        {
            SetReward((((float)checkpointsAcc) / checkpointsAmount) * 50f);

            checkpointsAcc = 0;
        }
        else if (notMoving)
        {
            notMoving = false;
            SetReward(-5f);
            Done();
        }
        else
        {
            SetReward(littleRew);
        }
    }

    private int checkpointsAmount = 0;

    public override void CollectObservations()
    {
        for (int i = 0; i < hingeJoints.Length; i++)
        {
            int index = flipped ? hingeJoints.Length - 1 - i : i;
            float fac = flipped ? -1f : 1f;

            AddVectorObs(hingeJoints[index].angle / 180f * fac);
            //Debug.Log("Angle[" + i + "]: " + hingeJoints[i].angle);
        }

        for (int i = 0; i < blocks.Length; i++)
        {
            int index = flipped ? blocks.Length - 1 - i : i;
            float fac = flipped ? -1f : 1f;

            float transAngle = blocks[index].rotation.eulerAngles.x * fac;
            if (transAngle >= 180)
            {
                transAngle = transAngle - 360f;
            }

            transAngle /= 180f;
            AddVectorObs(transAngle);

        }


        float frontDownSurfaceY = 0f;



        Vector3 sphereFrontPos = flipped ? children[children.Length - 9].position : children[children.Length - 1].position;
        Vector3 sphereBackPos = flipped ? children[children.Length - 1].position : children[children.Length - 9].position;


        float raycastMaxDistance = 2f;

        Vector3 normalDown = new Vector3(0f, 1f, 0f);
        float distanceDown = raycastMaxDistance;
        RaycastHit hitDown;
        if (Physics.Raycast(new Ray(sphereFrontPos, new Vector3(0f, -1f, 0f)), out hitDown, raycastMaxDistance, LayerMask.GetMask("mlsidewalls")))
        {
            distanceDown = hitDown.distance;
            normalDown = hitDown.normal;
            frontDownSurfaceY = hitDown.point.y;
        }
        Debug.DrawRay(sphereFrontPos, (new Vector3(0f, -1f, 0f)).normalized * distanceDown, Color.green);

        Vector3 normalFront = new Vector3(0f, 1f, 0f);
        float distanceFront = raycastMaxDistance;
        RaycastHit hitFront;
        if (Physics.Raycast(new Ray(sphereFrontPos, new Vector3(0f, 0f, 1f)), out hitFront, raycastMaxDistance, LayerMask.GetMask("mlsidewalls")))
        {
            distanceFront = hitFront.distance;
            normalFront = hitFront.normal;
        }
        Debug.DrawRay(sphereFrontPos, (new Vector3(0f, 0f, 1f)).normalized * distanceFront, Color.green);

        distanceFront /= raycastMaxDistance;
        distanceDown /= raycastMaxDistance;

        distanceFront = 1f - distanceFront;
        distanceDown = 1f - distanceDown;

        if (distanceDown + distanceFront != 0f)
        {
            distanceFront /= (distanceFront + distanceDown);
            distanceDown /= (distanceFront + distanceDown);
        }

        Vector3 summedNormal = normalFront.normalized * distanceFront + normalDown.normalized * distanceDown;

        float cos = Mathf.Asin(summedNormal.z);
        cos = (cos / Mathf.PI) * 2f;

        //Debug.Log("Cos: " + cos.ToString());


        debugTurner.transform.rotation = Quaternion.Euler(cos * 90f, 0f, 0f);

        AddVectorObs(cos);









        float backDownSurfaceY = 0f;

        distanceDown = raycastMaxDistance;
        if (Physics.Raycast(new Ray(sphereBackPos, new Vector3(0f, -1f, 0f)), out hitDown, raycastMaxDistance, LayerMask.GetMask("mlsidewalls")))
        {
            backDownSurfaceY = hitDown.point.y;
        }

        AddVectorObs((frontDownSurfaceY - backDownSurfaceY) * 3f);

        //Debug.Log("Val: " + (frontDownSurfaceY - backDownSurfaceY) * 3f);

        debugTurner2.transform.rotation = Quaternion.Euler((frontDownSurfaceY - backDownSurfaceY) * 3f * 90f, 0f, 0f);


    }

    private GameObject instParcour = null;

    public override void AgentReset()
    {
        if (instParcour != null)
        {
            Destroy(instParcour);
        }

        int parc = Random.Range(0, parcours.Length);

        instParcour = Instantiate(parcours[parc]);
        instParcour.transform.position = new Vector3(transform.position.x, 0f, 0f);
        for (int i = 0; i < instParcour.GetComponentsInChildren<SnakeCheckpoint>().Length; i++)
        {
            instParcour.GetComponentsInChildren<SnakeCheckpoint>()[i].SnakeAgent = this;
        }
        checkpointsAmount = instParcour.GetComponentsInChildren<SnakeCheckpoint>().Length;

        instParcour.GetComponentInChildren<SnakeGoal>().SnakeAgent = this;


        float zAddRand = Random.Range(0f, 1f);
        Vector3 randAdd = new Vector3(0f, zAddRand, -zAddRand);

        transform.position = startPoss[0];
        for (int i = 0; i < children.Length; i++)
        {
            children[i].localPosition = startPoss[i + 1] + randAdd;
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

        checkpointsAcc = 0;
        inGoal = false;
        notMoving = false;
        timeP = 0f;
    }
}
