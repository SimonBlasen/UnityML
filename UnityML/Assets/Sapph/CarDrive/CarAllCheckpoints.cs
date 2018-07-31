using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAllCheckpoints : MonoBehaviour {

    private CarCheckpoint[] allCheckpoints = null;

	// Use this for initialization
	void Start ()
    {
        if (allCheckpoints == null)
            allCheckpoints = GetComponentsInChildren<CarCheckpoint>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        int counter = 0;

		for (int i = 0; i < allCheckpoints.Length; i++)
        {
            if (allCheckpoints[i].Activated)
            {
                counter++;
            }
            if (allCheckpoints[i].ActivationAmount > 1)
            {
                allCheckpoints[i].ActivationAmount--;
                AmountNegActivated++;
            }
        }

        AmountActivated = counter;
	}

    public bool AreAll
    {
        get
        {
            return AmountActivated == allCheckpoints.Length;
        }
    }

    public void ResetAll()
    {
        for (int i = 0; i < allCheckpoints.Length; i++)
        {
            allCheckpoints[i].ResetCP();
        }

        AmountActivated = 0;
        AmountNegActivated = 0;
    }

    public Transform[] RandomCPPos
    {
        get
        {
            if (allCheckpoints == null)
            {
                Start();
            }
            int rand = Random.Range(0, allCheckpoints.Length - 1);
            int rand8 = (rand + 8) % allCheckpoints.Length;
            int rand2 = 0;

            float minDist = float.MaxValue;
            for (int i = 0; i < allCheckpoints.Length; i++)
            {
                if (i != rand)
                {
                    if (Vector3.Distance(allCheckpoints[rand].GetComponent<MeshCollider>().bounds.center, allCheckpoints[i].GetComponent<MeshCollider>().bounds.center) < minDist
                        /*&& Vector3.Angle(allCheckpoints[rand8].GetComponent<MeshCollider>().bounds.center - allCheckpoints[rand].GetComponent<MeshCollider>().bounds.center, allCheckpoints[i].GetComponent<MeshCollider>().bounds.center - allCheckpoints[rand].GetComponent<MeshCollider>().bounds.center) < 70f*/)
                    {
                        minDist = Vector3.Distance(allCheckpoints[rand].GetComponent<MeshCollider>().bounds.center, allCheckpoints[i].GetComponent<MeshCollider>().bounds.center);
                        rand2 = i;
                    }
                }
            }

            return new Transform[] { allCheckpoints[rand].transform, allCheckpoints[rand2].transform };
        }
    }

    public int AmountActivated { get; protected set; }
    public int AmountNegActivated { get; set; }
}
