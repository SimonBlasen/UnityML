using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private TextMesh textMesh;

    private GameObject lookAt = null;

    private bool visible = true;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (lookAt != null)
        {
            transform.LookAt(lookAt.transform);
        }
	}

    public string ShowText
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

    public GameObject LookAt
    {
        get
        {
            return lookAt;
        }
        set
        {
            lookAt = value;
        }
    }

    public bool Visibility
    {
        get
        {
            return visible;
        }
        set
        {
            visible = value;
            for (int i = 0; i < GetComponentsInChildren<MeshRenderer>().Length; i++)
            {
                GetComponentsInChildren<MeshRenderer>()[i].enabled = visible;
            }
        }
    }
}
