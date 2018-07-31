using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratedBezSpline : GeneratedElement
{
    private int resolution = 4;

    private float firstTwoCpsDistance = 1f;
    private float lastTwoCpsDistance = 1f;
    private Vector3 endPosition;
    private float endDirection;

    private Vector3 b0;
    private Vector3 b1;
    private Vector3 b2;
    private Vector3 b3;

    private Vector3[] rightVerts;
    private Vector3[] leftVerts;

    private Bezier<Vector3> bezier;

    public GeneratedBezSpline(Vector3 position, float direction, Vector3 endPosition, float endDirection, float widthStart, float widthEnd) : this(position, direction, endPosition, endDirection, widthStart, widthEnd, 0f, 0f)
    {

    }

    public GeneratedBezSpline(Vector3 position, float direction, Vector3 endPosition, float endDirection, float widthStart, float widthEnd, float firstCpsDistance, float lastCpsDistance)
    {
        if (direction < 0f)
        {
            direction = direction + 2f * Mathf.PI;
        }
        else if (direction > 2f * Mathf.PI)
        {
            direction = direction - 2f * Mathf.PI;
        }
        if (endDirection < 0f)
        {
            endDirection = endDirection + 2f * Mathf.PI;
        }
        else if (endDirection > 2f * Mathf.PI)
        {
            endDirection = endDirection - 2f * Mathf.PI;
        }

        this.position = position;
        this.direction = direction;
        this.widthStart = widthStart;
        this.widthEnd = widthEnd;
        this.endPosition = endPosition;
        this.endDirection = endDirection;

        firstTwoCpsDistance = firstCpsDistance;
        lastTwoCpsDistance = lastCpsDistance;
        

        Vector3 startDirectionVec = (new Vector3(Mathf.Sin(direction), 0f, Mathf.Cos(direction))).normalized;
        Vector3 endDirectionVecInverted = (new Vector3(Mathf.Sin(endDirection), 0f, Mathf.Cos(endDirection))).normalized * -1f;

        if (firstCpsDistance == 0f || lastCpsDistance == 0f)
        {

            float minNow = 0f;

            int counterCatch = 0;

            float factor = 0.6f; //0.6f
            /*
            while (minNow <= widthEnd && counterCatch < 30)
            {
                counterCatch++;

                factor += 0.03f;

                //TODO
                // Gleichmaessige Aufteilung der Kontrollpunkte berechnen
                firstTwoCpsDistance = Vector3.Distance(position, endPosition) * factor;
                lastTwoCpsDistance = Vector3.Distance(position, endPosition) * factor;

                Bezier<Vector3> bezierTemp = new Bezier<Vector3>();

                bezierTemp.b0 = position;
                bezierTemp.b1 = position + startDirectionVec * firstTwoCpsDistance;
                bezierTemp.b2 = endPosition + endDirectionVecInverted * lastTwoCpsDistance;
                bezierTemp.b3 = endPosition;


                float minHere = float.MaxValue;

                for (int i = 0; i < 100; i++)
                {
                    float s = i / 100f;
                    float s2 = (i + 1) / 100f;

                    Vector2 po1 = new Vector2(bezierTemp.At(s).x, bezierTemp.At(s).z);
                    Vector2 po2 = new Vector2(bezierTemp.At(s2).x, bezierTemp.At(s2).z);

                    Vector2 in1 = new Vector2((-(bezierTemp.TangentAt(s)[1] - bezierTemp.TangentAt(s)[0]).z), ((bezierTemp.TangentAt(s)[1] - bezierTemp.TangentAt(s)[0]).x));
                    Vector2 in2 = new Vector2((-(bezierTemp.TangentAt(s2)[1] - bezierTemp.TangentAt(s2)[0]).z), ((bezierTemp.TangentAt(s2)[1] - bezierTemp.TangentAt(s2)[0]).x));

                    Ray2D ray1 = new Ray2D(po1, in1);
                    Ray2D ray2 = new Ray2D(po2, in2);

                    bool parallel;
                    Vector2 intersectPoint = Utils.Intersect2D(ray1, ray2, out parallel);

                    if (!parallel)
                    {
                        if (Vector2.Distance(po1, intersectPoint) < minHere)
                        {
                            minHere = Vector2.Distance(po1, intersectPoint);
                        }
                        if (Vector2.Distance(po2, intersectPoint) < minHere)
                        {
                            minHere = Vector2.Distance(po2, intersectPoint);
                        }
                    }
                }

                minNow = minHere;
            }

            if (counterCatch >= 30)
            {
                Debug.LogError("Over 999");
            }*/
            firstTwoCpsDistance = Vector3.Distance(position, endPosition) * factor;
            lastTwoCpsDistance = Vector3.Distance(position, endPosition) * factor;
        }

        b0 = position;
        b1 = position + startDirectionVec * firstTwoCpsDistance;
        b2 = endPosition + endDirectionVecInverted * lastTwoCpsDistance;
        b3 = endPosition;

        ftcD = firstTwoCpsDistance;
        ltcD = lastTwoCpsDistance;

        bezier = new Bezier<Vector3>();
        bezier.b0 = b0;
        bezier.b1 = b1;
        bezier.b2 = b2;
        bezier.b3 = b3;

        resolution = (int)(getLength() * 0.25f);

        fillVerts();
    }

    private float ftcD = 0f;
    private float ltcD = 0f;

    public float FirstTwoCpsDistance
    {
        get
        {
            return ftcD;
        }
    }

    public float LastTwoCpsDistance
    {
        get
        {
            return ltcD;
        }
    }

    protected override float getEndDirection()
    {
        return endDirection;
    }

    protected override Vector3 getEndPosition()
    {
        return endPosition;
    }

    private void fillVerts()
    {
        rightVerts = new Vector3[Resolution + 1];
        leftVerts = new Vector3[Resolution + 1];

        for (int i = 0; i <= Resolution; i++)
        {
            float s = (((float)i) / ((float)Resolution));

            Vector3 midPoint = bezier.At(s);
            Vector3 tangent = bezier.TangentAt(s)[1] - bezier.TangentAt(s)[0];
            Vector3 toRight = (new Vector3(tangent.z, tangent.y, -tangent.x)).normalized;

            rightVerts[i] = midPoint + toRight * ((widthEnd - widthStart) * s + widthStart);
            leftVerts[i] = midPoint - toRight * ((widthEnd - widthStart) * s + widthStart);

            if (float.IsNaN(rightVerts[i].x) || float.IsNaN(rightVerts[i].y) || float.IsNaN(rightVerts[i].z) || float.IsNaN(leftVerts[i].x) || float.IsNaN(leftVerts[i].y) || float.IsNaN(leftVerts[i].z))
            {
                rightVerts[i] = Vector3.zero;
                leftVerts[i] = Vector3.zero;
                //Debug.LogError("Somewhere NaN");
            }
        }
    }

    public override bool ForceEndDirection(float newEndDirection)
    {
        endDirection = newEndDirection;

        Vector3 endDirectionVecInverted = (new Vector3(Mathf.Sin(endDirection), 0f, Mathf.Cos(endDirection))).normalized * -1f;
        b2 = endPosition + endDirectionVecInverted * lastTwoCpsDistance * 1f;
        ltcD = lastTwoCpsDistance * 1f;
        bezier.b2 = b2;
        resolution = (int)(getLength() * 0.25f);
        fillVerts();

        return true;
    }

    private float cachedLength = -1f;

    protected override float getLength()
    {
        if (cachedLength >= 0f)
        {
            return cachedLength;
        }
        else
        {
            float length = 0f;
            Vector3 oldPos = bezier.At(0f);
            for (int i = 1; i <= 100; i++)
            {
                length += Vector3.Distance(oldPos, bezier.At(i / 100f));
                oldPos = bezier.At(i / 100f);
            }

            cachedLength = length;

            return length;
        }
    }

    public override GeneratedElement Copy()
    {
        GeneratedBezSpline copy = new GeneratedBezSpline(position, direction, endPosition, endDirection, widthStart, widthEnd, firstTwoCpsDistance, lastTwoCpsDistance);

        copy.b0 = b0;
        copy.b1 = b1;
        copy.b2 = b2;
        copy.b3 = b3;
        copy.bezier.b0 = b0;
        copy.bezier.b1 = b1;
        copy.bezier.b2 = b2;
        copy.bezier.b3 = b3;
        copy.resolution = resolution;
        copy.fillVerts();

        return copy;
    }

    public Bezier<Vector3> Spline
    {
        get
        {
            return bezier;
        }
    }

    public int Resolution
    {
        get
        {
            return resolution;
        }
        set
        {
            resolution = value;

            fillVerts();
        }
    }

    public Vector3[] RenderVertsRight
    {
        get
        {
            return rightVerts;
        }
    }

    public Vector3[] RenderVertsLeft
    {
        get
        {
            return leftVerts;
        }
    }
}
