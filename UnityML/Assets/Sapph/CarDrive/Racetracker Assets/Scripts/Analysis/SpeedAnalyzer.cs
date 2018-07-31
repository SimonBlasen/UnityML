using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedAnalyzer
{
    private float[] vs;
    private Vector3[] force;

    private ClosedSpline<Vector3> idealLine;

    public SpeedAnalyzer(ClosedSpline<Vector3> idealLine, ClosedSpline<float> bankSpline, CurvatureAnalyzer curvature, int sampleRate)
    {
        this.idealLine = idealLine;

        vs = new float[sampleRate];
        force = new Vector3[sampleRate];

        for (int i = 0; i < sampleRate; i++)
        {
            vs[i] = 0f;
            force[i] = Vector3.zero;
        }

        for (int twice = 0; twice < 2; twice++)
        {
            for (int i = 0; i < sampleRate; i++)
            {
                int iPrev = i - 1;
                if (iPrev < 0) { iPrev = sampleRate - 1; }

                float sPrev = ((float)iPrev) / sampleRate;
                float s = ((float)i) / sampleRate;

                float angleHere = Vector3.Angle(idealLine.TangentAt(sPrev)[1] - idealLine.TangentAt(sPrev)[0], new Vector3((idealLine.TangentAt(sPrev)[1] - idealLine.TangentAt(sPrev)[0]).x, 0f, (idealLine.TangentAt(sPrev)[1] - idealLine.TangentAt(sPrev)[0]).z));
                if ((idealLine.TangentAt(sPrev)[1] - idealLine.TangentAt(sPrev)[0]).y < 0f)
                {
                    angleHere = -angleHere;
                }

                float estimatedT = -1f + Mathf.Sqrt(1f + (Vector3.Distance(idealLine.SplineAt(s), idealLine.SplineAt(sPrev)) * 2f) / carAcceleration(vs[iPrev], angleHere));
                float v = vs[iPrev] + estimatedT * carAcceleration(vs[iPrev], angleHere);

                float aZ = carAcceleration(vs[iPrev], angleHere);

                float radius = Mathf.Abs(1f / curvature.Curvature[(int)(s * curvature.Curvature.Length)]);

                float bankToRightAngle = bankSpline.SplineAt(s);

                float vMax = lateralAcceleration(bankToRightAngle, radius, curvature.Curvature[(int)(s * curvature.Curvature.Length)] >= 0f);

                //float vMax = Mathf.Sqrt(lateralAcceleration(v) * radius);

                if (vMax < v)
                {
                    v = vMax;
                    float vBack = v;
                    for (int j = 1; j < sampleRate; j++)
                    {
                        int index = i - j;
                        if (index < 0) { index = sampleRate + index; }
                        int indexPrev = index + 1;
                        if (indexPrev >= sampleRate) { indexPrev = 0; }

                        float sTempPrev = ((float)indexPrev) / sampleRate;
                        float sTemp = ((float)index) / sampleRate;

                        float estimatedTTemp = -1f + Mathf.Sqrt(1f + (Vector3.Distance(idealLine.SplineAt(s), idealLine.SplineAt(sPrev)) * 2f) / carAcceleration(vs[iPrev], angleHere));
                        float vMaxBack = vBack + estimatedTTemp * braking(vBack, angleHere);

                        if (vMaxBack < vs[index])
                        {
                            vs[index] = vMaxBack;
                            force[index] = new Vector3((vMaxBack * vMaxBack) / radius, force[index].y, -braking(vBack, angleHere));
                            vBack = vMaxBack;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                vs[i] = v;

                force[i] = new Vector3((v * v * Mathf.Sign(curvature.Curvature[(int)(s * curvature.Curvature.Length)])) / radius, 0f, aZ);
            }
        }



    }

    //public SpeedAnalyzer(ClosedSpline<Vector3> idealLine, CurvatureAnalyzer curvature) : this(idealLine, curvature, 300)
    //{

    //}


    public float[] Vs
    {
        get
        {
            return vs;
        }
    }

    public Vector3[] Forces
    {
        get
        {
            return force;
        }
    }


    private float carAcceleration(float v)
    {
        float time0to100 = 160f;      //time in s
        float maxSpeed = 200f;      //km/h

        float a0 = (100000f / (60f * 60f)) / time0to100;
        float m = a0 / ((50f - maxSpeed) * (50f - maxSpeed));

        //float iSectV = maxSpeed - (Mathf.Sqrt(a0 / m));

        if (v <= 50f)
        {
            return a0;
        }
        else if (v <= maxSpeed)
        {
            return (v - maxSpeed) * (v - maxSpeed) * m;
        }
        else
        {
            return 0f;
        }
    }


    private float carAcceleration(float v, float terrainAngle)
    {
        float mass = 1f;
        float time0to100 = 160f;      //time in s
        float maxSpeed = 200f;      //km/h

        float a0 = (100000f / (60f * 60f)) / time0to100;
        float m = a0 / ((50f - maxSpeed) * (50f - maxSpeed));

        float grav = Mathf.Sin(terrainAngle * Mathf.PI / 180f) * mass * 0.17f;

        //float iSectV = maxSpeed - (Mathf.Sqrt(a0 / m));

        float retVal = 0f;

        if (v <= 50f)
        {
            retVal = a0 * (1f / mass) - grav;
        }
        else if (v <= maxSpeed)
        {
            retVal = (v - maxSpeed) * (v - maxSpeed) * m * (1f / mass) - grav;
        }
        else
        {
            retVal = 0f;
        }


        if (retVal < 0.01f)
        {
            Debug.Log("retVal is < 0");
            return 0.01f;
        }
        else
        {
            return retVal;
        }
    }

    private float lateralAcceleration(float v)
    {
        return 12.2f * 2.7f;
    }

    private float lateralAcceleration(float angleToRight, float radius, bool rightTurn)
    {
        //Debug.Log(angleToRight);

        float tan = Mathf.Tan(Mathf.Abs(angleToRight) * Mathf.PI / 180f);

        bool helpfull = (angleToRight >= 0f) != rightTurn;

        float mu = helpfull ? 0.01f : -5f;

        float valT = ((2f / 3f) * 9.81f * (tan - mu)) / (1f + (helpfull ? 1f : -1f) * mu * tan) + (helpfull ? 32.7f : 0f);

        return Mathf.Sqrt(valT * radius);
    }

    private float braking(float v)
    {
        return 0.4f;
    }

    private float braking(float v, float terrainAngle)
    {
        float mass = 1f;
        float grav = Mathf.Sin(terrainAngle * Mathf.PI / 180f) * mass * 0.17f;

        //((2 / 3) * 9.81 * (tan(if (x < 0,-x,x)*3.141 / 180)-if (x < 0,0.01,-5)))/ (1 + (if (x < 0,1,-1)*if (x < 0,0.01,-5)*tan(if (x < 0,-x,x)*3.141 / 180)))+(if (x < 0,32.7,0))

        return 0.4f + grav;
    }
}
