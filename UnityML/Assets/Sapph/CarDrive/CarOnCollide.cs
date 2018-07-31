using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarOnCollide : MonoBehaviour {

    public CarDriveAgent carAgent;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collide");
        carAgent.YouCollided();
    }
}
