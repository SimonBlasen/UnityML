using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRenderer : MonoBehaviour
{
    [SerializeField]
    private int resolution = 32;

    private List<GameObject> instLinesLeft = new List<GameObject>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RefreshLines(Vector3 middlepoint, float radius)
    {
        for (int i = 0; i < resolution; i++)
        {
            float s = (((float)i) / resolution) * 2f * Mathf.PI;
            float s2 = (((float)i + 1) / resolution) * 2f * Mathf.PI;

            doLine(middlepoint + (new Vector3(Mathf.Sin(s), 0f, Mathf.Cos(s))) * radius, middlepoint + (new Vector3(Mathf.Sin(s2), 0f, Mathf.Cos(s2))) * radius, i, Color.white);

        }
    }

    public bool Clockwise
    {
        get
        {
            return GetComponent<InteractiveCheckpoint>().ParamDistancePrev > 20f;
        }
    }

    private void doLine(Vector3 startPos, Vector3 endPos, int counter, Color color)
    {
            if (counter >= instLinesLeft.Count)
            {
                GameObject line = new GameObject();
                line.layer = 9;
                line.transform.SetParent(transform);
                line.transform.position = startPos;
                line.AddComponent<LineRenderer>();
                LineRenderer lr = line.GetComponent<LineRenderer>();
                lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
                lr.SetColors(color, color);
                lr.SetWidth(0.5f, 0.5f);
                lr.SetPosition(0, startPos);
                lr.SetPosition(1, endPos);
                instLinesLeft.Add(line);
            }
            else
            {
                GameObject line = instLinesLeft[counter];
                line.transform.position = startPos;
                LineRenderer lr = line.GetComponent<LineRenderer>();
                lr.SetColors(color, color);
                lr.SetPosition(0, startPos);
                lr.SetPosition(1, endPos);
            }
        
    }
}
