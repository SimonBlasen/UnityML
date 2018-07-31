using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Timekeeper timeKeeper = null;

    private bool triggered = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public bool Triggered
    {
        get
        {
            return triggered;
        }
    }

    public void ResetCheckpoint()
    {
        triggered = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Playercar")
        {
            triggered = true;
            if (timeKeeper != null)
            {
                timeKeeper.RestartCurrentLap();
            }
        }
    }


}
