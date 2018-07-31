using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjCreator : MonoBehaviour {

    public MeshFilter toConvertMesh;

    [Space]
    public bool convert = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (convert)
        {
            convert = false;
            Convert();
        }
	}

    public void Convert()
    {
        List<string> lines = new List<string>();
        lines.Add("o Sapptrack");

        for (int i = 0; i < toConvertMesh.mesh.vertices.Length; i++)
        {
            lines.Add("v " + toConvertMesh.mesh.vertices[i].x + " " + toConvertMesh.mesh.vertices[i].y + " " + toConvertMesh.mesh.vertices[i].z);
        }

        lines.Add("s off");

        for (int i = 0; i < toConvertMesh.mesh.triangles.Length; i += 3)
        {
            lines.Add("f " + toConvertMesh.mesh.triangles[i] + " " + toConvertMesh.mesh.triangles[i + 1] + " " + toConvertMesh.mesh.triangles[i + 2]);
        }

        File.WriteAllLines(".\\Sapphiretrack.obj", lines.ToArray());
    }
}
