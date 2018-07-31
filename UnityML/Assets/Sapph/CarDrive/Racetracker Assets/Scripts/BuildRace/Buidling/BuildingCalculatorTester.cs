using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCalculatorTester : MonoBehaviour
{
    private AnalyzerJob analyzerJob;
    private CalculatedCar car;

    private bool isRunning = false;

	// Use this for initialization
	void Start ()
    {
        analyzerJob = new AnalyzerJob();
        car = null;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (isRunning && analyzerJob.IsDone)
        {
            isRunning = false;
            car = analyzerJob.car;

            if (car != null)
            {
                Debug.Log("Succ calculated car");
            }
        }
	}

    public CalculatedCar Car
    {
        get
        {
            return car;
        }
    }

    public void GoCalculate(byte[] bytes)
    {
        if (analyzerJob.IsDone || isRunning == false)
        {
            analyzerJob = new AnalyzerJob();
            analyzerJob.c = bytes;
            analyzerJob.Start();
            isRunning = true;
        }
    }
}
