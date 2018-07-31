using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinesRenderer : MonoBehaviour {

    private List<GameObject> instLinesLeft = new List<GameObject>();
    private List<GameObject> instLinesRight = new List<GameObject>();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RefreshTrack(GeneratedTrack track)
    {
        

        int counterLeft = 0;
        int counterRight = 0;

        for (int i = 0; i < track.Elements.Length; i++)
        {
            if (track.Elements[i].GetType() == typeof(GeneratedBezSpline))
            {
                GeneratedBezSpline spline = (GeneratedBezSpline)track.Elements[i];

                for (int j = 0; j < spline.RenderVertsLeft.Length; j++)
                {
                    Vector3 endPos = j + 1 < spline.RenderVertsLeft.Length ? new Vector3(spline.RenderVertsLeft[j + 1].x, track.GetTensorHeight(spline.RenderVertsLeft[j + 1]) + 1f, spline.RenderVertsLeft[j + 1].z) : track.Elements[(i + 1) % track.Elements.Length].Position;

                    doLine(new Vector3(spline.RenderVertsLeft[j].x, track.GetTensorHeight(spline.RenderVertsLeft[j]) + 1f, spline.RenderVertsLeft[j].z), endPos, true, counterLeft, Color.white);
                    counterLeft++;
                }
                for (int j = 0; j < spline.RenderVertsRight.Length; j++)
                {
                    Vector3 endPos = j + 1 < spline.RenderVertsRight.Length ? new Vector3(spline.RenderVertsRight[j + 1].x, track.GetTensorHeight(spline.RenderVertsRight[j + 1]) + 1f, spline.RenderVertsRight[j + 1].z) : track.Elements[(i + 1) % track.Elements.Length].Position;

                    doLine(new Vector3(spline.RenderVertsRight[j].x, track.GetTensorHeight(spline.RenderVertsRight[j]) + 1f, spline.RenderVertsRight[j].z), endPos, false, counterRight, Color.white);
                    counterRight++;
                }
            }
            else if (track.Elements[i].GetType() == typeof(GeneratedStraight))
            {
                int iPrev = i - 1 < 0 ? track.Elements.Length - 1 : i - 1;
                int iNext = (i + 1) % track.Elements.Length;
                GeneratedBezSpline splinePrev = (GeneratedBezSpline)track.Elements[iPrev];
                GeneratedBezSpline splineNext = (GeneratedBezSpline)track.Elements[iNext];

                Vector3 leftStartPos = splinePrev.RenderVertsLeft[splinePrev.RenderVertsLeft.Length - 1];
                Vector3 rightStartPos = splinePrev.RenderVertsRight[splinePrev.RenderVertsRight.Length - 1];
                Vector3 leftEndPos = splineNext.RenderVertsLeft[0];
                Vector3 rightEndPos = splineNext.RenderVertsRight[0];

                leftStartPos = new Vector3(leftStartPos.x, track.GetTensorHeight(leftStartPos) + 1f, leftStartPos.z);
                rightStartPos = new Vector3(rightStartPos.x, track.GetTensorHeight(rightStartPos) + 1f, rightStartPos.z);
                leftEndPos = new Vector3(leftEndPos.x, track.GetTensorHeight(leftEndPos) + 1f, leftEndPos.z);
                rightEndPos = new Vector3(rightEndPos.x, track.GetTensorHeight(rightEndPos) + 1f, rightEndPos.z);

                doLine(leftStartPos, leftEndPos, true, counterLeft, Color.blue);
                counterLeft++;
                doLine(rightStartPos, rightEndPos, false, counterRight, Color.blue);
                counterRight++;
            }
        }


        for (int i = counterLeft; i < instLinesLeft.Count; i++)
        {
            Destroy(instLinesLeft[i]);
            instLinesLeft.RemoveAt(i);
            i--;
        }
        for (int i = counterRight; i < instLinesRight.Count; i++)
        {
            Destroy(instLinesRight[i]);
            instLinesRight.RemoveAt(i);
            i--;
        }
    }

    private void doLine(Vector3 startPos, Vector3 endPos, bool left, int counter, Color color)
    {
        if (left)
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
        else
        {
            if (counter >= instLinesRight.Count)
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
                instLinesRight.Add(line);
            }
            else
            {
                GameObject line = instLinesRight[counter];
                line.transform.position = startPos;
                LineRenderer lr = line.GetComponent<LineRenderer>();
                lr.SetColors(color, color);
                lr.SetPosition(0, startPos);
                lr.SetPosition(1, endPos);
            }
        }
    }

}

