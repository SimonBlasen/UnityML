using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoVisualizer : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabBar;

    private float width = 512f;
    private float height = 256f;

    private List<GameObject> instBars = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
        width = GetComponent<RectTransform>().sizeDelta.x;
        height = GetComponent<RectTransform>().sizeDelta.y;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void RenderProfile(HistoProfile profile)
    {
        for (int i = 0; i < instBars.Count; i++)
        {
            Destroy(instBars[i]);
        }

        instBars.Clear();

        for (int i = 0; i < profile.Percents.Length; i++)
        {
            GameObject instBar = Instantiate(prefabBar, transform);

            instBar.GetComponent<RectTransform>().sizeDelta = new Vector2(width / profile.Percents.Length, profile.Percents[i] * height);
            instBar.GetComponent<RectTransform>().localPosition = new Vector3((width * -0.5f) + i * (width / profile.Percents.Length) + (width / profile.Percents.Length) * 0.5f, profile.Percents[i] * height * 0.5f - height * 0.5f, 0f);

            instBars.Add(instBar);
        }
    }
}
