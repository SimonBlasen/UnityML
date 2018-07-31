using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvatureIdeallineAnalyzer
{
    public float[] curvatureValues;

    public CurvatureIdeallineAnalyzer(ClosedSpline<Vector3> idealLine)
    {
        int n = idealLine.ControlPointsAmount;

        curvatureValues = new float[n];
        for (int i = 0; i < n; i++)
        {
            curvatureValues[i] = float.MaxValue;
        }


        for (int i = 0; i < n; i++)
        {
            float s = ((float)i) / n;
            float s2 = ((float)(i + 1)) / n;


            Vector3 tangent1 = idealLine.TangentAt(s)[1] - idealLine.TangentAt(s)[0];
            Vector3 tangent2 = idealLine.TangentAt(s2)[1] - idealLine.TangentAt(s2)[0];
            Vector2 pos1 = new Vector2(idealLine.SplineAt(s).x, idealLine.SplineAt(s).z);
            Vector2 pos2 = new Vector2(idealLine.SplineAt(s2).x, idealLine.SplineAt(s2).z);
            Vector2 dir1 = new Vector2(tangent1.z, -tangent1.x);
            Vector2 dir2 = new Vector2(tangent2.z, -tangent2.x);
            Ray2D ray1 = new Ray2D(pos1, dir1);
            Ray2D ray2 = new Ray2D(pos2, dir2);

            Ray2D straightRay = new Ray2D(pos1, tangent1);

            bool parallel;
            Vector2 intersect = Utils.Intersect2D(ray1, ray2, out parallel);
            if (parallel)
            {
                curvatureValues[i] = 0f;
            }
            else
            {
                float radius = (Vector2.Distance(intersect, pos1) + Vector2.Distance(intersect, pos2)) * 0.5f;

                curvatureValues[i] = (1f / radius) * (Utils.PointRightTo(straightRay, intersect) ? 1f : -1f);
            }
        }
    }

    public float[] Curvature
    {
        get
        {
            return curvatureValues;
        }
    }
}
