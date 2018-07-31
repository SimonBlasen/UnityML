using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoTryManager : MonoBehaviour {

    [Header("References")]
    [SerializeField]
    private GameObject car;


    public static int garageNumber = -1;
    private CalculatedCar calculatedCar;
    private ConnectorFile connFile;
    private CarPropertiesSetting carProps;

    private bool firstTime = true;

	// Use this for initialization
	void Start ()
    {
		if (garageNumber != -1)
        {
            calculatedCar = Analyzer.AnalyzeCar(".\\model" + garageNumber + ".baguette");
            connFile = FileLoader.LoadConnFile(".\\model" + garageNumber + ".connb");
            carProps = FileLoader.LoadSettingFromFile(connFile.PropertiesFile);

            if (calculatedCar == null)
            {
                SceneManager.LoadScene("Garages");
            }
            else
            {
                car.GetComponent<TestCar>().Freeze = true;
                car.GetComponent<TestCar>().ApplyCar(calculatedCar, carProps);
                car.transform.position = new Vector3(0f, 0f, 0f);
            }
        }
        else
        {
            SceneManager.LoadScene("Garages");
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Garages");
        }



        if (firstTime)
        {
            car.GetComponent<TestCar>().Freeze = false;
            firstTime = false;
        }
	}
}
