using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timekeeper : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject car;
    [SerializeField]
    private Checkpoint[] checkpoints;
    [SerializeField]
    public Text currentLapText;
    [SerializeField]
    public Text lastLapText;
    [SerializeField]
    public Text bestLapText;

    private bool roundStarted = false;
    private bool waitingToCrossFinish = false;
    private float curLaptime = 0f;
    private float lastLaptime = 0f;
    private float bestLaptime = 0f;

    // Use this for initialization
    void Start ()
    {
        if (currentLapText != null && lastLapText != null && bestLapText != null)
        {
            currentLapText.text = "--:--.---";
            lastLapText.text = "--:--.---";
            bestLapText.text = "--:--.---";
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        curLaptime += Time.deltaTime;

        if (roundStarted)
        {
            if (currentLapText != null)
            {
                currentLapText.text = floatTimeToString(curLaptime);
            }
        }
        else if (currentLapText != null && lastLapText != null && bestLapText != null && currentLapText.text != "--:--.---")
        {
            currentLapText.text = "--:--.---";
            lastLapText.text = "--:--.---";
            bestLapText.text = "--:--.---";
        }


        if (roundStarted == false && checkpoints[0].Triggered)
        {
            roundStarted = true;
            curLaptime = 0f;
        }
        
        else if (roundStarted && waitingToCrossFinish == false)
        {
            if (allCheckpointsTriggered())
            {
                waitingToCrossFinish = true;
                for (int i = 0; i < checkpoints.Length; i++)
                {
                    checkpoints[i].ResetCheckpoint();
                }
            }
        }

        else if (waitingToCrossFinish)
        {
            if (checkpoints[0].Triggered)
            {
                waitingToCrossFinish = false;

                crossStartFinish();

                roundStarted = true;
                curLaptime = 0f;
            }
        }
	}

    public void RestartCurrentLap()
    {
        if (allCheckpointsTriggered() == false && waitingToCrossFinish == false)
        {
            roundStarted = true;
            curLaptime = 0f;

            for (int i = 1; i < checkpoints.Length; i++)
            {
                checkpoints[i].ResetCheckpoint();
            }
        }
    }

    public void StopCurrentMeasurement()
    {
        curLaptime = 0f;
        currentLapText.text = "--:--.---";
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].ResetCheckpoint();
        }
        roundStarted = false;
        waitingToCrossFinish = false;
    }

    public void ResetAllTimes()
    {
        curLaptime = 0f;
        lastLaptime = 0f;
        bestLaptime = 0f;
        currentLapText.text = "--:--.---";
        lastLapText.text = "--:--.---";
        bestLapText.text = "--:--.---";
        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkpoints[i].ResetCheckpoint();
        }
        roundStarted = false;
        waitingToCrossFinish = false;
    }

    private void crossStartFinish()
    {
        lastLaptime = curLaptime;
        lastLapText.text = floatTimeToString(lastLaptime);
        if (lastLaptime < bestLaptime || bestLaptime == 0f)
        {
            bestLaptime = lastLaptime;
            bestLapText.text = floatTimeToString(lastLaptime);
        }
    }

    private bool allCheckpointsTriggered()
    {
        bool oneNot = false;
        for (int i = 0; i < checkpoints.Length; i++)
        {
            if (checkpoints[i].Triggered == false)
            {
                oneNot = true;
                break;
            }
        }

        return !oneNot;
    }

    private string floatTimeToString(float time)
    {
        string mins = "";
        string secs = "";
        string mili = "";
        mili = System.Convert.ToString((Mathf.Round((time - ((int)time)) * 1000f)));
        secs = System.Convert.ToString((int)(time - ((int)(time / 60f)) * 60f));
        mins = System.Convert.ToString(((int)(time / 60f)));

        if (mili.Length == 0)
        {
            mili = "000";
        }
        else if (mili.Length == 1)
        {
            mili = "00" + mili;
        }
        else if (mili.Length == 2)
        {
            mili = "0" + mili;
        }

        if (secs.Length == 0)
        {
            secs = "00";
        }
        else if (secs.Length == 1)
        {
            secs = "0" + secs;
        }

        return mins + ":" + secs + "." + mili;
    }
}
