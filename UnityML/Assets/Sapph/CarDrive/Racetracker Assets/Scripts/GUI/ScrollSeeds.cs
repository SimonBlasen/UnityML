using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollSeeds : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private GameObject scrollContentElement;
    [SerializeField]
    private InputField textStartSeed;
    [SerializeField]
    private GenerationRenderer generationRenderer;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject prefabElement;

    private List<GameObject> instElements = new List<GameObject>();

    private int[] oldSuccAmounts;
    private JobGenerateTrackContin[] jobs = new JobGenerateTrackContin[16];


	// Use this for initialization
	void Start ()
    {
        oldSuccAmounts = new int[jobs.Length];
        for (int i = 0; i < jobs.Length; i++)
        {
            jobs[i] = new JobGenerateTrackContin();
            oldSuccAmounts[i] = 0;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		for (int i = 0; i < jobs.Length; i++)
        {
            if (jobs[i].succTracks.Count > oldSuccAmounts[i])
            {
                AddElement(jobs[i].succTracks[oldSuccAmounts[i]], jobs[i].succTracks[oldSuccAmounts[i]].Seed, jobs[i].succTracks[oldSuccAmounts[i]].SpeedProfile.HC, jobs[i].succTracks[oldSuccAmounts[i]].CurvatureIdeallineProfile.HC);
                oldSuccAmounts[i]++;
            }
        }
	}

    public void ButtonStartClick()
    {
        long startSeed = System.Convert.ToInt64(textStartSeed.text);

        for (int i = 0; i < jobs.Length; i++)
        {
            jobs[i].succTracks.Clear();

            jobs[i].OffsetJump = jobs.Length;
            jobs[i].Seed = startSeed + i;
            jobs[i].Width = generationRenderer.width;
            jobs[i].Terrain = generationRenderer.terrain;
            jobs[i].TerrainModifier = generationRenderer.TerrainModifier;

            jobs[i].Start();
        }
    }

    public void ButtonStopClick()
    {
        for (int i = 0; i < jobs.Length; i++)
        {
            jobs[i].Stop = true;
            jobs[i].Abort();
        }
    }

    public void ButtonClearClick()
    {
        ClearElements();
        for (int i = 0; i < jobs.Length; i++)
        {
            jobs[i] = new JobGenerateTrackContin();
            oldSuccAmounts[i] = 0;
        }
    }

    public void ClearElements()
    {
        for (int i = 0; i < instElements.Count; i++)
        {
            Destroy(instElements[i]);
        }

        instElements.Clear();
    }

    public void AddElement(GeneratedTrack track, long seed, float hcSpeed, float hcCurvature)
    {
        GameObject instEle = Instantiate(prefabElement, scrollContentElement.transform);

        instEle.GetComponent<ScrollSeedsElement>().Seed = seed;
        instEle.GetComponent<ScrollSeedsElement>().HCSpeed = hcSpeed;
        instEle.GetComponent<ScrollSeedsElement>().HCCurvature = hcCurvature;
        instEle.GetComponent<ScrollSeedsElement>().Track = track;
        instEle.GetComponent<ScrollSeedsElement>().GenerationRenderer = generationRenderer;



        instEle.transform.localPosition = new Vector3(3f + 125f, (instElements.Count * -30f) - 25f, 0f);

        instElements.Add(instEle);

        instElements.Sort((x, y) => y.GetComponent<ScrollSeedsElement>().IntrstValue.CompareTo(x.GetComponent<ScrollSeedsElement>().IntrstValue));


        for (int i = 0; i < instElements.Count; i++)
        {
            instElements[i].transform.localPosition = new Vector3(3f + 125f, (i * -30f) - 25f, 0f);

        }





        scrollContentElement.GetComponent<RectTransform>().sizeDelta = new Vector2(0f, instElements.Count * 30f + 60f);
    }
}
