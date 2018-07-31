using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    [SerializeField]
    private GameObject[] cameras;
    [SerializeField]
    private float scrollSpeed = 1f;
    [SerializeField]
    private float dragSpeed = 1f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].GetComponent<Camera>().orthographicSize -= scrollSpeed;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            for (int i = 0; i < cameras.Length; i++)
            {
                cameras[i].GetComponent<Camera>().orthographicSize += scrollSpeed;
            }
        }

        if (Input.GetMouseButton(1))
        {
            //for (int i = 0; i < cameras.Length; i++)
            //{
                cameras[0].transform.position += new Vector3(Input.GetAxis("Mouse X") * dragSpeed * (cameras[0].GetComponent<Camera>().orthographicSize), 0f, Input.GetAxis("Mouse Y") * dragSpeed * (cameras[0].GetComponent<Camera>().orthographicSize));
            //}

            //Debug.Log(new Vector3(Input.GetAxis("Mouse X"), 0f, Input.GetAxis("Mouse Y")));
        }
    }
}
