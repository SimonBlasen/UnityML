using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCheckpoint : MonoBehaviour {

    public Material cpNotAct;
    public Material cpAct;

    // Use this for initialization
    void Start ()
    {
        GetComponent<MeshRenderer>().sharedMaterial = cpNotAct;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if ((!activated) && (other.name == "Sphere (4)" || other.name == "Sphere"))
        {
            GetComponent<MeshRenderer>().sharedMaterial = cpAct;
            activated = true;
            SnakeAgent.ReachedCheckpoint();
        }
    }

    public SnakeAgentSmart SnakeAgent
    {
        get;set;
    }
}
