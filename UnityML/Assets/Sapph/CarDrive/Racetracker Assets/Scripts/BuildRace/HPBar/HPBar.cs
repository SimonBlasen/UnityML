using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour {

    [Header("References")]
    [SerializeField]
    private GameObject planeMaxHealth;
    [SerializeField]
    private GameObject planeCurHealth;

    private float maxHealth = 1f;
    private float curHealth = 1f;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (maxHealth > 0f && curHealth >= 0f && curHealth <= maxHealth)
        {
            setPercentage((curHealth / maxHealth) * 100f);
        }
        else
        {
            setPercentage(100f);
        }
	}

    private void setPercentage(float percentage)
    {
        percentage = percentage < 0f ? 0f : (percentage > 100f ? 100f : percentage);

        float xPos = 0f;
        float xScale = 0.15f;

        xScale = (percentage / 100f) * 0.15f;
        xPos = (1f - (percentage / 100f)) * 0.75f;

        planeCurHealth.transform.localPosition = new Vector3(xPos, 0, 0.001f);
        planeCurHealth.transform.localScale = new Vector3(xScale, 1, 0.02f);
    }

    public float MaxHealth
    {
        get
        {
            return maxHealth;
        }
        set
        {
            maxHealth = value;
        }
    }

    public float CurHealth
    {
        get
        {
            return curHealth;
        }
        set
        {
            curHealth = value;
        }
    }
}
