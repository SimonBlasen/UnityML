using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvatureAnalyzer
{
    private int sampleRate = 300;
    private float maxBinValue = 2f;
    private float[] curvativeBins;
    private int[] curvativeProfile;

    public float[] curvatureValues;
    public float[] trackPartIndices;

    private ClosedSpline<Vector3> spline;

    public CurvatureAnalyzer(GeneratedTrack track) : this(track, 300)
    {
        
    }

    public CurvatureAnalyzer(GeneratedTrack track, int sampleRate)
    {
        curvatureValues = new float[sampleRate];
        trackPartIndices = new float[sampleRate];

        for (int i = 0; i < sampleRate; i++)
        {
            curvatureValues[i] = float.MaxValue;
        }
        float walkedDistance = 0f;

        for (int i = 0; i < track.Elements.Length; i++)
        {
            if (track.Elements[i].GetType() == typeof(GeneratedStraight))
            {
                GeneratedStraight straight = (GeneratedStraight)track.Elements[i];

                int startIndex = (int)((walkedDistance / track.TrackLength) * sampleRate);
                int endIndex = (int)(((walkedDistance + straight.Length) / track.TrackLength) * sampleRate);

                for (int j = startIndex; j < endIndex; j++)
                {
                    curvatureValues[j] = 0f;
                    trackPartIndices[j] = i + ((float)(j - startIndex)) / ((float)(endIndex - startIndex));
                }

                walkedDistance += straight.Length;
            }

            else if (track.Elements[i].GetType() == typeof(GeneratedTurn))
            {
                GeneratedTurn turn = (GeneratedTurn)track.Elements[i];

                int startIndex = (int)((walkedDistance / track.TrackLength) * sampleRate);
                int endIndex = (int)(((walkedDistance + turn.Length) / track.TrackLength) * sampleRate);

                for (int j = startIndex; j < endIndex; j++)
                {
                    curvatureValues[j] = (1f / turn.Radius) * Mathf.Sign(turn.Degree);
                }

                walkedDistance += turn.Length;
            }

            else if (track.Elements[i].GetType() == typeof(GeneratedBezSpline))
            {
                GeneratedBezSpline spline = (GeneratedBezSpline)track.Elements[i];

                int startIndex = (int)((walkedDistance / track.TrackLength) * sampleRate);
                int endIndex = (int)(((walkedDistance + spline.Length) / track.TrackLength) * sampleRate);

                for (int j = startIndex; j < endIndex; j++)
                {
                    Vector3 tangent1 = spline.Spline.TangentAt(((((float)j) - startIndex + 0f)) / (endIndex - startIndex))[1] - spline.Spline.TangentAt(((((float)j) - startIndex + 0f)) / (endIndex - startIndex))[0];
                    Vector3 tangent2 = spline.Spline.TangentAt(((((float)j) - startIndex + 1f)) / (endIndex - startIndex))[1] - spline.Spline.TangentAt(((((float)j) - startIndex + 1f)) / (endIndex - startIndex))[0];
                    Vector2 pos1 = new Vector2(spline.Spline.At(((((float)j) - startIndex + 0f)) / (endIndex - startIndex)).x, spline.Spline.At(((((float)j) - startIndex + 0f)) / (endIndex - startIndex)).z);
                    Vector2 pos2 = new Vector2(spline.Spline.At(((((float)j) - startIndex + 1f)) / (endIndex - startIndex)).x, spline.Spline.At(((((float)j) - startIndex + 1f)) / (endIndex - startIndex)).z);
                    Vector2 dir1 = new Vector2(tangent1.z, -tangent1.x);
                    Vector2 dir2 = new Vector2(tangent2.z, -tangent2.x);
                    Ray2D ray1 = new Ray2D(pos1, dir1);
                    Ray2D ray2 = new Ray2D(pos2, dir2);

                    Ray2D straightRay = new Ray2D(pos1, tangent1);

                    bool parallel;
                    Vector2 intersect = Utils.Intersect2D(ray1, ray2, out parallel);
                    if (parallel)
                    {
                        curvatureValues[j] = 0f;
                    }
                    else
                    {
                        float radius = (Vector2.Distance(intersect, pos1) + Vector2.Distance(intersect, pos2)) * 0.5f;

                        curvatureValues[j] = (1f / radius) * (Utils.PointRightTo(straightRay, intersect) ? 1f : -1f);
                    }

                    trackPartIndices[j] = i + ((float)(j - startIndex)) / ((float)(endIndex - startIndex));
                }

                walkedDistance += spline.Length;
            }
        }


        int notSet = 0;
        for (int i = 0; i < sampleRate; i++)
        {
            if (curvatureValues[i] == float.MaxValue)
            {
                notSet++;
            }
        }

        Debug.Log("NotSet: " + notSet);
    }

    public CurvatureAnalyzer(ClosedSpline<Vector3> spline, int sampleRate)
    {
        this.spline = spline;
        this.sampleRate = sampleRate;

        curvatureValues = new float[sampleRate];

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

            Vector3 pos1 = spline.SplineAt(curPos);
            Vector3 pos2 = spline.SplineAt(nextPos);

            Vector3 tangent1 = spline.TangentAt(curPos)[1] - spline.TangentAt(curPos)[0];
            Vector3 tangent2 = spline.TangentAt(nextPos)[1] - spline.TangentAt(nextPos)[0];

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

                curvatureValues[i] = curvative;
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
    }

    public CurvatureAnalyzer(ClosedSpline<Vector3> spline) : this(spline, 300)
    {
        
    }

    public float[] Curvature
    {
        get
        {
            return curvatureValues;
        }
    }

    public int[] CurvatureProfile
    {
        get
        {
            return curvativeProfile;
        }
    }
}
