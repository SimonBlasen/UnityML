using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortestpathTrajectory : GeneratedTrackSupplyer
{
    [Header("Settings")]
    [SerializeField]
    private Color colorBorder;
    [SerializeField]
    private Color colorMinCurvature;
    [SerializeField]
    private Color colorIdealLine;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject idealLine;
    [SerializeField]
    private GameObject speedTrap;

    [Header("References")]
    [SerializeField]
    private CarVisualize carVisual;
    [SerializeField]
    private HistoVisualizer histoSpeed;
    [SerializeField]
    private HistoVisualizer histoCurvature;

    [SerializeField]
    public float sSquare;
    [SerializeField]
    public float curvature;
    [SerializeField]
    public bool recalculate = false;
    [SerializeField]
    public float[] vs;
    [SerializeField]
    public float[] x;
    [SerializeField]
    public float[] xC;
    [SerializeField]
    public float[] cs;

    private DiscreteTrack discreteTrack = null;

    private GameObject instIdealLine = null;
    private List<GameObject> instSpeedTraps = new List<GameObject>();

    public override void TrackUpdated()
    {
        base.TrackUpdated();

        Statics.Log("New Discrete Track");
        discreteTrack = new DiscreteTrack(Track, Track.TerrainModifier);

        Statics.Log("Discrete Track created");
        x = discreteTrack.shortestPathTrajectory;
        xC = discreteTrack.minimumCurvatureTrajectory;

        gizCPoints = discreteTrack.GetCurvativePointsRight();
        Statics.Log("GetCurvativePointsRight");

        if (instIdealLine != null)
        {
            Destroy(instIdealLine);
            instIdealLine = null;
        }

        instIdealLine = Instantiate(idealLine);
        instIdealLine.transform.position = new Vector3(0f, 0.15f, 0f);
        instIdealLine.GetComponent<ClosedSplineRenderer>().Width = 0.7f;
        instIdealLine.GetComponent<ClosedSplineRenderer>().ClosedSpline = discreteTrack.idealLineSpline;

        vs = discreteTrack.speedAnalyzer.Vs;


        Statics.Log("vs = discreteTrack.speedAnalyzer.Vs");

        for (int i = 0; i < instSpeedTraps.Count; i++)
        {
            Destroy(instSpeedTraps[i]);
        }
        instSpeedTraps.Clear();

        for (int i = 0; i < discreteTrack.idealLineSpline.ControlPointsAmount; i++)
        {
            float s = ((float)i) / discreteTrack.idealLineSpline.ControlPointsAmount;

            instSpeedTraps.Add(Instantiate(speedTrap));
            instSpeedTraps[instSpeedTraps.Count - 1].transform.position = discreteTrack.idealLineSpline.SplineAt(s);
            //instSpeedTraps[instSpeedTraps.Count - 1].GetComponentInChildren<TextMesh>().text = (1f / discreteTrack.curvatureAnalyzer.Curvature[i]).ToString();
        }

        if (carVisual != null)
        {
            Statics.Log("SetTrack(discreteTrack)");
            carVisual.SetTrack(discreteTrack);
            Statics.Log("SetTrack(discreteTrack) -> Done");
        }

        if (histoSpeed != null)
        {
            HistoProfile histoProfileSpeed = new HistoProfile(17);
            histoProfileSpeed.SetBorders(0f, 100f);
            for (int i = 0; i < discreteTrack.speedAnalyzer.Vs.Length; i++)
            {
                histoProfileSpeed.AddValue(discreteTrack.speedAnalyzer.Vs[i]);
            }
            histoSpeed.RenderProfile(histoProfileSpeed);
        }

        if (histoCurvature != null)
        {
            HistoProfile histoProfileCurvature = new HistoProfile(17);
            histoProfileCurvature.SetBorders(-0.05f, 0.05f);
            for (int i = 0; i < discreteTrack.curvatureAnalyzerTrack.Curvature.Length; i++)
            {
                histoProfileCurvature.AddValue(discreteTrack.curvatureAnalyzerTrack.Curvature[i]);
            }
            histoCurvature.RenderProfile(histoProfileCurvature);
        }
    }

    private void Update()
    {
        if (recalculate)
        {
            recalculate = false;
        }
    }
    Vector3[] gizCPoints = null;

    private void OnDrawGizmosSelected()
    {
        if (discreteTrack != null)
        {
            Gizmos.color = colorBorder;
            for (int i = 0; i < discreteTrack.leftPointsCurv.Length; i++)
            {
                int i2 = (i + 1) % discreteTrack.leftPointsCurv.Length;
                Gizmos.DrawLine(new Vector3(discreteTrack.alphaICurv(i, 1f).x, 0f, discreteTrack.alphaICurv(i, 1f).y), new Vector3(discreteTrack.alphaICurv(i, 0f).x, 0f, discreteTrack.alphaICurv(i, 0f).y));
                Gizmos.DrawLine(new Vector3(discreteTrack.alphaICurv(i, 1f).x, 0f, discreteTrack.alphaICurv(i, 1f).y), new Vector3(discreteTrack.alphaICurv(i2, 1f).x, 0f, discreteTrack.alphaICurv(i2, 1f).y));
                Gizmos.DrawLine(new Vector3(discreteTrack.alphaICurv(i, 0f).x, 0f, discreteTrack.alphaICurv(i, 0f).y), new Vector3(discreteTrack.alphaICurv(i2, 0f).x, 0f, discreteTrack.alphaICurv(i2, 0f).y));
            }

            /*Gizmos.color = colorIdealLine;
            for (int i = 0; i < discreteTrack.SegmentsAmount; i++)
            {
                Gizmos.DrawLine(discreteTrack.GetIdealLinePoint(i), discreteTrack.GetIdealLinePoint((i + 1) % discreteTrack.SegmentsAmount));
            }*/

            /*if (gizCPoints != null)
            {
                Gizmos.color = colorMinCurvature;
                for (int j = 0; j < gizCPoints.Length; j++)
                {
                    Gizmos.DrawLine(gizCPoints[j], gizCPoints[(j + 1) % gizCPoints.Length]);
                }
            }*/

            /*Gizmos.color = colorMinCurvature;
            for (int j = 0; j < discreteTrack.GetCurvativePoints().Length; j++)
            {
                Gizmos.DrawLine(discreteTrack.GetCurvativePoints()[j], discreteTrack.GetCurvativePoints()[(j + 1) % discreteTrack.GetCurvativePoints().Length]);
            }*/
        }
    }
}

