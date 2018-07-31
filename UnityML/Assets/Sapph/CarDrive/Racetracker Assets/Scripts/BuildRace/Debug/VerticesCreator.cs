using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class VerticesCreator : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Transform[] childs = GetComponentsInChildren<Transform>();

        string[] lines = new string[childs.Length - 1];

        for (int i = 0; i < lines.Length; i++)
        {
            lines[i] = "verts.Add(rotAxis * rotQuat * ((new Vector3(" + childs[i + 1].position.x + "f, " + childs[i + 1].position.y + "f, " + childs[i + 1].position.z + "f)) * fac));";
        }


        File.WriteAllLines(".\\vertices.txt", lines);

        Debug.Log("Created File!");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
