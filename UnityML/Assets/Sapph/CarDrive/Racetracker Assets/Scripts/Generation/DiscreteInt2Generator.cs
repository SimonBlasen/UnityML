using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DiscreteInt2Generator
{
    public static float param_AnglePeriod = 0.1f;
    public static float param_AngleAmplitude = 40f;
    public static float param_AngleOffset = 4f;
    public static float param_RadiusPeriod = 0.4f;
    public static float param_RadiusAmplitude = 140f;
    public static float param_RadiusOffset = 150f;
    public static float param_SegDecMin = -2f;
    public static float param_SegDecMax = 2f;
    public static float param_SegDecPeriod = 1f;
    public static float param_ModePeriod = 1f;
    public static Vector2[] param_Circles = new Vector2[0];
    public static Vector2[] param_Circles_Outs = new Vector2[0];
    public static Vector2[] param_Circles_Ins = new Vector2[0];
    public static float[] param_Circles_Radius = new float[0];
    public static bool[] param_Circles_Clockwise = new bool[0];
    public static bool param_Circles_Use = false;
    public static Vector3[] param_BetwLB = new Vector3[0];
    public static Vector3[] param_BetwLT = new Vector3[0];
    public static Vector3[] param_BetwRB = new Vector3[0];
    public static Vector3[] param_BetwRT = new Vector3[0];
    public static DiscreteGenerationMode generationMode = DiscreteGenerationMode.PERFECT_PART_CIRCLES;

    private static PerlinNoise perlinNoise = null;

    public static GeneratedTrack GenerateTrack(float length, float width, long seed)
    {
        int cutsamount = 0;
        float anglePeriod = param_AnglePeriod;
        float angleAmplitude = param_AngleAmplitude;
        float angleOffset = param_AngleOffset;
        float radiusPeriod = param_RadiusPeriod;
        float radiusAmplitude = param_RadiusAmplitude;
        float radiusOffset = param_RadiusOffset;

        // Segment Decider
        //
        // Decider works like this:
        //   If noise returns < -1, the segment is a straight
        //   If noise returns > 1, the segment is a turn
        //   Else, it is a combination of both
        //
        float segDecMin = param_SegDecMin;
        float segDecMax = param_SegDecMax;
        float segDecPeriod = param_SegDecPeriod;

        float modePeriod = param_ModePeriod;


        perlinNoise = new PerlinNoise(seed);
        GeneratedTrack track = new GeneratedTrack();
        track.Faulty = false;

        List<Circle> circles = new List<Circle>();
        List<OvercrossBetweening> overcrossBetweenings = new List<OvercrossBetweening>();

        bool overCrossBetweening = false;

        int circlesAmount = perlinNoise.noise1(1564f) >= 0f ? 4 : 3;

        float maxCircleMidpointDistance = 400f;

        if (param_Circles_Use == false)
        {
            if (circlesAmount == 3)
            {

                Vector2 c1Dir = new Vector2(0f, 1f);
                float c1Dist = (perlinNoise.noise1(15f) + 1f) * 0.5f * maxCircleMidpointDistance;
                circles.Add(new Circle(c1Dir * c1Dist, 250f, true));

                Vector2 c2Dir = new Vector2(Mathf.Sin(120f * Mathf.PI / 180f), Mathf.Cos(120f * Mathf.PI / 180f));
                float c2Dist = (perlinNoise.noise1(441f) + 1f) * 0.5f * maxCircleMidpointDistance;
                circles.Add(new Circle(c2Dir * c2Dist, 250f, true));

                Vector2 c3Dir = new Vector2(Mathf.Sin(240f * Mathf.PI / 180f), Mathf.Cos(240f * Mathf.PI / 180f));
                float c3Dist = (perlinNoise.noise1(7424f) + 1f) * 0.5f * maxCircleMidpointDistance;
                circles.Add(new Circle(c3Dir * c3Dist, 250f, true));
            }
            else if (circlesAmount == 4)
            {
                Vector2 c1Dir = new Vector2(0f, 1f);
                float c1Dist = (perlinNoise.noise1(15f) + 1f) * 0.5f * maxCircleMidpointDistance;
                circles.Add(new Circle(c1Dir * c1Dist, 250f, true));

                Vector2 c2Dir = new Vector2(Mathf.Sin(90f * Mathf.PI / 180f), Mathf.Cos(90f * Mathf.PI / 180f));
                float c2Dist = (perlinNoise.noise1(441f) + 1f) * 0.5f * maxCircleMidpointDistance;
                circles.Add(new Circle(c2Dir * c2Dist, 250f, true));

                Vector2 c3Dir = new Vector2(Mathf.Sin(180f * Mathf.PI / 180f), Mathf.Cos(180f * Mathf.PI / 180f));
                float c3Dist = (perlinNoise.noise1(7424f) + 1f) * 0.5f * maxCircleMidpointDistance;
                circles.Add(new Circle(c3Dir * c3Dist, 250f, true));

                Vector2 c4Dir = new Vector2(Mathf.Sin(270f * Mathf.PI / 180f), Mathf.Cos(270f * Mathf.PI / 180f));
                float c4Dist = (perlinNoise.noise1(14447f) + 1f) * 0.5f * maxCircleMidpointDistance;
                circles.Add(new Circle(c4Dir * c4Dist, 250f, true));
            }
        }
        else
        {
            for (int i = 0; i < param_Circles.Length; i++)
            {
                circles.Add(new Circle(param_Circles[i], param_Circles_Radius[i], param_Circles_Clockwise[i], param_Circles_Ins[i], param_Circles_Outs[i]));
                overcrossBetweenings.Add(new OvercrossBetweening(param_BetwLB[i], param_BetwLT[i], param_BetwRB[i], param_BetwRT[i]));
            }
        }
        

        track.Circles = circles.ToArray();

        //circles.Add(new Circle(new Vector2(150f, -100f), 250f, true));
        //circles.Add(new Circle(new Vector2(-150f, -100f), 250f, true));

        float startingAngle = Vector2.Angle(circles[0].In - circles[0].Midpoint, new Vector2(0f, 1f));
        if (Vector2.Angle(circles[0].In - circles[0].Midpoint, new Vector2(1f, 0f)) > 90f)
        {
            startingAngle = 360f - startingAngle;
        }
        List<float> circleDirections = new List<float>();
        //List<bool> circleClockwises = new List<bool>();
        for (int i = 0; i < circles.Count; i++)
        {
            /* int i2 = (i + 1) % circles.Count;

             Vector2 c1Toc2 = circles[i2].Midpoint - circles[i].Midpoint;
             Vector2 c1ToLeft = (new Vector2(-c1Toc2.y, c1Toc2.x)).normalized * circles[i].Radius;
             Vector2 c2ToLeft = (new Vector2(-c1Toc2.y, c1Toc2.x)).normalized * circles[i2].Radius;

             Vector2 c1Outs = circles[i].Midpoint + c1ToLeft;
             Vector2 c2Outs = circles[i2].Midpoint + c2ToLeft;

             Vector2 vec = c2Outs - c1Outs;

             float zAngle = Vector2.Angle(new Vector2(0f, 1f), vec);
             float xAngle = Vector2.Angle(new Vector2(1f, 0f), vec);
             float circleDirection = zAngle;

             if (xAngle > 90f)
             {
                 circleDirection = 360f - zAngle;
             }

             circleDirection = circleDirection - 90f;
             if (circleDirection < 0f)
             {
                 circleDirection = 360f - circleDirection;
             }

             circleDirections.Add(circleDirection);*/

            float angleBetw = Vector2.Angle(circles[i].In - circles[i].Midpoint, circles[i].Out - circles[i].Midpoint);
            if (Vector2.Angle(new Vector2((circles[i].In - circles[i].Midpoint).y, -(circles[i].In - circles[i].Midpoint).x), circles[i].Out - circles[i].Midpoint) > 90f)
            {
                angleBetw = 360f - angleBetw;
            }

            if (circles[i].Clockwise == false)
            {
                angleBetw = 360f - angleBetw;
            }

            circleDirections.Add(angleBetw);
        }


        int angleStep = 0;
        int radiusStep = 0;
        int segmentStep = 0;

        int circleIndex = 0;

        float partAngleCircle = 0f;

        float partDistanceOvercrossBetween = 0f;

        float currentAngle = 0f;

        currentAngle = startingAngle;

        //float startRadius = perlinNoise.noise1(radiusStep * radiusPeriod) * circles[0].Radius + circles[0].Radius;
        float startRadius = perlinNoise.noise1(radiusStep * radiusPeriod) * circles[circleIndex].Radius * 0.55f + circles[circleIndex].Radius * 0.5f;

        Vector2 startPoint = new Vector2(Mathf.Sin((currentAngle * Mathf.PI) / 180f) * startRadius, Mathf.Cos((currentAngle * Mathf.PI) / 180f) * startRadius);
        startPoint += circles[circleIndex].Midpoint;

        while (true && angleStep < 100)
        {
            angleStep++;
            radiusStep++;
            segmentStep++;

            // Segment Decider is between [-1, 1]
            float segDec = perlinNoise.noise1(segmentStep * segDecPeriod) * ((Mathf.Abs(segDecMin) + Mathf.Abs(segDecMax)) / 2f);
            segDec = segDec + ((segDecMin + segDecMax) / 2f);
            segDec = segDec > 1f ? 1f : (segDec < -1f ? -1f : segDec);

            // Angle Delta between [0, angleAmplitude]
            float angleDelta = (perlinNoise.noise1(angleStep * anglePeriod) * 0.5f + 0.5f) * angleAmplitude + angleOffset;

            angleDelta *= (315f / circles[circleIndex].Radius);

            if (overCrossBetweening == false)
            {
                if (circles[circleIndex].Clockwise)
                {
                    currentAngle += angleDelta;
                }
                else
                {
                    currentAngle -= angleDelta;
                }
                partAngleCircle += angleDelta;
            }

            // Radius between [0, 2 * radiusAmplitude]
            //float radius = perlinNoise.noise1(radiusStep * radiusPeriod) * param_RadiusAmplitude /*circles[circleIndex].Radius*/ + circles[circleIndex].Radius;
            float radius = perlinNoise.noise1(radiusStep * radiusPeriod) * circles[circleIndex].Radius * 0.5f + circles[circleIndex].Radius * 0.5f;

            // Mode is either PERFECT_PART_CIRCLES or BEZIER_SPLINES
            bool takeBezierSpline = false;

            if (generationMode == DiscreteGenerationMode.BEZIER_SPLINES)
            {
                takeBezierSpline = true;
            }
            else if (generationMode == DiscreteGenerationMode.PERFECT_PART_CIRCLES)
            {
                takeBezierSpline = false;
            }
            else
            {
                takeBezierSpline = perlinNoise.noise1(angleStep * modePeriod) >= 0f;
            }

            bool driveBackToStart = false;

            if (partAngleCircle >= circleDirections[circleIndex] && overCrossBetweening == false)
            {
                partAngleCircle = 0f;
                //circleIndex++;

                overCrossBetweening = true;
                partDistanceOvercrossBetween = 0f;

                //if (circleIndex >= circles.Count)
                //{
                //    circleIndex = 0;
                //    driveBackToStart = true;
                //}
                //else
                //{
                //    if (circles[circleIndex].Clockwise != circles[circleIndex - 1].Clockwise)
                //    {
                //        currentAngle += 180f;
                //    }
                //}
            }




            float estimatedDistance = Vector2.Distance(param_Circles_Outs[circleIndex], param_Circles_Ins[(circleIndex + 1) % param_Circles_Ins.Length]);

            float overcrossbetweenDelta = (perlinNoise.noise1(angleStep * anglePeriod) * 0.5f + 0.5f) * (320f / estimatedDistance) + 0f;
            partDistanceOvercrossBetween += overcrossbetweenDelta;


            if (partDistanceOvercrossBetween >= 1f && overCrossBetweening)
            {
                partDistanceOvercrossBetween = 0f;
                partAngleCircle = 0f;

                overCrossBetweening = false;

                circleIndex++;
                if (circleIndex >= circles.Count)
                {
                    circleIndex = 0;
                    driveBackToStart = true;
                }
                else
                {
                    if (circles[circleIndex].Clockwise != circles[circleIndex - 1].Clockwise)
                    {
                        currentAngle += 180f;
                    }
                }
            }

            Vector2 point;
            if (overCrossBetweening)
            {
                float yStep = perlinNoise.noise1(radiusStep * radiusPeriod) * 0.5f + 0.5f;

                point = new Vector2(overcrossBetweenings[circleIndex].Interpolate(partDistanceOvercrossBetween, yStep).x, overcrossBetweenings[circleIndex].Interpolate(partDistanceOvercrossBetween, yStep).z);
            }
            else
            {
                point = new Vector2(Mathf.Sin((currentAngle * Mathf.PI) / 180f) * radius, Mathf.Cos((currentAngle * Mathf.PI) / 180f) * radius);

                point += circles[circleIndex].Midpoint;
            }

            //Debug.Log("Iteration: " + angleStep);
            //Debug.Log("CurrentAngle: " + currentAngle);
            //Debug.Log("Radius: " + radius);
            //Debug.Log("SegDec: " + segDec);

            bool fixedEndpointDirection = false;
            float endpointDirection = Mathf.PI * 0.5f;
            float endpointWidth = width;


            Vector2 oldPoint;
            float oldDirection;
            if (track.Elements.Length == 0)
            {
                oldPoint = startPoint;

                Vector2 hopefullyForward = new Vector2((startPoint - circles[circleIndex].Midpoint).y, -(startPoint - circles[circleIndex].Midpoint).x);
                if (circles[circleIndex].Clockwise == false)
                {
                    hopefullyForward *= -1f;
                }

                float angleTempForw = Vector2.Angle(new Vector2(0f, 1f), hopefullyForward);
                if (Vector2.Angle(new Vector2(1f, 0f), hopefullyForward) > 90f)
                {
                    angleTempForw = 360 - angleTempForw;
                }

                oldDirection = (angleTempForw / 180f) * Mathf.PI;

                //oldDirection = Mathf.PI * 0.5f;
            }
            else
            {
                oldPoint = new Vector2(track.Elements[track.Elements.Length - 1].EndPosition.x, track.Elements[track.Elements.Length - 1].EndPosition.z);
                oldDirection = track.Elements[track.Elements.Length - 1].EndDirection;
            }

            if (Vector2.Distance(point, oldPoint) < 60f)
            {
                //Debug.Log("Point too near");
                Vector2 oldToP = (point - oldPoint).normalized * 60f;

                partAngleCircle += angleDelta * 1.3f;
                if (circles[circleIndex].Clockwise)
                {
                    currentAngle += angleDelta * 1.3f;
                }
                else
                {
                    currentAngle -= angleDelta * 1.3f;
                }




                point = oldPoint + oldToP;
            }

            //if (currentAngle >= 360f)
            if (driveBackToStart)
            {
                fixedEndpointDirection = true;

                point = new Vector2(track.Elements[0].Position.x, track.Elements[0].Position.z);
                endpointDirection = track.Elements[0].Direction;
                endpointWidth = track.Elements[0].WidthStart;
            }


            if (fixedEndpointDirection == false)
            {
                // Completely a turn
                if (segDec >= 1f)
                {
                    if (takeBezierSpline == false)
                    {
                        float[] radiusAndDegree = makeTurn(oldPoint, point, oldDirection);

                        float circleRadius = radiusAndDegree[0];
                        float circleDegree = radiusAndDegree[1];

                        int splinesAmount = (int)(Mathf.Abs(circleDegree) / 90f) + 1;

                        float degreeOneSpline = circleDegree / splinesAmount;

                        float tanValue = 360f / degreeOneSpline;
                        tanValue = Mathf.Abs((4f / 3f) * Mathf.Tan(Mathf.PI / (2f * tanValue)) * circleRadius);

                        Vector2 prevSplinePoint = oldPoint;

                        //Debug.Log("Gonna do new");

                        for (int i = 0; i < splinesAmount; i++)
                        {
                            float tempStartDir = oldDirection + i * degreeOneSpline * (Mathf.PI / 180f);
                            Vector2 midToPoint = (new Vector2(Mathf.Cos(tempStartDir) * Mathf.Sign(circleDegree), -Mathf.Sin(tempStartDir) * Mathf.Sign(circleDegree))).normalized * circleRadius;

                            float tempEndDir = oldDirection + (i + 1) * degreeOneSpline * (Mathf.PI / 180f);
                            Vector2 midToEndpoint = (new Vector2(Mathf.Cos(tempEndDir) * Mathf.Sign(circleDegree), -Mathf.Sin(tempEndDir) * Mathf.Sign(circleDegree))).normalized * circleRadius;

                            Vector2 tempEndpoint = (prevSplinePoint + midToPoint) - midToEndpoint;

                            GeneratedBezSpline spline = new GeneratedBezSpline(new Vector3(prevSplinePoint.x, 0f, prevSplinePoint.y), tempStartDir, new Vector3(tempEndpoint.x, 0f, tempEndpoint.y), tempEndDir, width, width, tanValue, tanValue);

                            track.AddElement(spline);

                            prevSplinePoint = tempEndpoint;

                            //Debug.Log("Spline[" + i + "]");
                        }





                        //GeneratedTurn turn = new GeneratedTurn(new Vector3(oldPoint.x, 0f, oldPoint.y), oldDirection, circleRadius, circleDegree, width);

                        //Vector2 turnEndPoint = new Vector2(turn.EndPosition.x, turn.EndPosition.z);
                        if (Vector2.Distance(new Vector2(track.Elements[track.Elements.Length - 1].EndPosition.x, track.Elements[track.Elements.Length - 1].EndPosition.z), point) > 1f)
                        {
                            track.Faulty = true;
                            Debug.LogError("Too big distance 1");
                        }

                        //track.AddElement(turn);
                    }
                    else
                    {
                        float[] radiusAndDegree = makeTurn(oldPoint, point, oldDirection);

                        float circleRadius = radiusAndDegree[0];
                        float circleDegree = radiusAndDegree[1];

                        GeneratedTurn turn = new GeneratedTurn(new Vector3(oldPoint.x, 0f, oldPoint.y), oldDirection, circleRadius, circleDegree, width);

                        float turnEndDirection = turn.EndDirection;

                        float interDirection = (perlinNoise.noise1(angleStep * anglePeriod) * 0.5f + 0.5f) * (oldDirection - turnEndDirection) + turnEndDirection;

                        if (Mathf.Abs(interDirection - oldDirection) > Mathf.PI * 0.25f)
                        {
                            float directionBetween = (oldDirection - turnEndDirection) * 0.5f + turnEndDirection;

                            Vector2 oldToPoint = point - oldPoint;
                            /*float oldToPointDirection = Vector2.Angle(new Vector2(0f, 1f), oldToPoint);
                            if (Vector2.Angle(new Vector2(1f, 0f), oldToPoint) > 90f)
                            {
                                oldToPointDirection = 360 - oldToPointDirection;
                            }
                            */
                            Ray2D rayOldDirection = new Ray2D(oldPoint, new Vector2(Mathf.Sin(oldDirection), Mathf.Cos(oldDirection)));

                            Vector2 orthogonVec = Vector2.zero;

                            if (Utils.PointRightTo(rayOldDirection, oldPoint + oldToPoint))
                            {
                                orthogonVec = new Vector2(-oldToPoint.y, oldToPoint.x);
                            }
                            else
                            {
                                orthogonVec = new Vector2(oldToPoint.y, -oldToPoint.x);
                            }

                            Vector2 middlePoint = oldToPoint * 0.5f + oldPoint + orthogonVec * 0.5f;


                            GeneratedBezSpline turnSpline = new GeneratedBezSpline(new Vector3(oldPoint.x, 0f, oldPoint.y), oldDirection, new Vector3(middlePoint.x, 0f, middlePoint.y), directionBetween, width, width);

                            track.AddElement(turnSpline);



                            GeneratedBezSpline turnSpline2 = new GeneratedBezSpline(new Vector3(middlePoint.x, 0f, middlePoint.y), directionBetween, new Vector3(point.x, 0f, point.y), interDirection, width, width);

                            track.AddElement(turnSpline2);
                        }
                        else
                        {
                            GeneratedBezSpline turnSpline = new GeneratedBezSpline(new Vector3(oldPoint.x, 0f, oldPoint.y), oldDirection, new Vector3(point.x, 0f, point.y), interDirection, width, width);

                            track.AddElement(turnSpline);
                        }

                    }
                }

                // Part straight part turn
                else
                {
                    if (takeBezierSpline == false)
                    {
                        float maxWidth = width;
                        //TODO not only one width all the time

                        float minRadius = maxWidth + 0.8f;

                        float[] radiusAndDegree = makeTurn(oldPoint, point, oldDirection);

                        float circleRadius = radiusAndDegree[0];
                        float circleDegree = radiusAndDegree[1];

                        float maxRadius = circleRadius;

                        float circlePartRadius = (maxRadius - minRadius) * ((segDec + 1f) * 0.5f) + minRadius;

                        Vector2 circleMiddle = Vector2.zero;

                        float[] partcircleRadiusAndDegree = makeTurnFixedRadius(oldPoint, circlePartRadius, oldDirection, (circleDegree >= 0f), point);

                        float partcircleDegree = partcircleRadiusAndDegree[0];
                        float partstraightLength = partcircleRadiusAndDegree[1];


                        if (partcircleDegree < 20f)
                        {
                            //Debug.Log("Less than 10 degrees");
                            GeneratedBezSpline spline = new GeneratedBezSpline(new Vector3(oldPoint.x, 0f, oldPoint.y), oldDirection, new Vector3(point.x, 0f, point.y), partcircleDegree * (Mathf.PI / 180f) + oldDirection, width, width);

                            track.AddElement(spline);
                        }
                        else
                        {
                            int splinesAmount = (int)(Mathf.Abs(partcircleDegree) / 90f) + 1;

                            float degreeOneSpline = partcircleDegree / splinesAmount;

                            float tanValue = 360f / degreeOneSpline;
                            tanValue = Mathf.Abs((4f / 3f) * Mathf.Tan(Mathf.PI / (2f * tanValue)) * circlePartRadius);

                            Vector2 prevSplinePoint = oldPoint;

                            //Debug.Log("Gonna do new");

                            for (int i = 0; i < splinesAmount; i++)
                            {
                                float tempStartDir = oldDirection + i * degreeOneSpline * (Mathf.PI / 180f);
                                Vector2 midToPoint = (new Vector2(Mathf.Cos(tempStartDir) * Mathf.Sign(partcircleDegree), -Mathf.Sin(tempStartDir) * Mathf.Sign(partcircleDegree))).normalized * circlePartRadius;

                                float tempEndDir = oldDirection + (i + 1) * degreeOneSpline * (Mathf.PI / 180f);
                                Vector2 midToEndpoint = (new Vector2(Mathf.Cos(tempEndDir) * Mathf.Sign(partcircleDegree), -Mathf.Sin(tempEndDir) * Mathf.Sign(partcircleDegree))).normalized * circlePartRadius;

                                Vector2 tempEndpoint = (prevSplinePoint + midToPoint) - midToEndpoint;

                                /*if (Vector2.Distance(tempEndpoint, prevSplinePoint) < 4.3f)
                                {
                                    Vector2 oldToP = (tempEndpoint - prevSplinePoint).normalized * 4.3f;

                                    Debug.Log("Went in 4.3f");

                                    tempEndpoint = prevSplinePoint + oldToP;
                                }*/

                                GeneratedBezSpline spline = new GeneratedBezSpline(new Vector3(prevSplinePoint.x, 0f, prevSplinePoint.y), tempStartDir, new Vector3(tempEndpoint.x, 0f, tempEndpoint.y), tempEndDir, width, width, tanValue, tanValue);

                                track.AddElement(spline);

                                prevSplinePoint = tempEndpoint;

                                //Debug.Log("Spline[" + i + "]");
                            }


















                            ///GeneratedTurn turn = new GeneratedTurn(new Vector3(oldPoint.x, 0f, oldPoint.y), oldDirection, circlePartRadius, partcircleDegree, width);
                            //track.AddElement(turn);

                            GeneratedStraight straight = new GeneratedStraight(track.Elements[track.Elements.Length - 1].EndPosition, track.Elements[track.Elements.Length - 1].EndDirection, partstraightLength, width);
                            //straight.AddCurb(new GeneratedCurb(0.1f, 0.9f, true, true, true));
                            track.AddElement(straight);


                            Vector2 bothEndPoint = new Vector2(straight.EndPosition.x, straight.EndPosition.z);
                            if (Vector2.Distance(bothEndPoint, point) > 1f)
                            {
                                Debug.LogError("Too big distance 2");
                                track.Faulty = true;
                            }
                        }



                        
                    }
                    else
                    {
                        // Only straight
                        if (segDec <= -1f && track.Elements.Length > 0 && track.Elements[track.Elements.Length - 1].GetType() == typeof(GeneratedBezSpline))
                        {
                            float angleBetween = Vector2.Angle(new Vector2(0f, 1f), point - oldPoint);
                            float angleRight = Vector2.Angle(new Vector2(1f, 0f), point - oldPoint);
                            if (angleRight > 90f)
                            {
                                angleBetween = 360f - angleBetween;
                            }
                            float radianOldEndDirection = (angleBetween * Mathf.PI) / 180f;

                            track.Elements[track.Elements.Length - 1].ForceEndDirection(radianOldEndDirection);

                            GeneratedStraight straight = new GeneratedStraight(new Vector3(oldPoint.x, 0f, oldPoint.y), radianOldEndDirection, Vector2.Distance(oldPoint, point), width);

                            track.AddElement(straight);
                        }

                        // Really part turn part straight
                        else
                        {
                            float maxWidth = width;
                            //TODO not only one width all the time

                            float minRadius = maxWidth + 0.8f;

                            float[] radiusAndDegree = makeTurn(oldPoint, point, oldDirection);

                            float circleRadius = radiusAndDegree[0];
                            float circleDegree = radiusAndDegree[1];

                            float maxRadius = circleRadius;

                            float circlePartRadius = (maxRadius - minRadius) * ((segDec + 1f) * 0.5f) + minRadius;

                            Vector2 circleMiddle = Vector2.zero;

                            float[] partcircleRadiusAndDegree = makeTurnFixedRadius(oldPoint, circlePartRadius, oldDirection, (circleDegree >= 0f), point);

                            float partcircleDegree = partcircleRadiusAndDegree[0];
                            float partstraightLength = partcircleRadiusAndDegree[1];

                            GeneratedTurn turn = new GeneratedTurn(new Vector3(oldPoint.x, 0f, oldPoint.y), oldDirection, circlePartRadius, partcircleDegree, width);
                            //track.AddElement(turn);

                            float turnEndDirection = turn.EndDirection;

                            GeneratedBezSpline turnSpline = new GeneratedBezSpline(new Vector3(oldPoint.x, 0f, oldPoint.y), oldDirection, turn.EndPosition, turnEndDirection, width, width);
                            track.AddElement(turnSpline);

                            GeneratedStraight straight = new GeneratedStraight(turn.EndPosition, turnEndDirection, Vector2.Distance(point, new Vector2(turn.EndPosition.x, turn.EndPosition.z)), width);
                            //straight.AddCurb(new GeneratedCurb(0.1f, 0.9f, true, true, true));
                            track.AddElement(straight);


                            Vector2 bothEndPoint = new Vector2(straight.EndPosition.x, straight.EndPosition.z);
                            if (Vector2.Distance(bothEndPoint, point) > 1f)
                            {
                                track.Faulty = true;
                                //Debug.LogError("Too big distance");
                            }
                        }
                    }
                }
            }

            // Drive to a fixed endpoint
            else
            {
                // Two turns one straight
                if (false)
                {
                    float[] angleStraightAndAngle = makeTwoTurnsOneStraight(oldPoint, point, oldDirection, endpointDirection, 1f, 0.5f, Mathf.Max(endpointWidth, width));
                    float circle1Radius = angleStraightAndAngle[0];
                    float circle1Degree = angleStraightAndAngle[1];
                    float straightLegnth = angleStraightAndAngle[2];
                    float circle2Radius = angleStraightAndAngle[3];
                    float circle2Degree = angleStraightAndAngle[4];

                    if (circle1Radius == 0f && circle1Degree == 0f && straightLegnth == 0f && circle2Degree == 0f && circle2Radius == 0f)
                    {
                        track.Faulty = true;
                    }
                    else
                    {
                        GeneratedTurn turn = new GeneratedTurn(new Vector3(oldPoint.x, 0f, oldPoint.y), oldDirection, circle1Radius, circle1Degree, width);
                        track.AddElement(turn);

                        GeneratedStraight straight = new GeneratedStraight(turn.EndPosition, turn.EndDirection, straightLegnth, width);
                        track.AddElement(straight);

                        GeneratedTurn turn2 = new GeneratedTurn(straight.EndPosition, straight.EndDirection, circle2Radius, circle2Degree, width);
                        track.AddElement(turn2);
                    }
                }


                // Do it with Beziers
                else
                {
                    GeneratedBezSpline turnSpline = new GeneratedBezSpline(new Vector3(oldPoint.x, 0f, oldPoint.y), oldDirection, new Vector3(point.x, 0f, point.y), endpointDirection, width, width);

                    track.AddElement(turnSpline);
                }
            }






            

            if (driveBackToStart)
            {
                break;
            }

            //if (currentAngle >= 360f)
            //{
            //    break;
            //}
        }





        

        for (int at = 0; at < track.Elements.Length; at++)
        {
            List<Line> lastElementLinesLeft = new List<Line>();
            List<Line> lastElementLinesRight = new List<Line>();
            Vector2 tempPrevVertLeft = Vector2.zero;
            Vector2 tempPrevVertRight = Vector2.zero;

            if (track.Elements[at].GetType() == typeof(GeneratedBezSpline))
            {
                GeneratedBezSpline spline = (GeneratedBezSpline)track.Elements[at];

                tempPrevVertLeft = new Vector2(spline.RenderVertsLeft[0].x, spline.RenderVertsLeft[0].z);
                tempPrevVertRight = new Vector2(spline.RenderVertsRight[0].x, spline.RenderVertsRight[0].z);

                for (int j = 1; j < spline.RenderVertsLeft.Length; j++)
                {
                    Line lineLeft = new Line(tempPrevVertLeft, new Vector2(spline.RenderVertsLeft[j].x, spline.RenderVertsLeft[j].z));
                    tempPrevVertLeft = new Vector2(spline.RenderVertsLeft[j].x, spline.RenderVertsLeft[j].z);
                    Line lineRight = new Line(tempPrevVertRight, new Vector2(spline.RenderVertsRight[j].x, spline.RenderVertsRight[j].z));
                    tempPrevVertRight = new Vector2(spline.RenderVertsRight[j].x, spline.RenderVertsRight[j].z);

                    lastElementLinesLeft.Add(lineLeft);
                    lastElementLinesRight.Add(lineRight);
                }
            }
            else if (track.Elements[at].GetType() == typeof(GeneratedStraight))
            {
                GeneratedStraight straight = (GeneratedStraight)track.Elements[at];

                Vector3 toRight = (Quaternion.Euler(0f, (straight.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(straight.WidthEnd, 0f, 0f));
                Vector3 toFront = (Quaternion.Euler(0f, (straight.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(0f, 0f, straight.Length));

                tempPrevVertLeft = new Vector2((straight.Position + (toFront * 0f) - toRight).x, (straight.Position + (toFront * 0f) - toRight).z);
                tempPrevVertRight = new Vector2((straight.Position + (toFront * 0f) + toRight).x, (straight.Position + (toFront * 0f) + toRight).z);

                Line lineLeft = new Line(tempPrevVertLeft, new Vector2((straight.Position + (toFront * 1f) - toRight).x, (straight.Position + (toFront * 1f) - toRight).z));
                tempPrevVertLeft = new Vector2((straight.Position + (toFront * 1f) - toRight).x, (straight.Position + (toFront * 1f) - toRight).z);
                Line lineRight = new Line(tempPrevVertRight, new Vector2((straight.Position + (toFront * 1f) + toRight).x, (straight.Position + (toFront * 1f) + toRight).z));
                tempPrevVertRight = new Vector2((straight.Position + (toFront * 1f) + toRight).x, (straight.Position + (toFront * 1f) + toRight).z);

                lastElementLinesLeft.Add(lineLeft);
                lastElementLinesRight.Add(lineRight);
            }



            Vector2 prevVertLeft = Vector2.zero;
            Vector2 prevVertRight = Vector2.zero;

            if (track.Elements[0].GetType() == typeof(GeneratedBezSpline))
            {
                GeneratedBezSpline spline = (GeneratedBezSpline)track.Elements[0];
                prevVertLeft = new Vector2(spline.RenderVertsLeft[0].x, spline.RenderVertsLeft[0].z);
                prevVertRight = new Vector2(spline.RenderVertsRight[0].x, spline.RenderVertsRight[0].z);
            }
            else if (track.Elements[0].GetType() == typeof(GeneratedStraight))
            {
                GeneratedStraight straight = (GeneratedStraight)track.Elements[0];

                Vector3 toRight = (Quaternion.Euler(0f, (straight.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(straight.WidthEnd, 0f, 0f));
                Vector3 toFront = (Quaternion.Euler(0f, (straight.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(0f, 0f, straight.Length));

                prevVertLeft = new Vector2((straight.Position + (toFront * 0f) - toRight).x, (straight.Position + (toFront * 0f) - toRight).z);
                prevVertRight = new Vector2((straight.Position + (toFront * 0f) + toRight).x, (straight.Position + (toFront * 0f) + toRight).z);
            }



            bool cutSomewhere = false;
            Vector2 isecOut;

            for (int i = 0; i < at; i++)
            {
                List<Line> testLinesLeft = new List<Line>();
                List<Line> testLinesRight = new List<Line>();

                if (track.Elements[i].GetType() == typeof(GeneratedBezSpline))
                {
                    GeneratedBezSpline spline = (GeneratedBezSpline)track.Elements[i];
                    for (int j = 1; j < spline.RenderVertsLeft.Length; j++)
                    {
                        Line lineLeft = new Line(prevVertLeft, new Vector2(spline.RenderVertsLeft[j].x, spline.RenderVertsLeft[j].z));
                        prevVertLeft = new Vector2(spline.RenderVertsLeft[j].x, spline.RenderVertsLeft[j].z);
                        Line lineRight = new Line(prevVertRight, new Vector2(spline.RenderVertsRight[j].x, spline.RenderVertsRight[j].z));
                        prevVertRight = new Vector2(spline.RenderVertsRight[j].x, spline.RenderVertsRight[j].z);

                        testLinesLeft.Add(lineLeft);
                        testLinesRight.Add(lineRight);
                    }
                }
                else if (track.Elements[i].GetType() == typeof(GeneratedStraight))
                {
                    GeneratedStraight straight = (GeneratedStraight)track.Elements[i];

                    Vector3 toRight = (Quaternion.Euler(0f, (straight.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(straight.WidthEnd, 0f, 0f));
                    Vector3 toFront = (Quaternion.Euler(0f, (straight.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(0f, 0f, straight.Length));

                    Line lineLeft = new Line(prevVertLeft, new Vector2((straight.Position + (toFront * 1f) - toRight).x, (straight.Position + (toFront * 1f) - toRight).z));
                    prevVertLeft = new Vector2((straight.Position + (toFront * 1f) - toRight).x, (straight.Position + (toFront * 1f) - toRight).z);
                    Line lineRight = new Line(prevVertRight, new Vector2((straight.Position + (toFront * 1f) + toRight).x, (straight.Position + (toFront * 1f) + toRight).z));
                    prevVertRight = new Vector2((straight.Position + (toFront * 1f) + toRight).x, (straight.Position + (toFront * 1f) + toRight).z);

                    testLinesLeft.Add(lineLeft);
                    testLinesRight.Add(lineRight);
                }

                for (int j = 0; j < lastElementLinesLeft.Count; j++)
                {
                    if (i != at - 1)
                    {
                        for (int k = 0; k < testLinesLeft.Count; k++)
                        {
                            if (lastElementLinesLeft[j].Intersects(testLinesLeft[k], out isecOut))
                            {
                                //Debug.Log("Isec: " + isecOut);
                                cutSomewhere = true;
                                cutsamount++;
                            }
                        }
                        for (int k = 0; k < testLinesRight.Count; k++)
                        {
                            if (lastElementLinesLeft[j].Intersects(testLinesRight[k], out isecOut))
                            {
                                //Debug.Log("Isec: " + isecOut);
                                cutSomewhere = true;
                                cutsamount++;
                            }
                        }
                    }

                    // In this case, leave an epsilon at the end to prevent self cutting
                    else
                    {
                        for (int k = 0; k < testLinesLeft.Count - 1; k++)
                        {
                            if (lastElementLinesLeft[j].Intersects(testLinesLeft[k], out isecOut))
                            {
                                //Debug.Log("Isec: " + isecOut);
                                cutSomewhere = true;
                                cutsamount++;
                            }
                        }
                        for (int k = 0; k < testLinesRight.Count - 1; k++)
                        {
                            if (lastElementLinesLeft[j].Intersects(testLinesRight[k], out isecOut))
                            {
                                //Debug.Log("Isec: " + isecOut);
                                cutSomewhere = true;
                                cutsamount++;
                            }
                        }
                    }
                }
                for (int j = 0; j < lastElementLinesRight.Count; j++)
                {
                    if (i != at - 1)
                    {
                        for (int k = 0; k < testLinesLeft.Count; k++)
                        {
                            if (lastElementLinesRight[j].Intersects(testLinesLeft[k], out isecOut))
                            {
                                //Debug.Log("Isec: " + isecOut);
                                cutSomewhere = true;
                                cutsamount++;
                            }
                        }
                        for (int k = 0; k < testLinesRight.Count; k++)
                        {
                            if (lastElementLinesRight[j].p1.x < -58.91f && lastElementLinesRight[j].p2.x < -58.91f && testLinesRight[k].p1.x < -58.91f && testLinesRight[k].p2.x < -58.91f
                                && lastElementLinesRight[j].p1.x > -70.7f && lastElementLinesRight[j].p2.x > -70.7f && testLinesRight[k].p1.x > -70.7f && testLinesRight[k].p2.x > -70.7f
                                && lastElementLinesRight[j].p1.y < 22.21f && lastElementLinesRight[j].p2.y < 22.21f && testLinesRight[k].p1.y < 22.21f && testLinesRight[k].p2.y < 22.21f
                                && lastElementLinesRight[j].p1.y > 19.18f && lastElementLinesRight[j].p2.y > 19.18f && testLinesRight[k].p1.y > 19.18f && testLinesRight[k].p2.y > 19.18f)
                            {
                                //Debug.Log("Threre");

                            }
                            if (lastElementLinesRight[j].Intersects(testLinesRight[k], out isecOut))
                            {
                                //Debug.Log("Isec: " + isecOut);
                                cutSomewhere = true;
                                cutsamount++;
                            }
                        }
                    }

                    // In this case, leave an epsilon at the end to prevent self cutting
                    else
                    {
                        for (int k = 0; k < testLinesLeft.Count - 1; k++)
                        {
                            if (lastElementLinesRight[j].Intersects(testLinesLeft[k], out isecOut))
                            {
                                //Debug.Log("Isec: " + isecOut);
                                cutSomewhere = true;
                                cutsamount++;
                            }
                        }
                        for (int k = 0; k < testLinesRight.Count - 1; k++)
                        {
                            if (lastElementLinesRight[j].Intersects(testLinesRight[k], out isecOut))
                            {
                                //Debug.Log("Isec: " + isecOut);
                                cutSomewhere = true;
                                cutsamount++;
                            }
                        }
                    }
                }
            }


            if (cutSomewhere)
            {
                //Debug.Log("Faulty due to cutting");
                //track.Faulty = true;
            }
        }

        if (cutsamount > 1)
        {
            track.Faulty = true;
        }



        //Debug.Log("Amount of cuts: " + cutsamount);

        return track;
    }

    public static bool Test()
    {
        bool success = true;
        if (makeTurn(new Vector2(0f, 0f), new Vector2(2f, 2f), 0f) != (new float[] { 2f, 90f })) success = false;
        if (makeTurn(new Vector2(0f, 0f), new Vector2(3f, 2f), 0f) != (new float[] { 2.166666666666f, 112.6199f })) success = false;
        if (makeTurn(new Vector2(0f, 0f), new Vector2(4f, 0f), 0f) != (new float[] { 2f, 180f })) success = false;
        if (makeTurn(new Vector2(0f, 0f), new Vector2(2f, -2f), 0f) != (new float[] { 2f, 270f })) success = false;

        if (makeTurnFixedRadius(new Vector2(0f, 0f), 2f, 0f, true, new Vector2(8f, 2f)) != (new float[] { 90f, 6f })) success = false;
        if (makeTurnFixedRadius(new Vector2(0f, 0f), 2f, 0f, true, new Vector2(4f, 0f)) != (new float[] { 180f, 0f })) success = false;
        if (makeTurnFixedRadius(new Vector2(0f, 1f), 1f, Mathf.PI * 0.5f, true, new Vector2(8f, 0f)) != (new float[] { 180f, 0f })) success = false;
        if (makeTurnFixedRadius(new Vector2(0f, 0f), 10000f, Mathf.PI * 0.5f, true, new Vector2(7f, 0f)) != (new float[] { 180f, 0f })) success = false;

        if (makeTwoTurnsOneStraight(new Vector2(0f, 0f), new Vector2(2f, 3f), Mathf.PI * 0.5f, Mathf.PI * 0.5f, 1f, 0.5f, 1f) != (new float[] { 180f, 0f })) success = false;
        if (makeTwoTurnsOneStraight(new Vector2(0f, 0f), new Vector2(5f, 0f), 0f, Mathf.PI, 1f, 0.5f, 1f) != (new float[] { 180f, 0f })) success = false;

        return success;
    }


    /// <summary>
    /// This method creates a part, consisting of a turn, then a straight, and then a turn again.
    /// It attaches the part matching to the given start-direction and the given end-direction
    /// </summary>
    /// <param name="oldPoint"></param>
    /// <param name="point"></param>
    /// <param name="oldDirection"></param>
    /// <param name="direction"></param>
    /// <param name="partStraight">Which amount should be built out of straight. Value between 0 and 1</param>
    /// <param name="weightFirstTurn">The relation between the first turns radius and the second turns radius. Value between 0 and 1</param>
    /// <param name="minRadius"></param>
    /// <returns></returns>
    private static float[] makeTwoTurnsOneStraight(Vector2 oldPoint, Vector2 point, float oldDirection, float direction, float partStraight, float weightFirstTurn, float minRadius)
    {
        Ray2D rayOldpoint = new Ray2D(oldPoint, new Vector2(Mathf.Sin(oldDirection), Mathf.Cos(oldDirection)));

        Vector2 toRightOldpoint = new Vector2(Mathf.Cos(oldDirection), -Mathf.Sin(oldDirection));
        toRightOldpoint.Normalize();

        Vector2 circle1Midpoint = oldPoint - (toRightOldpoint * minRadius);
        bool is1RightTurn = false;

        if (Utils.PointRightTo(rayOldpoint, point))
        {
            circle1Midpoint = oldPoint + (toRightOldpoint * minRadius);
            is1RightTurn = true;
        }




        Ray2D rayPoint = new Ray2D(point, new Vector2(Mathf.Sin(direction), Mathf.Cos(direction)));

        Vector2 toRightPoint = new Vector2(Mathf.Cos(direction), -Mathf.Sin(direction));
        toRightPoint.Normalize();

        Vector2 circle2Midpoint = point - (toRightPoint * minRadius);
        bool is2RightTurn = false;

        if (Utils.PointRightTo(rayPoint, oldPoint))
        {
            circle2Midpoint = point + (toRightPoint * minRadius);
            is2RightTurn = true;
        }



        float radius1 = Vector2.Distance(oldPoint, circle1Midpoint);
        float radius2 = Vector2.Distance(point, circle2Midpoint);

        Vector2 c1Toc2 = circle2Midpoint - circle1Midpoint;
        Ray2D c1Toc2Ray = new Ray2D(circle1Midpoint, c1Toc2);

        Vector2 circleToLeft = (new Vector2(-c1Toc2.y, c1Toc2.x)).normalized;

        Vector2 tangentOuterLeftC1; Vector2 tangentOuterLeftC1Direction;
        Vector2 tangentOuterLeftC2; Vector2 tangentOuterLeftC2Direction;
        Vector2 tangentOuterRightC1; Vector2 tangentOuterRightC1Direction;
        Vector2 tangentOuterRightC2; Vector2 tangentOuterRightC2Direction;
        Vector2 tangentInnerLeftC1; Vector2 tangentInnerLeftC1Direction;
        Vector2 tangentInnerLeftC2; Vector2 tangentInnerLeftC2Direction;
        Vector2 tangentInnerRightC1; Vector2 tangentInnerRightC1Direction;
        Vector2 tangentInnerRightC2; Vector2 tangentInnerRightC2Direction;

        if (Mathf.Abs(radius1 - radius2) < 0.001f)
        {
            tangentOuterLeftC1 = circle1Midpoint + circleToLeft * radius1;

            tangentOuterLeftC2 = circle2Midpoint + circleToLeft * radius2;

            tangentOuterRightC1 = circle1Midpoint - circleToLeft * radius1;

            tangentOuterRightC2 = circle2Midpoint - circleToLeft * radius2;


            tangentOuterLeftC1Direction = (tangentOuterLeftC2 - tangentOuterLeftC1).normalized;
            tangentOuterLeftC2Direction = (tangentOuterLeftC2 - tangentOuterLeftC1).normalized;
            tangentOuterRightC1Direction = (tangentOuterRightC2 - tangentOuterRightC1).normalized;
            tangentOuterRightC2Direction = (tangentOuterRightC2 - tangentOuterRightC1).normalized;
        }
        else
        {
            Ray2D outerCutRay = new Ray2D(circle1Midpoint + circleToLeft * radius1, (circle1Midpoint + circleToLeft * radius1) - (circle2Midpoint + circleToLeft * radius2));

            bool parallel;

            Vector2 sPointOuter = Utils.Intersect2D(c1Toc2Ray, outerCutRay, out parallel);

            float sToC1 = Vector2.Distance(sPointOuter, circle1Midpoint);
            float sToC2 = Vector2.Distance(sPointOuter, circle2Midpoint);
            float tangentCircleRadius1 = sToC1 * 0.5f;
            float tangentCircleRadius2 = sToC2 * 0.5f;

            //TODO calculate outer tangents with the sPointOuter

            float xIntersectCircles1 = (radius1 * radius1) / (2f * tangentCircleRadius1);
            float[] yIntersectCircles1 = new float[] { Mathf.Sqrt(radius1 * radius1 - xIntersectCircles1 * xIntersectCircles1), -Mathf.Sqrt(radius1 * radius1 - xIntersectCircles1 * xIntersectCircles1) };

            float xIntersectCircles2 = (radius2 * radius2) / (2f * tangentCircleRadius2);
            float[] yIntersectCircles2 = new float[] { Mathf.Sqrt(radius2 * radius2 - xIntersectCircles2 * xIntersectCircles2), -Mathf.Sqrt(radius2 * radius2 - xIntersectCircles2 * xIntersectCircles2) };

            Vector2 circleToS = (sPointOuter - circle1Midpoint).normalized;

            tangentOuterLeftC1 = circle1Midpoint + circleToS * xIntersectCircles1 + circleToLeft.normalized * Mathf.Abs(yIntersectCircles1[0]);
            tangentOuterLeftC2 = circle2Midpoint + circleToS * xIntersectCircles2 + circleToLeft.normalized * Mathf.Abs(yIntersectCircles2[0]);
            tangentOuterRightC1 = circle1Midpoint + circleToS * xIntersectCircles1 - circleToLeft.normalized * Mathf.Abs(yIntersectCircles1[0]);
            tangentOuterRightC2 = circle2Midpoint + circleToS * xIntersectCircles2 - circleToLeft.normalized * Mathf.Abs(yIntersectCircles2[0]);

            tangentOuterLeftC1Direction = (tangentOuterLeftC1 - sPointOuter).normalized;
            tangentOuterLeftC2Direction = (tangentOuterLeftC2 - sPointOuter).normalized;
            tangentOuterRightC1Direction = (tangentOuterRightC1 - sPointOuter).normalized;
            tangentOuterRightC2Direction = (tangentOuterRightC2 - sPointOuter).normalized;
        }

        Ray2D rayForSMiddle = new Ray2D(circle1Midpoint + circleToLeft * radius1, (circle2Midpoint - circleToLeft * radius2) - (circle1Midpoint + circleToLeft * radius1));


        bool parallelS;
        Vector2 sMiddle = Utils.Intersect2D(c1Toc2Ray, rayForSMiddle, out parallelS);

        float innerTangentCircleRadius1 = Vector2.Distance(sMiddle, circle1Midpoint) * 0.5f;
        float innerTangentCircleRadius2 = Vector2.Distance(sMiddle, circle2Midpoint) * 0.5f;


        float xInnerIntersectCircles1 = (radius1 * radius1) / (2f * innerTangentCircleRadius1);
        float[] yInnerIntersectCircles1 = new float[] { Mathf.Sqrt(radius1 * radius1 - xInnerIntersectCircles1 * xInnerIntersectCircles1), -Mathf.Sqrt(radius1 * radius1 - xInnerIntersectCircles1 * xInnerIntersectCircles1) };

        float xInnerIntersectCircles2 = (radius2 * radius2) / (2f * innerTangentCircleRadius2);
        float[] yInnerIntersectCircles2 = new float[] { Mathf.Sqrt(radius2 * radius2 - xInnerIntersectCircles2 * xInnerIntersectCircles2), -Mathf.Sqrt(radius2 * radius2 - xInnerIntersectCircles2 * xInnerIntersectCircles2) };


        tangentInnerLeftC1 = circle1Midpoint + (sMiddle - circle1Midpoint).normalized * xInnerIntersectCircles1 + circleToLeft * Mathf.Abs(yInnerIntersectCircles1[0]);
        tangentInnerRightC2 = circle2Midpoint + (sMiddle - circle2Midpoint).normalized * xInnerIntersectCircles2 - circleToLeft * Mathf.Abs(yInnerIntersectCircles2[0]);

        tangentInnerRightC1 = circle1Midpoint + (sMiddle - circle1Midpoint).normalized * xInnerIntersectCircles1 - circleToLeft * Mathf.Abs(yInnerIntersectCircles1[0]);
        tangentInnerLeftC2 = circle2Midpoint + (sMiddle - circle2Midpoint).normalized * xInnerIntersectCircles2 + circleToLeft * Mathf.Abs(yInnerIntersectCircles2[0]);


        tangentInnerLeftC1Direction = (tangentInnerRightC2 - tangentInnerLeftC1).normalized;
        tangentInnerRightC2Direction = (tangentInnerRightC2 - tangentInnerLeftC1).normalized;
        tangentInnerRightC1Direction = (tangentInnerLeftC2 - tangentInnerRightC1).normalized;
        tangentInnerLeftC2Direction = (tangentInnerLeftC2 - tangentInnerRightC1).normalized;

        Vector2 point1 = Vector2.zero;
        Vector2 point2 = Vector2.zero;

        Vector2 tangentOuterLeftC1DirectionDrive = new Vector2((tangentOuterLeftC1 - circle1Midpoint).normalized.y, -((tangentOuterLeftC1 - circle1Midpoint).normalized.x));
        Vector2 tangentOuterLeftC2DirectionDrive = new Vector2((tangentOuterLeftC2 - circle2Midpoint).normalized.y, -((tangentOuterLeftC2 - circle2Midpoint).normalized.x));
        Vector2 tangentOuterRightC1DirectionDrive = new Vector2((tangentOuterRightC1 - circle1Midpoint).normalized.y, -((tangentOuterRightC1 - circle1Midpoint).normalized.x));
        Vector2 tangentOuterRightC2DirectionDrive = new Vector2((tangentOuterRightC2 - circle2Midpoint).normalized.y, -((tangentOuterRightC2 - circle2Midpoint).normalized.x));
        Vector2 tangentInnerLeftC1DirectionDrive = new Vector2((tangentInnerLeftC1 - circle1Midpoint).normalized.y, -((tangentInnerLeftC1 - circle1Midpoint).normalized.x));
        Vector2 tangentInnerLeftC2DirectionDrive = new Vector2((tangentInnerLeftC2 - circle2Midpoint).normalized.y, -((tangentInnerLeftC2 - circle2Midpoint).normalized.x));
        Vector2 tangentInnerRightC1DirectionDrive = new Vector2((tangentInnerRightC1 - circle1Midpoint).normalized.y, -((tangentInnerRightC1 - circle1Midpoint).normalized.x));
        Vector2 tangentInnerRightC2DirectionDrive = new Vector2((tangentInnerRightC2 - circle2Midpoint).normalized.y, -((tangentInnerRightC2 - circle2Midpoint).normalized.x));

        if (is1RightTurn == false)
        {
            tangentOuterLeftC1DirectionDrive *= -1f;
            tangentOuterRightC1DirectionDrive *= -1f;
            tangentInnerLeftC1DirectionDrive *= -1f;
            tangentInnerRightC1DirectionDrive *= -1f;
        }
        if (is2RightTurn == false)
        {
            tangentOuterLeftC2DirectionDrive *= -1f;
            tangentOuterRightC2DirectionDrive *= -1f;
            tangentInnerLeftC2DirectionDrive *= -1f;
            tangentInnerRightC2DirectionDrive *= -1f;
        }

        bool debugTestWentIn = false;

        if (Vector2.Angle(tangentOuterLeftC1Direction, tangentOuterLeftC1DirectionDrive) < 90f && Vector2.Angle(tangentOuterLeftC2Direction, tangentOuterLeftC2DirectionDrive) < 90f)
        {
            point1 = tangentOuterLeftC1;
            point2 = tangentOuterLeftC2;
            debugTestWentIn = true;
        }
        if (Vector2.Angle(tangentOuterRightC1Direction, tangentOuterRightC1DirectionDrive) < 90f && Vector2.Angle(tangentOuterRightC2Direction, tangentOuterRightC2DirectionDrive) < 90f)
        {
            if (debugTestWentIn) Debug.LogError("Went in twice");
            point1 = tangentOuterRightC1;
            point2 = tangentOuterRightC2;
            debugTestWentIn = true;
        }
        if (Vector2.Angle(tangentInnerLeftC1Direction, tangentInnerLeftC1DirectionDrive) < 90f && Vector2.Angle(tangentInnerRightC2Direction, tangentInnerRightC2DirectionDrive) < 90f)
        {
            if (debugTestWentIn) Debug.LogError("Went in twice");
            point1 = tangentInnerLeftC1;
            point2 = tangentInnerRightC2;
            debugTestWentIn = true;
        }
        if (Vector2.Angle(tangentInnerRightC1Direction, tangentInnerRightC1DirectionDrive) < 90f && Vector2.Angle(tangentInnerLeftC2Direction, tangentInnerLeftC2DirectionDrive) < 90f)
        {
            if (debugTestWentIn) Debug.LogError("Went in twice");
            point1 = tangentInnerRightC1;
            point2 = tangentInnerLeftC2;
            debugTestWentIn = true;
        }

        if (point1 == Vector2.zero || point2 == Vector2.zero)
        {
            Debug.LogError("Went never in");
            return new float[] { 0f, 0f, 0f, 0f, 0f };
        }











        float circle1Degree = Vector2.Angle(circle1Midpoint - oldPoint, circle1Midpoint - point1);
        float circle1Radius = minRadius;

        bool is1GreaterThan180 = Utils.PointRightTo(new Ray2D(oldPoint, new Vector2(Mathf.Cos(oldDirection), -Mathf.Sin(oldDirection))), point1);

        if (is1GreaterThan180)
        {
            circle1Degree = 360 - circle1Degree;
        }

        if (is1RightTurn == false)
        {
            circle1Degree = -circle1Degree;
        }



        float circle2Degree = Vector2.Angle(circle2Midpoint - point, circle2Midpoint - point2);
        float circle2Radius = minRadius;

        bool is2GreaterThan180 = Utils.PointRightTo(new Ray2D(point, new Vector2(-Mathf.Cos(direction), Mathf.Sin(direction))), point2);

        if (is2GreaterThan180)
        {
            circle2Degree = 360 - circle2Degree;
        }

        if (is2RightTurn == false)
        {
            circle2Degree = -circle2Degree;
        }



        float straightLength = Vector2.Distance(point1, point2);

        return new float[] { circle1Radius, circle1Degree, straightLength, circle2Radius, circle2Degree };
    }

    /// <summary>
    /// This method creates a part-turn first, and then a straight part towards the destiny-point
    /// </summary>
    /// <param name="oldPoint">The origin point, where this track-part starts</param>
    /// <param name="radius">The radius of the turn-part</param>
    /// <param name="oldDirection">The direction in oldPoint</param>
    /// <param name="rightTurn">Whether it is a right turn or a left turn</param>
    /// <param name="point">The destination point</param>
    /// <returns>A float-array with two elements. The first contains the angle of the turn-part, the second element contains the lenght of the straight</returns>
    private static float[] makeTurnFixedRadius(Vector2 oldPoint, float radius, float oldDirection, bool rightTurn, Vector2 point)
    {
        if (radius > 1000f)
        {
            radius = 1000f;
        }

        Vector2 toForward = new Vector2(Mathf.Sin(oldDirection), Mathf.Cos(oldDirection));
        Vector2 toRight = new Vector2(Mathf.Cos(oldDirection), -Mathf.Sin(oldDirection));
        if (!rightTurn)
        {
            toRight *= -1f;
        }
        toRight.Normalize();
        toRight *= radius;

        Vector2 circleMiddle = oldPoint + toRight;

        Vector2 tangentCircleMiddle = (point - circleMiddle) * 0.5f + circleMiddle;
        float tangentCircleRadius = Vector2.Distance(circleMiddle, tangentCircleMiddle);

        float xIntersectCircles = (radius * radius) / (2f * tangentCircleRadius);
        float[] yIntersectCircles = new float[] { Mathf.Sqrt(radius * radius - xIntersectCircles * xIntersectCircles), -Mathf.Sqrt(radius * radius - xIntersectCircles * xIntersectCircles) };

        Vector2 circleMiddleToTangentCircle = (tangentCircleMiddle - circleMiddle).normalized;
        Vector2 circleMiddleToTangentCircleUp = new Vector2(-circleMiddleToTangentCircle.y, circleMiddleToTangentCircle.x);

        Vector2[] tangentPoints = new Vector2[] {   circleMiddle + circleMiddleToTangentCircle * xIntersectCircles + circleMiddleToTangentCircleUp * yIntersectCircles[0],
                                                    circleMiddle + circleMiddleToTangentCircle * xIntersectCircles + circleMiddleToTangentCircleUp * yIntersectCircles[1]};

        Vector2[] driveDirectionTangents = new Vector2[] { new Vector2(tangentPoints[0].y - circleMiddle.y, -1f * (tangentPoints[0].x - circleMiddle.x)), new Vector2(tangentPoints[1].y - circleMiddle.y, -1f * (tangentPoints[1].x - circleMiddle.x)) };

        if (rightTurn == false)
        {
            driveDirectionTangents[0] *= -1f;
            driveDirectionTangents[1] *= -1f;
        }

        Vector2 tangentPointToTake = tangentPoints[0];
        if (Vector2.Angle(driveDirectionTangents[0], point - tangentPoints[0]) > 90f)
        {
            tangentPointToTake = tangentPoints[1];
        }

        float[] angleAndLength = new float[2];

        float circleDegree = Vector2.Angle(circleMiddle - oldPoint, circleMiddle - tangentPointToTake);
        //float circleDegree = Vector2.Angle(g.direction, circleMiddle - point);

        bool isRightTurn = Utils.PointRightTo(new Ray2D(oldPoint, new Vector2(Mathf.Sin(oldDirection), Mathf.Cos(oldDirection))), tangentPointToTake);

        bool isGreaterThan180 = Utils.PointRightTo(new Ray2D(oldPoint, new Vector2(Mathf.Cos(oldDirection), -Mathf.Sin(oldDirection))), tangentPointToTake);

        if (isGreaterThan180)
        {
            circleDegree = 360 - circleDegree;
        }

        if (isRightTurn == false)
        {
            circleDegree = -circleDegree;
        }

        float straightLength = Vector2.Distance(tangentPointToTake, point);

        return new float[] { circleDegree, straightLength };
    }

    /// <summary>
    /// This method creates a turn given a start-point with a direction, and a destiny-point
    /// </summary>
    /// <param name="oldPoint">The origin point, where this turn starts</param>
    /// <param name="point">The destination point</param>
    /// <param name="oldDirection">The direction in oldPoint</param>
    /// <returns>A float-array with two elements. The first contains the radius of the turn, the second element contains the angle of the turn</returns>
    private static float[] makeTurn(Vector2 oldPoint, Vector2 point, float oldDirection)
    {
        Vector2 s = point - oldPoint;

        Ray2D g = new Ray2D(oldPoint, new Vector2(Mathf.Cos(oldDirection), -Mathf.Sin(oldDirection)));

        Ray2D sO = new Ray2D(oldPoint + 0.5f * (s), new Vector2(s.y, -s.x)); // Due to 90 degree rotation, the switched s.y and s.x is intended

        bool parallel;
        Vector2 circleMiddle = Utils.Intersect2D(g, sO, out parallel);

        if (!parallel)
        {
            float circleRadius = (circleMiddle - oldPoint).magnitude;
            float circleDegree = Vector2.Angle(circleMiddle - oldPoint, circleMiddle - point);
            //float circleDegree = Vector2.Angle(g.direction, circleMiddle - point);

            bool isRightTurn = Utils.PointRightTo(new Ray2D(oldPoint, new Vector2(Mathf.Sin(oldDirection), Mathf.Cos(oldDirection))), point);

            bool isGreaterThan180 = Utils.PointRightTo(g, point);

            if (isGreaterThan180)
            {
                circleDegree = 360 - circleDegree;
            }

            if (isRightTurn == false)
            {
                circleDegree = -circleDegree;
            }

            //TODO right-turn vs left-turn. Sign of the angle is influenced. what if > 180 ? needs to be handled

            //TODO test whole function :P

            return new float[] { circleRadius, circleDegree };
        }

        return null;
    }
}