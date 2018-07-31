using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerGarages : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Text textAnalyzed;
    [SerializeField]
    private Button buttonTestCar;

    private CalculatedCar calculatedCar = null;

    public Canvas loadingCanvas;

    private InventoryElement[] allElements;

    private int selectedGarage = -1;

	// Use this for initialization
	void Start ()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        allElements = new InventoryElement[38];
        
        for (int i = 0; i < allElements.Length; i++)
        {
            allElements[i] = new InventoryElement();
            allElements[i].partType = (PartType)(i + 1);
            allElements[i].amount = 500;
        }

        buttonTestCar.enabled = false;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OpenGarage(int garageIndex)
    {
        selectedGarage = garageIndex;
        Level1.garageIndex = selectedGarage;
        calculatedCar = Analyzer.AnalyzeCar(".\\model" + selectedGarage + ".baguette");
        //calculatedCar = Analyzer.AnalyzeCarD(".\\model" + selectedGarage + ".baguette");

        if (calculatedCar != null)
        {
            textAnalyzed.text = calculatedCar.ToString();
            buttonTestCar.enabled = true;
        }
        else
        {
            textAnalyzed.text = "Car could not be calculated";
            buttonTestCar.enabled = false;
        }
    }

    public void BuildButtonClick()
    {
        if (selectedGarage >= 1 && selectedGarage <= 7)
        {
            Level1.fileWithModel = ".\\model" + selectedGarage + ".baguette";
            Level1.garageIndex = selectedGarage;
            Level1.elements = allElements;

            loadingCanvas.enabled = true;

            SceneManager.LoadScene("Level1");
        }
        else
        {
            textAnalyzed.text = "Select a garage first!";
        }
    }

    public void ButtonStartTestClick()
    {
        if (calculatedCar != null)
        {
            Racing.calculatedCarNew = calculatedCar;

            SceneManager.LoadScene("RacingTesting");
        }
    }

    public void ButtonJoinMultiplayerClick()
    {
        if (calculatedCar != null)
        {
            Racing.calculatedCarNew = calculatedCar;

            SceneManager.LoadScene("MultiplayerMenu");
        }
    }
}
