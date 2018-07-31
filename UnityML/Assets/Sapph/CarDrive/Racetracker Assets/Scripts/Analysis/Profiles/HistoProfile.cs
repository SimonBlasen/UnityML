using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HistoProfile
{
    private int[] bins;
    private List<float> values;
    private float borderLow;
    private float step;

    public HistoProfile(int binsAmount)
    {
        bins = new int[binsAmount];
        values = new List<float>();
        borderLow = 0f;
        step = 1f;

        for (int i = 0; i < bins.Length; i++)
        {
            bins[i] = 0;
        }
    }

    public void SetBorders(float low, float high)
    {
        borderLow = low;
        step = (high - low) / bins.Length;

        for (int i = 0; i < bins.Length; i++)
        {
            bins[i] = 0;
        }
    }

    public void AddValue(float value)
    {
        bool foundBin = false;
        for (int i = 0; i < bins.Length; i++)
        {
            if ((i + 1f) * step + borderLow > value)
            {
                bins[i]++;
                foundBin = true;
                break;
            }
        }

        if (!foundBin)
        {
            bins[bins.Length - 1]++;
        }
    }

    public void ClearValues()
    {
        for (int i = 0; i < bins.Length; i++)
        {
            bins[i] = 0;
        }
    }

    public int[] Bins
    {
        get
        {
            return bins;
        }
    }

    public float HC
    {
        get
        {
            float sum = 0f;
            float[] percents = Percents;
            for (int i = 0; i < percents.Length; i++)
            {
                if (percents[i] > 0f)
                {
                    sum += percents[i] * Mathf.Log(percents[i], 2f);
                }
            }

            return -sum;
        }
    }

    public float[] Percents
    {
        get
        {
            float[] percents = new float[bins.Length];

            int totalAmount = 0;
            for (int i = 0; i < bins.Length; i++)
            {
                totalAmount += bins[i];
            }

            for (int i = 0; i < percents.Length; i++)
            {
                percents[i] = ((float)bins[i]) / totalAmount;
            }

            return percents;
        }
    }
}
