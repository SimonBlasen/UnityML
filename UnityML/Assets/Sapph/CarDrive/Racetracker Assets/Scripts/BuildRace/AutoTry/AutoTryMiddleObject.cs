using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTryMiddleObject : MonoBehaviour {

    [SerializeField]
    private Transform followTransform;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = followTransform.position;
	}
}
