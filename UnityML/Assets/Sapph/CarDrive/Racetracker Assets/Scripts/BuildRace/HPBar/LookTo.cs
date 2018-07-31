using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTo : MonoBehaviour {

    public string gameobjectToLook;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (gameobjectToLook != null && gameobjectToLook.Length > 0 && GameObject.Find(gameobjectToLook) != null)
        {
            transform.LookAt(GameObject.Find(gameobjectToLook).transform);
        }
	}
}
