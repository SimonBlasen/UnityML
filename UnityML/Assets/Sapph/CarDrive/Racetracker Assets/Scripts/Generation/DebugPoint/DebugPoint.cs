using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPoint : MonoBehaviour
{
    [SerializeField]
    private TextMesh textMesh;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public string Text
    {
        get
        {
            return textMesh.text;
        }
        set
        {
            textMesh.text = value;
        }
    }
}
