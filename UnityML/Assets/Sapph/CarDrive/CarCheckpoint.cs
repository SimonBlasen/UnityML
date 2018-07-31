using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCheckpoint : MonoBehaviour {


    public Material activatedMaterial;
    public Material deActivatedMaterial;
    private bool isActivated = false;

    public bool Activated
    {
        get
        {
            return isActivated;
        }
        protected set
        {
            isActivated = value;

            if (isActivated)
            {
                GetComponent<MeshRenderer>().material = activatedMaterial;
            }
            else
            {
                GetComponent<MeshRenderer>().material = deActivatedMaterial;
            }
        }
    }


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ResetCP()
    {
        ActivationAmount = 0;
        Activated = false;
        deadzone = 3;
    }

    public int ActivationAmount { get; set; }

    private int deadzone = 3;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Car")
        {
            if (deadzone == 0)
            {
                ActivationAmount++;
            }
            else if (deadzone == 3)
            {
                ActivationAmount++;
                Activated = true;
                deadzone--;
            }
            else
            {
                deadzone--;
            }

        }
    }
}
