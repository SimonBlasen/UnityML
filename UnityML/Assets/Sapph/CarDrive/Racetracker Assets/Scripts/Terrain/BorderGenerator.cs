using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderGenerator
{
    private List<Vector3> leftWall;
    private List<Vector3> rightWall;

    private float minAngle = 15f;
    private float fixDistance = 30f;
    private float maxTurnDegreeResolution = 90f / 8f;

    public BorderGenerator(TerrainModifier terrainModifier, List<GeneratedElement> elements, float startFinishLineIndex)
    {
        leftWall = new List<Vector3>();
        rightWall = new List<Vector3>();

        List<Vector2> left = new List<Vector2>();
        List<Vector2> right = new List<Vector2>();

        int startIndex = (int)startFinishLineIndex;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[(i + startIndex) % elements.Count].GetType() == typeof(GeneratedStraight))
            {
                GeneratedStraight straight = (GeneratedStraight)elements[(i + startIndex) % elements.Count];

                Vector3 toRight = (Quaternion.Euler(0f, (straight.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(straight.WidthEnd, 0f, 0f));
                Vector3 toFront = (Quaternion.Euler(0f, (straight.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(0f, 0f, straight.Length));


                int segmentsAmount = (int)(straight.Length / 4f);

                for (int j = 1; j <= segmentsAmount; j++)
                {
                    float s = ((float)j) / segmentsAmount;


                    left.Add(new Vector2((straight.Position + (toFront * s) - toRight - (toRight.normalized * fixDistance)).x, (straight.Position + (toFront * s) - toRight - (toRight.normalized * fixDistance)).z));
                    right.Add(new Vector2((straight.Position + (toFront * s) + toRight + (toRight.normalized * fixDistance)).x, (straight.Position + (toFront * s) + toRight + (toRight.normalized * fixDistance)).z));
                }
            }
            else if (elements[(i + startIndex) % elements.Count].GetType() == typeof(GeneratedTurn))
            {
                GeneratedTurn turn = (GeneratedTurn)elements[(i + startIndex) % elements.Count];

                bool rightTurn = turn.Degree >= 0f;

                Vector3 toRight = (Quaternion.Euler(0f, (turn.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(1f, 0f, 0f));
                Vector3 toFront = (Quaternion.Euler(0f, (turn.Direction * 180f) / Mathf.PI, 0f)) * (new Vector3(0f, 0f, 1f));

                Vector3 middlePoint = toRight * turn.Radius * (rightTurn ? 1f : -1f);

                int segmentsAmount = ((int)(Mathf.Abs(turn.Degree) / maxTurnDegreeResolution)) + 1;

                for (int j = 1; j <= segmentsAmount; j++)
                {
                    Vector3 toRightTurned = (Quaternion.Euler(0f, (turn.Degree / ((float)segmentsAmount)) * j, 0f)) * toRight;

                    Vector3 segmentPos = middlePoint + (toRightTurned * (rightTurn ? -1f : 1f) * turn.Radius);

                    float currentWidth = (turn.WidthEnd - turn.WidthStart) * (float)((float)j / (float)segmentsAmount) + turn.WidthStart;


                    left.Add(new Vector2((segmentPos + toRightTurned * currentWidth * -1f + (toRightTurned.normalized * fixDistance * -1f) + turn.Position).x, (segmentPos + toRightTurned * currentWidth * -1f + (toRightTurned.normalized * fixDistance * -1f) + turn.Position).z));
                    right.Add(new Vector2((segmentPos + toRightTurned * currentWidth + (toRightTurned.normalized * fixDistance) + turn.Position).x, (segmentPos + toRightTurned * currentWidth + (toRightTurned.normalized * fixDistance) + turn.Position).z));

                }
            }
            else if (elements[(i + startIndex) % elements.Count].GetType() == typeof(GeneratedBezSpline))
            {
                GeneratedBezSpline bezierSpline = (GeneratedBezSpline)elements[(i + startIndex) % elements.Count];

                for (int j = 1; j < bezierSpline.RenderVertsLeft.Length; j++)
                {
                    Vector3 toRight = (bezierSpline.RenderVertsRight[j] - bezierSpline.RenderVertsLeft[j]).normalized;


                    left.Add(new Vector2((bezierSpline.RenderVertsLeft[j] + (toRight * -fixDistance)).x, (bezierSpline.RenderVertsLeft[j] + (toRight * -fixDistance)).z));
                    right.Add(new Vector2((bezierSpline.RenderVertsRight[j] + (toRight * fixDistance)).x, (bezierSpline.RenderVertsRight[j] + (toRight * fixDistance)).z));
                }
            }
        }


        bool[] leftIgnores = new bool[left.Count];
        bool[] rightIgnores = new bool[right.Count];
        for (int i = 0; i < leftIgnores.Length; i++)
        {
            leftIgnores[i] = false;
        }
        for (int i = 0; i < rightIgnores.Length; i++)
        {
            rightIgnores[i] = false;
        }


        for (int i = 0; i < left.Count; i++)
        {
            int i2 = (i + 1) % left.Count;
            Line lineToTest = new Line(left[i % left.Count], left[i2]);

            int jumpIndex = -1;

            for (int jT = 0 + 2; jT < left.Count - 40; jT++)
            {
                int j = (jT + i) % left.Count;
                int j2 = (j + 1) % left.Count;
                if (leftIgnores[j] == false)
                {
                    Line other = new Line(left[j], left[j2]);

                    Vector2 intersectPoint;
                    if (lineToTest.Intersects(other, out intersectPoint))
                    {
                        jumpIndex = (j < i ? j + left.Count : j);
                        break;
                    }
                }
            }

            leftWall.Add(new Vector3(left[i].x, terrainModifier.GetTensorHeight(left[i].x, left[i].y), left[i].y));
            if (jumpIndex != -1)
            {
                for (int k = i; k < jumpIndex; k++)
                {
                    leftIgnores[k % leftIgnores.Length] = true;
                }

                i = jumpIndex;
                leftWall.Add(new Vector3(left[i % left.Count].x, terrainModifier.GetTensorHeight(left[i % left.Count].x, left[i % left.Count].y), left[i % left.Count].y));
            }
        }


        for (int i = 0; i < right.Count; i++)
        {
            int i2 = (i + 1) % right.Count;
            Line lineToTest = new Line(right[i], right[i2]);

            int jumpIndex = -1;

            for (int jT = 0 + 2; jT < right.Count - 40; jT++)
            {
                int j = (jT + i) % right.Count;
                int j2 = (j + 1) % right.Count;
                if (rightIgnores[j] == false)
                {
                    Line other = new Line(right[j], right[j2]);

                    Vector2 intersectPoint;
                    if (lineToTest.Intersects(other, out intersectPoint))
                    {
                        jumpIndex = (j < i ? j + right.Count : j);
                        break;
                    }
                }
            }

            rightWall.Add(new Vector3(right[i].x, terrainModifier.GetTensorHeight(right[i].x, right[i].y), right[i].y));
            if (jumpIndex != -1)
            {
                for (int k = i; k < jumpIndex; k++)
                {
                    rightIgnores[k % rightIgnores.Length] = true;
                }

                i = jumpIndex;
                rightWall.Add(new Vector3(right[i % right.Count].x, terrainModifier.GetTensorHeight(right[i % right.Count].x, right[i % right.Count].y), right[i % right.Count].y));
            }
        }
    }


    public BorderGenerator(TerrainModifier terrainModifier, List<GeneratedElement> elements) : this(terrainModifier, elements, 0f)
    {
        
    }


    public Vector3[] WallLeft
    {
        get
        {
            return leftWall.ToArray();
        }
    }
    
    public Vector3[] WallRight
    {
        get
        {
            return rightWall.ToArray();
        }
    }
}
