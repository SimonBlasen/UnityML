using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIPropertiesManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField]
    private Canvas canvasGUI;
    [SerializeField]
    private ManagerBuilding manager;

    [Space]
    [SerializeField]
    private Button buttonSave;
    [SerializeField]
    private Button buttonReset;
    [SerializeField]
    private InputField inputPropertiesPath;
    [SerializeField]
    private Button buttonPropertiesPath;
    [SerializeField]
    private Text textPathPropertiesError;
    [Space]
    [SerializeField]
    private InputField inputPropDamperheightLF;
    [SerializeField]
    private InputField inputPropDamperheightRF;
    [SerializeField]
    private InputField inputPropDamperheightLR;
    [SerializeField]
    private InputField inputPropDamperheightRR;
    [SerializeField]
    private InputField inputPropDamperstrengthLF;
    [SerializeField]
    private InputField inputPropDamperstrengthRF;
    [SerializeField]
    private InputField inputPropDamperstrengthLR;
    [SerializeField]
    private InputField inputPropDamperstrengthRR;
    [SerializeField]
    private InputField inputPropSpringstrengthLF;
    [SerializeField]
    private InputField inputPropSpringstrengthRF;
    [SerializeField]
    private InputField inputPropSpringstrengthLR;
    [SerializeField]
    private InputField inputPropSpringstrengthRR;

    [SerializeField]
    private Slider sliderDamperheightLF;
    [SerializeField]
    private Slider sliderDamperheightRF;
    [SerializeField]
    private Slider sliderDamperheightLR;
    [SerializeField]
    private Slider sliderDamperheightRR;
    [SerializeField]
    private Slider sliderDamperstrengthLF;
    [SerializeField]
    private Slider sliderDamperstrengthRF;
    [SerializeField]
    private Slider sliderDamperstrengthLR;
    [SerializeField]
    private Slider sliderDamperstrengthRR;
    [SerializeField]
    private Slider sliderSpringstrengthLF;
    [SerializeField]
    private Slider sliderSpringstrengthRF;
    [SerializeField]
    private Slider sliderSpringstrengthLR;
    [SerializeField]
    private Slider sliderSpringstrengthRR;


    private ConnectorFile connFile;
    private CarPropertiesSetting carProps;


    // Use this for initialization
    void Start ()
    {
        // Eine Datei zu jedem Model schreiben, in der der Pfad zur Properties-file steht
        // Die hier einfach laden, bzw erstellen. Dann das inputField zum Path mit dem Pfad zur Properties-file füllen. Wenn nicht, leer lassen.
        connFile = FileLoader.LoadConnFile(".\\model" + Level1.garageIndex + ".connb");
        inputPropertiesPath.text = connFile.PropertiesFile;

        if (File.Exists(connFile.PropertiesFile))
        {
            carProps = FileLoader.LoadSettingFromFile(connFile.PropertiesFile);
        }
        else
        {
            carProps = new CarPropertiesSetting();
        }

        // TODO
        // Fill inputFields with carProps

        inputPropDamperheightLF.text = Convert.ToString(carProps.DamperHeightLF);
        inputPropDamperheightRF.text = Convert.ToString(carProps.DamperHeightRF);
        inputPropDamperheightLR.text = Convert.ToString(carProps.DamperHeightLR);
        inputPropDamperheightRR.text = Convert.ToString(carProps.DamperHeightRR);
        inputPropDamperstrengthLF.text = Convert.ToString(carProps.DamperStrengthLF);
        inputPropDamperstrengthRF.text = Convert.ToString(carProps.DamperStrengthRF);
        inputPropDamperstrengthLR.text = Convert.ToString(carProps.DamperStrengthLR);
        inputPropDamperstrengthRR.text = Convert.ToString(carProps.DamperStrengthRR);
        inputPropSpringstrengthLF.text = Convert.ToString(carProps.SpringStrengthLF);
        inputPropSpringstrengthRF.text = Convert.ToString(carProps.SpringStrengthRF);
        inputPropSpringstrengthLR.text = Convert.ToString(carProps.SpringStrengthLR);
        inputPropSpringstrengthRR.text = Convert.ToString(carProps.SpringStrengthRR);

        GetComponent<Canvas>().enabled = false;
        GetComponent<Canvas>().enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public bool GUIShown
    {
        get
        {
            return canvasGUI.enabled;
        }
        set
        {
            canvasGUI.enabled = value;
        }
    }

    public void InputFieldPropertiesPathChanged()
    {

    }

    public void ButtonSaveClick()
    {
        PropFieldsEndEdit();
        string propsFilepath = inputPropertiesPath.text;

        if (propsFilepath.Length > 0)
        {
            connFile.PropertiesFile = propsFilepath;

            FileLoader.SaveConnFile(".\\model" + Level1.garageIndex + ".connb", connFile);

            FileLoader.SavePropsToFile(carProps, propsFilepath);
        }
    }

    public void ButtonSaveClickAndRefresh()
    {
        PropFieldsEndEdit();
        string propsFilepath = inputPropertiesPath.text;

        if (propsFilepath.Length > 0)
        {
            connFile.PropertiesFile = propsFilepath;

            FileLoader.SaveConnFile(".\\model" + Level1.garageIndex + ".connb", connFile);

            FileLoader.SavePropsToFile(carProps, propsFilepath);
        }

        SceneManager.LoadScene("AutoTry");
    }

    public void PropFieldsEndEdit()
    {
        try
        {
            carProps.DamperHeightLF = Convert.ToSingle(inputPropDamperheightLF.text);
        }
        catch (Exception ex)
        {
            inputPropDamperheightLF.text = "0";
        }
        try
        {
            carProps.DamperHeightRF = Convert.ToSingle(inputPropDamperheightRF.text);
        }
        catch (Exception ex)
        {
            inputPropDamperheightRF.text = "0";
        }
        try
        {
            carProps.DamperHeightLR = Convert.ToSingle(inputPropDamperheightLR.text);
        }
        catch (Exception ex)
        {
            inputPropDamperheightLR.text = "0";
        }
        try
        {
            carProps.DamperHeightRR = Convert.ToSingle(inputPropDamperheightRR.text);
        }
        catch (Exception ex)
        {
            inputPropDamperheightRR.text = "0";
        }


        try
        {
            carProps.DamperStrengthLF = Convert.ToSingle(inputPropDamperstrengthLF.text);
        }
        catch (Exception ex)
        {
            if (inputPropDamperstrengthLF.text == "def")
            {
                inputPropDamperstrengthLF.text = Convert.ToString((new CarPropertiesSetting()).DamperStrengthLF);
            }
            else
            {
                inputPropDamperstrengthLF.text = "0";
            }
        }
        try
        {
            carProps.DamperStrengthRF = Convert.ToSingle(inputPropDamperstrengthRF.text);
        }
        catch (Exception ex)
        {
            if (inputPropDamperstrengthRF.text == "def")
            {
                inputPropDamperstrengthRF.text = Convert.ToString((new CarPropertiesSetting()).DamperStrengthRF);
            }
            else
            {
                inputPropDamperstrengthRF.text = "0";
            }
        }
        try
        {
            carProps.DamperStrengthLR = Convert.ToSingle(inputPropDamperstrengthLR.text);
        }
        catch (Exception ex)
        {
            if (inputPropDamperstrengthLR.text == "def")
            {
                inputPropDamperstrengthLR.text = Convert.ToString((new CarPropertiesSetting()).DamperStrengthLR);
            }
            else
            {
                inputPropDamperstrengthLR.text = "0";
            }
        }
        try
        {
            carProps.DamperStrengthRR = Convert.ToSingle(inputPropDamperstrengthRR.text);
        }
        catch (Exception ex)
        {
            if (inputPropDamperstrengthRR.text == "def")
            {
                inputPropDamperstrengthRR.text = Convert.ToString((new CarPropertiesSetting()).DamperStrengthRR);
            }
            else
            {
                inputPropDamperstrengthRR.text = "0";
            }
        }




        try
        {
            carProps.SpringStrengthLF = Convert.ToSingle(inputPropSpringstrengthLF.text);
        }
        catch (Exception ex)
        {
            if (inputPropSpringstrengthLF.text == "def")
            {
                inputPropSpringstrengthLF.text = Convert.ToString((new CarPropertiesSetting()).SpringStrengthLF);
            }
            else
            {
                inputPropSpringstrengthLF.text = "0";
            }
        }
        try
        {
            carProps.SpringStrengthRF = Convert.ToSingle(inputPropSpringstrengthRF.text);
        }
        catch (Exception ex)
        {
            if (inputPropSpringstrengthRF.text == "def")
            {
                inputPropSpringstrengthRF.text = Convert.ToString((new CarPropertiesSetting()).SpringStrengthRF);
            }
            else
            {
                inputPropSpringstrengthRF.text = "0";
            }
        }
        try
        {
            carProps.SpringStrengthLR = Convert.ToSingle(inputPropSpringstrengthLR.text);
        }
        catch (Exception ex)
        {
            if (inputPropSpringstrengthLR.text == "def")
            {
                inputPropSpringstrengthLR.text = Convert.ToString((new CarPropertiesSetting()).SpringStrengthLR);
            }
            else
            {
                inputPropSpringstrengthLR.text = "0";
            }
        }
        try
        {
            carProps.SpringStrengthRR = Convert.ToSingle(inputPropSpringstrengthRR.text);
        }
        catch (Exception ex)
        {
            if (inputPropSpringstrengthRR.text == "def")
            {
                inputPropSpringstrengthRR.text = Convert.ToString((new CarPropertiesSetting()).SpringStrengthRR);
            }
            else
            {
                inputPropSpringstrengthRR.text = "0";
            }
        }
    }

    public void SliderValueChanged(int id)
    {
        switch (id)
        {
            case 0:
                inputPropDamperheightLF.text = Convert.ToString(sliderDamperheightLF.value);
                break;
            case 1:
                inputPropDamperheightRF.text = Convert.ToString(sliderDamperheightRF.value);
                break;
            case 2:
                inputPropDamperheightLR.text = Convert.ToString(sliderDamperheightLR.value);
                break;
            case 3:
                inputPropDamperheightRR.text = Convert.ToString(sliderDamperheightRR.value);
                break;
            case 4:
                inputPropDamperstrengthLF.text = Convert.ToString((sliderDamperstrengthLF.value + 0.5f) * 4344.03f);
                break;
            case 5:
                inputPropDamperstrengthRF.text = Convert.ToString((sliderDamperstrengthRF.value + 0.5f) * 4344.03f);
                break;
            case 6:
                inputPropDamperstrengthLR.text = Convert.ToString((sliderDamperstrengthLR.value + 0.5f) * 4344.03f);
                break;
            case 7:
                inputPropDamperstrengthRR.text = Convert.ToString((sliderDamperstrengthRR.value + 0.5f) * 4344.03f);
                break;
            case 8:
                inputPropSpringstrengthLF.text = Convert.ToString((sliderSpringstrengthLF.value + 0.5f) * 33786.9f);
                break;
            case 9:
                inputPropSpringstrengthRF.text = Convert.ToString((sliderSpringstrengthRF.value + 0.5f) * 33786.9f);
                break;
            case 10:
                inputPropSpringstrengthLR.text = Convert.ToString((sliderSpringstrengthLR.value + 0.5f) * 33786.9f);
                break;
            case 11:
                inputPropSpringstrengthRR.text = Convert.ToString((sliderSpringstrengthRR.value + 0.5f) * 33786.9f);
                break;
        }
    }
}
