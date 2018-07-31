using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class VisualizerProfile : MonoBehaviour
{
    public RectTransform[] bars;
    public Text textTrackName;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetTrackName(string trackname)
    {
        textTrackName.text = trackname;
    }

    public void SetBarValue(int bar, float value)
    {
        value = value < 0f ? 0f : (value > 1f ? 1f : value);
        if (bar >= 0 && bar < bars.Length)
        {
            bars[bar].sizeDelta = new Vector2(100f, value * 1600f);
            bars[bar].anchoredPosition3D = new Vector3(bars[bar].anchoredPosition3D.x, (value * 1600f) * 0.5f, bars[bar].anchoredPosition3D.z);
        }
    }
}
