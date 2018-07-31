using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyzerJob : ThreadedJob
{
    public byte[] c = null;

    public CalculatedCar car = null;

    protected override void ThreadFunction()
    {
        if (c != null)
        {
            car = Analyzer.AnalyzeCar(c);
        }
    }
}
