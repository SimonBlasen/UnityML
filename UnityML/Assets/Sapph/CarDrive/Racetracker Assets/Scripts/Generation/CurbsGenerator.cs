using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurbsGenerator
{
    private List<GeneratedCurb> genCurbs;

    // The widths are needed for the grass planes
    private float[] leftWidths;
    private float[] rightWidths;

    private Vector2[] leftV;
    private Vector2[] rightV;


    private int offset = 0;

    public CurbsGenerator(Vector2[] leftVertices, Vector2[] rightVertices, int offset)
    {
        genCurbs = new List<GeneratedCurb>();

        leftV = leftVertices;
        rightV = rightVertices;

        leftWidths = new float[leftVertices.Length];
        rightWidths = new float[rightVertices.Length];

        this.offset = offset;

        /*
         * 
         * 
         * 
         * for (int i = 0; i < leftVertices.Length; i++)
        {
            if ((i % 50) == 19)
            {
                Vector3[] curbVertices = new Vector3[20];
                Vector3[] curbDirections = new Vector3[20];
                for (int j = 0; j < curbVertices.Length; j++)
                {
                    curbVertices[j] = new Vector3(leftVertices[i + j - 19].x, 0f, leftVertices[i + j - 19].y);
                    curbDirections[j] = new Vector3((leftVertices[i + j - 19] - rightVertices[i + j - 19]).x, 0f, (leftVertices[i + j - 19] - rightVertices[i + j - 19]).y);

                    leftWidths[i + j - 19] = 3f;
                }

                int prevIndex = (i + 0 - 20) < 0 ? leftVertices.Length - 1 : (i + 0 - 20);
                int afteIndex = (i + 1) % leftVertices.Length;

                GeneratedCurb curb = new GeneratedCurb(curbVertices, curbDirections, 3f, 0f, 0f, new Vector3(leftVertices[prevIndex].x, 0f, leftVertices[prevIndex].y), new Vector3(leftVertices[afteIndex].x, 0f, leftVertices[afteIndex].y));

                genCurbs.Add(curb);
            }
        }

        for (int i = 0; i < rightVertices.Length; i++)
        {
            if ((i % 50) == 19)
            {
                Vector3[] curbVertices = new Vector3[20];
                Vector3[] curbDirections = new Vector3[20];
                for (int j = 0; j < curbVertices.Length; j++)
                {
                    curbVertices[j] = new Vector3(rightVertices[i + j - 19].x, 0f, rightVertices[i + j - 19].y);
                    curbDirections[j] = new Vector3((rightVertices[i + j - 19] - leftVertices[i + j - 19]).x, 0f, (rightVertices[i + j - 19] - leftVertices[i + j - 19]).y);

                    rightWidths[i + j - 19] = 3f;
                }

                int prevIndex = (i + 0 - 20) < 0 ? leftVertices.Length - 1 : (i + 0 - 20);
                int afteIndex = (i + 1) % leftVertices.Length;

                GeneratedCurb curb = new GeneratedCurb(curbVertices, curbDirections, 3f, 0f, 0f, new Vector3(rightVertices[prevIndex].x, 0f, rightVertices[prevIndex].y), new Vector3(rightVertices[afteIndex].x, 0f, rightVertices[afteIndex].y));

                genCurbs.Add(curb);
            }
        }
         * 
         * 
         * */
    }

    public void CalculateCurbs(DiscreteTrack track)
    {
        bool[] leftCurbs = new bool[track.idealLineMesh.Length];
        bool[] rightCurbs = new bool[track.idealLineMesh.Length];
        for (int i = 0; i < leftCurbs.Length; i++)
        {
            leftCurbs[i] = false;
            rightCurbs[i] = false;
        }


        float threshhold = 0.23f;
        int addbefaft = 8;

        bool inv = false;
        int startIndex = -1;
        int endIndex = -1;

        for (int i = 0; i < track.idealLineMesh.Length; i++)
        {
            if (track.idealLineMesh[i] < threshhold && startIndex == -1)
            {
                inv = false;
                startIndex = i;
            }
            else if (track.idealLineMesh[i] >= threshhold && startIndex != -1)
            {
                endIndex = i;
            }

            if (track.idealLineMesh[i] > (1f - threshhold) && startIndex == -1)
            {
                inv = true;
                startIndex = i;
            }
            else if (track.idealLineMesh[i] <= (1f - threshhold) && startIndex != -1)
            {
                endIndex = i;
            }


            if (startIndex != -1 && endIndex != -1)
            {
                for (int j = (startIndex - addbefaft); j < endIndex + addbefaft; j++)
                {
                    int jR = j < 0 ? j + track.idealLineMesh.Length : (j % track.idealLineMesh.Length);

                    if (inv)
                    {
                        leftCurbs[jR] = true;
                    }
                    else
                    {
                        rightCurbs[jR] = true;
                    }
                }

                startIndex = -1;
                endIndex = -1;
            }
        }

        startIndex = -1;

        for (int i = 0; i < leftCurbs.Length; i++)
        {
            if (leftCurbs[i] && startIndex == -1)
            {
                startIndex = i;
            }
            else if (leftCurbs[i] == false && startIndex != -1)
            {
                Vector3[] curbVertices = new Vector3[i - startIndex];
                Vector3[] curbDirections = new Vector3[i - startIndex];
                for (int j = 0; j < curbVertices.Length; j++)
                {
                    curbVertices[j] = new Vector3(leftV[(startIndex + j - offset) < 0 ? (startIndex + j - offset + leftV.Length) : (startIndex + j - offset)].x, 0f, leftV[(startIndex + j - offset) < 0 ? (startIndex + j - offset + leftV.Length) : (startIndex + j - offset)].y);
                    curbDirections[j] = new Vector3((leftV[(startIndex + j - offset) < 0 ? (startIndex + j - offset + leftV.Length) : (startIndex + j - offset)] - rightV[(startIndex + j - offset) < 0 ? (startIndex + j - offset + leftV.Length) : (startIndex + j - offset)]).x, 0f, (leftV[(startIndex + j - offset) < 0 ? (startIndex + j - offset + leftV.Length) : (startIndex + j - offset)] - rightV[(startIndex + j - offset) < 0 ? (startIndex + j - offset + leftV.Length) : (startIndex + j - offset)]).y);

                    leftWidths[(startIndex + j - offset) < 0 ? (startIndex + j - offset + leftV.Length) : (startIndex + j - offset)] = 3f;
                }

                int prevIndex = startIndex - 1 - offset < 0 ? startIndex - 1 - offset + track.idealLineMesh.Length : startIndex - 1 - offset;
                int afteIndex = i - offset < 0 ? i - offset + track.idealLineMesh.Length : i - offset;


                GeneratedCurb curb = new GeneratedCurb(curbVertices, curbDirections, 3f, 0f, 0f, new Vector3(leftV[prevIndex].x, 0f, leftV[prevIndex].y), new Vector3(leftV[afteIndex].x, 0f, leftV[afteIndex].y));

                genCurbs.Add(curb);

                startIndex = -1;
            }
        }

        startIndex = -1;

        for (int i = 0; i < rightCurbs.Length; i++)
        {
            if (rightCurbs[i] && startIndex == -1)
            {
                startIndex = i;
            }
            else if (rightCurbs[i] == false && startIndex != -1)
            {
                Vector3[] curbVertices = new Vector3[i - startIndex];
                Vector3[] curbDirections = new Vector3[i - startIndex];
                for (int j = 0; j < curbVertices.Length; j++)
                {
                    curbVertices[j] = new Vector3(rightV[(startIndex + j - offset) < 0 ? (startIndex + j - offset + leftV.Length) : (startIndex + j - offset)].x, 0f, rightV[(startIndex + j - offset) < 0 ? (startIndex + j - offset + leftV.Length) : (startIndex + j - offset)].y);
                    curbDirections[j] = new Vector3((rightV[(startIndex + j - offset) < 0 ? (startIndex + j - offset + leftV.Length) : (startIndex + j - offset)] - leftV[(startIndex + j - offset) < 0 ? (startIndex + j - offset + leftV.Length) : (startIndex + j - offset)]).x, 0f, (rightV[(startIndex + j - offset) < 0 ? (startIndex + j - offset + leftV.Length) : (startIndex + j - offset)] - leftV[(startIndex + j - offset) < 0 ? (startIndex + j - offset + leftV.Length) : (startIndex + j - offset)]).y);

                    rightWidths[(startIndex + j - offset) < 0 ? (startIndex + j - offset + leftV.Length) : (startIndex + j - offset)] = 3f;
                }

                int prevIndex = startIndex - 1 - offset < 0 ? startIndex - 1 - offset + track.idealLineMesh.Length : startIndex - 1 - offset;
                int afteIndex = i - offset < 0 ? i - offset + track.idealLineMesh.Length : i - offset;


                GeneratedCurb curb = new GeneratedCurb(curbVertices, curbDirections, 3f, 0f, 0f, new Vector3(rightV[prevIndex].x, 0f, rightV[prevIndex].y), new Vector3(rightV[afteIndex].x, 0f, rightV[afteIndex].y));

                genCurbs.Add(curb);

                startIndex = -1;
            }
        }
    }


    public GeneratedCurb[] Curbs
    {
        get
        {
            return genCurbs.ToArray();
        }
    }

    public float[] LeftWidths
    {
        get
        {
            return leftWidths;
        }
    }

    public float[] RightWidths
    {
        get
        {
            return rightWidths;
        }
    }
}
