using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagerBuilding : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private BuildingMouseMovement buildingMouseMovement;
    [SerializeField]
    private BuildingHealthManager buildingHealthManager;
    [SerializeField]
    private BuildingCalculatorTester buildingCalculatorTester;
    [SerializeField]
    private GUIElementManager guiElementManager;
    [SerializeField]
    private GUIPropertiesManager guiPropertiesManager;
    [SerializeField]
    private Toggle toggleShowPartsHealth;

    [Header("Settings")]
    [SerializeField]
    private float saveToFileRate = 5f;

    private float saveToFileCounter = 0f;
    private bool qOpened = false;
    private bool tOpened = false;

    // Use this for initialization
    void Start ()
    {
        guiElementManager.LoadFromLevel1();

        PartConfiguration[] configurations = FileLoader.LoadFromFile(Level1.fileWithModel);
        if (configurations != null)
        {
            bool success = false;

            for (int i = 0; i < configurations.Length; i++)
            {
                success = buildingMouseMovement.PartBuildingContainer.AddPart(configurations[i].partType, configurations[i].partDirection, configurations[i].partRotation, configurations[i].partPosition);
                if (! success)
                {
                    Debug.Log("A part wasn't placed, which should have been placed");
                }
                else
                {
                    ReducePartAmount(configurations[i].partType);
                }
            }
        }

        qOpened = false;
        tOpened = false;
        buildingMouseMovement.GUIOpened = qOpened;
        guiElementManager.GUIShown = qOpened;
        guiPropertiesManager.GUIShown = tOpened;

        if (qOpened)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !tOpened)
        {
            qOpened = !qOpened;
            buildingMouseMovement.GUIOpened = qOpened;
            guiElementManager.GUIShown = qOpened;

            if (qOpened)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.T) && !qOpened)
        {
            tOpened = !tOpened;
            buildingMouseMovement.GUIOpened = tOpened;
            guiPropertiesManager.GUIShown = tOpened;

            if (tOpened)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }


        saveToFileCounter += Time.deltaTime;
        if (saveToFileCounter >= saveToFileRate)
        {
            saveToFileCounter = 0f;
            if (Level1.garageIndex != -1)
            {
                FileLoader.SaveToFile(buildingMouseMovement.PartBuildingContainer.Parts, ".\\model" + Level1.garageIndex + ".baguette");
                Debug.Log("Saved Garage " + Level1.garageIndex);

                buildingCalculatorTester.GoCalculate(System.IO.File.ReadAllBytes(".\\model" + Level1.garageIndex + ".baguette"));
            }
        }
    }

    public void PartSelectedClicked(PartType type)
    {
        buildingMouseMovement.SelectPart(type);

        qOpened = false;
        buildingMouseMovement.GUIOpened = qOpened;
        buildingMouseMovement.SwitchToMode(CursorMode.PLACING);
        guiElementManager.GUIShown = qOpened;
    }

    public void ReducePartAmount(PartType type)
    {
        guiElementManager.GetElement(type).Amount--;
        if (guiElementManager.GetElement(type).Amount <= 0)
        {
            buildingMouseMovement.SelectPart(PartType.NONE);
        }
    }

    public void IncreasePartAmount(PartType type)
    {
        guiElementManager.GetElement(type).Amount++;
    }

    public void SaveCurrentToFile()
    {
        //FileLoader.SaveToFile(buildingMouseMovement.PartBuildingContainer.Parts, ".\\model.ahablyat");
    }

    public void ButtonBackToGarageClick()
    {
        saveToFileCounter = 0f;
        if (Level1.garageIndex != -1)
        {
            FileLoader.SaveToFile(buildingMouseMovement.PartBuildingContainer.Parts, ".\\model" + Level1.garageIndex + ".baguette");
            Debug.Log("Saved Garage " + Level1.garageIndex);
        }

        SceneManager.LoadScene("Garages");
    }

    public void ButtonTryOutClick()
    {
        /*CarPropertiesSetting cps = new CarPropertiesSetting();
        cps.DamperHeightLF = 1.0f;
        cps.DamperHeightRF = 1.0f;
        cps.DamperHeightLR = 3.0f;
        cps.DamperHeightRR = 3.1f;
        FileLoader.SavePropsToFile(cps, ".\\props1.bpr");

        CarPropertiesSetting lcps = FileLoader.LoadSettingFromFile(".\\props1.bpr");*/

        AutoTryManager.garageNumber = Level1.garageIndex;

        SceneManager.LoadScene("AutoTry");
    }

    public void ToggleShowPartsHealthClick()
    {
        buildingHealthManager.PartsHealthShown = toggleShowPartsHealth.isOn;
    }
}
