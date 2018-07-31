using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Vehicles.Car;

/// <summary>
/// 
/// Seeds:
///  -1137061697
///  456252396
///  -716017077
///  337427355
///  947660626
///  144650433
///  1975664267
///  -864663110  (-- yeah
///  1685782299
///  
///  -78865635
///  -1538910433
///  
/// -1531598274
/// -1163438866
/// 58528152
/// -1304947318
/// 871843198
/// 
/// </summary>

public class GenRendererGUI : MonoBehaviour
{
    [Header("Generation Renderer")]
    [SerializeField]
    private GenerationRenderer generationRenderer;
    [SerializeField]
    private CompleteRenderer completeRenderer;

    [Header("References")]
    [SerializeField]
    private HistoVisualizer histoSpeed;
    [SerializeField]
    private HistoVisualizer histoCurvature;
    [SerializeField]
    private HistoVisualizer histoCurvatureIdealline;
    [SerializeField]
    private InputField inputSeed;
    [SerializeField]
    private InputField inputHCCurvature;
    [SerializeField]
    private InputField inputHCSpeed;
    [SerializeField]
    private InputField inputHCTrackSelect;
    [SerializeField]
    private Toggle toggleIdealLine;
    [SerializeField]
    private Toggle toggleJumpFaulty;
    [SerializeField]
    private Text textHCTracksAmount;
    [SerializeField]
    private Text textHCCurvativeCurrentTrack;
    [SerializeField]
    private Text textHCCurvativeIdeallineCurrentTrack;
    [SerializeField]
    private Text textHCSpeedCurrentTrack;
    [SerializeField]
    private Text textTrackLength;
    [SerializeField]
    private Text textTimeNeeded;
    [SerializeField]
    private CarVisualize carVisualize;
    [SerializeField]
    private GameObject driveCar;
    [SerializeField]
    private Camera topCamera;
    [SerializeField]
    private Camera topCamera2;
    [SerializeField]
    private Terrain terrain;
    [SerializeField]
    private InteractiveGenTrack interactiveGenTrack;
    [SerializeField]
    private Button[] editTrackButtons;
    [SerializeField]
    private InputField inputCirclesIndex;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject prefabIdealLine;

    public static bool JumpFaultyTracks = true;

    private GeneratedTrack lastRefreshedTrack = null;

    private GameObject instIdealLine = null;
    private float oldTrackLength = 0f;


    // Use this for initialization
    void Start ()
    {
        for (int i = 0; i < editTrackButtons.Length; i++)
        {
            editTrackButtons[i].enabled = false;
        }
    }

    private bool measureTime = false;
    private DateTime timeStart;

	// Update is called once per frame
	void Update ()
    {
        if (measureTime && completeRenderer.tempRem == false)
        {
            Debug.Log("Start: " + timeStart);

            Debug.Log("Now: " + Time.time);

            measureTime = false;
            double needed = DateTime.Now.Subtract(timeStart).TotalMilliseconds;

            Debug.Log("Needed: " + needed);

            textTimeNeeded.text = needed.ToString();
        }

        if (startReallyRenderNow)
        {
            startReallyRenderNow = false;

            completeRenderer.Render(generationRenderer.CurrentRenderedTrack, generateMLStuff);

            generateMLStuff = false;
        }



		if (generationRenderer.HCTracks.Length >= 0)
        {
            textHCTracksAmount.text = generationRenderer.HCTracks.Length.ToString();
        }

        if (oldTrackLength != generationRenderer.TrackLength)
        {
            oldTrackLength = generationRenderer.TrackLength;
            textTrackLength.text = oldTrackLength.ToString();
        }

        if (generationRenderer.CurrentRenderedTrack != null && lastRefreshedTrack != generationRenderer.CurrentRenderedTrack)
        {
            lastRefreshedTrack = generationRenderer.CurrentRenderedTrack;
            textHCCurvativeCurrentTrack.text = generationRenderer.CurrentRenderedTrack.CurvatureProfile.HC.ToString();
            textHCCurvativeIdeallineCurrentTrack.text = generationRenderer.CurrentRenderedTrack.CurvatureIdeallineProfile.HC.ToString();
            textHCSpeedCurrentTrack.text = generationRenderer.CurrentRenderedTrack.SpeedProfile.HC.ToString();

            histoCurvature.RenderProfile(lastRefreshedTrack.CurvatureProfile);
            histoSpeed.RenderProfile(lastRefreshedTrack.SpeedProfile);
            histoCurvatureIdealline.RenderProfile(lastRefreshedTrack.CurvatureIdeallineProfile);

            if (carVisualize != null)
            {
                carVisualize.SetTrack(lastRefreshedTrack.AnalyzedTrack, lastRefreshedTrack.TerrainModifier);
            }

            if (prefabIdealLine != null && toggleIdealLine.isOn)
            {
                if (instIdealLine != null)
                {
                    Destroy(instIdealLine);
                    instIdealLine = null;
                }

                instIdealLine = Instantiate(prefabIdealLine);
                instIdealLine.transform.position = new Vector3(0f, 0.15f, 0f);
                instIdealLine.GetComponent<ClosedSplineRenderer>().Width = 0.7f;
                instIdealLine.GetComponent<ClosedSplineRenderer>().ClosedSpline = lastRefreshedTrack.AnalyzedTrack.idealLineSpline;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            topCamera.enabled = !topCamera.enabled;
            topCamera2.enabled = !topCamera2.enabled;

            GetComponentInParent<Canvas>().enabled = topCamera.enabled;

            driveCar.GetComponent<CarController>().enabled = !topCamera.enabled;
            driveCar.GetComponentInChildren<Camera>().enabled = !topCamera.enabled;

            driveCar.GetComponent<Rigidbody>().velocity = Vector3.zero;
            driveCar.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            driveCar.transform.rotation = Quaternion.identity;

            driveCar.transform.position = new Vector3(completeRenderer.startFinishBow.transform.position.x, completeRenderer.startFinishBow.transform.position.y + 2f, completeRenderer.startFinishBow.transform.position.z);
            driveCar.transform.rotation = completeRenderer.startFinishBow.transform.rotation;

        }
	}


    public void ButtonGenerateRandomClick()
    {
        generationRenderer.ActionGenerateRandom();

        inputSeed.text = generationRenderer.Seed.ToString();
    }

    public void ButtonGenerateToSeedClick()
    {
        generationRenderer.ActionGenerateToSeed(System.Convert.ToInt64(inputSeed.text));
    }

    public void ButtonGenerateToHCBlick()
    {
        generationRenderer.ActionGenerateToHCs(System.Convert.ToInt64(inputSeed.text), System.Convert.ToSingle(inputHCCurvature.text), System.Convert.ToSingle(inputHCSpeed.text));
    }

    public void ButtonShowHCTrackClick()
    {
        int trackNumber = System.Convert.ToInt32(inputHCTrackSelect.text);

        generationRenderer.Render(generationRenderer.HCTracks[trackNumber]);
    }

    public void ButtonMakeEditableClick()
    {
        interactiveGenTrack.MakeEditable();

        for (int i = 0; i < editTrackButtons.Length; i++)
        {
            editTrackButtons[i].enabled = true;
        }
    }

    private bool startReallyRenderNow = false;

    public void ButtonRealyRenderClick()
    {
        completeRenderer.tempRem = true;
        timeStart = DateTime.Now;
        startReallyRenderNow = true;
        measureTime = true;
    }

    private bool generateMLStuff = false;

    public void ButtonExportMLCLick()
    {
        generateMLStuff = true;
        completeRenderer.tempRem = true;
        timeStart = DateTime.Now;
        startReallyRenderNow = true;
        measureTime = true;
    }

    public void ButtonResetTerrain()
    {
        completeRenderer.ResetTerrain();
    }

    public void ButtonGenerateCirclesClick(int index)
    {
        generationRenderer.GenerateCircles(index);
    }

    public void ToggleIdealLineChanged()
    {
        if (toggleIdealLine.isOn && lastRefreshedTrack != null)
        {
            if (instIdealLine != null)
            {
                Destroy(instIdealLine);
                instIdealLine = null;
            }

            instIdealLine = Instantiate(prefabIdealLine);
            instIdealLine.transform.position = new Vector3(0f, 0.15f, 0f);
            instIdealLine.GetComponent<ClosedSplineRenderer>().Width = 0.7f;
            instIdealLine.GetComponent<ClosedSplineRenderer>().ClosedSpline = lastRefreshedTrack.AnalyzedTrack.idealLineSpline;
        }
        else if (toggleIdealLine.isOn == false && instIdealLine != null)
        {
            Destroy(instIdealLine);
            instIdealLine = null;
        }
    }

    public void ToggleJumpFaultyChanged()
    {
        JumpFaultyTracks = toggleJumpFaulty.isOn;
    }

    public void DistableEditButtons()
    {
        for (int i = 0; i < editTrackButtons.Length; i++)
        {
            editTrackButtons[i].enabled = false;
        }
    }

    public void EnableEditButtons()
    {
        for (int i = 0; i < editTrackButtons.Length; i++)
        {
            editTrackButtons[i].enabled = true;
        }
    }

    private void OnApplicationQuit()
    {
        ButtonResetTerrain();
    }
}
