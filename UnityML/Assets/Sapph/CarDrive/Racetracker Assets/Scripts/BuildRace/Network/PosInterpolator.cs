using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosInterpolator : MonoBehaviour
{
    public float factor = 1f;

    Vector3[] poss = new Vector3[255];
    Vector3[] possOld = new Vector3[255];
    Vector3[] rots = new Vector3[255];
    Vector3[] vels = new Vector3[255];
    float[] durs = new float[255];

    //public Vector3 vel0 = Vector3.zero;
    //public Vector3 vel1 = Vector3.zero;

    //public float distance0 = 0f;
    //public float distance1 = 0f;


    private void Start()
    {
        for (int i = 0; i < poss.Length; i++)
        {
            poss[i] = Vector3.zero;
            possOld[i] = Vector3.zero;
            rots[i] = Vector3.zero;
            vels[i] = Vector3.zero;
            durs[i] = 1f;
        }
    }

    private void Update()
    {
        for (int i = 0; i < poss.Length; i++)
        {
            poss[i] += vels[i] * Time.deltaTime * factor;
        }
        //vel0 = vels[0];
        //vel1 = vels[1];
    }

    private void FixedUpdate()
    {
    }



    public void AddInformation(byte id, Vector3 position, Vector3 rotation, Vector3 velocity)
    {
        /*if (id == 0)
        {
            distance0 = Vector3.Distance(position, possOld[0]);
        }
        if (id == 1)
        {
            distance1 = Vector3.Distance(position, possOld[1]);
        }*/

        if (possOld[id] != position)
        {
            Debug.Log("Distance: " + Vector3.Distance(position, poss[id]));
            poss[id] = position;
            rots[id] = rotation;
            vels[id] = velocity;
            possOld[id] = position;
        }
    }

    public Vector3 GetInfoPosition(byte id)
    {
        return poss[id];
    }

    public Vector3 GetInfoRotation(byte id)
    {
        return rots[id];
    }
}
