using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRenderer : MonoBehaviour
{
    protected Vector3[] controlPoints;

    protected virtual void refreshMesh()
    {

    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Vector3[] Points
    {
        get
        {
            return controlPoints;
        }
        set
        {
            controlPoints = value;

            refreshMesh();
        }
    }
}
