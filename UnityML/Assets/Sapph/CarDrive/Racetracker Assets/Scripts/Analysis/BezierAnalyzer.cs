using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BezierAnalyzer : MonoBehaviour
{
    public bool calculateTrack = false;
    [Header("Settings")]
    public string trackname;
    public float maxBinValue = 2f;
    public int sampleRate = 17;
    public bool debugInfo = false;
    [Header("Information")]
    public float[] curvativeBins;
    public int[] curvativeProfile;
    [Header("References")]
    public VisualizerProfile visualizerProfile = null;
    public SappClosedSpline sappSpline = null;
    public SappClosedSpline sappSplineLeft = null;
    public SappClosedSpline sappSplineRight = null;






    // Use this for initialization
    void Start()
    {
        if (sappSpline != null)
        {
            GeneratedTrack generatedTrack = new GeneratedTrack();
             
            for (int i = 0; i < sappSpline.controlPoints.Length; i++)
            {
                float s = ((float)i) / sappSpline.controlPoints.Length;
                int i2 = (i + 1) % sappSpline.controlPoints.Length;
                float s2 = ((float)i2) / sappSpline.controlPoints.Length;

                Vector3 pos1 = sappSpline.controlPoints[i].transform.position;
                Vector3 pos2 = sappSpline.controlPoints[i2].transform.position;
                Vector3 tangent1 = (sappSpline.TangentAt(s)[1] - sappSpline.TangentAt(s)[0]).normalized;
                Vector3 tangent2 = (sappSpline.TangentAt(s2)[1] - sappSpline.TangentAt(s2)[0]).normalized;
                float dir1 = Mathf.Acos(tangent1.z);
                // TODO restilche werte ausrechnen, und den entsprechenden GeneratedBezSpline erstellen
                GeneratedBezSpline bezSpline = new GeneratedBezSpline(pos1, dir1, pos2, dir1, 1f, 1f);
                generatedTrack.AddElement(bezSpline);
            }

            curvativeBins = new float[17];
            for (int i = -8; i <= 8; i++)
            {
                curvativeBins[i + 8] = (((float)i) * maxBinValue) / 8.0f;
            }

            if (curvativeBins != null && curvativeBins.Length > 0)
            {
                curvativeProfile = new int[curvativeBins.Length];
                for (int i = 0; i < curvativeProfile.Length; i++)
                {
                    curvativeProfile[i] = 0;
                }
            }

            for (int i = 0; i < sampleRate; i++)
            {
                float curPos = ((float)i) / ((float)sampleRate);
                float nextPos = ((float)(i + 1)) / ((float)sampleRate);

                Vector3 pos1 = sappSpline.SplineAt(curPos);
                Vector3 pos2 = sappSpline.SplineAt(nextPos);

                Vector3 tangent1 = sappSpline.TangentAt(curPos)[1] - sappSpline.TangentAt(curPos)[0];
                Vector3 tangent2 = sappSpline.TangentAt(nextPos)[1] - sappSpline.TangentAt(nextPos)[0];

                Vector3 inside1 = Vector3.Cross(tangent1, Vector3.up);
                Vector3 inside2 = Vector3.Cross(tangent2, Vector3.up);

                Vector2 vec2Pos1 = new Vector2(pos1.x, pos1.z);
                Vector2 vec2Pos2 = new Vector2(pos2.x, pos2.z);
                Vector2 vec2Inside1 = new Vector2(inside1.x, inside1.z);
                Vector2 vec2Inside2 = new Vector2(inside2.x, inside2.z);

                Line line1 = new Line(vec2Pos1, vec2Pos1 + vec2Inside1);
                Line line2 = new Line(vec2Pos2, vec2Pos2 + vec2Inside2);

                Vector2 intersect = Vector2.zero;

                bool intersects = line1.Intersects(line2, out intersect);

                float curvative = 0f;

                if (intersect != Vector2.zero)
                {
                    float radius1 = Vector2.Distance(intersect, vec2Pos1);
                    float radius2 = Vector2.Distance(intersect, vec2Pos2);

                    float avgRadius = (radius1 + radius2) * 0.5f;

                    curvative = 1f / avgRadius;

                    Vector3 toRight = (inside1 - pos1).normalized;
                    Vector3 intersectDir = ((new Vector3(intersect.x, pos1.y, intersect.y)) - pos1).normalized;

                    if (Vector3.Angle(toRight, intersectDir) >= 90)
                    {
                        curvative *= -1f;
                    }

                    if (debugInfo)
                    {
                        Debug.DrawLine(pos1, new Vector3(intersect.x, pos1.y, intersect.y), Color.yellow);
                        Debug.DrawLine(pos2, new Vector3(intersect.x, pos2.y, intersect.y), Color.yellow);
                    }
                }

                if (debugInfo)
                {
                    //Debug.Log("Curvative: " + curvative);
                }

                if (curvativeBins != null && curvativeBins.Length > 0)
                {
                    bool found = false;
                    for (int j = 0; j < curvativeBins.Length; j++)
                    {
                        if (curvative < curvativeBins[j])
                        {
                            curvativeProfile[j]++;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        curvativeProfile[curvativeProfile.Length - 1]++;
                    }
                }
            }

            if (visualizerProfile != null)
            {
                visualizerProfile.SetTrackName(trackname);

                for (int i = 0; i < curvativeProfile.Length; i++)
                {
                    visualizerProfile.SetBarValue(i, curvativeProfile[i] / ((float)sampleRate));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (calculateTrack)
        {
            calculateTrack = false;

            Start();
        }
    }
}
