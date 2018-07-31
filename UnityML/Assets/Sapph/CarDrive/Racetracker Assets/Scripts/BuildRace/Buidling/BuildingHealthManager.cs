using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHealthManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private BuildingCalculatorTester buildingCalculatorTester;
    [SerializeField]
    private GameObject cameraObject;

    [Header("Settings")]
    [SerializeField]
    private float refreshRate = 1f;

    [Header("Prefabs")]
    [SerializeField]
    private GameObject textHealth3dPrefab;

    private bool partsHealthShown;
    private float counter = 0f;

    private List<GameObject> texts3d;
    private CalculatedCar car;
    private int textsIndex = 0;

    // Use this for initialization
    void Start ()
    {
        texts3d = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        counter += Time.deltaTime;
		if (counter > refreshRate)
        {
            counter = 0f;
            
            if (buildingCalculatorTester.Car != null)
            {
                car = buildingCalculatorTester.Car;
                refreshTexts();
            }
        }
	}

    public bool PartsHealthShown
    {
        get
        {
            return partsHealthShown;
        }
        set
        {
            partsHealthShown = value;
        }
    }

    private void refreshTexts()
    {
        textsIndex = 0;

        if (car != null)
        {
            setText(car.PartSeat.Position, System.Convert.ToString(Mathf.Round(car.PartSeat.Health)));


            for (int i = 0; i < car.PartPowertrainAxes.Length; i++)
            {
                setText(car.PartPowertrainAxes[i].Position, System.Convert.ToString(Mathf.Round(car.PartPowertrainAxes[i].Health)));
            }
            for (int i = 0; i < car.PartStatics.Length; i++)
            {
                setText(car.PartStatics[i].Position, System.Convert.ToString(Mathf.Round(car.PartStatics[i].Health)));
            }
            for (int i = 0; i < car.PartSteerBars.Length; i++)
            {
                setText(car.PartSteerBars[i].Position, System.Convert.ToString(Mathf.Round(car.PartSteerBars[i].Health)));
            }
            for (int i = 0; i < car.PartSteerParts.Length; i++)
            {
                setText(car.PartSteerParts[i].Position, System.Convert.ToString(Mathf.Round(car.PartSteerParts[i].Health)));
            }
            for (int i = 0; i < car.PartWheels.Length; i++)
            {
                setText(car.PartWheels[i].Position, System.Convert.ToString(Mathf.Round(car.PartWheels[i].Health)));
            }
        }

        

        for (int i = textsIndex; i < texts3d.Count; i++)
        {
            texts3d[i].GetComponent<HealthText>().Visibility = false;
        }
    }

    private void setText(Vector3 position, string text)
    {
        if (textsIndex < texts3d.Count)
        {
            texts3d[textsIndex].transform.position = position - car.SeatOffset;
            texts3d[textsIndex].GetComponent<HealthText>().ShowText = text;
            texts3d[textsIndex].GetComponent<HealthText>().Visibility = partsHealthShown;
        }
        else
        {
            GameObject inst = (GameObject)Instantiate(textHealth3dPrefab);
            texts3d.Add(inst);
            inst.transform.position = position - car.SeatOffset;
            inst.GetComponent<HealthText>().ShowText = text;
            inst.GetComponent<HealthText>().LookAt = cameraObject;
            inst.GetComponent<HealthText>().Visibility = partsHealthShown;
        }

        textsIndex++;
    }
}
