using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTryCamera : MonoBehaviour
{
    [SerializeField]
    private Transform middleObject;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 delta = Vector3.zero;
        // X is Side movement
        // Y is up-down movement
        // Z is zooming

		if (Input.GetButton("A"))
        {
            delta.x = 1f;
        }
        if (Input.GetButton("W"))
        {
            delta.y = 1f;
        }
        if (Input.GetButton("D"))
        {
            delta.x = -1f;
        }
        if (Input.GetButton("S"))
        {
            delta.y = -1f;
        }

        int wheel = Input.GetAxis("Mouse ScrollWheel") > 0f ? 1 : (Input.GetAxis("Mouse ScrollWheel") < 0f ? -1 : 0);

        delta.z = wheel;

        middleObject.Rotate(new Vector3(0f, 1f, 0f), delta.x, Space.World);
        middleObject.Rotate(new Vector3(1f, 0f, 0f), delta.y);
        //middleObject.localRotation = Quaternion.Euler(middleObject.localRotation.eulerAngles + new Vector3(delta.y, 0f, 0f));
        transform.localPosition = new Vector3(0f, 0f, delta.z + transform.localPosition.z);
	}
}
