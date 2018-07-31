using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleShootingMinigun : MonoBehaviour {

    public Vector3 velocity = Vector3.zero;
    public float destroyTime = 3f;    

	// Use this for initialization
	void Start () {
        Destroy(gameObject, destroyTime);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.position += Time.deltaTime * velocity;
	}
}
