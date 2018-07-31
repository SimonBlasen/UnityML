using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGoal : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!activated)
        {
            activated = true;
            SnakeAgent.ReachedGoal();
        }
    }

    public SnakeAgentSmart SnakeAgent
    {
        get; set;
    }
}
