using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointBreaking : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnJointBreak(float breakForce)
    {
        GetComponentInParent<SnakeAgent>().YouBroke();
    }
}
