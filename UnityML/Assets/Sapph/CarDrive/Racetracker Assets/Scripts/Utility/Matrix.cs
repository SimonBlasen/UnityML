using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix
{
    private float[,] els;

    public Matrix(int rows, int columns)
    {
        els = new float[rows, columns];
        for (int x = 0; x < Rows; x++)
        {
            for (int y = 0; y < Columns; y++)
            {
                els[x, y] = 0f;
            }
        }
    }

    public int Rows
    {
        get
        {
            return els.GetLength(0);
        }
    }

    public int Columns
    {
        get
        {
            return els.GetLength(1);
        }
    }

    public float[,] At
    {
        get
        {
            return els;
        }
        set
        {
            els = value;
        }
    }

    public Matrix transposed
    {
        get
        {
            Matrix tMatrix = new Matrix(Columns, Rows);
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    tMatrix.At[y, x] = At[x, y];
                }
            }
            return tMatrix;
        }
    }

    public static Matrix operator +(Matrix a, Matrix b)
    {
        Matrix matrix;

        if (a.Rows == b.Rows && a.Columns == b.Columns)
        {
            matrix = new Matrix(a.Rows, a.Columns);

            for (int x = 0; x < a.Rows; x++)
            {
                for (int y = 0; y < a.Columns; y++)
                {
                    matrix.At[x, y] = a.At[x, y] + b.At[x, y];
                }
            }
        }
        else
        {
            matrix = new Matrix(0, 0);
            Debug.LogError("Matrix addition failed");
        }

        return matrix;
    }

    public static Matrix operator -(Matrix a, Matrix b)
    {
        Matrix matrix;

        if (a.Rows == b.Rows && a.Columns == b.Columns)
        {
            matrix = new Matrix(a.Rows, a.Columns);

            for (int x = 0; x < a.Rows; x++)
            {
                for (int y = 0; y < a.Columns; y++)
                {
                    matrix.At[x, y] = a.At[x, y] - b.At[x, y];
                }
            }
        }
        else
        {
            matrix = new Matrix(0, 0);
            Debug.LogError("Matrix subtraction failed");
        }

        return matrix;
    }

    public static Matrix operator *(Matrix a, Matrix b)
    {
        Matrix matrix;

        if (a.Columns == b.Rows)
        {
            matrix = new Matrix(a.Rows, b.Columns);

            for (int x = 0; x < a.Rows; x++)
            {
                for (int y = 0; y < b.Columns; y++)
                {
                    float value = 0f;

                    for (int i = 0; i < a.Columns; i++)
                    {
                        value += a.At[x, i] * b.At[i, y];
                    }

                    matrix.At[x, y] = value;
                }
            }
        }
        else
        {
            matrix = new Matrix(0, 0);
            Debug.LogError("Matrix multiplication failed");
        }

        return matrix;
    }

    public static Matrix operator *(float skalar, Matrix a)
    {
        Matrix matrix;

        matrix = new Matrix(a.Rows, a.Columns);

        for (int x = 0; x < a.Rows; x++)
        {
            for (int y = 0; y < a.Columns; y++)
            {
                matrix.At[x, y] = a.At[x, y] * skalar;
            }
        }

        return matrix;
    }
}
