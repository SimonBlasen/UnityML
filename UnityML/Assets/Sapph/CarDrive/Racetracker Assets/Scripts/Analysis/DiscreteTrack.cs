using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DiscreteTrackSegment
{
    public Vector3 Pos1Left { get; set; }
    public Vector3 Pos1Right { get; set; }
    public Vector3 Pos2Left { get; set; }
    public Vector3 Pos2Right { get; set; }
}


public class DiscreteTrack
{
    public static float straightSegmentingFactorIdealline = 10f;
    public static float straightSegmentingFactorMesh = 4f;

    private Vector3[] leftPoints = new Vector3[0];
    private Vector3[] rightPoints = new Vector3[0];
    public Vector3[] leftPointsCurv = new Vector3[0];
    public Vector3[] rightPointsCurv = new Vector3[0];
    public float[] shortestPathTrajectory;
    public int[] indcsFromPathIntoCurvature;
    public float[] minimumCurvatureTrajectory;
    public float[] idealLine;
    public ClosedSpline<Vector3> idealLineSpline;
    public CurvatureAnalyzer curvatureAnalyzer;
    public SpeedAnalyzer speedAnalyzer;
    public CurvatureAnalyzer curvatureAnalyzerTrack;
    public CurvatureIdeallineAnalyzer curvatureIdealLineAnalyzer;

    public float startFinishLineIndex;

    public float[] idealLineMesh;

    public DiscreteTrack(GeneratedTrack track, TerrainModifier terrainModifier)
    {
        List<Vector3> lefts = new List<Vector3>();
        List<Vector3> rights = new List<Vector3>();
        List<Vector3> leftsCurv = new List<Vector3>();
        List<Vector3> rightsCurv = new List<Vector3>();
        List<int> indcs = new List<int>();
        int indcsCounter = 0;

        List<float> idealLineMeshList = new List<float>();
        int idLineMeshCounter = 0;

        for (int i = 0; i < track.Elements.Length; i++)
        {
            if (track.Elements[i].GetType() == typeof(GeneratedStraight))
            {
                Vector3 middle = track.Elements[i].Position;

                Vector3 toRight = (new Vector3(Mathf.Cos(track.Elements[i].Direction), 0f, -Mathf.Sin(track.Elements[i].Direction))).normalized;
                Vector3 toFront = (new Vector3(Mathf.Sin(track.Elements[i].Direction), 0f, Mathf.Cos(track.Elements[i].Direction))).normalized;

                lefts.Add(middle - toRight * track.Elements[i].WidthStart);
                rights.Add(middle + toRight * track.Elements[i].WidthStart);
                indcsCounter = -1;

                GeneratedStraight straight = (GeneratedStraight)track.Elements[i];
                int segsAmount = (int)(straight.Length / straightSegmentingFactorIdealline);

                int segmentsAmountMesh = (int)(straight.Length / straightSegmentingFactorMesh);
                for (int j = 0; j < segmentsAmountMesh; j++)
                {
                    //idealLineMesh[idLineMeshCounter] = 1f;
                    idealLineMeshList.Add(1f);
                    idLineMeshCounter++;
                }

                for (int j = 0; j < segsAmount; j++)
                {
                    leftsCurv.Add(middle - toRight * track.Elements[i].WidthStart + toFront * straight.Length * (((float)j) / ((float)segsAmount)));
                    rightsCurv.Add(middle + toRight * track.Elements[i].WidthStart + toFront * straight.Length * (((float)j) / segsAmount));
                    indcsCounter++;
                }

                indcs.Add(indcsCounter);
            }
            else if (track.Elements[i].GetType() == typeof(GeneratedTurn))
            {
                GeneratedTurn turn = (GeneratedTurn)track.Elements[i];

                bool rightTurn = turn.Degree >= 0f;

                Vector3 toRight = (Quaternion.Euler(0f, (turn.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(1f, 0f, 0f));
                Vector3 toFront = (Quaternion.Euler(0f, (turn.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(0f, 0f, 1f));

                Vector3 middlePoint = toRight * turn.Radius * (rightTurn ? 1f : -1f);

                int segmentsAmount = ((int)(Mathf.Abs(turn.Degree) / 30f)) + 1;

                for (int j = 0; j < segmentsAmount; j++)
                {
                    Vector3 toRightTurned = (Quaternion.Euler(0f, (turn.Degree / ((float)segmentsAmount)) * j, 0f)) * toRight;

                    Vector3 segmentPos = middlePoint + (toRightTurned * (rightTurn ? -1f : 1f) * turn.Radius);

                    float currentWidth = (turn.WidthEnd - turn.WidthStart) * (float)((float)j / (float)segmentsAmount) + turn.WidthStart;

                    lefts.Add(segmentPos + toRightTurned * currentWidth * -1f + turn.Position);
                    rights.Add(segmentPos + toRightTurned * currentWidth + turn.Position);
                    leftsCurv.Add(segmentPos + toRightTurned * currentWidth * -1f + turn.Position);
                    rightsCurv.Add(segmentPos + toRightTurned * currentWidth + turn.Position);
                    indcs.Add(0);
                }
            }
            else if (track.Elements[i].GetType() == typeof(GeneratedBezSpline))
            {
                GeneratedBezSpline bezierSpline = (GeneratedBezSpline)track.Elements[i];

                for (int j = 0; j < bezierSpline.RenderVertsLeft.Length - 1; j++)
                {
                    if (j == 0)
                    {
                        //idealLineMesh[idLineMeshCounter] = 2f;
                        idealLineMeshList.Add(2f);
                    }
                    else
                    {
                        //idealLineMesh[idLineMeshCounter] = 0f;
                        idealLineMeshList.Add(0f);
                    }
                    idLineMeshCounter++;

                    if (j % 3 == 0)
                    {
                        lefts.Add(bezierSpline.RenderVertsLeft[j]);
                        rights.Add(bezierSpline.RenderVertsRight[j]);
                        leftsCurv.Add(bezierSpline.RenderVertsLeft[j]);
                        rightsCurv.Add(bezierSpline.RenderVertsRight[j]);
                        indcs.Add(0);
                    }
                }
            }
        }

        idealLineMesh = idealLineMeshList.ToArray();

        leftPoints = lefts.ToArray();
        rightPoints = rights.ToArray();
        leftPointsCurv = leftsCurv.ToArray();
        rightPointsCurv = rightsCurv.ToArray();
        indcsFromPathIntoCurvature = indcs.ToArray();

        shortestPathTrajectory = minimizePathTrajectory();
        if (false)
        {
            for (int i = 0; i < leftPoints.Length; i++)
            {
                int i2 = (i + 1) % leftPoints.Length;
                Debug.DrawLine(leftPoints[i], leftPoints[i2], Color.white, 10000f);
                Debug.DrawLine(rightPoints[i], rightPoints[i2], Color.white, 10000f);
                Debug.DrawLine(leftPoints[i], rightPoints[i], Color.white, 10000f);


                Debug.DrawLine(shortestPathTrajectory[i] * (leftPoints[i] - rightPoints[i]) + rightPoints[i], shortestPathTrajectory[i2] * (leftPoints[i2] - rightPoints[i2]) + rightPoints[i2], Color.green, 10000f);
            }
        }

        minimumCurvatureTrajectory = minimizeCurvatureTrajectory();

        if (false)
        {
            for (int i = 0; i < leftPointsCurv.Length; i++)
            {
                int i2 = (i + 1) % leftPointsCurv.Length;
                Debug.DrawLine(leftPointsCurv[i], leftPointsCurv[i2], Color.white, 10000f);
                Debug.DrawLine(rightPointsCurv[i], rightPointsCurv[i2], Color.white, 10000f);
                Debug.DrawLine(leftPointsCurv[i], rightPointsCurv[i], Color.white, 10000f);


                Debug.DrawLine(minimumCurvatureTrajectory[i] * (leftPointsCurv[i] - rightPointsCurv[i]) + rightPointsCurv[i], minimumCurvatureTrajectory[i2] * (leftPointsCurv[i2] - rightPointsCurv[i2]) + rightPointsCurv[i2], Color.green, 10000f);
            }
        }

        float epsilon = 0.1f;
        idealLine = applyEpsilon(shortestPathTrajectory, minimumCurvatureTrajectory, epsilon, indcsFromPathIntoCurvature);

        /*for (int i = 0; i < idealLine.Length; i++)
        {
            Debug.Log("IL [" + i + "]: " + idealLine[i]);
        }*/

        Debug.Log("cnter: " + idLineMeshCounter);

        transferToMeshIdealLine(track, idealLine, idealLineMesh);

        List<float> cpsBankAngles = new List<float>();
        List<Vector3> cpsMinDistance = new List<Vector3>();
        float minDistance = 15f;
        Vector3[] cps = new Vector3[idealLine.Length];
        float[] cpsBank = new float[idealLine.Length];
        cpsMinDistance.Add(idealLine[0] * (leftPointsCurv[0] - rightPointsCurv[0]) + rightPointsCurv[0]);
        for (int i = 1; i < idealLine.Length; i++)
        {
            Vector3 point = idealLine[i] * (leftPointsCurv[i] - rightPointsCurv[i]) + rightPointsCurv[i];
            //cps[i] = point;
            if (Vector3.Distance(cpsMinDistance[cpsMinDistance.Count - 1], point) > minDistance)
            {
                cpsBankAngles.Add(Vector3.Angle(new Vector3(rightPointsCurv[i].x - leftPointsCurv[i].x, terrainModifier.GetTensorHeight(rightPointsCurv[i].x, rightPointsCurv[i].z) - terrainModifier.GetTensorHeight(leftPointsCurv[i].x, leftPointsCurv[i].z), rightPointsCurv[i].z - leftPointsCurv[i].z), new Vector3((rightPointsCurv[i] - leftPointsCurv[i]).x, 0f, (rightPointsCurv[i] - leftPointsCurv[i]).z)) * Mathf.Sign((rightPointsCurv[i] - leftPointsCurv[i]).y));
                cpsMinDistance.Add(point);
            }
        }

        cps = cpsMinDistance.ToArray();
        cpsBank = cpsBankAngles.ToArray();

        for (int i = 0; i < cps.Length; i++)
        {
            cps[i] = new Vector3(cps[i].x, terrainModifier.GetTensorHeight(cps[i].x, cps[i].z), cps[i].z);
        }

        idealLineSpline = new ClosedSpline<Vector3>(cps);
        ClosedSpline<float> bankSpline = new ClosedSpline<float>(cpsBank);

        curvatureAnalyzer = new CurvatureAnalyzer(idealLineSpline, 300);

        speedAnalyzer = new SpeedAnalyzer(idealLineSpline, bankSpline, curvatureAnalyzer, 300);

        curvatureAnalyzerTrack = new CurvatureAnalyzer(track, 300);

        curvatureIdealLineAnalyzer = new CurvatureIdeallineAnalyzer(idealLineSpline);

        float startFinishLength = 500f;
        float partStartFinish = startFinishLength / track.TrackLength;

        int elementsStartFinishAmount = (int)(idealLineSpline.ControlPointsAmount * partStartFinish);
        elementsStartFinishAmount = (int)(partStartFinish * 300f);

        int startFinishIndex = minCurvatureForAmount(curvatureAnalyzerTrack.Curvature, elementsStartFinishAmount);

        float partpart = curvatureAnalyzerTrack.trackPartIndices[startFinishIndex];

        //Vector3 beginStartFinishPoint = idealLineSpline.controlPoints[startFinishIndex];

        Debug.Log("Start Finish point: " + partpart);


        int elementStartFinishBeginIndex = (int)partpart;
        Vector3 elementStartPos = track.Elements[elementStartFinishBeginIndex].Position;

        startFinishLineIndex = partpart;
        //Debug.Log("Start Finish point: " + elementStartPos);
    }

    private int minCurvatureForAmount(float[] values, int lengthAmounts)
    {
        float[] sums = new float[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            float sum = 0f;
            for (int j = 0; j < lengthAmounts; j++)
            {
                //int index = (i - j) < 0 ? (i - j) + lengthAmounts : (i - j);
                int index = (i - (lengthAmounts / 2)) + j;
                if (index < 0)
                {
                    index += values.Length;
                }
                else
                {
                    index = index % values.Length;
                }


                sum += Mathf.Abs(values[index]);
            }

            sums[i] = sum;
        }

        float min = float.MaxValue;
        int minIndex = -1;

        for (int i = 0; i < sums.Length; i++)
        {
            if (sums[i] < min)
            {
                min = sums[i];
                minIndex = i;
            }
        }

        return minIndex;
    }

    private void transferToMeshIdealLine(GeneratedTrack track, float[] idealline, float[] ideallineMeshp)
    {
        int idLineMeshCounter = 0;
        int idLineCounter = 0;

        for (int i = 0; i < track.Elements.Length; i++)
        {
            if (track.Elements[i].GetType() == typeof(GeneratedStraight))
            {
                GeneratedStraight straight = (GeneratedStraight)track.Elements[i];
                int segsAmount = (int)(straight.Length / straightSegmentingFactorIdealline);

                int segmentsAmountMesh = (int)(straight.Length / straightSegmentingFactorMesh);
                for (int j = 0; j < segmentsAmountMesh; j++)
                {
                    float s = ((float)j) / segmentsAmountMesh;
                    int bef = (int)(s * segsAmount);
                    int aft = bef + 1;
                    float partialS = aft - (s * segsAmount);

                    ideallineMeshp[idLineMeshCounter] = idealline[bef + idLineCounter] * partialS + idealline[aft + idLineCounter] * (1f - partialS);
                    idLineMeshCounter++;
                }

                idLineCounter += segsAmount;
            }
            else if (track.Elements[i].GetType() == typeof(GeneratedBezSpline))
            {
                GeneratedBezSpline bezierSpline = (GeneratedBezSpline)track.Elements[i];

                for (int j = 0; j < bezierSpline.RenderVertsLeft.Length - 1; j++)
                {

                    if (j % 3 == 0 && idLineMeshCounter < ideallineMeshp.Length)
                    {
                        ideallineMeshp[idLineMeshCounter] = idealline[idLineCounter];
                        idLineMeshCounter++;
                        idLineCounter++;
                    }
                    else if (j % 3 == 1 && (idLineCounter + 1) < idealline.Length && idLineMeshCounter < ideallineMeshp.Length)
                    {
                        ideallineMeshp[idLineMeshCounter] = idealline[idLineCounter] * (2f / 3f) + idealline[idLineCounter + 1] * (1f / 3f);
                        idLineMeshCounter++;
                    }
                    else if (j % 3 == 2 && (idLineCounter + 1) < idealline.Length && idLineMeshCounter < ideallineMeshp.Length)
                    {
                        ideallineMeshp[idLineMeshCounter] = idealline[idLineCounter] * (1f / 3f) + idealline[idLineCounter + 1] * (2f / 3f);
                        idLineMeshCounter++;
                    }
                }
            }

            //Debug.Log("cnter: " + idLineMeshCounter);
        }

        /*for (int i = 0; i < ideallineMeshp.Length; i++)
        {
            Debug.Log("IdlLineMsh [" + i + "]: " + ideallineMeshp[i]);
        }*/
    }

    private float[] applyEpsilon(float[] shortestPath, float[] minimumCurvature, float epsilon, int[] indices)
    {
        float[] idLine = new float[minimumCurvature.Length];
        float[] sPath = new float[minimumCurvature.Length];

        int counter = 0;
        for (int i = 0; i < shortestPath.Length; i++)
        {
            int i2 = (i + 1) % shortestPath.Length;

            if (counter + 2 >= sPath.Length)
            {
                int ghgh = 0;
            }

            sPath[counter] = shortestPath[i];
            if (indices[i] > 0)
            {
                float stepVal = 1f / (indices[i] + 1);
                for (int j = 1; j <= indices[i]; j++)
                {
                    sPath[counter + j] = (stepVal * j) * (shortestPath[i2] - shortestPath[i]) + shortestPath[i];
                }
                counter += indices[i];
            }
            else
            {
                counter++;
            }
        }

        for (int i = 0; i < idLine.Length; i++)
        {
            idLine[i] = epsilon * (sPath[i] - minimumCurvature[i]) + minimumCurvature[i];
        }

        return idLine;
    }

    private float[] minimizeCurvatureTrajectory()
    {
        int n = leftPointsCurv.Length;
        float[] alpha = new float[leftPointsCurv.Length];

        for (int i = 0; i < n; i++)
        {
            alpha[i] = 0.5f;
        }

        float[] f = new float[n];
        for (int i = 0; i < n; i++)
        {
            f[i] = 0f;
        }

        float[] v = new float[n];
        for (int i = 0; i < n; i++)
        {
            v[i] = 0f;
        }

        float[] cs = new float[n];
        for (int i = 0; i < n; i++)
        {
            cs[i] = 0f;
        }

        int iterations = 1000;

        float k = 0.35f;
        float mass = 1f;
        float deltaT = 0.1f;

        float minDist = 0.1f;

        for (int it = 0; it < iterations; it++)
        {
            for (int i = 0; i < n; i++)
            {
                f[i] = 0f;
            }

            for (int i = 0; i < n; i++)
            {
                int i1 = i - 1;
                if (i1 < 0)
                {
                    i1 = n - 1;
                }
                int i2 = i;
                int i3 = (i + 1) % n;


                Vector2 m1 = alphaICurv(i1, alpha[i1]);
                Vector2 m2 = alphaICurv(i2, alpha[i2]);
                Vector2 m3 = alphaICurv(i3, alpha[i3]);

                Vector2 fCM1 = alphaICurv(i1, 1f) - alphaICurv(i1, 0f); // F correct M1
                Vector2 fCM3 = alphaICurv(i3, 1f) - alphaICurv(i3, 0f); // F correct M3
                Vector2 fWM1 = new Vector2((-(m2 - m1).y), ((m2 - m1).x)); // F wirkend M1
                Vector2 fWM3 = new Vector2((-(m3 - m2).y), ((m3 - m2).x)); // F wirkend M3

                Ray2D raym1m2 = new Ray2D(m1, m2 - m1);
                bool isRight = Utils.PointRightTo(raym1m2, m3);

                float distance = (Vector2.Distance(m1, m2) + Vector2.Distance(m2, m3)) * 0.5f;

                float theta = (Vector2.Angle(m2 - m1, m3 - m2) * Mathf.PI) / 180f;

                Ray2D ray1 = new Ray2D(m1, fWM1);
                Ray2D ray3 = new Ray2D(m3, fWM3);
                bool parallel;
                Vector2 intersect = Utils.Intersect2D(ray1, ray3, out parallel);
                float radius = (Vector2.Distance(intersect, m1) + Vector2.Distance(intersect, m3)) * 0.5f;
                cs[i2] = 1f / radius;

                float magF3 = Mathf.Abs(k * cs[i2]) * (isRight ? 1f : -1f);
                float magF1 = magF3;
                float magF2 = -2f * magF3 * 1f;


                f[i1] += magF1;
                f[i2] += magF2;
                f[i3] += magF3;
            }


            for (int i = 0; i < n; i++)
            {
                v[i] = v[i] + (f[i] / mass + 0f * (-cs[i] * v[i])) * deltaT;
                alpha[i] = alpha[i] + v[i] * deltaT;
                if (alpha[i] > 1f - minDist)
                {
                    alpha[i] = 1f - minDist;
                }
                if (alpha[i] < 0f + minDist)
                {
                    alpha[i] = 0f + minDist;
                }
            }
        }

        return alpha;
    }

    public float getCurvature(ClosedSpline<Vector2> spline)
    {
        float curvature = 0f;
        int resolution = 20;

        for (int i = 0; i < spline.ControlPointsAmount; i++)
        {
            float localMax = 0f;

            float s = ((float)i) / spline.ControlPointsAmount;
            float s2 = ((float)((i + 1))) / spline.ControlPointsAmount;

            for (int j = 0; j < resolution; j++)
            {
                float locS = s + (s2 - s) * (((float)j) / resolution);
                float locS2 = s + (s2 - s) * (((float)(j + 1)) / resolution);

                Vector2 t1 = spline.TangentAt(s)[1] - spline.TangentAt(s)[0];
                Vector2 t2 = spline.TangentAt(s2)[1] - spline.TangentAt(s2)[0];

                float curv = Vector2.Angle(t1, t2);

                if (curv > localMax)
                {
                    localMax = curv;
                }
            }

            curvature += localMax;
        }

        return curvature;
    }

    public Vector2 alphaICurv(int i, float alpha)
    {
        if (alpha > 1f)
        { alpha = 1f; }
        if (alpha < 0f)
        { alpha = 0f; }
        if (i >= 0 && i < leftPointsCurv.Length)
        {
            return (new Vector2(rightPointsCurv[i].x, rightPointsCurv[i].z)) + alpha * ((new Vector2(leftPointsCurv[i].x, leftPointsCurv[i].z)) - (new Vector2(rightPointsCurv[i].x, rightPointsCurv[i].z)));
        }

        return Vector2.zero;
    }

    private float[] minimizePathTrajectory()
    {
        int n = leftPoints.Length;

        int iterations = 150;


        int freezedAmount = 0;
        bool[] freezed = new bool[n];
        int[] freezeCount = new int[n];
        float[] oldXs = new float[n];
        float[] x = new float[n];
        for (int i = 0; i < n; i++)
        {
            x[i] = 0.5f;
            oldXs[i] = 0.5f;
            freezeCount[i] = 0;
            freezed[i] = false;
        }


        float minStep = 0.05f;
        for (int i = 0; i < iterations; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (freezed[j] == false)
                {
                    int i2 = j - 1;
                    if (i2 < 0)
                    {
                        i2 = n - 1;
                    }
                    int i4 = j + 1;
                    if (i4 >= n)
                    {
                        i4 = 0;
                    }

                    if (x[j] == oldXs[j] && x[i4] == oldXs[i4] && x[i2] == oldXs[i2])
                    {
                        freezeCount[j]++;
                        if (freezeCount[j] >= 3)
                        {
                            freezed[j] = true;
                            freezedAmount++;
                        }
                    }
                    else
                    {
                        freezeCount[j] = 0;
                    }
                }
            }

            for (int j = 0; j < n; j++)
            {
                if (freezed[j] == false)
                {
                    oldXs[j] = x[j];

                    int i2 = j - 1;
                    if (i2 < 0)
                    {
                        i2 = n - 1;
                    }
                    int i4 = j + 1;
                    if (i4 >= n)
                    {
                        i4 = 0;
                    }

                    float fac = 1f;
                    float oldX = x[j];
                    float here = getLineLength(x);
                    int countCatcher = 0;
                    while (countCatcher < 10)
                    {
                        countCatcher++;
                        x[j] = oldX - minStep * fac;
                        if (x[j] < 0f)
                        {
                            x[j] = 0f;
                        }
                        float left = getLineLength(x);

                        x[j] = oldX + minStep * fac;
                        if (x[j] > 1f)
                        {
                            x[j] = 1f;
                        }
                        float right = getLineLength(x);

                        if (here <= left && here <= right)
                        {
                            fac *= 0.5f;
                            if (fac * minStep <= 0.001f)
                            {
                                x[j] = oldX;
                                break;
                            }
                        }
                        else if (left < here)
                        {
                            x[j] = oldX - minStep * fac;
                            if (x[j] < 0f)
                            {
                                x[j] = 0f;
                            }
                            break;
                        }
                        else if (right < here)
                        {
                            x[j] = oldX + minStep * fac;
                            if (x[j] > 1f)
                            {
                                x[j] = 1f;
                            }
                            break;
                        }
                    }
                }
            }
        }

        return x;
    }

    public float getLineLength(float[] alphas)
    {
        float le = 0f;
        for (int i = 0; i < alphas.Length; i++)
        {
            int i2 = i + 1;
            if (i2 >= alphas.Length)
            {
                i2 = 0;
            }

            float xVal = rightPoints[i].x + alphas[i] * (leftPoints[i].x - rightPoints[i].x);
            float yVal = rightPoints[i].z + alphas[i] * (leftPoints[i].z - rightPoints[i].z);

            float x2Val = rightPoints[i2].x + alphas[i2] * (leftPoints[i2].x - rightPoints[i2].x);
            float y2Val = rightPoints[i2].z + alphas[i2] * (leftPoints[i2].z - rightPoints[i2].z);

            float diffX = xVal - x2Val;
            float diffY = yVal - y2Val;

            le += Mathf.Sqrt(diffX * diffX + diffY * diffY);
        }

        return le;
    }

    public DiscreteTrackSegment GetSegment(int index)
    {
        if (index >= 0 && index < leftPoints.Length)
        {
            DiscreteTrackSegment segment = new DiscreteTrackSegment();
            segment.Pos1Left = leftPoints[index];
            segment.Pos1Right = rightPoints[index];
            if (index == leftPoints.Length - 1)
            {
                segment.Pos2Left = leftPoints[0];
                segment.Pos2Right = rightPoints[0];
            }
            else
            {
                segment.Pos2Left = leftPoints[index + 1];
                segment.Pos2Right = rightPoints[index + 1];
            }

            return segment;
        }

        return null;
    }

    public DiscreteTrackSegment GetSegmentCurv(int index)
    {
        if (index >= 0 && index < leftPointsCurv.Length)
        {
            DiscreteTrackSegment segment = new DiscreteTrackSegment();
            segment.Pos1Left = leftPointsCurv[index];
            segment.Pos1Right = rightPointsCurv[index];
            if (index == leftPointsCurv.Length - 1)
            {
                segment.Pos2Left = leftPointsCurv[0];
                segment.Pos2Right = rightPointsCurv[0];
            }
            else
            {
                segment.Pos2Left = rightPointsCurv[index + 1];
                segment.Pos2Right = rightPointsCurv[index + 1];
            }

            return segment;
        }

        return null;
    }

    public Vector3[] GetCurvativePointsRight()
    {
        Vector3[] cps = new Vector3[leftPointsCurv.Length];

        for (int i = 0; i < cps.Length; i++)
        {
            DiscreteTrackSegment seg = GetSegmentCurv(i);
            cps[i] = seg.Pos1Right + (seg.Pos1Left - seg.Pos1Right) * minimumCurvatureTrajectory[i];
        }
        return cps;
    }



    public int SegmentsAmount
    {
        get
        {
            return leftPoints.Length;
        }
    }


    public DiscreteTrack Copy(GeneratedTrack copyTrack)
    {
        DiscreteTrack copy = new DiscreteTrack(copyTrack, copyTrack.TerrainModifier);

        return copy;
    }
}