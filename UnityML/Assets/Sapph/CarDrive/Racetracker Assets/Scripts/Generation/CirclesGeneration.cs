using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CirclesGeneration : InteractiveHandler
{
    [Header("Prefabs")]
    [SerializeField]
    private GameObject prefabControlpoint;
    [SerializeField]
    private bool refresh;
    [SerializeField]
    private bool proceduralGen = false;

    private List<GameObject> instCircles;
    private List<Vector3> circlesPointsOut;
    private List<Vector3> circlesPointsIn;

    private Vector3[] betwLBs;
    private Vector3[] betwLTs;
    private Vector3[] betwRBs;
    private Vector3[] betwRTs;

    private List<GameObject> instLinesLeft = new List<GameObject>();

    private CirclesConstilation[] circlesConsts = new CirclesConstilation[4];


    // Use this for initialization
    void Start ()
    {
        if (instCircles == null)
        {
            instCircles = new List<GameObject>();

            circlesPointsOut = new List<Vector3>();
            circlesPointsIn = new List<Vector3>();
        }

        circlesConsts[0] = new CirclesConstilation();
        circlesConsts[0].Midpoints = new Vector2[] { new Vector2(0f, 0f), new Vector2(400f, 2f), new Vector2(200f, -200f) };
        circlesConsts[0].Radiuss = new float[] { 300f, 302f, 304f };
        circlesConsts[0].Clockwises = new bool[] { true, true, true };
        circlesConsts[0].AverageLength = 2900f;
        circlesConsts[1] = new CirclesConstilation();
        circlesConsts[1].Midpoints = new Vector2[] { new Vector2(0f, 0f), new Vector2(400f, 2f), new Vector2(2f, -400f) };
        circlesConsts[1].Radiuss = new float[] { 300f, 302f, 304f };
        circlesConsts[1].Clockwises = new bool[] { true, true, true };
        circlesConsts[1].AverageLength = 3700f;
        circlesConsts[2] = new CirclesConstilation();
        circlesConsts[2].Midpoints = new Vector2[] { new Vector2(115f, 192f), new Vector2(647f, -34f), new Vector2(134f, -16f), new Vector2(-380f, -71f) };
        circlesConsts[2].Radiuss = new float[] { 300f, 302f, 150f, 304f };
        circlesConsts[2].Clockwises = new bool[] { true, true, false, true };
        circlesConsts[2].AverageLength = 4600f;
        circlesConsts[3] = new CirclesConstilation();
        circlesConsts[3].Midpoints = new Vector2[] { new Vector2(170f, 46f), new Vector2(547f, -373f), new Vector2(133f, -131f), new Vector2(-259, -507f) };
        circlesConsts[3].Radiuss = new float[] { 30f * 15f, 20f * 15f, 8f * 15f, 20f * 15f };
        circlesConsts[3].Clockwises = new bool[] { true, true, false, true };
        circlesConsts[3].AverageLength = 8000f;
    }

    // Update is called once per frame
    void Update()
    {
        if (refresh)
        {
            refresh = false;

            for (int i = 0; i < instCircles.Count; i++)
            {
                instCircles[i].GetComponent<InteractiveCheckpoint>().Position = Vector3.zero;
            }
        }

        if (proceduralGen)
        {
            proceduralGen = false;

            for (int i = 0; i < instCircles.Count; i++)
            {
                instCircles[i].GetComponent<InteractiveCheckpoint>().Position = Vector3.zero;
            }

        }
    }

    public void DoProc(int configIndex, long seed, float destLength)
    {
        float epsilon = 20f;

        PerlinNoise perlinNoise = new PerlinNoise(seed);
        
        int rotation = ((int)(((perlinNoise.noise1(0f) * 0.5f) + 0.5f) * 360f * 100f)) % 360;

        int circleConstIndex = configIndex % circlesConsts.Length;

        //circleConstIndex = 3;

        AddCircle();

        for (int i = 0; i < instCircles.Count; i++)
        {
            RemoveLastCircle();
        }
        RemoveLastCircle();

        for (int i = 0; i < circlesConsts[circleConstIndex].Midpoints.Length - 1; i++)
        {
            AddCircle();
        }

        Debug.Log("Factor: " + (destLength / circlesConsts[circleConstIndex].AverageLength));

        for (int i = 0; i < circlesConsts[circleConstIndex].Midpoints.Length; i++)
        {
            instCircles[i].GetComponent<InteractiveCheckpoint>().Position = (new Vector3(circlesConsts[circleConstIndex].Midpoints[i].x, 0f, circlesConsts[circleConstIndex].Midpoints[i].y)) * (destLength / circlesConsts[circleConstIndex].AverageLength);
            instCircles[i].GetComponent<InteractiveCheckpoint>().ParamDistanceNext = circlesConsts[circleConstIndex].Radiuss[i] * (destLength / circlesConsts[circleConstIndex].AverageLength);
            instCircles[i].GetComponent<InteractiveCheckpoint>().ParamDistancePrev = circlesConsts[circleConstIndex].Clockwises[i] ? 15f : 30f;
        }

        for (int i = 0; i < circlesConsts[circleConstIndex].Midpoints.Length; i++)
        {
            instCircles[i].GetComponent<InteractiveCheckpoint>().Position = Quaternion.Euler(0f, rotation, 0f) * instCircles[i].GetComponent<InteractiveCheckpoint>().Position;
        }

        recalculatePoints();
    }

    public void DoProc(long seed, float destLength)
    {
        float epsilon = 20f;

        PerlinNoise perlinNoise = new PerlinNoise(seed);

        /*int circlesAmount = (int)((perlinNoise.noise1(0f) + 1f) * 2f + 2f);

        Debug.Log("Gonna do " + circlesAmount + " circles");

        for (int i = 0; i < circlesAmount; i++)
        {
            AddCircle();
        }

        float radiusFirst = ((perlinNoise.noise1(0f) + 1f) * 150f + 150f);
        instCircles[0].GetComponent<InteractiveCheckpoint>().Position = new Vector3(0f, 0f, 0f);
        instCircles[0].GetComponent<InteractiveCheckpoint>().ParamDistanceNext = radiusFirst;
        instCircles[0].GetComponent<InteractiveCheckpoint>().ParamDistancePrev = 19f;

        Debug.Log("Radius[0] = " + radiusFirst);

        for (int i = 1; i < circlesAmount; i++)
        {
            instCircles[i].GetComponent<InteractiveCheckpoint>().ParamDistancePrev = 19f;
            float radius = ((perlinNoise.noise1(0f + i * 20f) + 1f) * 150f + 150f);

            Debug.Log("Radius[" + i + "] = " + radius);

            instCircles[i].GetComponent<InteractiveCheckpoint>().ParamDistanceNext = radius;

            if (i == 1)
            {
                instCircles[i].GetComponent<InteractiveCheckpoint>().Position = new Vector3(instCircles[0].GetComponent<InteractiveCheckpoint>().ParamDistanceNext + radius + epsilon, 0f, 0f);
            }
            else
            {
                int prevCircleIndex = i - 1;
                float prevRadius = radius + epsilon + instCircles[prevCircleIndex].GetComponent<InteractiveCheckpoint>().ParamDistanceNext;
                Vector2 prevCircleMid = new Vector2(instCircles[prevCircleIndex].GetComponent<InteractiveCheckpoint>().Position.x, instCircles[prevCircleIndex].GetComponent<InteractiveCheckpoint>().Position.z);

                Vector2 finalMidPoint = new Vector2(float.MinValue, float.MinValue);

                for (int j = 0; j < i - 1; j++)
                {
                    float testRadius = radius + epsilon + instCircles[j].GetComponent<InteractiveCheckpoint>().ParamDistanceNext;
                    Vector2 testCircleMid = new Vector2(instCircles[j].GetComponent<InteractiveCheckpoint>().Position.x, instCircles[j].GetComponent<InteractiveCheckpoint>().Position.z);

                    float d = Vector2.Distance(testCircleMid, prevCircleMid);

                    float x = (d * d - (testRadius * testRadius) + prevRadius * prevRadius) / (2f * d);

                    float y = (Mathf.Sqrt(testRadius * testRadius - x * x)) * -1f;

                    Vector2 newMidpoint = (testCircleMid - prevCircleMid).normalized * x + (new Vector2(-(testCircleMid - prevCircleMid).normalized.y, (testCircleMid - prevCircleMid).normalized.x)) * y;

                    bool doesntWork = false;

                    for (int k = 0; k < i; k++)
                    {
                        float radiusHere = instCircles[k].GetComponent<InteractiveCheckpoint>().ParamDistanceNext;
                        Vector2 midpointHere = new Vector2(instCircles[k].GetComponent<InteractiveCheckpoint>().Position.x, instCircles[k].GetComponent<InteractiveCheckpoint>().Position.z);

                        if (Vector2.Distance(midpointHere, newMidpoint) < (radiusHere + radius))
                        {
                            doesntWork = true;
                        }
                    }

                    if (doesntWork == false)
                    {
                        finalMidPoint = newMidpoint;
                        break;
                    }
                }

                if (finalMidPoint.x != float.MinValue)
                {
                    Debug.Log("Found point for: " + i);
                    instCircles[i].GetComponent<InteractiveCheckpoint>().Position = new Vector3(finalMidPoint.x, 0f, finalMidPoint.y);
                }
            }
        }
        */
        /*float amp = 175f;
        float offs = 118f;

        instCircles[1].GetComponent<InteractiveCheckpoint>().Position = instCircles[0].GetComponent<InteractiveCheckpoint>().Position + new Vector3((perlinNoise.noise1(0f) + 1f) * amp + offs, 0f, 10f);
        instCircles[2].GetComponent<InteractiveCheckpoint>().Position = instCircles[1].GetComponent<InteractiveCheckpoint>().Position + new Vector3(-10f, 0f, (perlinNoise.noise1(200f) + 1f) * -amp - offs);
        instCircles[3].GetComponent<InteractiveCheckpoint>().Position = instCircles[2].GetComponent<InteractiveCheckpoint>().Position + new Vector3((perlinNoise.noise1(400f) + 1f) * -amp - offs, 0f, -10f);

        instCircles[0].GetComponent<InteractiveCheckpoint>().ParamDistanceNext = 430f;
        instCircles[1].GetComponent<InteractiveCheckpoint>().ParamDistanceNext = 440f;
        instCircles[2].GetComponent<InteractiveCheckpoint>().ParamDistanceNext = 450f;
        instCircles[3].GetComponent<InteractiveCheckpoint>().ParamDistanceNext = 460f;
        instCircles[0].GetComponent<InteractiveCheckpoint>().ParamDistancePrev = 19f;
        instCircles[1].GetComponent<InteractiveCheckpoint>().ParamDistancePrev = 19f;
        instCircles[2].GetComponent<InteractiveCheckpoint>().ParamDistancePrev = 19f;
        instCircles[3].GetComponent<InteractiveCheckpoint>().ParamDistancePrev = 19f;


        instCircles[0].GetComponent<CircleRenderer>().RefreshLines(instCircles[0].GetComponent<InteractiveCheckpoint>().Position, instCircles[0].GetComponent<InteractiveCheckpoint>().ParamDistanceNext);
        instCircles[1].GetComponent<CircleRenderer>().RefreshLines(instCircles[1].GetComponent<InteractiveCheckpoint>().Position, instCircles[1].GetComponent<InteractiveCheckpoint>().ParamDistanceNext);
        instCircles[2].GetComponent<CircleRenderer>().RefreshLines(instCircles[2].GetComponent<InteractiveCheckpoint>().Position, instCircles[2].GetComponent<InteractiveCheckpoint>().ParamDistanceNext);
        instCircles[3].GetComponent<CircleRenderer>().RefreshLines(instCircles[3].GetComponent<InteractiveCheckpoint>().Position, instCircles[3].GetComponent<InteractiveCheckpoint>().ParamDistanceNext);
        */

        int rotation = ((int)(((perlinNoise.noise1(0f) * 0.5f) + 0.5f) * 360f * 100f)) % 360;

        int circleConstIndex = ((int)(((perlinNoise.noise1(0f) * 0.5f) + 0.5f) * 2000f)) % circlesConsts.Length;

        //circleConstIndex = 3;

        AddCircle();

        for (int i = 0; i < instCircles.Count; i++)
        {
            RemoveLastCircle();
        }
        RemoveLastCircle();

        for (int i = 0; i < circlesConsts[circleConstIndex].Midpoints.Length - 1; i++)
        {
            AddCircle();
        }

        Debug.Log("Factor: " + (destLength / circlesConsts[circleConstIndex].AverageLength));

        for (int i = 0; i < circlesConsts[circleConstIndex].Midpoints.Length; i++)
        {
            instCircles[i].GetComponent<InteractiveCheckpoint>().Position = (new Vector3(circlesConsts[circleConstIndex].Midpoints[i].x, 0f, circlesConsts[circleConstIndex].Midpoints[i].y)) * (destLength / circlesConsts[circleConstIndex].AverageLength);
            instCircles[i].GetComponent<InteractiveCheckpoint>().ParamDistanceNext = circlesConsts[circleConstIndex].Radiuss[i] * (destLength / circlesConsts[circleConstIndex].AverageLength);
            instCircles[i].GetComponent<InteractiveCheckpoint>().ParamDistancePrev = circlesConsts[circleConstIndex].Clockwises[i] ? 15f : 30f;
        }

        for (int i = 0; i < circlesConsts[circleConstIndex].Midpoints.Length; i++)
        {
            instCircles[i].GetComponent<InteractiveCheckpoint>().Position = Quaternion.Euler(0f, rotation, 0f) * instCircles[i].GetComponent<InteractiveCheckpoint>().Position;
        }

        recalculatePoints();

    }

    public Vector3[] CirclesMidpoints
    {
        get
        {
            if (instCircles == null)
                Start();

            Vector3[] ret = new Vector3[instCircles.Count];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = instCircles[i].GetComponent<InteractiveCheckpoint>().Position;
            }

            return ret;
        }
    }

    public Vector3[] BetwLBs
    {
        get
        {
            return betwLBs;
        }
    }

    public Vector3[] BetwLTs
    {
        get
        {
            return betwLTs;
        }
    }

    public Vector3[] BetwRBs
    {
        get
        {
            return betwRBs;
        }
    }

    public Vector3[] BetwRTs
    {
        get
        {
            return betwRTs;
        }
    }

    public Vector3[] CirclesOuts
    {
        get
        {
            if (instCircles == null)
                Start();

            return circlesPointsOut.ToArray();
        }
    }

    public Vector3[] CirclesIns
    {
        get
        {
            if (instCircles == null)
                Start();

            return circlesPointsIn.ToArray();
        }
    }

    public bool[] CirclesClockwise
    {
        get
        {
            if (instCircles == null)
                Start();

            bool[] ret = new bool[instCircles.Count];

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = !instCircles[i].GetComponent<CircleRenderer>().Clockwise;
            }

            return ret;
        }
    }

    public float[] CirclesRadius
    {
        get
        {
            if (instCircles == null)
                Start();

            float[] ret = new float[instCircles.Count];

            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = instCircles[i].GetComponent<InteractiveCheckpoint>().ParamDistanceNext;
            }

            return ret;
        }
    }

    public void AddCircle()
    {
        if (instCircles == null)
        {
            Start();
        }

        GameObject instCP = Instantiate(prefabControlpoint);
        instCP.layer = 9;
        instCP.transform.position = Vector3.zero;
        instCP.GetComponent<InteractiveCheckpoint>().Moveable = true;
        instCP.GetComponent<InteractiveCheckpoint>().ElementsIndex = instCircles.Count;
        instCP.GetComponent<InteractiveCheckpoint>().CpRole = IntCpRole.MIDPOINT;
        instCP.GetComponent<InteractiveCheckpoint>().Position = Vector3.zero;
        instCP.GetComponent<InteractiveCheckpoint>().GenTrack = this;
        //instCP.GetComponent<InteractiveCheckpoint>().ParamDirection = 0f;


        instCP.GetComponent<InteractiveCheckpoint>().ParamDistancePrev = 20f;
        instCP.GetComponent<InteractiveCheckpoint>().ParamDistanceNext = 20f;
        
        instCP.GetComponent<InteractiveCheckpoint>().Turnable = false;
        

        instCircles.Add(instCP);
        circlesPointsOut.Add(Vector3.zero);
        circlesPointsIn.Add(Vector3.zero);
    }

    public void RemoveLastCircle()
    {
        if (instCircles.Count > 1)
        {
            Destroy(instCircles[instCircles.Count - 1]);
            instCircles.RemoveAt(instCircles.Count - 1);
            circlesPointsOut.RemoveAt(circlesPointsOut.Count - 1);
            circlesPointsIn.RemoveAt(circlesPointsIn.Count - 1);
        }
    }

    public void RemoveLastCircleClick()
    {
        if (instCircles.Count > 2)
        {
            Destroy(instCircles[instCircles.Count - 1]);
            instCircles.RemoveAt(instCircles.Count - 1);
            circlesPointsOut.RemoveAt(circlesPointsOut.Count - 1);
            circlesPointsIn.RemoveAt(circlesPointsIn.Count - 1);
        }
    }

    public override void CPChanged(int index, IntCpRole role, Vector3 newPosition, float newNextDistance, float newPrevDistance, float newDirection)
    {
        if (role == IntCpRole.MIDPOINT || role == IntCpRole.NEXT_B1 || role == IntCpRole.PREV_B2)
        {
            instCircles[index].GetComponent<CircleRenderer>().RefreshLines(instCircles[index].GetComponent<InteractiveCheckpoint>().Position, instCircles[index].GetComponent<InteractiveCheckpoint>().ParamDistanceNext);

            recalculatePoints();
        }
    }

    private void recalculatePoints()
    {
        betwLBs = new Vector3[instCircles.Count];
        betwLTs = new Vector3[instCircles.Count];
        betwRBs = new Vector3[instCircles.Count];
        betwRTs = new Vector3[instCircles.Count];

        if (instCircles.Count == 1)
        {
            circlesPointsIn[0] = Vector3.zero;
            circlesPointsOut[0] = Vector3.zero;
        }
        else if (instCircles.Count > 1)
        {
            for (int i = 0; i < instCircles.Count; i++)
            {
                int i2 = (i + 1) % instCircles.Count;

                Vector2 c1Mid = new Vector2(instCircles[i].GetComponent<InteractiveCheckpoint>().Position.x, instCircles[i].GetComponent<InteractiveCheckpoint>().Position.z);
                Vector2 c2Mid = new Vector2(instCircles[i2].GetComponent<InteractiveCheckpoint>().Position.x, instCircles[i2].GetComponent<InteractiveCheckpoint>().Position.z);
                float c1Radius = instCircles[i].GetComponent<InteractiveCheckpoint>().ParamDistanceNext;
                float c2Radius = instCircles[i2].GetComponent<InteractiveCheckpoint>().ParamDistanceNext;

                Vector2[,] points = calcAll4Tangents(c1Mid, c1Radius, c2Mid, c2Radius);

                Vector3[] line1 = new Vector3[] { new Vector3(points[0, 0].x, 0f, points[0,0].y), new Vector3(points[0, 1].x, 0f, points[0, 1].y) };
                Vector3[] line2 = new Vector3[] { new Vector3(points[1, 0].x, 0f, points[1, 0].y), new Vector3(points[1, 1].x, 0f, points[1, 1].y) };
                Vector3[] line3 = new Vector3[] { new Vector3(points[2, 0].x, 0f, points[2, 0].y), new Vector3(points[2, 1].x, 0f, points[2, 1].y) };
                Vector3[] line4 = new Vector3[] { new Vector3(points[3, 0].x, 0f, points[3, 0].y), new Vector3(points[3, 1].x, 0f, points[3, 1].y) };

                if (instCircles[i].GetComponent<CircleRenderer>().Clockwise)
                {
                    if (instCircles[i2].GetComponent<CircleRenderer>().Clockwise)
                    {
                        circlesPointsOut[i] = line1[0];
                        circlesPointsIn[i2] = line1[1];


                        doLine(line1[0], line1[1], i, Color.white);
                    }
                    else
                    {
                        circlesPointsOut[i] = line4[0];
                        circlesPointsIn[i2] = line4[1];

                        doLine(line4[0], line4[1], i, Color.blue);
                    }
                }
                else
                {

                    if (instCircles[i2].GetComponent<CircleRenderer>().Clockwise)
                    {
                        circlesPointsOut[i] = line3[0];
                        circlesPointsIn[i2] = line3[1];

                        doLine(line3[0], line3[1], i, Color.red);
                    }
                    else
                    {
                        circlesPointsOut[i] = line2[0];
                        circlesPointsIn[i2] = line2[1];

                        doLine(line2[0], line2[1], i, Color.yellow);
                    }
                }


                betwLBs[i] = new Vector3(c1Mid.x, 0f, c1Mid.y);
                betwLTs[i] = circlesPointsOut[i];
                betwRBs[i] = new Vector3(c2Mid.x, 0f, c2Mid.y);
                betwRTs[i] = circlesPointsIn[i2];
            }
        }
    }

    private Vector2[,] calcAll4Tangents(Vector2 c1Mid, float c1Radius, Vector2 c2Mid, float c2Radius)
    {
        Line midToMid = new Line(c1Mid, c2Mid);
        Vector2 c1ToC1 = c2Mid - c1Mid;
        Vector2 toRight = (new Vector2(c1ToC1.y, -c1ToC1.x)).normalized;

        Line outerLine = new Line(c1Mid + toRight * c1Radius, c2Mid + toRight * c2Radius);
        Line innerLine = new Line(c1Mid - toRight * c1Radius, c2Mid + toRight * c2Radius);

        Vector2 sInner;
        Vector2 sOuter;
        midToMid.Intersects(outerLine, out sOuter);
        midToMid.Intersects(innerLine, out sInner);

        Vector2[] outer1 = new Vector2[2];
        Vector2[] outer2 = new Vector2[2];
        Vector2[] inner1 = new Vector2[2];
        Vector2[] inner2 = new Vector2[2];

        // If the radius are too identical, the lines are parallel
        if (Mathf.Abs(c1Radius - c2Radius) > 0.01f)
        {
            float xIntersectCircles;
            float[] yIntersectCircles;
            float xIntersectCircles2;
            float[] yIntersectCircles2;

            Vector2[] tangentPoints;
            Vector2[] tangentPoints2;

            if (Vector2.Distance(sOuter, c1Mid) < Vector2.Distance(sOuter, c2Mid))
            {
                Vector2 tangentCircleMiddle = (sOuter - c1Mid) * 0.5f + c1Mid;
                float tangentCircleRadius = Vector2.Distance(c1Mid, tangentCircleMiddle);

                xIntersectCircles = (c1Radius * c1Radius) / (2f * tangentCircleRadius);
                yIntersectCircles = new float[] { Mathf.Sqrt(c1Radius * c1Radius - xIntersectCircles * xIntersectCircles), -Mathf.Sqrt(c1Radius * c1Radius - xIntersectCircles * xIntersectCircles) };


                Vector2 circleMiddleToTangentCircle = (tangentCircleMiddle - c1Mid).normalized;
                Vector2 circleMiddleToTangentCircleUp = new Vector2(-circleMiddleToTangentCircle.y, circleMiddleToTangentCircle.x);

                tangentPoints = new Vector2[] {   c1Mid + circleMiddleToTangentCircle * xIntersectCircles + circleMiddleToTangentCircleUp * yIntersectCircles[0],
                                                    c1Mid + circleMiddleToTangentCircle * xIntersectCircles + circleMiddleToTangentCircleUp * yIntersectCircles[1]};
                outer1[0] = tangentPoints[0];
                outer2[0] = tangentPoints[1];

                Vector2 tangentCircleMiddle2 = (sOuter - c2Mid) * 0.5f + c2Mid;
                float tangentCircleRadius2 = Vector2.Distance(c2Mid, tangentCircleMiddle2);

                xIntersectCircles2 = (c2Radius * c2Radius) / (2f * tangentCircleRadius2);
                yIntersectCircles2 = new float[] { Mathf.Sqrt(c2Radius * c2Radius - xIntersectCircles2 * xIntersectCircles2), -Mathf.Sqrt(c2Radius * c2Radius - xIntersectCircles2 * xIntersectCircles2) };


                Vector2 circleMiddleToTangentCircle2 = (tangentCircleMiddle2 - c2Mid).normalized;
                Vector2 circleMiddleToTangentCircleUp2 = new Vector2(-circleMiddleToTangentCircle2.y, circleMiddleToTangentCircle2.x);

                tangentPoints2 = new Vector2[] {   c2Mid + circleMiddleToTangentCircle2 * xIntersectCircles2 + circleMiddleToTangentCircleUp2 * yIntersectCircles2[0],
                                                    c2Mid + circleMiddleToTangentCircle2 * xIntersectCircles2 + circleMiddleToTangentCircleUp2 * yIntersectCircles2[1]};
                outer1[1] = tangentPoints2[0];
                outer2[1] = tangentPoints2[1];
            }
            else
            {
                Vector2 tangentCircleMiddle = (sOuter - c2Mid) * 0.5f + c2Mid;
                float tangentCircleRadius = Vector2.Distance(c2Mid, tangentCircleMiddle);

                xIntersectCircles = (c2Radius * c2Radius) / (2f * tangentCircleRadius);
                yIntersectCircles = new float[] { Mathf.Sqrt(c2Radius * c2Radius - xIntersectCircles * xIntersectCircles), -Mathf.Sqrt(c2Radius * c2Radius - xIntersectCircles * xIntersectCircles) };


                Vector2 circleMiddleToTangentCircle = (tangentCircleMiddle - c2Mid).normalized;
                Vector2 circleMiddleToTangentCircleUp = new Vector2(-circleMiddleToTangentCircle.y, circleMiddleToTangentCircle.x);

                tangentPoints = new Vector2[] {   c2Mid + circleMiddleToTangentCircle * xIntersectCircles + circleMiddleToTangentCircleUp * yIntersectCircles[0],
                                                    c2Mid + circleMiddleToTangentCircle * xIntersectCircles + circleMiddleToTangentCircleUp * yIntersectCircles[1]};
                outer1[1] = tangentPoints[0];
                outer2[1] = tangentPoints[1];


                Vector2 tangentCircleMiddle2 = (sOuter - c1Mid) * 0.5f + c1Mid;
                float tangentCircleRadius2 = Vector2.Distance(c1Mid, tangentCircleMiddle2);

                xIntersectCircles2 = (c1Radius * c1Radius) / (2f * tangentCircleRadius2);
                yIntersectCircles2 = new float[] { Mathf.Sqrt(c1Radius * c1Radius - xIntersectCircles2 * xIntersectCircles2), -Mathf.Sqrt(c1Radius * c1Radius - xIntersectCircles2 * xIntersectCircles2) };


                Vector2 circleMiddleToTangentCircle2 = (tangentCircleMiddle2 - c1Mid).normalized;
                Vector2 circleMiddleToTangentCircleUp2 = new Vector2(-circleMiddleToTangentCircle2.y, circleMiddleToTangentCircle2.x);

                tangentPoints2 = new Vector2[] {   c1Mid + circleMiddleToTangentCircle2 * xIntersectCircles2 + circleMiddleToTangentCircleUp2 * yIntersectCircles2[0],
                                                    c1Mid + circleMiddleToTangentCircle2 * xIntersectCircles2 + circleMiddleToTangentCircleUp2 * yIntersectCircles2[1]};
                outer1[0] = tangentPoints2[0];
                outer2[0] = tangentPoints2[1];
            }
        }
        else
        {
            outer1[0] = c1Mid - toRight * c1Radius;
            outer1[1] = c2Mid - toRight * c2Radius;
            outer2[0] = c1Mid + toRight * c1Radius;
            outer2[1] = c2Mid + toRight * c2Radius;
        }


        Vector2 inner_tangentCircleMiddle1 = (sInner - c1Mid) * 0.5f + c1Mid;
        float inner_tangentCircleRadius1 = Vector2.Distance(c1Mid, inner_tangentCircleMiddle1);

        float inner_innerXIntersectCircles1 = (c1Radius * c1Radius) / (2f * inner_tangentCircleRadius1);
        float[] inner_innerYIntersectCircles1 = new float[] { Mathf.Sqrt(c1Radius * c1Radius - inner_innerXIntersectCircles1 * inner_innerXIntersectCircles1), -Mathf.Sqrt(c1Radius * c1Radius - inner_innerXIntersectCircles1 * inner_innerXIntersectCircles1) };


        Vector2 inner_circleMiddleToTangentCircle1 = (inner_tangentCircleMiddle1 - c1Mid).normalized;
        Vector2 inner_circleMiddleToTangentCircleUp1 = new Vector2(-inner_circleMiddleToTangentCircle1.y, inner_circleMiddleToTangentCircle1.x);

        Vector2[] inner_innerTangentPointsC1 = new Vector2[] {   c1Mid + inner_circleMiddleToTangentCircle1 * inner_innerXIntersectCircles1 + inner_circleMiddleToTangentCircleUp1 * inner_innerYIntersectCircles1[0],
                                                    c1Mid + inner_circleMiddleToTangentCircle1 * inner_innerXIntersectCircles1 + inner_circleMiddleToTangentCircleUp1 * inner_innerYIntersectCircles1[1]};
        inner1[0] = inner_innerTangentPointsC1[0];
        inner2[0] = inner_innerTangentPointsC1[1];


        Vector2 inner_tangentCircleMiddle2 = (sInner - c2Mid) * 0.5f + c2Mid;
        float inner_tangentCircleRadius2 = Vector2.Distance(c2Mid, inner_tangentCircleMiddle2);

        float inner_innerXIntersectCircles2 = (c2Radius * c2Radius) / (2f * inner_tangentCircleRadius2);
        float[] inner_innerYIntersectCircles2 = new float[] { Mathf.Sqrt(c2Radius * c2Radius - inner_innerXIntersectCircles2 * inner_innerXIntersectCircles2), -Mathf.Sqrt(c2Radius * c2Radius - inner_innerXIntersectCircles2 * inner_innerXIntersectCircles2) };


        Vector2 inner_circleMiddleToTangentCircle2 = (inner_tangentCircleMiddle2 - c2Mid).normalized;
        Vector2 inner_circleMiddleToTangentCircleUp2 = new Vector2(-inner_circleMiddleToTangentCircle2.y, inner_circleMiddleToTangentCircle2.x);

        Vector2[] inner_innerTangentPointsC2 = new Vector2[] {   c2Mid + inner_circleMiddleToTangentCircle2 * inner_innerXIntersectCircles2 + inner_circleMiddleToTangentCircleUp2 * inner_innerYIntersectCircles2[0],
                                                    c2Mid + inner_circleMiddleToTangentCircle2 * inner_innerXIntersectCircles2 + inner_circleMiddleToTangentCircleUp2 * inner_innerYIntersectCircles2[1]};
        inner1[1] = inner_innerTangentPointsC2[1];
        inner2[1] = inner_innerTangentPointsC2[0];

        if (c1Radius > c2Radius)
        {
            Vector2[] temp1 = new Vector2[2];
            temp1[0] = outer1[0];
            temp1[1] = outer1[1];

            outer1[0] = outer2[0];
            outer1[1] = outer2[1];

            outer2[0] = temp1[0];
            outer2[1] = temp1[1];
        }

        return new Vector2[,] { { outer1[0], outer1[1] }, { outer2[0], outer2[1] }, { inner1[0], inner2[1] }, { inner2[0], inner1[1] } };
    }








    private void doLine(Vector3 startPos, Vector3 endPos, int counter, Color color)
    {
        if ((float.IsNaN(startPos.x) || float.IsNaN(startPos.y) || float.IsNaN(startPos.z) || float.IsNaN(endPos.x) || float.IsNaN(endPos.y) || float.IsNaN(endPos.z)) == false)
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
}

public class CirclesConstilation
{
    public Vector2[] Midpoints { get; set; }

    public float[] Radiuss { get; set; }

    public float AverageLength { get; set; }

    public bool[] Clockwises { get; set; }

}