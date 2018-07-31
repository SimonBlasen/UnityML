using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSeedsElement : MonoBehaviour
{
    public Text textSeed;
    public Text texthcSpeed;
    public Text texthcCurv;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void ButtonRenderClick()
    {
        GenerationRenderer.Render(Track);

        for (int i = 0; i < GenerationRenderer.supplyers.Length; i++)
        {
            GenerationRenderer.supplyers[i].Track = Track;
            GenerationRenderer.supplyers[i].TrackUpdated();
        }
    }

    private long seed;

    public long Seed
    {
        get
        {
            return seed;
        }
        set
        {
            seed = value;

            textSeed.text = seed.ToString();
        }
    }

    private float hcSeed;

    public float IntrstValue
    {
        get
        {
            return HCSpeed + HCCurvature;
        }
    }

    public float HCSpeed
    {
        get
        {
            return hcSeed;
        }
        set
        {
            hcSeed = value;

            texthcSpeed.text = hcSeed.ToString();
        }
    }

    private float hcCurvature;

    public float HCCurvature
    {
        get
        {
            return hcCurvature;
        }
        set
        {
            hcCurvature = value;

            texthcCurv.text = hcCurvature.ToString();
        }
    }

    public GeneratedTrack Track { get; set; }

    public GenerationRenderer GenerationRenderer { get; set; }
}
